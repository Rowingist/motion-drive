using UnityEngine;

namespace CodeBase.Infrastructure.Events.Subscribers
{
  public class VFXAtFinishEffectEnabler : OnLevelEndSubscriber
  {
    [SerializeField] private Camera _uICamera;
    [SerializeField] private Canvas _3DUICanvas;
    
    protected override void OnLevelStarted(CurrentLevelFinishInfo levelFinishInfo)
    {
      _uICamera.gameObject.SetActive(true);
      _3DUICanvas.gameObject.SetActive(true);
    }
  }
}