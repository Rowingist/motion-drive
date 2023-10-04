using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarDeath : MonoBehaviour
  {
    public ParticleSystem BurnEffect;
    public ParticleSystem DieEffect;

    public GameObject View;
    
    private Rigidbody _affectedRigidBody;

    public float InFlameDuration;
    public float PushPower;
    
    private EnemyFollowingTarget _followingTarget;

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

    public void BeginDeath()
    {
      if(!_followingTarget || !_affectedRigidBody)
        return;
      
      _followingTarget.enabled = false;
      
      BurnEffect.gameObject.SetActive(true);
      
      _affectedRigidBody.AddForce(Vector3.forward * PushPower, ForceMode.VelocityChange);

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