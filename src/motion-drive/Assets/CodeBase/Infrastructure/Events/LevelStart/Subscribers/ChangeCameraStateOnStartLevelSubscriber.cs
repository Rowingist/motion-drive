using UnityEngine;

namespace CodeBase.Infrastructure.Events.LevelStart.Subscribers
{
  public class ChangeCameraStateOnStartLevelSubscriber : OnStartLevelSubscriber
  {
    [SerializeField] private Camera _circularMoveCamera;
    
    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      _circularMoveCamera.gameObject.SetActive(false);
  }
}