using UnityEngine;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public abstract class OnStartLevelSubscriber : MonoBehaviour
  {
    private void Awake() => 
      EventHolder<CurrentLevelStartInfo>.AddListener(OnLevelStarted, false);

    private void OnDestroy() =>
      EventHolder<CurrentLevelStartInfo>.RemoveListener(OnLevelStarted);

    protected abstract void OnLevelStarted(CurrentLevelStartInfo levelStartInfo);
  }
}