using System;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Events.Subscribers
{
  public class EnableFinishCongratulationWindowSubscriber : OnLevelEndSubscriber
  {
    public WindowId WindowId = WindowId.FinishCongratulation;
    public float Delay;
    
    private IWindowService _windowService;

    public void Construct(IWindowService windowService) => 
      _windowService = windowService;

    protected override void OnLevelStarted(CurrentLevelFinishInfo levelFinishInfo)
    {
      Invoke( nameof(OpenWindow), Delay);
    }

    private void OpenWindow() => 
      _windowService.Open(WindowId);
  }
}