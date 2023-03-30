using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class WheelSteering : MonoBehaviour
  {
    private const string Horizontal = nameof(Horizontal);

    public Animator[] Animators;
    
    private IInputService _inputService;
    private float _currentInputX;

    public void Construct(IInputService inputService) => 
      _inputService = inputService;

    private void Update()
    {
      if(_inputService == null) return;

      ChangeCurrentInput();

      SteerByAnimation();
    }

    private void ChangeCurrentInput() => 
      _currentInputX = !_inputService.IsFingerHoldOnScreen() ? 0 : _inputService.Axis.x;

    private void SteerByAnimation()
    {
      foreach (Animator animator in Animators)
        animator.SetFloat(Horizontal, _currentInputX);
    }
  }
}