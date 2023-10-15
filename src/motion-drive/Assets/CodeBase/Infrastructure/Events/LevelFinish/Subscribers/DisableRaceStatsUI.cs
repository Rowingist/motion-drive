using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure.Events.Subscribers
{
  public class DisableRaceStatsUI : OnLevelEndSubscriber
  {
    public CanvasGroup CanvasGroup;
    public float FadeDuration;

    protected override void OnLevelStarted(CurrentLevelFinishInfo levelFinishInfo) => 
      CanvasGroup.DOFade(0, FadeDuration);
  }
}