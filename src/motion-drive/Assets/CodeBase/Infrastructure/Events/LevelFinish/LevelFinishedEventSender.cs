using System;
using CodeBase.FollowingTarget;
using CodeBase.HeroCar;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.Events
{
  public class LevelFinishedEventSender : MonoBehaviour
  {
    public TriggerObserver _triggerObserver;

    private bool _isActivated;

    private void Start() =>
      _triggerObserver.TriggerExit += OnPlayerFinished;

    private void OnDestroy() =>
      _triggerObserver.TriggerExit -= OnPlayerFinished;

    private void OnPlayerFinished(Collider obj)
    {
      if (_isActivated)
        return;

      if (obj.GetComponent<PlayerFollowingTarget>())
      {
        EventHolder<CurrentLevelFinishInfo>.RaiseRegistrationInfo(new CurrentLevelFinishInfo());
        _isActivated = true;
      }
    }
  }
}