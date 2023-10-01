using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraCircularMove : MonoBehaviour
  {
    [SerializeField] private float _speed;
    [SerializeField] private float _width;
    [SerializeField] private float _height;
    [SerializeField] private float _depth;

    private float _timeCounter;
    private Vector3 _currentPosition;
    
    private void Update()
    {
      _timeCounter += Time.deltaTime * _speed;

      _currentPosition.x = Mathf.Cos(_timeCounter) * _width;
      _currentPosition.y = _height;
      _currentPosition.z = Mathf.Sin(_timeCounter) * _depth;
      
      transform.position = _currentPosition;
      transform.rotation = Quaternion.LookRotation(Vector3.zero - transform.position);
    }
  }
}
