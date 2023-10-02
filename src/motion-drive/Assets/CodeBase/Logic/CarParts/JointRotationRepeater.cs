using System;
using CodeBase.HeroCar;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class JointRotationRepeater : MonoBehaviour
  {
    public Transform Rotator;
    public float Speed;
    private HeroCarOnGroundChecker _groundChecker;

    public bool IsNeedToCheckGround;
    private bool _isLookingStraight;

    public void Construct(HeroCarOnGroundChecker groundChecker)
    {
      _groundChecker = groundChecker;

      _groundChecker.LandedOnGround += OnStopLookStraight;
    }

    private void Update()
    {
      if (IsNeedToCheckGround)
      {
        if (_groundChecker && !_groundChecker.IsOnGround)
        {
          if (!_isLookingStraight)
          {
            MakeProperRotation();
         
            return;
          }

          return;
        }
      }

      transform.rotation = NewRotation();
    }

    private void OnDestroy() =>
      _groundChecker.LandedOnGround -= OnStopLookStraight;

    private void OnStopLookStraight() => 
      _isLookingStraight = false;

    private void MakeProperRotation()
    {
      transform.localRotation = quaternion.identity;
      _isLookingStraight = true;
    }

    private Quaternion NewRotation() =>
      Quaternion.Lerp(transform.rotation, Rotator.rotation, Time.deltaTime * Speed);
  }
}