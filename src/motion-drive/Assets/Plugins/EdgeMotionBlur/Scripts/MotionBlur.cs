using UnityEngine;

namespace EdgeMotionBlur
{
    public class MotionBlur : MonoBehaviour
    {
        [SerializeField] private Material motionBlurMaterial;
        [SerializeField] public float speedCoeff;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            SetSpeedCoefficient(speedCoeff);
            Graphics.Blit(src, dest, motionBlurMaterial);
        }

        private void OnDestroy() => SetSpeedCoefficient(0);
        private void SetSpeedCoefficient(float speedCoeff) => motionBlurMaterial.SetFloat("_SpeedCoeff", speedCoeff);
    }
}
