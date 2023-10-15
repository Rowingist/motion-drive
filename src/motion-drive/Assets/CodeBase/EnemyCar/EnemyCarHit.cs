using System;
using System.Linq;
using CodeBase.HeroCar;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  [RequireComponent(typeof(EnemyCarDeath))]
  public class EnemyCarHit : MonoBehaviour
  {
    public float Cleavage = 0.5f;
    public float Power = 40f;

    public TriggerObserver TriggerObserver;

    private Collider[] _hits = new Collider[1];
    private int _layerMask;

    private EnemyCarDeath _enemyCarDeath;

    private void Awake()
    {
      _layerMask = 1 << LayerMask.NameToLayer("Following");

      _enemyCarDeath = GetComponent<EnemyCarDeath>();
    }

    private Collider _player;

    private void OnEnable()
    {
      _player = null;
      TriggerObserver.TriggerEnter += TryDead;
    }

    private void TryDead(Collider obj)
    {
      if (TryGetHit(out _player))
      {
        if (_enemyCarDeath)
          _enemyCarDeath.BeginDeath(_player.ClosestPoint(transform.position), _player.transform.position);
        
          AddPushForceAfterHit(Vector3.forward);

        LevelRaceStatistics statistics = _player.GetComponent<LevelRaceStatistics>();
        
        if(statistics) 
          statistics.CollectKill();

        CleanUp();
        enabled = false;
      }
    }

    private void CleanUp() => 
      TriggerObserver.TriggerEnter -= TryDead;

    private bool PlayerOnLeft() => 
      _player.transform.position.x - transform.position.x <= 0;

    private void AddPushForceAfterHit(Vector3 direction)
    {
      _player.GetComponent<Rigidbody>().AddForce( direction * Power / 2f,
        ForceMode.VelocityChange);
    }

    private bool TryGetHit(out Collider hit)
    {
      var hitAmount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
      hit = _hits.FirstOrDefault();
      return hitAmount > 0;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = new Color(255, 255, 255, .7f);
      Gizmos.DrawSphere(StartPoint(), Cleavage);
    }

    private Vector3 StartPoint() => 
      new(transform.position.x, transform.position.y + 0.5f, transform.position.z);
  }
}