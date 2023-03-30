using System.Collections;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarBlinker : MonoBehaviour
  {
    private const int Cycles = 5;
    private const float Interval = 0.15f;

    public HeroCarCrashChecker CrashChecker;
    public HeroCarRespawn CarRespawn;
    public GameObject[] HeroCarViewParts;

    private void Start()
    {
      CrashChecker.Crashed += DisableParts;
      CarRespawn.Completed += Blink;
    }

    private void OnDestroy()
    {
      CrashChecker.Crashed -= DisableParts;
      CarRespawn.Completed -= Blink;
    }

    private void Blink() =>
      StartCoroutine(Blinking());

    private IEnumerator Blinking()
    {
      yield return new WaitForSecondsRealtime(Interval);
      
      for (int i = 0; i < Cycles; i++)
      {
        EnableParts();
        yield return new WaitForSecondsRealtime(Interval);
        DisableParts();
        yield return new WaitForSecondsRealtime(Interval);
      }

      EnableParts();
    }

    private void DisableParts()
    {
      foreach (GameObject part in HeroCarViewParts)
        part.SetActive(false);
    }

    private void EnableParts()
    {
      foreach (GameObject part in HeroCarViewParts)
        part.SetActive(true);
    }
  }
}