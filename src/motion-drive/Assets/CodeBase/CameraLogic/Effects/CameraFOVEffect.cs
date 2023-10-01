using System;
using System.Collections;
using CodeBase.HeroCar.TricksInAir;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.CameraLogic.Effects
{
  public class CameraFOVEffect : MonoBehaviour
  {
    public float FOVMaxValue;
    private float _fOVDefault;
    
    private ParticleSystem _windyFX;
    
    [SerializeField] private ParticleSystem _leftPipeFire;
    [SerializeField] private ParticleSystem _rightPipeFire;
    
    private Camera _main;
    private BoostEffectAfterLanding _boostEffect;

    private Coroutine _smoothTransition;
    private Coroutine _fOVEffect;

    public void Construct(BoostEffectAfterLanding boostEffect)
    {
      _boostEffect = boostEffect;

      Subscribe();
    }

    private void Subscribe()
    {
      _boostEffect.Started += OnStartEffect;
    }

    private void Awake()
    {
      _main = Camera.main;
      _fOVDefault = _main.fieldOfView;
    }

    private void Start()
    {
      _windyFX = _main.GetComponentInChildren<ParticleSystem>();
      _windyFX.gameObject.SetActive(false);
      _leftPipeFire.gameObject.SetActive(false);
      _rightPipeFire.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
      _boostEffect.Started -= OnStartEffect;
    }

    private void OnStartEffect()
    {
      DisableActive(_fOVEffect);
      _fOVEffect = StartCoroutine(ChangeCameraFOV());
    }
    
    public void OnStartEffectNoFX() => 
      StartCoroutine(ChangeCameraFOVNoFX());

    private IEnumerator ChangeCameraFOV()
    {
      DisableActive(_smoothTransition);
      _smoothTransition = StartCoroutine(TransitSmooth(FOVMaxValue));
      _windyFX.gameObject.SetActive(true);
      _leftPipeFire.gameObject.SetActive(true);
      _rightPipeFire.gameObject.SetActive(true);
      
      yield return new WaitUntil(() => !_boostEffect.IsBoosting);
      
      DisableActive(_smoothTransition);
      _smoothTransition = StartCoroutine(TransitSmooth(_fOVDefault));
      _windyFX.Stop();
      _windyFX.gameObject.SetActive(false);
      _leftPipeFire.gameObject.SetActive(false);
      _rightPipeFire.gameObject.SetActive(false);
    }
    
    private IEnumerator ChangeCameraFOVNoFX()
    {
      DisableActive(_smoothTransition);
      _smoothTransition = StartCoroutine(TransitSmooth(FOVMaxValue));
      _windyFX.gameObject.SetActive(true);

      yield return new WaitForSecondsRealtime(2f);
      
      DisableActive(_smoothTransition);
      _smoothTransition = StartCoroutine(TransitSmooth(_fOVDefault));
      _windyFX.Stop();
      _windyFX.gameObject.SetActive(false);
    }

    private IEnumerator TransitSmooth(float targetFOV)
    {
      _main.DOFieldOfView(targetFOV, 1).SetEase(Ease.OutQuad);
      while (_main.fieldOfView != targetFOV)
        yield return null;
    }

    private void DisableActive(Coroutine active)
    {
      if (active != null) 
        StopCoroutine(active);
    }
  }
}