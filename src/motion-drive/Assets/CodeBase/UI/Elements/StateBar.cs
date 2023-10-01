using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
  public abstract class StateBar : MonoBehaviour
  {
    [SerializeField]
    private Image _current;

    public virtual void SetValue(float current, float max) => 
      _current.fillAmount = current / max;
  }
}