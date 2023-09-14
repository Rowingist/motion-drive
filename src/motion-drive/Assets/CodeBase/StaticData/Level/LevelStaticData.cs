using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
  public class LevelStaticData : ScriptableObject
  {
    public string LevelKey;
    //public List<EnemySpawnerStaticData> EnemySpawners;
    public Vector3 InitialHeroPosition;
    public LevelTransferStaticData LevelTransfer;

    public LevelCheckPointsHubStaticData LevelCheckPointsHub;
    public LevelCameraSwitchPointsHubStaticData LevelCameraSwitchPointsHub;
  }
}