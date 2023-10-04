using System;
using CodeBase.Car;
using CodeBase.FollowingTarget;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class PlayerCarSwipeRotationInAir : MonoBehaviour
  {
    public float RotationDuration = 1f;

    private SwipeListener _swipeListener;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotateTweener;
    
    private CarOnGroundChecker _groundChecker;
    private HeroFollowingTarget _heroFollowingTarget;

    public float Power = 1f;

    private Vector3 _downForwardDirection = Vector3.forward + Vector3.down;
    
    public event Action Flipped; 

    public void Construct(CarOnGroundChecker groundChecker, SwipeListener swipeListener, HeroFollowingTarget heroFollowingTarget)
    {
      _groundChecker = groundChecker;
      _heroFollowingTarget = heroFollowingTarget;

      InitSwipeListener(swipeListener);
      SubscribeOnSwipes();
    }

    private void InitSwipeListener(SwipeListener swipeListener) =>
      _swipeListener = swipeListener;

    private void SubscribeOnSwipes() => 
      _swipeListener.OnSwipe.AddListener(OnSwipe);

    private void OnDisable() => 
      _swipeListener.OnSwipe.RemoveListener(OnSwipe);

    private void OnSwipe(string swipe)
    {
      if (_groundChecker.IsOnGround)
        return;
      
      if (_rotateTweener.IsActive())
        return;

      switch (swipe)
      {
        case "Up":
          _rotateTweener = Rotate360ByEuler(Constants.EulerAngleX360);
          AffectFightTrajectory(_downForwardDirection);
          break;
        case "Down":
          _rotateTweener = Rotate360ByEuler(Constants.EulerAngleMinusX360);
          AffectFightTrajectory(_downForwardDirection);
          break;
        case "Right":
          _rotateTweener = Rotate360ByEuler(Constants.EulerAngleMinusY360);
          AffectFightTrajectory(Vector3.right);
          break;
        case "Left":
          _rotateTweener = Rotate360ByEuler(Constants.EulerAngleY360);
          AffectFightTrajectory(Vector3.left);
          break;
      }
      
      Flipped?.Invoke();
    }

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> Rotate360ByEuler(Vector3 eulerAngle) =>
      transform.DORotate(eulerAngle, RotationDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutCubic);

    private void AffectFightTrajectory(Vector3 direction) => 
      _heroFollowingTarget.AddDirectionalForceInAir(direction * Power);
  }
}