using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarSwipeRotationInAir : MonoBehaviour
  {
    public float RotationDuration = 1f;

    private SwipeListener _swipeListener;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotate;
    private HeroCarOnGroundChecker _groundChecker;
    private HeroFollowingTarget.HeroFollowingTarget _heroFollowingTarget;

    public float DirectionForce = 1f;
    
    public event Action Flipped; 

    public void Construct(HeroCarOnGroundChecker groundChecker, SwipeListener swipeListener, HeroFollowingTarget.HeroFollowingTarget heroFollowingTarget)
    {
      _groundChecker = groundChecker;
      InitSwipeListener(swipeListener);
      _swipeListener.OnSwipe.AddListener(OnSwipe);
      _heroFollowingTarget = heroFollowingTarget;
    }

    private void InitSwipeListener(SwipeListener swipeListener)
    {
      _swipeListener = swipeListener;
    }

    private void OnDisable()
    {
      _swipeListener.OnSwipe.RemoveListener(OnSwipe);
    }
    
    private void OnSwipe(string swipe)
    {
      if (_groundChecker.IsOnGround)
        return;
      
      if (_rotate.IsActive())
        return;

      switch (swipe)
      {
        case "Up":
          _rotate = transform.DORotate(new Vector3(360f, 0f, 0f), RotationDuration,
            RotateMode.LocalAxisAdd).SetEase(DG.Tweening.Ease.InOutCubic);
          _heroFollowingTarget.AddDirectionalForceInAir((Vector3.forward + Vector3.down) * DirectionForce);
          break;
        case "Down":
          _rotate = transform.DORotate(new Vector3(-360f, 0f, 0f), RotationDuration,
            RotateMode.LocalAxisAdd);
          _heroFollowingTarget.AddDirectionalForceInAir((Vector3.back + Vector3.down)  * DirectionForce);

          break;
        case "Right":
          _rotate = transform.DORotate(new Vector3(0f, -360f, 0f), RotationDuration,
            RotateMode.LocalAxisAdd);
          _heroFollowingTarget.AddDirectionalForceInAir(Vector3.right * DirectionForce);
          break;
        case "Left":
          _rotate = transform.DORotate(new Vector3(0f, 360f, 0f), RotationDuration,
            RotateMode.LocalAxisAdd);
          _heroFollowingTarget.AddDirectionalForceInAir(Vector3.left * DirectionForce);
          break;
      }
      
      Flipped?.Invoke();
    }
  }
}