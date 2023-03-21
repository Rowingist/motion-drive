using UnityEngine;
using UnityEngine.EventSystems;

namespace Plugins.Joystick_Pack.Scripts.Joysticks
{
  public class DrivingJoystick : Joystick
  {
    private const float DeadZoneInAir = 0.1f;
    
    [SerializeField] private float _moveThreshold = 1f;

    protected override void Start()
    {
      base.Start();
      DisableBackground();
    }

    private void DisableBackground() => 
      background.gameObject.SetActive(false);

    public override void OnPointerDown(PointerEventData eventData)
    {
      background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);

      base.OnPointerDown(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
      if (magnitude > _moveThreshold)
      {
        Vector2 difference = normalised * (magnitude - _moveThreshold) * radius;
        background.anchoredPosition += difference;
      }
      
      base.HandleInput(magnitude, normalised, radius, cam);
    }

    public void EnableInAirSetting()
    {
      SetDeadZoneInAir();

      background.anchoredPosition = ScreenPointToAnchoredPosition(NewBackgroundPosition());

      HandleToDefault();
      InputToDefault();
    }

    private void SetDeadZoneInAir() => 
      DeadZone = DeadZoneInAir;

    private Vector2 NewBackgroundPosition() => 
      background.anchoredPosition + LastHandlePosition;

    private void InputToDefault() => 
      input = Vector2.zero;

    private void HandleToDefault() => 
      handle.anchoredPosition = Vector2.zero;

    public void DisableInAirSetting() => 
      ResetDeadZone();

    private void ResetDeadZone() => 
      DeadZone = 0;
  }
}
