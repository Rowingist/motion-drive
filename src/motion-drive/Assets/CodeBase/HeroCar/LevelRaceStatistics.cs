using UnityEngine;

namespace CodeBase.HeroCar
{
  public class LevelRaceStatistics : MonoBehaviour
  {
    private int _kills;
    private int _flips;
    private int _coins;

    public void CollectKill()
    {
      _kills++;
      if (_kills > int.MaxValue) 
        Debug.LogError($"{_kills} cannot be greater than {int.MaxValue}");
      
      if(_kills < 0)
        Debug.LogError($"{_kills} cannot be less than 0");
    }
  }
}