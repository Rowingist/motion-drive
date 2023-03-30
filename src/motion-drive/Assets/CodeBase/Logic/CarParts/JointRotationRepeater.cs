using System;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class JointRotationRepeater : MonoBehaviour
  {
    public Transform Rotator;
    public float Speed;
    
    private void Update() => 
      transform.rotation = NewRotation();

    private Quaternion NewRotation() => 
      Quaternion.Lerp(transform.rotation, Rotator.rotation, Time.deltaTime * Speed);
  }
}