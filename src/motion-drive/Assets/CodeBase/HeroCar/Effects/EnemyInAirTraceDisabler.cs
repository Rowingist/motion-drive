using CodeBase.EnemyCar;

namespace CodeBase.HeroCar.Effects
{
  class EnemyInAirTraceDisabler : InAirTraceDisabler
  {
    public EnemyRespawn Respawn;
    public EnemyCarDeath CarDeath;

    public override void Start()
    {
      base.Start();
      Respawn.Completed += StartParticleEmission;
      CarDeath.Dead += StopParticleEmission;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      Respawn.Completed -= StartParticleEmission;
      CarDeath.Dead -= StopParticleEmission;
    }
  }
}