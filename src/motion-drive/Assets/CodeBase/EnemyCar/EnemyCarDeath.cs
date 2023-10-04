using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarDeath : MonoBehaviour
  {
    public ParticleSystem BurnEffect;
    public ParticleSystem DieEffect;

    public GameObject View;
    
    public Rigidbody AffectedRigidBody;
    public EnemyCarFollowingTarget FollowingTarget;

    public float InFlameDuration;
    public float PushPower;

    private void Start()
    {
      BurnEffect.gameObject.SetActive(false);
      DieEffect.gameObject.SetActive(false);
    }

    [ContextMenu("Play")]
    public void BeginDeath()
    {
      FollowingTarget.enabled = false;
      
      BurnEffect.gameObject.SetActive(true);
      AffectedRigidBody.AddForce(Vector3.forward * PushPower, ForceMode.VelocityChange);
      
      
      Invoke(nameof(Die), InFlameDuration);
    }

    public void Die()
    {
      BurnEffect.gameObject.SetActive(false);
      View.SetActive(false);
      DieEffect.gameObject.SetActive(true);
      enabled = false;
    }
  }
}