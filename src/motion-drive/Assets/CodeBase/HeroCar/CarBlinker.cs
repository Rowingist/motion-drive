using System.Collections;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public abstract class CarBlinker : MonoBehaviour
  {
    private const int Cycles = 10;
    private const float Interval = 0.1f;
    
    [SerializeField]
    private GameObject[] HeroCarViewParts;

    protected abstract void Blink();
    
    protected IEnumerator Blinking()
    {
      yield return new WaitForSecondsRealtime(Interval);
      
      for (int i = 0; i < Cycles; i++)
      {
        EnableParts(HeroCarViewParts);
        yield return new WaitForSecondsRealtime(Interval);
        DisableParts();
        yield return new WaitForSecondsRealtime(Interval);
      }

      EnableParts(HeroCarViewParts);
    }

    protected void DisableParts()
    {
      foreach (GameObject part in HeroCarViewParts)
        part.SetActive(false);
    }

    private void EnableParts(GameObject[] HeroCarViewParts)
    {
      foreach (GameObject part in HeroCarViewParts)
        part.SetActive(true);
    }
  }
}