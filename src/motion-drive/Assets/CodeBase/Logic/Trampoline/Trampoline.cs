using System.Collections;
using CodeBase.EnemyCar;
using CodeBase.FollowingTarget;
using CodeBase.HeroCar;
using CodeBase.Logic.CarParts;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Logic.Trampoline
{
  public class Trampoline : MonoBehaviour
  {
    public Transform TakeOffPoint;
    public Transform LandingPoint;

    public float TakeOffAngle;

    public TriggerObserver TriggerObserver;

    public bool GuaranteedTakeOff;
    public bool IsForTricks;

    private readonly float _gravity = Physics.gravity.y;
    private float _defaultTakeOffPower;
    private float _lastTakeOffHorizontalForce;

    private void OnValidate() =>
      AdjustTakeOffPointRotation(TakeOffAngle);

    private void Start()
    {
      AdjustTakeOffPointRotation(TakeOffAngle);
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      _defaultTakeOffPower = ComputeTakeOffPower();
    }

    private void OnDestroy()
    {
      TriggerObserver.TriggerEnter -= TriggerEnter;
      TriggerObserver.TriggerExit -= TriggerExit;
    }


    public void AdjustTakeOffPointRotation(float value)
    {
      TakeOffAngle = value;
      TakeOffPoint.rotation = Quaternion.Euler(-TakeOffAngle, 0f, 0f);
      _defaultTakeOffPower = ComputeTakeOffPower();
    }

    private float ComputeTakeOffPower()
    {
      Vector3 fromTo = LandingPoint.position - TakeOffPoint.position;
      Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

      float x = fromToXZ.magnitude;
      float y = fromTo.y;

      float angleInRadians = TakeOffAngle * Mathf.PI / 180;

      float v2 = (_gravity * x * x) /
                 (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
      return Mathf.Sqrt(Mathf.Abs(v2));
    }

    private void TriggerEnter(Collider obj)
    {
      if (obj.TryGetComponent(out TriggerObserver heroCar) && ObjectLayerIsPlayer(heroCar))
      {
        _lastTakeOffHorizontalForce = heroCar.transform.forward.x;
        heroCar.transform.parent.transform.DORotate(Vector3.zero, 1);
      }
    }

    private static bool ObjectLayerIsPlayer(TriggerObserver heroCar) =>
      heroCar.gameObject.layer == LayerMask.NameToLayer(Constants.PlayerLayer);

    private void TriggerExit(Collider obj)
    {
      if (obj.TryGetComponent(out HeroFollowingTarget heroFollowingTarget))
        TakeOff(heroFollowingTarget);

      if (obj.TryGetComponent(out EnemyCarFollowingTarget enemyCarFollowingTarget)) 
        TakeOff(enemyCarFollowingTarget);
    }

    private void TakeOff(HeroFollowingTarget heroFollowingTarget)
    {
      heroFollowingTarget.DisableSnapping();
      StartCoroutine(EnablingSnapping(heroFollowingTarget));
      Rigidbody targetBody = heroFollowingTarget.GetComponent<Rigidbody>();
      Vector3 targetForward = new Vector3(_lastTakeOffHorizontalForce, TakeOffPoint.forward.y, TakeOffPoint.forward.z);
      targetBody.velocity = targetForward * _defaultTakeOffPower;
    }

    private void TakeOff(EnemyCarFollowingTarget enemyCarFollowingTarget)
    {
      enemyCarFollowingTarget.StopSnapping();
      Rigidbody enemyRigidBody = enemyCarFollowingTarget.GetComponent<Rigidbody>();
      Vector3 targetForward = new Vector3(_lastTakeOffHorizontalForce, TakeOffPoint.forward.y, TakeOffPoint.forward.z);
      enemyRigidBody.velocity = targetForward * _defaultTakeOffPower;
    }

    private IEnumerator EnablingSnapping(HeroFollowingTarget heroFollowingTarget)
    {
      yield return new WaitForSecondsRealtime(1f);
      heroFollowingTarget.EnableSnapping();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawRay(TakeOffPoint.position, TakeOffPoint.forward * 5);
    }
  }
}