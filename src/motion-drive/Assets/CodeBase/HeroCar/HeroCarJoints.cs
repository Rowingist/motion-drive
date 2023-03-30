using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarJoints : MonoBehaviour
  {
    [SerializeField] private Rigidbody[] _targetJoints;
    [SerializeField] private HeroCarRespawn _carRespawn;

    private Transform _defaultParent;
    
    private void Start()
    {
      transform.parent = null;
      _defaultParent = transform.parent;
      _carRespawn.Completed += StopJoints;
    }

    private void OnDestroy() => 
      _carRespawn.Completed -= StopJoints;

    public void ResetParent() => 
      transform.parent = _defaultParent;

    public void StopJoints() => 
      StartCoroutine(StoppingJoints());

    private IEnumerator StoppingJoints()
    {
      float t = 0f;

      while (t < 1f)
      {
        foreach (Rigidbody joint in _targetJoints)
        {
          joint.velocity = Vector3.zero;
          joint.angularVelocity = Vector3.zero;
        }

        t += Time.deltaTime;
        yield return null;
      }
    }
  }
}