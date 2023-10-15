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
        TriggerObserver.TriggerExit += LookStraight;
      TriggerObserver.TriggerExit += TriggerExit;
      _defaultTakeOffPower = ComputeTakeOffPower();
    }

    private void OnDestroy()
    {
      TriggerObserver.TriggerExit -= LookStraight;
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

    private void LookStraight(Collider obj)
    {
      if (obj.TryGetComponent(out TriggerObserver heroCar) && ObjectLayerIsPlayer(heroCar))
      {
        //_lastTakeOffHorizontalForce = heroCar.transform.forward.x;
        heroCar.transform.parent.transform.DORotate(Vector3.zero, 0.25f);
      }
    }

    private static bool ObjectLayerIsPlayer(TriggerObserver heroCar) =>
      heroCar.gameObject.layer == LayerMask.NameToLayer(Constants.PlayerLayer);

    private void TriggerExit(Collider obj)
    {
      if (obj.TryGetComponent(out PlayerFollowingTarget heroFollowingTarget))
        TakeOff(heroFollowingTarget);

      if (obj.TryGetComponent(out EnemyFollowingTarget enemyCarFollowingTarget))
      {
        enemyCarFollowingTarget.SplineWalker.ChangeLastPositionZ(LandingPoint.position.z);
        TakeOff(enemyCarFollowingTarget);
      }    }

    private void TakeOff(PlayerFollowingTarget playerFollowingTarget)
    {
      playerFollowingTarget.DisableSnapping();
      StartCoroutine(EnablingSnapping(playerFollowingTarget));
      Rigidbody targetBody = playerFollowingTarget.GetComponent<Rigidbody>();
      Vector3 targetForward = new Vector3(TakeOffPoint.forward.x, TakeOffPoint.forward.y, TakeOffPoint.forward.z);
      targetBody.velocity = targetForward * _defaultTakeOffPower;
    }

    private void TakeOff(EnemyFollowingTarget enemyFollowingTarget)
    {
      enemyFollowingTarget.StopSnapping();
      Rigidbody enemyRigidBody = enemyFollowingTarget.GetComponent<Rigidbody>();
      Vector3 targetForward = new Vector3(TakeOffPoint.forward.x, TakeOffPoint.forward.y, TakeOffPoint.forward.z);
      enemyRigidBody.velocity = targetForward * _defaultTakeOffPower;
    }

    private IEnumerator EnablingSnapping(PlayerFollowingTarget playerFollowingTarget)
    {
      yield return new WaitForSecondsRealtime(1f);
      playerFollowingTarget.EnableSnapping();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawRay(TakeOffPoint.position, TakeOffPoint.forward * 5);
    }
  }
}