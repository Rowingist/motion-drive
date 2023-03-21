using System;
using CodeBase.Logic;
using CodeBase.Logic.Obstacles;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarCrashChecker : MonoBehaviour
  {
    public TriggerObserver TriggerObserver;
    public HeroCarLandingEvaluator LandingEvaluator;

    public event Action Crashed;
    
    private void Start() => 
      TriggerObserver.TriggerEnter += TriggerEnter;

    private void OnDestroy() => 
      TriggerObserver.TriggerEnter -= TriggerEnter;

    private void TriggerEnter(Collider obj)
    {
      if (CarOnGroundCollideWithStaticObstacle(obj) || CollideWithGround(obj)) 
        Crashed?.Invoke();
    }

    private bool CarOnGroundCollideWithStaticObstacle(Collider obj) =>
      obj.TryGetComponent(out Obstacle obstacle) && obstacle.Type == ObstacleType.Static;

    private bool CollideWithGround(Collider obj) => 
      obj.GetComponent<Ground>() && !LandingEvaluator.IsLandedProperlyNoCrash;
  }
}