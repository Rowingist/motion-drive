using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Animations
{
  [RequireComponent(typeof(RawImage))]
  public class BackgroundScroller : MonoBehaviour
  {
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;

    private RawImage _image;
    private float _imagePositionX;
    private float _imagePositionY;

    private void Awake() => 
      _image = GetComponent<RawImage>();

    private void Update()
    {
      _imagePositionX += _horizontalSpeed * Time.deltaTime;
      _imagePositionY += _verticalSpeed * Time.deltaTime;

      _image.uvRect = new Rect(_imagePositionX, _imagePositionY, _image.uvRect.width, _image.uvRect.height);
    }
  }
}