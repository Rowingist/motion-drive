using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public abstract class WindowBase : MonoBehaviour
  {
    [SerializeField] private Button _closeButton;

    protected IPersistentProgressService ProgressService;
    protected PlayerProgress Progress => ProgressService.Progress;

    public void Construct(IPersistentProgressService progressService) => 
      ProgressService = progressService;

    private void Awake() => 
      OnAwake();

    private void Start()
    {
      Initialize();
      SubscribeUpdates();
    }

    private void OnDestroy() => 
      CleanUp();

    protected virtual void OnAwake()
    {
      if(_closeButton)
        _closeButton.onClick.AddListener(() => Destroy(gameObject));
    }

    protected virtual void Initialize() {}
    protected virtual void SubscribeUpdates() {}
    protected virtual void CleanUp() {}
  }
}