using System.Collections;
using CodeBase.HeroCar.TricksInAir;
using UnityEngine;

namespace CodeBase.FollowingTarget
{
  public class PlayerFollowingTargetHandler : MonoBehaviour
  {
    public PlayerFollowingTarget FollowingTarget;
    
    public float MaxBoostSpeed;
    public float CurrentSpeed;
    
    public float MaxBoosAcceleration;
    public float CurrentAcceleration;
    
    private BoostEffectAfterLanding _boostEffect;
    private Coroutine _boostedMove;
    
    public void Construct(BoostEffectAfterLanding boostEffect)
    {
      _boostEffect = boostEffect;
      _boostEffect.Started += SetMoveSettings;

      SetCurrentParameters();
    }

    private void OnDestroy() => 
      _boostEffect.Started -= SetMoveSettings;

    private void SetCurrentParameters()
    {
      CurrentSpeed = FollowingTarget.MaxSpeed;
      CurrentAcceleration = FollowingTarget.MaxAcceleration;
    }

    private void SetMoveSettings()
    {
      StopActiveCoroutine();
      
      _boostedMove = StartCoroutine(MoveWithAdditionalPower());
    }

    private void StopActiveCoroutine()
    {
      if (_boostedMove != null)
      {
        StopCoroutine(_boostedMove);
        _boostedMove = null;
      }
    }

    private IEnumerator MoveWithAdditionalPower()
    {
      MoveWith(MaxBoostSpeed, MaxBoosAcceleration);
      
      while (_boostEffect.IsBoosting)
        yield return null;

      ResetToDefaults();
    }

    private void ResetToDefaults() => 
      MoveWith(CurrentSpeed, CurrentAcceleration);

    private void MoveWith(float speed, float acceleration)
    {
      FollowingTarget.MaxSpeed = speed;
      FollowingTarget.MaxAcceleration = acceleration;
    }
  }
}