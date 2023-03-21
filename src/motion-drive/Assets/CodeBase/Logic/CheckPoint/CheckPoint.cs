using System;
using UnityEngine;

namespace CodeBase.Logic.CheckPoint
{
  public class CheckPoint : MonoBehaviour
  {
    private const string Player = "Player";

    public TriggerObserver TriggerObserver;
    
    public event Action<Vector3> Reached;

    private void Start() => 
      TriggerObserver.TriggerEnter += TriggerEnter;

    private void OnDestroy() => 
      TriggerObserver.TriggerEnter -= TriggerEnter;

    private void TriggerEnter(Collider other)
    {
      if (!other.TryGetComponent(out TriggerObserver triggerObserver)) return;
      
      if(triggerObserver.tag == Player )
        Reached?.Invoke(transform.position);
        
      DisableObject();
    }

    private void DisableObject() => 
      gameObject.SetActive(false);
  }
}