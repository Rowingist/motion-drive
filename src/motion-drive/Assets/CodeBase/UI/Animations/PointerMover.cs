using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Sequence = DG.Tweening.Sequence;

namespace CodeBase.UI.Animations
{
  [RequireComponent(typeof(RectTransform))]
  public class PointerMover : MonoBehaviour, IPointerDownHandler
  {
    public RectTransform LeftPoint;
    public RectTransform RightPoint;

    public Vector3 Angle = new Vector3(0, 0, 45);

    public float MoveDuration = 1f;
    public float RotateDuration = 1f;

    public RectTransform Pointer;
    public RectTransform Container;

    private void Start()
    {
      Sequence animation = DOTween.Sequence();

      animation.Append(Pointer.DORotate(Angle * -1, RotateDuration));
      animation.Append(Pointer.DOMove(LeftPoint.position, MoveDuration));
      animation.Append(Pointer.DORotate(Angle, RotateDuration));
      animation.Append(Pointer.DOMove(RightPoint.position, MoveDuration));
      animation.SetLoops(-1);
    }

    public void OnPointerDown(PointerEventData eventData) => 
      Container.gameObject.SetActive(false);
  }
}