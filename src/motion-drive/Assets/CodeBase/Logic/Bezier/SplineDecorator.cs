using CodeBase.Logic.Bezier.Movement;
using UnityEngine;

namespace CodeBase.Logic.Bezier
{
  public class SplineDecorator : MonoBehaviour
  {
    public BezierSpline Spline;

    public int Frequency;

    public bool LookForward;

    public Transform[] Items;

    private void Awake()
    {
      if (Frequency <= 0 || Items == null || Items.Length == 0)
        return;

      float stepSize = Frequency * Items.Length;
      if (Spline.Loop || stepSize == 1) {
        stepSize = 1f / stepSize;
      }
      else {
        stepSize = 1f / (stepSize - 1);
      }
      
      for (int p = 0, f = 0; f < Frequency; f++)
      {
        for (int i = 0; i < Items.Length; i++, p++)
        {
          Transform item = Instantiate(Items[i]) as Transform;
          Vector3 position = Spline.GetPoint(p * stepSize);
          item.transform.localPosition = position;
          if (LookForward)
          {
            item.transform.LookAt(position + Spline.GetDirection(p * stepSize));
          }

          item.transform.parent = transform;
        }
      }
    }
  }
}