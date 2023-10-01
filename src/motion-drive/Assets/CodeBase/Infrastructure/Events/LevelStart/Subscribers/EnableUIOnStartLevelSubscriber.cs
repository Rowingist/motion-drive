using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public class EnableUIOnStartLevelSubscriber : OnStartLevelSubscriber
  {
    public CanvasGroup CanvasGroup;
    public float FadeDuration;

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      CanvasGroup.DOFade(1, FadeDuration);
  }
}