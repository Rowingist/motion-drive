using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  public class HeroCarAirTricksCounter : MonoBehaviour
  {
    [SerializeField] private List<HeroCarRotationStepReader> _rotationReaders;
    [SerializeField] private string _firstTrickName;
    [SerializeField] private string _secondTrickName;
    [SerializeField] private int _minReaches;
    [SerializeField] private HeroCarRotationStepReader _firstReader;
    [SerializeField] private HeroCarOnGroundChecker _groundCheck;
    [SerializeField] private HeroCarCrashChecker _crashChecker;

    [SerializeField] private TriggerObserver _triggerObserver;

    private bool _isCounting;

    public event Action<string> TrickCompleted;

    public int CompletedFlipsCached { get; set; }

    private void Start()
    {
      _triggerObserver.TriggerExit += TriggerExit;
      _groundCheck.TookOff += Activate;
      _groundCheck.LandedOnGround += Deactivate;
      _crashChecker.Crashed += ClearUp;
    }

    private void OnDestroy()
    {
      _triggerObserver.TriggerExit -= TriggerExit;
      _groundCheck.TookOff -= Activate;
      _groundCheck.LandedOnGround -= Deactivate;
      _crashChecker.Crashed -= ClearUp;
    }

    private void Activate() =>
      _isCounting = true;

    private void Deactivate()
    {
      _isCounting = false;

      CompletedFlipsCached = 0;
      
      if (_rotationReaders.Count > 0)
        ClearUp();
    }

    private void ClearUp() =>
      _rotationReaders.Clear();

    private void TriggerExit(Collider obj)
    {
      if (!_isCounting)
        return;

      if (!obj.TryGetComponent(out HeroCarRotationStepReader rotationReader)) 
        return;
      
      if (rotationReader == _firstReader)
      {
        ClearUp();
          
        StepPassed(rotationReader);
        return;
      }

      if (IsInterruptedByReverseRotation(rotationReader))
        return;

      StepPassed(rotationReader);
      UpdateCounter();
    }

    private bool IsInterruptedByReverseRotation(HeroCarRotationStepReader rotationReader) => 
      _rotationReaders.Any(trigger => rotationReader == trigger);

    private void StepPassed(HeroCarRotationStepReader rotationReader) => 
      _rotationReaders.Add(rotationReader);

    private void UpdateCounter()
    {
      if(_rotationReaders.Count != _minReaches)
        return;
      
      TrickCompleted?.Invoke(RotatedClockwise() ? _firstTrickName : _secondTrickName);
      
      ClearUp();
      CompletedFlipsCached++;
    }

    private bool RotatedClockwise() => 
      _rotationReaders[1].RotateDirection == RotateDirection.Clockwise;
  }
}