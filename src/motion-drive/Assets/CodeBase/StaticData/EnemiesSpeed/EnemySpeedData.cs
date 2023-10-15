using System;
using UnityEngine;

namespace CodeBase.StaticData.EnemiesSpeed
{
  [Serializable]
  public class EnemySpeedData
  {
    [SerializeField] private string _label;
    [field: SerializeField, Range(0, 100)] public float AdditionalSpeed { get; private set; }
  }
}