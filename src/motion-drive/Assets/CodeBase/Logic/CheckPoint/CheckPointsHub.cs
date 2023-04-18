using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.CheckPoint
{
  public class CheckPointsHub : MonoBehaviour
  {
    public Vector3 ActiveCheckPointPosition;
    public Quaternion ActiveCheckPointRotation;
    public float YOffset;
    private List<GameObject> _levelCheckPoints;

    public void Construct(List<GameObject> levelCheckPoints, Vector3 heroCarInitialPoint)
    {
      _levelCheckPoints = levelCheckPoints;
      UpdateCheckPointPosition(heroCarInitialPoint, Quaternion.identity);
      Subscribe();
    }

    private void Subscribe()
    {
      foreach (GameObject checkPoint in _levelCheckPoints) 
        checkPoint.GetComponent<CheckPoint>().Reached += UpdateCheckPointPosition;
    }

    private void OnDestroy() => 
      Cleanup();

    private void Cleanup()
    {
      foreach (GameObject checkPoint in _levelCheckPoints)
      {
        if(checkPoint)
          checkPoint.GetComponent<CheckPoint>().Reached -= UpdateCheckPointPosition;
      }
    }

    private void UpdateCheckPointPosition(Vector3 position, Quaternion rotation)
    {
      Vector3 offset = new Vector3(0f, YOffset, 0f);
      ActiveCheckPointPosition = position + offset;
      ActiveCheckPointRotation = rotation;
    }
  }
}
