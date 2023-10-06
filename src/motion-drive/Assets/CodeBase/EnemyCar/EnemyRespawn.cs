using System;
using CodeBase.Car;
using CodeBase.Logic.Bezier;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  [RequireComponent(typeof(EnemyCarDeath))]
  [RequireComponent(typeof(EnemyRespawnPositionUpdater))]
  [RequireComponent(typeof(EnemyCarHit))]
  public class EnemyRespawn : MonoBehaviour
  {
    private SplineWalker _splineWalker;
    private EnemyFollowingTarget _enemyFollowingTarget;
    
    private EnemyCarDeath _carDeath;
    private EnemyCarHit _carEnemyCarHit;
    private EnemyCarBlinker _carBlinker;

    public float DeathDuration = 3f;
    private EnemyRespawnPositionUpdater _respawnPositionUpdater;
    private CarJoints _carJoints;

    public event Action Started;
    
    public void Construct(SplineWalker splineWalker, EnemyFollowingTarget enemyFollowingTarget, CarJoints carJoints)
    {
      _splineWalker = splineWalker;
      _enemyFollowingTarget = enemyFollowingTarget;
      _carJoints = carJoints;
    }
    
    private void Awake()
    {
      _carDeath = GetComponent<EnemyCarDeath>();
      _respawnPositionUpdater = GetComponent<EnemyRespawnPositionUpdater>();
      _carEnemyCarHit = GetComponent<EnemyCarHit>();
    }

    private void Start()
    {
      _carDeath.Dead += Respawn;
    }
    private void OnDestroy()
    {
      _carDeath.Dead -= Respawn;
    }

    private void Respawn()
    {
      Invoke(nameof(StartRespawn), DeathDuration);
    }

    private void StartRespawn()
    {
      _splineWalker.StopMovement();
      _splineWalker.BackToCachedProgress();
      _enemyFollowingTarget.Rigidbody.position = _respawnPositionUpdater.CurrentCheckpoint;
      transform.position = _respawnPositionUpdater.CurrentCheckpoint;
      _carJoints.StopJointsOnRespawn();
      Invoke(nameof(StartMovement), 1f);
    }

    private void StartMovement()
    {
      Started?.Invoke();
      
      _carDeath.enabled = true;
      _carEnemyCarHit.enabled = true;
      _splineWalker.StartMovement();
      _enemyFollowingTarget.enabled = true;
    }
  }
}