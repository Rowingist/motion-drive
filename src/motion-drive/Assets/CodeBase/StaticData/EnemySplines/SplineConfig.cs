using System;
using CodeBase.Data;
using CodeBase.Logic.Bezier.Movement;

namespace CodeBase.StaticData.EnemySplines
{
  [Serializable]
  public class SplineConfig
  {
    public string Label;
    public BezierSpline Template;
    public Vector3Data InitialPosition;
  }
}