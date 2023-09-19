using TMPro;
using UnityEngine;

public class FrameRateCouter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _display;
    [SerializeField, Range(0.1f, 2f)] private float _sampleDuration = 1f;
    [SerializeField] private DisplayMode _displayMode = DisplayMode.FPS;

    public enum DisplayMode { FPS, MS }

    private int _frames;
    private float _duration, _bestDuration = float.MaxValue, _worstDuration;

    private void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        _frames += 1;
        _duration += frameDuration;

        if (frameDuration < _bestDuration)
        {
            _bestDuration = frameDuration;
        }
        if (frameDuration > _worstDuration)
        {
            _worstDuration = frameDuration;
        }

        if (_duration >= _sampleDuration)
        {
            if (_displayMode == DisplayMode.FPS)
            {
                _display.SetText("FPS\nлучший: {0:0}\nтекущий: {1:0}\nхудший: {2:0}", 1f / _bestDuration, _frames / _duration, 1f / _worstDuration);
            }
            else
            {
                _display.SetText("MS\nлучший: {0:0}\nтекущий: {1:0}\nхудший: {2:0}", 1000f * _bestDuration, 1000f * _duration / _frames, 1000f * _worstDuration);
            }

            _frames = 0;
            _duration = 0f;
            _bestDuration = float.MaxValue;
            _worstDuration = 0f;
        }
    }
}
