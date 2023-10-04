using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class Hit : MonoBehaviour
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

    private Collider player;

    private void Start() =>
      TriggerObserver.TriggerEnter += TryDead;

    private void TryDead(Collider obj)
    {
      if (TryGetHit(out player))
      {
        if (_enemyCarDeath)
          _enemyCarDeath.BeginDeath();

        if (PlayerOnLeft())
          AddPushForceAfterHit(-player.transform.right);
        else
          AddPushForceAfterHit(player.transform.right);

        TriggerObserver.TriggerEnter -= TryDead;
        enabled = false;
      }
    }

    private bool PlayerOnLeft() => 
      player.transform.position.x - transform.position.x <= 0;

    private void AddPushForceAfterHit(Vector3 direction)
    {
      player.GetComponent<Rigidbody>().AddForce( direction * Power,
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