using System;
using CodeBase.StaticData.EnemiesSpeed;
using UnityEngine;

namespace CodeBase.Logic.CheckPoint
{
  public class CheckPoint : MonoBehaviour
  {
    private const string Player = "Player";

    public TriggerObserver TriggerObserver;
    public RaycasterToGround RaycasterToGround;
    public EnemiesSpeedMixerConfig EnemiesSpeedMixerConfig;
    
    private bool _isReachedByPlayer;

    public event Action<Vector3, Quaternion> Reached;
    public event Action<CheckPoint> ReachedPoint;

    private void Start() =>
      TriggerObserver.TriggerEnter += TriggerEnter;

    private void OnDestroy() =>
      TriggerObserver.TriggerEnter -= TriggerEnter;

    private void TriggerEnter(Collider other)
    {
      if (!other.TryGetComponent(out TriggerObserver triggerObserver)) return;
      if (_isReachedByPlayer) return;

      if (triggerObserver.tag == Player)
      {
        Reached?.Invoke(RaycasterToGround.PointOnGround(other.transform.position).Item1,
          RaycasterToGround.PointOnGround(other.transform.position).Item2);

        ReachedPoint?.Invoke(this);
      }

      DisableObject();
    }

    private void DisableObject() =>
      _isReachedByPlayer = true;
  }
}