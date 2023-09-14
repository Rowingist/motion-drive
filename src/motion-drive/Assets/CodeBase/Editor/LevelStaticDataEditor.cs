using System.Linq;
using CodeBase.Logic.CameraSwitchPoint;
using CodeBase.Logic.CheckPoint;
using CodeBase.StaticData;
using CodeBase.StaticData.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if (GUILayout.Button("Collect"))
      {
        // levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
        //   .Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
        //   .ToList();

        levelData.LevelKey = SceneManager.GetActiveScene().name;
        
        levelData.InitialHeroPosition =  GameObject.FindWithTag(InitialPointTag).transform.position;
        
        //levelData.LevelTransfer.Position = GameObject.FindWithTag(LevelTransferInitialPointTag).transform.position;

        levelData.LevelCheckPointsHub.Points = FindObjectsOfType<CheckPointMarker>()
          .Select(x => new LevelCheckPointsStaticData(x.transform.position, x.RaycasterOnGround.position))
          .Reverse().ToArray();
        
        levelData.LevelCameraSwitchPointsHub.Points = FindObjectsOfType<CameraSwitchPointMarker>()
          .Select(x => new LevelCameraSwitchPointStaticData(x.transform.position, x.FollowSetting, x.LookAtSetting))
          .Reverse().ToArray();
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}