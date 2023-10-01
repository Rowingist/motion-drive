using System;
using System.Collections;
using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class ProgressActorUI : MonoBehaviour
  {
    public LevelProgressBar ProgressBar;
    public float UpdateRate;

    private float _finishPositionZ;
    private HeroCarMove _heroCar;

    public void Construct(float finishPositionZ, HeroCarMove heroCar)
    {
      _finishPositionZ = finishPositionZ;
      _heroCar = heroCar;
      
      StartCoroutine(UpdatePlayerPositionInfo());
    }
    
    private IEnumerator UpdatePlayerPositionInfo()
    {
      WaitForSecondsRealtime updateRoutine = new WaitForSecondsRealtime(UpdateRate);
      while (true)
      {
        ProgressBar.SetValue(_heroCar.transform.position.z, _finishPositionZ);
        
        yield return updateRoutine;
      }
    }
  }
}