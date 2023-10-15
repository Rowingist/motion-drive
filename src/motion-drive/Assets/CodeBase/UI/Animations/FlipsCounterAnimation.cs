using System;
using CodeBase.HeroCar;
using CodeBase.HeroCar.TricksInAir;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Animations
{
  public class FlipsCounterAnimation : MonoBehaviour
  {
    private const string OneFlipAnimation = "1Flip";
    
    public TMP_Text FlipName;
    public TMP_Text FlipsAmount;
    public Animator Animator;
    
    private PlayerCarSwipeRotationInAir _rotationInAir;

    private readonly int _flipAnimationHash = Animator.StringToHash(OneFlipAnimation);
    private string _lastFlipName;
    private int _current;
    
    public void Construct(PlayerCarSwipeRotationInAir rotationInAir)
    {
      _rotationInAir = rotationInAir;
      _lastFlipName = string.Empty;
      _rotationInAir.FlippedDirection += PlayAnimation;
    }

    private void OnDestroy()
    {
      _rotationInAir.FlippedDirection -= PlayAnimation;
    }

    private void PlayAnimation(string flipName)
    {
      if (!_lastFlipName.Equals(flipName))
        _current = 0;

      FlipName.text = flipName;
      _current++;
      FlipsAmount.text = _current.ToString();
      Animator.Play(_flipAnimationHash, 0);
      
      _lastFlipName = flipName;
    }
  }
}