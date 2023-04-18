using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class BodyViewChanger : MonoBehaviour
  {
    public Transform Parent;

    public void Construct(GameObject bodyPrefab) => 
      Instantiate(bodyPrefab, Parent);
  }
}