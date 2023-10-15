using System;
using CodeBase.Car;
using UnityEngine;

namespace CodeBase.HeroCar.Effects
{
  public abstract class InAirTraceDisabler : MonoBehaviour
  {
    public ParticleSystem[] TraceEffects;
    public CarOnGroundChecker GroundChecker;

    private void Awake() => 
      StopParticleEmission();

    public virtual void Start()
    {
      GroundChecker.TookOff += StopParticleEmission;
      GroundChecker.LandedOnGround += StartParticleEmission;
    }

    public virtual void OnDestroy()
    {
      GroundChecker.TookOff += StopParticleEmission;
      GroundChecker.LandedOnGround += StartParticleEmission;
    }


    protected void StartParticleEmission()
    {
      foreach (ParticleSystem traceEffect in TraceEffects) 
        traceEffect.Play();
    }

    protected void StopParticleEmission()
    {
      foreach (ParticleSystem traceEffect in TraceEffects) 
        traceEffect.Stop();
    }
  }
}