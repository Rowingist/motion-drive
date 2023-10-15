using System;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarDeath : MonoBehaviour
  {
    public ParticleSystem BurnEffect;
    public ParticleSystem DieEffect;
    public ParticleSystem SparksEffect;

    private Rigidbody _affectedRigidBody;

    public float InFlameDuration;
    public float PushPower;

    private EnemyFollowingTarget _followingTarget;

    public event Action Dead;

    public void Construct(Rigidbody affectedRigidBody)
    {
      _affectedRigidBody = affectedRigidBody;
      _followingTarget = _affectedRigidBody.GetComponent<EnemyFollowingTarget>();
    }

    private void Start()
    {
      BurnEffect.gameObject.SetActive(false);
      DieEffect.gameObject.SetActive(false);
    }

    public void BeginDeath(Vector3 hitPoint, Vector3 hitterPosition)
    {
      if (!_followingTarget || !_affectedRigidBody)
        return;

      _followingTarget.enabled = false;

      BurnEffect.gameObject.SetActive(true);

      SparksEffect.transform.position = hitPoint;
      SparksEffect.gameObject.SetActive(true);

      AddForceAside(hitPoint - hitterPosition + (Vector3.forward + Vector3.down) * 5);


      Invoke(nameof(Die), InFlameDuration);
    }

    private void AddForceAside(Vector3 direction)
    {
      _affectedRigidBody.AddForce(direction * PushPower, ForceMode.VelocityChange);
    }

    public void Die()
    {
      SparksEffect.gameObject.SetActive(false);
      BurnEffect.gameObject.SetActive(false);
      DieEffect.gameObject.SetActive(true);

      enabled = false;

      Dead?.Invoke();
    }
  }
}