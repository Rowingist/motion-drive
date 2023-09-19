using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRotateLookPoint : MonoBehaviour
  {
    [SerializeField] private float _zPositionOffset;
    [SerializeField] private float _deflectionAngle;

    private HeroCarRotationInAir _rotateAxis;
    private Joystick _joystick;

    public bool _isRotateEnabled;
    private bool _isInversionRotation;

    public void SetInversionRotation(bool value) =>
      _isInversionRotation = value;

    public void Construct(HeroCarRotationInAir rotationInAir, Joystick joystick)
    {
      _rotateAxis = rotationInAir;
      _joystick = joystick;

      _rotateAxis.Dragged += OnDragged;
      _rotateAxis.RotateEnabled += OnRotateEnabled;
      _rotateAxis.RotateDisabled += OnRotateDisabled;
    }

    private Vector3 _positionJoystick => new Vector3(
      _rotateAxis.transform.position.x + _joystick.Horizontal * _deflectionAngle,
      _rotateAxis.transform.position.y + _joystick.Vertical * _deflectionAngle,
      _rotateAxis.transform.position.z + _zPositionOffset
    );

    private Vector3 _positionJoystickInverse => new Vector3(
      _rotateAxis.transform.position.x + _joystick.Horizontal * _deflectionAngle,
      _rotateAxis.transform.position.y - _joystick.Vertical * _deflectionAngle,
      _rotateAxis.transform.position.z + _zPositionOffset
    );

    private Vector3 _rotateAxisPositionOffset => new Vector3(
      _rotateAxis.transform.position.x,
      _rotateAxis.transform.position.y,
      _rotateAxis.transform.position.z + _zPositionOffset
    );

    private void OnDestroy()
    {
      _rotateAxis.Dragged -= OnDragged;
      _rotateAxis.RotateEnabled -= OnRotateEnabled;
      _rotateAxis.RotateDisabled -= OnRotateDisabled;
    }

    private void Update()
    {
      if (_isRotateEnabled == false)
        return;

      transform.position = _isInversionRotation == false ? _positionJoystick : _positionJoystickInverse;
    }

    private void OnDragged() =>
      transform.position = _rotateAxisPositionOffset;

    private void OnRotateEnabled() =>
      _isRotateEnabled = true;

    private void OnRotateDisabled()
    {
      _isRotateEnabled = false;
      transform.position = _rotateAxisPositionOffset;
    }
  }
}