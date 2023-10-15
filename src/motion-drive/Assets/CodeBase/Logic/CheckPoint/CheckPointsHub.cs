using System.Collections.Generic;
using CodeBase.EnemiesSpeedHandler;
using UnityEngine;

namespace CodeBase.Logic.CheckPoint
{
  public class CheckPointsHub : MonoBehaviour
  {
    public Vector3 ActiveCheckPointPosition;
    public Quaternion ActiveCheckPointRotation;
    public float YOffset;
    private List<GameObject> _levelCheckPoints;

    private EnemySpeedMixer _enemySpeedMixer;
    
    public void Construct(List<GameObject> levelCheckPoints, Vector3 heroCarInitialPoint,
      EnemySpeedMixer enemySpeedMixer)
    {
      _levelCheckPoints = levelCheckPoints;
      _enemySpeedMixer = enemySpeedMixer;
      
      UpdateCheckPointPosition(heroCarInitialPoint, Quaternion.identity);
      Subscribe();
    }

    private void Subscribe()
    {
      foreach (GameObject checkPoint in _levelCheckPoints)
      {
        CheckPoint point = checkPoint.GetComponent<CheckPoint>();
        point.Reached += UpdateCheckPointPosition;
        point.ReachedPoint += MixEnemiesSpeed;
        checkPoint.transform.parent = transform;
      } 
    }

    private void OnDestroy() => 
      Cleanup();

    private void Cleanup()
    {
      foreach (GameObject checkPoint in _levelCheckPoints)
      {
        if (checkPoint)
        {
          CheckPoint point = checkPoint.GetComponent<CheckPoint>();
          point.Reached -= UpdateCheckPointPosition;
          point.ReachedPoint -= MixEnemiesSpeed;
        }
      }
    }

    private void UpdateCheckPointPosition(Vector3 position, Quaternion rotation)
    {
      Vector3 offset = new Vector3(0f, YOffset, 0f);
      ActiveCheckPointPosition = position + offset;
      ActiveCheckPointRotation = rotation;
    }

    private void MixEnemiesSpeed(CheckPoint checkPoint)
    {
      _enemySpeedMixer.UpdateConfig(checkPoint.EnemiesSpeedMixerConfig);
    }
  }
}
