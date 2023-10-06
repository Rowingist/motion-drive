using System;
using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  [RequireComponent(typeof(EnemyCarDeath), typeof(EnemyRespawn))]
  public class EnemyCarBlinker : CarBlinker
  {
    private EnemyCarDeath _carDeath;
    private EnemyRespawn _enemyRespawn;

    private void Awake()
    {
      _carDeath = GetComponent<EnemyCarDeath>();
      _enemyRespawn = GetComponent<EnemyRespawn>();
    }

    private void Start()
    {
      _carDeath.Dead += DisableParts;
      _enemyRespawn.Started += Blink;
    }

    private void OnDestroy()
    {
      _carDeath.Dead -= DisableParts;
      _enemyRespawn.Started -= Blink;
    }

    protected override void Blink() => 
      StartCoroutine(Blinking());
  }
}