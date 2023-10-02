using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Infrastructure.Events.LevelStart
{
  public class LevelStartedEventSender : MonoBehaviour, IPointerDownHandler
  {
    private bool _isActivated;
    
    public void OnPointerDown(PointerEventData eventData)
    {
      if(_isActivated)
        return;
      
      EventHolder<CurrentLevelStartInfo>.RaiseRegistrationInfo(new CurrentLevelStartInfo());
      _isActivated = true;
    }
  }
}