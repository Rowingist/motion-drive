using CodeBase.HeroCar.TricksInAir;
using UnityEngine;

namespace CodeBase.HeroFollowingTarget
{
  public class HeroFollowingTargetHandler : MonoBehaviour
  {
    public HeroFollowingTarget FollowingTarget;
    
    public int _tricksForMaxBoost = 3;
    
    public float MaxBoostSpeed;
    public float DefaultBoostSpeed;
    public float MaxBoosAcceleration;
    public float DefaultBoostAcceleration;
    
    private BoostEffectAfterLanding _boostEffect;

    public void Construct(BoostEffectAfterLanding boostEffect)
    {
      _boostEffect = boostEffect;
      _boostEffect.Boosted += SetMoveSettings;
      _boostEffect.Finished += ResetToDefaults;
    }
    
    private void OnDestroy()
    {
      _boostEffect.Boosted -= SetMoveSettings;
      _boostEffect.Finished -= ResetToDefaults;
    }

    public void ResetToDefaults() => 
      MoveWith(DefaultBoostSpeed, DefaultBoostAcceleration);

    private void SetMoveSettings(int powerOf)
    {
      if (powerOf > 0)
      {
        if (powerOf >= _tricksForMaxBoost)
          MoveWith(MaxBoostSpeed, MaxBoosAcceleration);
      }
    }

    private void MoveWith(float speed, float acceleration)
    {
      FollowingTarget.MaxSpeed = speed;
      FollowingTarget.MaxAcceleration = acceleration;
    }
  }
}