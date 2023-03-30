using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  public class RotateDisabler : MonoBehaviour
  {
    private void LateUpdate() => 
      transform.rotation = Quaternion.identity;
  }
}