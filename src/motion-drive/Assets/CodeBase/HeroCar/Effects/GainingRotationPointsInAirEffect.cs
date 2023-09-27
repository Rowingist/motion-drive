using System;
using System.Linq;
using CodeBase.HeroCar.TricksInAir;
using UnityEngine;

namespace CodeBase.HeroCar.Effects
{
  public class GainingRotationPointsInAirEffect : MonoBehaviour
  {
    [SerializeField] private ParticleSystem[] _effects;

    [SerializeField] private int _firstStep;
    [SerializeField] private int _secondStep;

    private HeroCarSwipeRotationInAir _swipeRotationInAir;
    private HeroCarOnGroundChecker _groundChecker;
    private HeroCarAirTricksCounter _tricksCounter;

    private int _nextCounter;

    public void Construct(HeroCarSwipeRotationInAir swipeRotationInAir, HeroCarOnGroundChecker groundChecker,
      HeroCarAirTricksCounter tricksCounter)
    {
      _swipeRotationInAir = swipeRotationInAir;
      _groundChecker = groundChecker;
      _tricksCounter = tricksCounter;

      Subscribe();
    }

    private void Start() => 
      _nextCounter = _firstStep;

    private void OnDestroy()
    {
      _swipeRotationInAir.Flipped -= UpdateEffects;
      _groundChecker.LandedOnGround -= DisableAll;
    }

    private void Subscribe()
    {
      _swipeRotationInAir.Flipped += UpdateEffects;
      _groundChecker.LandedOnGround += DisableAll;
    }

    private void UpdateEffects()
    {
      PlayPointUpEffect();

      switch (_nextCounter)
      {
        case 2:
          _nextCounter = _secondStep;
          ActivateNextEffect();
          break;
        case 4:
          _nextCounter = 6;
          ActivateNextEffect();
          break;
      }
    }

    private void PlayPointUpEffect() => 
      _effects[^1].gameObject.SetActive(true);

    private void DisableAll()
    {
      foreach (ParticleSystem effect in _effects)
        effect.gameObject.SetActive(false);
    }

    private int _increment;

    private void ActivateNextEffect()
    {
      ParticleSystem effect = _effects.FirstOrDefault(e => e.gameObject.activeSelf == false);
      if(effect)
        effect.gameObject.SetActive(true);
    }
  }
}