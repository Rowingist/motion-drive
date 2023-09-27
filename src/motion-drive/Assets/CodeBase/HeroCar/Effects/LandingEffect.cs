using CodeBase.HeroCar.TricksInAir;
using UnityEngine;

namespace CodeBase.HeroCar.Effects
{
  public class LandingEffect : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _landEffect;
    [SerializeField] private int _gainValue;
    
    private HeroCarOnGroundChecker _groundChecker;
    private HeroCarRespawn _respawn;
    private HeroCarAirTricksCounter _tricksCounter;

    public void Construct(HeroCarOnGroundChecker groundChecker, HeroCarRespawn respawn,
      HeroCarAirTricksCounter tricksCounter)
    {
      _groundChecker = groundChecker;
      _respawn = respawn;
      _tricksCounter = tricksCounter;
      
      Subscribe();
    }

    private void OnDestroy() => 
      _groundChecker.LandedOnGround += OnLanded;

    private void Subscribe() => 
      _groundChecker.LandedOnGround += OnLanded;

    [ContextMenu("paly")]
    public void OnLanded()
    {
      if(_respawn.IsRespawning)
        return;

      if (EnoughPowerToPlay()) 
        _landEffect.gameObject.SetActive(true);
    }

    private bool EnoughPowerToPlay() => 
      _tricksCounter.CompletedFlips >= _gainValue;
  }
}