using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRotationIndicators : MonoBehaviour
  {
    public HeroCarOnGroundChecker GroundChecker;
    public GameObject[] Indicators;

    private void Start()
    {
      GroundChecker.TookOff += EnableIndicators;
      GroundChecker.LandedOnGround += DisableIndicators;
    }

    private void OnDestroy()
    {
      GroundChecker.TookOff -= EnableIndicators;
      GroundChecker.LandedOnGround -= DisableIndicators;
    }

    private void EnableIndicators()
    {
      foreach (GameObject indicator in Indicators) 
        indicator.gameObject.SetActive(true);
    }

    private void DisableIndicators()
    {
      foreach (GameObject indicator in Indicators) 
        indicator.gameObject.SetActive(false);
    }
  }
}