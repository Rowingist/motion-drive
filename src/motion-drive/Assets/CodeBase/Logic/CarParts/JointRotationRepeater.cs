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

    public void Construct(HeroCarOnGroundChecker groundChecker)
    {
      _groundChecker = groundChecker;

      _groundChecker.TookOff += MakeProperRotation;
    }

    private void Update()
    {
      if (IsNeedToCheckGround)
        if (!(_groundChecker && _groundChecker.IsOnGround))
          return;

          transform.rotation = NewRotation();
    }

    private void OnDestroy() => 
      _groundChecker.TookOff -= MakeProperRotation;

    private void MakeProperRotation()
    {
      if (IsNeedToCheckGround)
        transform.DORotate(Vector3.zero, .2f);
    }

    private Quaternion NewRotation() =>
      Quaternion.Lerp(transform.rotation, Rotator.rotation, Time.deltaTime * Speed);
  }
}