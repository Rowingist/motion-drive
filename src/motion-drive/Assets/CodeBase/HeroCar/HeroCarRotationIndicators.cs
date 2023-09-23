using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRotationIndicators : MonoBehaviour
  {
    public HeroCarOnGroundChecker GroundChecker;
    public GameObject Indicator;

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

    private void EnableIndicators() => 
      Indicator.gameObject.SetActive(true);

    private void DisableIndicators() => 
      Indicator.gameObject.SetActive(false);
  }
}