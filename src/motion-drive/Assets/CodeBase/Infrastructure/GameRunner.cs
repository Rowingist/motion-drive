using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameRunner : MonoBehaviour
  {
    public GameBootstrapper BootstrapperPrefab;
    public int TargetFPS;

    private void Awake()
    {
      var bootstrapper = FindObjectOfType<GameBootstrapper>();

      if (bootstrapper != null) return;

      Instantiate(BootstrapperPrefab);
    }

    private void Start() => 
      TestFPS();

    private void TestFPS()
    {
      if (TargetFPS > 0)
        Application.targetFrameRate = TargetFPS;
    }
  }
}