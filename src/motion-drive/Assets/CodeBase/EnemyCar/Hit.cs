using System;
using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class Hit : MonoBehaviour
  {
    public float Cleavage = 0.5f;

    public TriggerObserver TriggerObserver;

    private Collider[] _hits = new Collider[1];
    private int _layerMask;

    private void Awake()
    {
      _layerMask = 1 << LayerMask.NameToLayer("Following");
    }

    private Collider player = null;

    private void Start() =>
      TriggerObserver.TriggerEnter += TryDead;
    

    private void TryDead(Collider obj)
    {
      if (TryGetHit(out player))
      {
        GetComponent<EnemyCarDeath>().BeginDeath();
        if (player.transform.position.x - transform.position.x <= 0)
        {
          player.GetComponent<Rigidbody>().AddForce(-player.transform.right * 40,
            ForceMode.VelocityChange);
        }
        else
        {
          player.GetComponent<Rigidbody>().AddForce(player.transform.right * 40,
            ForceMode.VelocityChange);
        }

        TriggerObserver.TriggerEnter -= TryDead;
        enabled = false;
      }
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

    private Vector3 StartPoint()
    {
      return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }
  }
}