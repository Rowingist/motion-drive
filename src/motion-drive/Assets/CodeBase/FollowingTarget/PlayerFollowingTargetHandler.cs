using System.Collections;
using CodeBase.HeroCar.TricksInAir;
using UnityEngine;

namespace CodeBase.FollowingTarget
{
  public class PlayerFollowingTargetHandler : MonoBehaviour
  {
    public PlayerFollowingTarget FollowingTarget;
    
    public float MaxBoostSpeed;
    public float DefaultSpeed;
    public float MaxBoosAcceleration;
    public float DefaultAcceleration;
    
    private BoostEffectAfterLanding _boostEffect;
    private Coroutine _boostedMove;
    
    public void Construct(BoostEffectAfterLanding boostEffect)
    {
      _boostEffect = boostEffect;
      _boostEffect.Started += SetMoveSettings;

      SetDefaults();
    }

    private void OnDestroy()
    {
      _boostEffect.Started -= SetMoveSettings;
    }

    private void SetMoveSettings()
    {
      StopActiveCoroutine();
      
      _boostedMove = StartCoroutine(MoveWithAdditionalPower());
      //FollowingTarget.GetComponent<Rigidbody>().AddForce(FollowingTarget.transform.forward * 15, ForceMode.VelocityChange);
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

    private void SetDefaults()
    {
      DefaultSpeed = FollowingTarget.MaxSpeed;
      DefaultAcceleration = FollowingTarget.MaxAcceleration;
    }

    private void ResetToDefaults() => 
      MoveWith(DefaultSpeed, DefaultAcceleration);

    private void MoveWith(float speed, float acceleration)
    {
      FollowingTarget.MaxSpeed = speed;
      FollowingTarget.MaxAcceleration = acceleration;
    }
  }
}