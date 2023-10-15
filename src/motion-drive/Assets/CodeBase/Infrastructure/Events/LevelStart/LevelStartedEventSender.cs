using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Infrastructure.Events.LevelStart
{
  public class LevelStartedEventSender : MonoBehaviour, IPointerDownHandler
  {
    public float PreviewDelayDuration;
    
    private bool _isBlocked;

    private void Start() => 
      StartCoroutine(PreviewDelay(PreviewDelayDuration));

    private IEnumerator PreviewDelay(float previewDelayDuration)
    {
      Block();

      WaitForSecondsRealtime yieldDelay = new WaitForSecondsRealtime(previewDelayDuration);
      
      yield return yieldDelay;
      Release();
    }

    private void Block() => 
      _isBlocked = true;

    private void Release() => 
      _isBlocked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
      if(_isBlocked)
        return;
      
      EventHolder<CurrentLevelStartInfo>.RaiseRegistrationInfo(new CurrentLevelStartInfo());
      Block();
    }
  }
}