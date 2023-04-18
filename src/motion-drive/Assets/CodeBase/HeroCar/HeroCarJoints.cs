using System.Collections;
using CodeBase.Logic.CarParts;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarJoints : MonoBehaviour
  {
    [SerializeField] private Rigidbody[] _targetJoints;
    [SerializeField] private HeroCarRespawn _carRespawn;
    [SerializeField] private SuspensionJointReset _jointReset;
    [SerializeField] private float ResetSpeedRespawned;

    private Transform _defaultParent;

    private void Start()
    {
      transform.parent = null;
      _defaultParent = transform.parent;

      _carRespawn.Completed += StopJointsOnRespawn;
    }

    private void OnDestroy()
    {
      _carRespawn.Completed -= StopJointsOnRespawn;
    }

    public void ResetParent() => 
      transform.parent = _defaultParent;
    

    public void StopJointsOnRespawn() => 
      StartCoroutine(StoppingJoints(ResetSpeedRespawned));

    private IEnumerator StoppingJoints(float resetSpeed)
    {
      float t = 0f;

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
    }
  }
}