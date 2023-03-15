using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LoadingCurtain : MonoBehaviour
  {
    private const float HideStep = 0.03f;
    
    public CanvasGroup Curtain;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void Show()
    {
      gameObject.SetActive(true);
      Curtain.alpha = 1;
    }

    public void Hide()
    {
      StartCoroutine(DoFadeIn());
    }

    private IEnumerator DoFadeIn()
    {
      WaitForSeconds fadeInSeconds = new WaitForSeconds(HideStep);
      
      while (Curtain.alpha > 0f)
      {
        Curtain.alpha -= HideStep;
        yield return fadeInSeconds;
      }
      
      gameObject.SetActive(false);
    }
  }
}