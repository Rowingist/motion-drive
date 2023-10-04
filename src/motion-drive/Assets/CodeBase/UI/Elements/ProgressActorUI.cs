using System;
using System.Collections;
using CodeBase.Car;
using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class ProgressActorUI : MonoBehaviour
  {
    public LevelProgressBar ProgressBar;
    public float UpdateRate;

    private float _finishPositionZ;
    private CarMove _car;

    public void Construct(float finishPositionZ, CarMove car)
    {
      _finishPositionZ = finishPositionZ;
      _car = car;
      
      StartCoroutine(UpdatePlayerPositionInfo());
    }
    
    private IEnumerator UpdatePlayerPositionInfo()
    {
      WaitForSecondsRealtime updateRoutine = new WaitForSecondsRealtime(UpdateRate);
      while (true)
      {
        ProgressBar.SetValue(_car.transform.position.z, _finishPositionZ);
        
        yield return updateRoutine;
      }
    }
  }
}