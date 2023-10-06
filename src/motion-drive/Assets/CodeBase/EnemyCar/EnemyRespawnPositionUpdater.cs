using CodeBase.Logic;
using CodeBase.Logic.Bezier;
using CodeBase.Logic.CheckPoint;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyRespawnPositionUpdater : MonoBehaviour
  {
    public TriggerObserver TriggerObserver;
    public float OffsetY = 1f;
    
    private SplineWalker _splineWalker;
    private Vector3 _lastCheckpoint;

    public Vector3 CurrentCheckpoint => _lastCheckpoint;

    public void Construct(SplineWalker splineWalker) => 
      _splineWalker = splineWalker;

    private void Awake() => 
      _lastCheckpoint = transform.position;

    private void Start() => 
      TriggerObserver.TriggerExit += UpdateRespawnPosition;
    private void OnDestroy() => 
      TriggerObserver.TriggerExit -= UpdateRespawnPosition;

    private void UpdateRespawnPosition(Collider obj)
    {
      if (!obj.GetComponent<CheckPoint>())
        return;

      _splineWalker.CacheCurrentProgress();
      Vector3 respawnPoint = _splineWalker.transform.position;
      if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, float.MaxValue,
            LayerMask.NameToLayer("Ground")))
        respawnPoint.y = hitInfo.point.y + OffsetY;
      else
        respawnPoint.y = transform.position.y;

      _lastCheckpoint = respawnPoint;
    }
  }
}