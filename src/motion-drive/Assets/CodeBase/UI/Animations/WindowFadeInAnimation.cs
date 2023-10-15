using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Animations
{
  public class WindowFadeInAnimation : MonoBehaviour
  {
    public CanvasGroup CanvasGroup;

    private void OnEnable() => 
      CanvasGroup.DOFade(1, .5f).SetEase(Ease.InCubic);
  }
}