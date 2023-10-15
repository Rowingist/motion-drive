using Plugins.Joystick_Pack.Scripts.Joysticks;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public class EnableJoystickInputOnStartLevelSubscriber : OnStartLevelSubscriber
  {
    public DrivingJoystick Joystick;

    private void Start() => 
      Joystick.enabled = false;

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      Joystick.enabled = true;
  }
}