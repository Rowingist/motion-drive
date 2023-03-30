using CodeBase.HeroCar;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class WheelsDrive : MonoBehaviour
  {
    public GameObject[] Wheels;

    public float Speed;
    public float MaxDriveSpeed;
    public HeroCarOnGroundChecker GroundChecker;

    private Vector3 _rotation;
    private float _rotationX;
    private float _rotationMagnitude;

    private Rigidbody _following;
    private IInputService _inputService;

    public void Construct(IInputService inputService, Rigidbody following)
    {
      _following = following;
      _inputService = inputService;
    }

    private void Update()
    {
      if (_inputService is null) return;

      if (!_inputService.IsFingerHoldOnScreen() && !GroundChecker.IsOnGround) return;

      ChoseRotationMagnitude(_following.velocity.magnitude);

      _rotationX += _rotationMagnitude * Speed * Time.deltaTime;

      Drive(_rotationX);
    }

    private void ChoseRotationMagnitude(float currentMagnitude) => 
      _rotationMagnitude = currentMagnitude > MaxDriveSpeed ? MaxDriveSpeed : currentMagnitude;

    private void Drive(float rotationX)
    {
      foreach (GameObject wheel in Wheels)
      {
        _rotation = new Vector3(rotationX, wheel.transform.localRotation.y, 0);
        wheel.transform.localRotation = Quaternion.Euler(_rotation);
      }
    }
  }
}