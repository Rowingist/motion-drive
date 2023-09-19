using System;
using CodeBase.HeroCar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.UI
{
  public class SettingsPresenter : MonoBehaviour
  {
    public Slider[] Sliders;
    public TMP_Text[] Stats;

    private HeroCarRotationInAir _heroCar;
    
    private bool _isFirstUpdate = true;

    public void Construct(HeroCarRotationInAir heroCar)
    {
      _heroCar = heroCar;
      UpdateSliders();
    }

    private void OnDestroy()
    {
      for (int i = 0; i < Sliders.Length; i++)
      {
        Sliders[0].onValueChanged.RemoveListener(ChangeValueOnHero0);
        Sliders[1].onValueChanged.RemoveListener(ChangeValueOnHero1);
        Sliders[2].onValueChanged.RemoveListener(ChangeValueOnHero2);
        Sliders[3].onValueChanged.RemoveListener(ChangeValueOnHero3);
        Sliders[4].onValueChanged.RemoveListener(ChangeValueOnHero4);
        Sliders[5].onValueChanged.RemoveListener(ChangeValueOnHero5);
      }
    }

    private void UpdateSliders()
    {
      Sliders[0].onValueChanged.AddListener(ChangeValueOnHero0);
      Sliders[1].onValueChanged.AddListener(ChangeValueOnHero1);
      Sliders[2].onValueChanged.AddListener(ChangeValueOnHero2);
      Sliders[3].onValueChanged.AddListener(ChangeValueOnHero3);
      Sliders[4].onValueChanged.AddListener(ChangeValueOnHero4);
      Sliders[5].onValueChanged.AddListener(ChangeValueOnHero5);
      
      if (_isFirstUpdate)
      {
        _isFirstUpdate = false;
        Sliders[0].value = _heroCar.RotationSpeed;
        Sliders[1].value = _heroCar.AdditionalTurnSpeed;
        Sliders[2].value = _heroCar.RotateAngle;
        Sliders[3].value = _heroCar.ReductionRate;
        Sliders[4].value = _heroCar.MaxAngleMagnitude;
        Sliders[5].value = _heroCar.MinAngleMagnitude;
        
        for (int i = 0; i < Sliders.Length; i++)
        {
          Stats[i].text = Sliders[i].value.ToString();
        }
      }
    }

    private void ChangeValueOnHero0(float value)
    {
      print("work");
      _heroCar.RotationSpeed = value;
      Stats[0].text = Sliders[0].value.ToString();
    }
    
    private void ChangeValueOnHero1(float value)
    {
      _heroCar.AdditionalTurnSpeed = value;
      Stats[1].text = Sliders[1].value.ToString();
    }
    private void ChangeValueOnHero2(float value)
    {
      _heroCar.RotateAngle = value;
      Stats[2].text = Sliders[2].value.ToString();
    }
    private void ChangeValueOnHero3(float value)
    {
      _heroCar.ReductionRate = value;
      Stats[3].text = Sliders[3].value.ToString();
    }
    private void ChangeValueOnHero4(float value)
    {
      _heroCar.MaxAngleMagnitude = value;
      Stats[4].text = Sliders[4].value.ToString();
    }
    private void ChangeValueOnHero5(float value)
    {
      _heroCar.MinAngleMagnitude = value;
      Stats[5].text = Sliders[5].value.ToString();
    }
  }
}