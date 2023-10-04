using System.Collections;
using CodeBase.HeroCar;
using CodeBase.Logic.CarParts;
using UnityEngine;

namespace CodeBase.Car
{
  public class CarJoints : MonoBehaviour
  {
    [SerializeField] private MoveType _moveType = MoveType.Player;
    [SerializeField] private Rigidbody[] _targetJoints;
    [SerializeField] private PlayerCarRespawn _carRespawn;
    [SerializeField] private SuspensionJointReset _jointReset;
    [SerializeField] private float ResetSpeedRespawned;

    private Transform _defaultParent;

    public bool AreReadyToMove = true;

    private void Start()
    {
      _defaultParent = transform.parent;

      if(_moveType == MoveType.Player)
        _carRespawn.Completed += StopJointsOnRespawn;
    }

    private void OnDestroy()
    {
      if(_moveType == MoveType.Player)
        _carRespawn.Completed -= StopJointsOnRespawn;
    }

    public void ResetParent() => 
      transform.parent = _defaultParent;
    

    public void StopJointsOnRespawn() => 
      StartCoroutine(StoppingJoints(ResetSpeedRespawned));

    private IEnumerator StoppingJoints(float resetSpeed)
    {
      float t = 0f;

      AreReadyToMove = false;
      _jointReset.StopMove();
      
      while (t < 1f)
      {
        foreach (Rigidbody joint in _targetJoints)
        {
          joint.velocity = Vector3.zero;
          joint.angularVelocity = Vector3.zero;
        }

        t += Time.deltaTime * resetSpeed;
        yield return null;
      }
      
      _jointReset.Reset();
      AreReadyToMove = true;
    }
  }
}