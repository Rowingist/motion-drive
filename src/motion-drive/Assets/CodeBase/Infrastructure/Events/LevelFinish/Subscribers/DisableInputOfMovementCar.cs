using CodeBase.FollowingTarget;
using CodeBase.Services.Input;

namespace CodeBase.Infrastructure.Events.Subscribers
{
  public class DisableInputOfMovementCar : OnLevelEndSubscriber
  {
    private IInputService _input;
    private PlayerFollowingTarget _followingTarget;

    public void Construct(IInputService input, PlayerFollowingTarget followingTarget)
    {
      _input = input;
      _followingTarget = followingTarget;
    }

    protected override void OnLevelStarted(CurrentLevelFinishInfo levelFinishInfo)
    {
      print(false);
      _input.Deactivate();
      _followingTarget.StopBoosting();
      _followingTarget.StopSmooth();
      _followingTarget.enabled = false;
    }
  }
}