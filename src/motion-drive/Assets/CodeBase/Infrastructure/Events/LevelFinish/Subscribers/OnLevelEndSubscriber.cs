using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace CodeBase.Infrastructure.Events.Subscribers
{
  public abstract class OnLevelEndSubscriber : MonoBehaviour
  {
    private void Awake() => 
      EventHolder<CurrentLevelFinishInfo>.AddListener(OnLevelStarted, false);

    private void OnDestroy() =>
      EventHolder<CurrentLevelFinishInfo>.RemoveListener(OnLevelStarted);

    protected abstract void OnLevelStarted(CurrentLevelFinishInfo levelFinishInfo);
  }
}