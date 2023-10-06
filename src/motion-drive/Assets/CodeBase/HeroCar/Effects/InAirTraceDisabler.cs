using System;
using CodeBase.Car;
using UnityEngine;

namespace CodeBase.HeroCar.Effects
{
  public class InAirTraceDisabler : MonoBehaviour
  {
    [SerializeField] private ParticleSystem[] _traceEffects;
    [SerializeField] private CarOnGroundChecker _groundChecker;
    [SerializeField] private PlayerCarCrashChecker _carCrash;
    [SerializeField] private PlayerCarRespawn _carRespawn;

    private void Awake() => 
      StopParticleEmission();

    private void Start()
    {
      _carRespawn.Completed += StartParticleEmission;
      _carCrash.Crashed += StopParticleEmission;
      _groundChecker.TookOff += StopParticleEmission;
      _groundChecker.LandedOnGround += StartParticleEmission;
    }

    private void OnDestroy()
    {
      _carRespawn.Completed += StartParticleEmission;
      _carCrash.Crashed += StopParticleEmission;
      _groundChecker.TookOff += StopParticleEmission;
      _groundChecker.LandedOnGround += StartParticleEmission;
    }

    private void StartParticleEmission()
    {
      foreach (ParticleSystem traceEffect in _traceEffects) 
        traceEffect.Play();
    }

    private void StopParticleEmission()
    {
      foreach (ParticleSystem traceEffect in _traceEffects) 
        traceEffect.Stop();
    }
  }
}