using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRespawn : MonoBehaviour
  {
    public HeroCarCrashChecker CrashChecker;

    private void Start()
    {
      CrashChecker.Crashed += OnRespawn;
    }

    private void OnRespawn()
    {
      
    }
  }
}