using CodeBase.FollowingTarget;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public class EnablePlayerMovementOnStartLevelSubscriber : OnStartLevelSubscriber
  {
    public PlayerFollowingTarget PlayerFollowingTarget;

    private void Start() => 
      PlayerFollowingTarget.enabled = false;

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      PlayerFollowingTarget.enabled = true;
  }
}