using System.Collections.Generic;
using CodeBase.FollowingTarget;
using CodeBase.Logic.Bezier;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData.EnemiesSpeed;
using UnityEngine;

namespace CodeBase.EnemiesSpeedHandler
{
  public class EnemySpeedMixer : MonoBehaviour
  {
    public float FarOvertakersDistance = 3f;

    private SplineWalker[] _splineWalkers;
    private PlayerFollowingTarget _playerFollowingTarget;

    private EnemiesSpeedMixerConfig _speedConfig;

    private readonly List<SplineWalker> _outsiders = new();
    private readonly List<SplineWalker> _farOvertakers = new();

    private IRandomService _randomService;
    
    public void Construct(IRandomService randomService, SplineWalker[] splineWalkers,
      PlayerFollowingTarget playerFollowingTarget)
    {
      _splineWalkers = splineWalkers;
      _playerFollowingTarget = playerFollowingTarget;
      _randomService = randomService;
    }
    
    public void UpdateConfig(EnemiesSpeedMixerConfig speedConfig)
    {
      _speedConfig = speedConfig;
      
      CheckPlayerLeadingOrLoosingState();
      //SetUpNewConfigs();
    }

    private void CheckPlayerLeadingOrLoosingState()
    {
      foreach (var walker in _splineWalkers)
      {
        if(walker.transform.position.z < _playerFollowingTarget.transform.position.z)
          _outsiders.Add(walker);
        
        if(walker.transform.position.z - _playerFollowingTarget.transform.position.z > FarOvertakersDistance)
          _farOvertakers.Add(walker);
        
      }
    }

    private void SetUpNewConfigs()
    {
      for (int i = 0; i < _splineWalkers.Length; i++)
      {
        if (_outsiders.Contains(_splineWalkers[i]))
        {
          _splineWalkers[i].ChangeDuration(_speedConfig.EnemiesSpeed[i].AdditionalSpeed - _randomService.Next(1, 5));
          continue;
        }

        if (_farOvertakers.Contains(_splineWalkers[i]))
        {
          _splineWalkers[i].ChangeDuration(_speedConfig.EnemiesSpeed[i].AdditionalSpeed + _randomService.Next(5, 10));
          continue;
        }
        
        _splineWalkers[i].ChangeDuration(_speedConfig.EnemiesSpeed[i].AdditionalSpeed);
      }
    }
  }
}