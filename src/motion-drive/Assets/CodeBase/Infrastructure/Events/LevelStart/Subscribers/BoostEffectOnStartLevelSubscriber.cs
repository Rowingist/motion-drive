using CodeBase.CameraLogic.Effects;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public class BoostEffectOnStartLevelSubscriber : OnStartLevelSubscriber
  {
    private CameraFOVEffect _fOVEffect;

    public void Construct(CameraFOVEffect fOVEffect) => 
      _fOVEffect = fOVEffect;

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      _fOVEffect.OnStartEffectNoFX();
  }
}