using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRotationInAir : MonoBehaviour
  {
    public HeroCarOnGroundChecker GroundChecker;
    public HeroCarCrashChecker CrashChecker;
    public float RotationSpeed;
    public float AdditionalTurnSpeed;
    public float RotateAngle;
    public float ReductionRate;
    public float MaxAngleMagnitude = 2;
    public float MinAngleMagnitude = 0.5f;
    public HeroCarRotateLookPoint RotateLookPoint;

    private IEnumerator _coroutine;
    private float _lastRotateAngle;
    private float _angleMagnitude;
    private bool _isDrag;
    private bool _isAdditionalTurn;
    private bool _isInversionRotation = true;
    private bool _isOnlyTwoRotateAxis = false;
    private float _coordX;
    private float _coordY;
    private Vector3 _angle = Vector3.zero;
    private float _disablerY = 1;
    private float _disablerX = 1;

    private IInputService _inputService;

    public event Action Dragged;
    public event Action RotateEnabled;
    public event Action RotateDisabled;

    public bool IsInversionRotation => _isInversionRotation;
    public bool IsOnlyTwoRotateAxis => _isOnlyTwoRotateAxis;

    public Vector3 Angle => _angle;
    
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void OnEnable()
    {
      GroundChecker.TookOff += OnTookOff;
      GroundChecker.LandedOnGround += OnLanded;
      CrashChecker.Crashed += OnLanded;
    }

    private void Start() => 
      transform.parent = null;

    private void OnDisable()
    {
      GroundChecker.TookOff -= OnTookOff;
      GroundChecker.LandedOnGround -= OnLanded;
      CrashChecker.Crashed -= OnLanded;
    }

    private void Update()
    {
      if (GroundChecker.IsOnGround)
        return;

      if (_inputService == null)
        return;

      if (_inputService.IsFingerDownScreen())
      {
        DropActiveCoroutine();

        _angle = Vector3.zero;
        _isDrag = true;
        _isAdditionalTurn = false;
      }

      if (_inputService.IsFingerUpScreen())
      {
        _coordX = Input.GetAxis("Mouse X") * RotationSpeed * Time.fixedDeltaTime * _disablerX;
        _coordY = Input.GetAxis("Mouse Y") * RotationSpeed * Time.fixedDeltaTime * _disablerY;

        _lastRotateAngle = RotateAngle;
        _isDrag = false;

        if (Mathf.Abs(_coordX) > Mathf.Abs(_coordY))
        {
          _angle = new Vector3(0, _coordX, 0);
        }
        else if (Mathf.Abs(_coordX) < Mathf.Abs(_coordY))
        {
          _angle = _isInversionRotation == false ? new Vector3(-_coordY, 0, 0) : new Vector3(_coordY, 0, 0);
        }

        if (_angle.magnitude > MinAngleMagnitude)
        {
          _angleMagnitude = Mathf.Clamp(_angle.magnitude, MinAngleMagnitude, MaxAngleMagnitude);
          _coroutine = ReductionCoord(_lastRotateAngle, _angleMagnitude);
          StartCoroutine(_coroutine);
        }
        else
        {
          _isAdditionalTurn = true;
        }

        Dragged?.Invoke();
      }
    }

    private void LateUpdate()
    {
      if (_isDrag)
        LookRotate();
      else if (_isAdditionalTurn)
        LookRotate();
    }

    private void LookRotate()
    {
      Vector3 lookVector = RotateLookPoint.transform.position - transform.position;
      
      if (lookVector != Vector3.zero)
      {
        Quaternion targetRotation = Quaternion.LookRotation(lookVector);
        transform.rotation =
          Quaternion.RotateTowards(transform.rotation, targetRotation, AdditionalTurnSpeed * Time.deltaTime);
      }
    }


    private void OnTookOff()
    {
      RotateEnabled?.Invoke();
      _isDrag = true;
    }

    private void OnLanded()
    {
      DropActiveCoroutine();
      RotateDisabled?.Invoke();
      _isAdditionalTurn = false;
      _isDrag = false;
      _coordX = 0;
      _coordY = 0;
    }

    private IEnumerator ReductionCoord(float angle, float magnitude)
    {
      while (angle > 0)
      {
        angle -= ReductionRate / magnitude * Time.deltaTime;

        if (_isOnlyTwoRotateAxis == false)
          transform.RotateAround(transform.position, _angle.normalized, angle * Time.deltaTime);
        else
          transform.Rotate(_angle.normalized, angle * Time.deltaTime);

        yield return null;
      }

      _angle = Vector3.zero;
      _isAdditionalTurn = true;
      _coroutine = null;
    }

    private void DropActiveCoroutine()
    {
      if (_coroutine != null)
      {
        StopCoroutine(_coroutine);
        _coroutine = null;
      }
    }
  }
}