using UnityEngine;

namespace CodeBase.HeroCar.Effects
{
  public class PlayerInAirTraceDisabler : InAirTraceDisabler
  {
    [SerializeField] private PlayerCarCrashChecker _carCrash;
    [SerializeField] private PlayerCarRespawn _carRespawn;
    
    public override void Start()
    {
      base.Start();
      _carRespawn.Completed += StartParticleEmission;
      _carCrash.Crashed += StopParticleEmission;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      _carRespawn.Completed -= StartParticleEmission;
      _carCrash.Crashed -= StopParticleEmission;
    }
  }
}