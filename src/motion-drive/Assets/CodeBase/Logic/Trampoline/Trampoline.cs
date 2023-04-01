using System;
using System.Collections;
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

    private void OnValidate() =>
      AdjustTakeOffPointRotation(TakeOffAngle);

    private void Start()
    {
      AdjustTakeOffPointRotation(TakeOffAngle);
      TriggerObserver.TriggerExit += TriggerExit;
      _defaultTakeOffPower = ComputeTakeOffPower();
    }

    private void OnDestroy() => 
      TriggerObserver.TriggerExit -= TriggerExit;

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

    private void TriggerExit(Collider obj)
    {
      if (obj.TryGetComponent(out HeroFollowingTarget.HeroFollowingTarget heroFollowingTarget))
        TakeOff(heroFollowingTarget);
    }

    private void TakeOff(HeroFollowingTarget.HeroFollowingTarget heroFollowingTarget)
    {
      heroFollowingTarget.DisableSnapping();
      StartCoroutine(EnablingSnapping(heroFollowingTarget));
      Rigidbody targetBody = heroFollowingTarget.GetComponent<Rigidbody>();
      targetBody.velocity = Vector3.zero;
      Vector3 targetForward = new Vector3(0f, TakeOffPoint.forward.y, TakeOffPoint.forward.z);
      targetBody.velocity = targetForward * _defaultTakeOffPower;

      // if(_guaranteedTakeOff == true)
      // {
      //   playerCar.SphereControl.SetMaxSpeedToSnap(0);
      //   playerCar.CheckPreset(playerCar.StandartPreset, true);
      // }
      //
      // if (_isSpringboardForTricks == true)
      //   playerCar.SphereControl.SpeedUpAfterTrick();
    }

    private IEnumerator EnablingSnapping(HeroFollowingTarget.HeroFollowingTarget heroFollowingTarget)
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