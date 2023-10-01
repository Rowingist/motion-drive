using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Animations
{
  public class CashFlyToWalletAnimations : MonoBehaviour
  {
    public RectTransform Target;
    public RectTransform[] MoneyIcons;

    public RectTransform ResourceText;
    public RectTransform WalletText;

    public float SpawnInterval;
    public float SpawnDuration;
    public float PresentDelay;

    public float ToWalletInterval;
    public float ToWalletDuration;

    public Vector2 MaxRange = Vector2.zero;
    public Vector2 MinRange = Vector2.zero;

    private Vector2 _lastRandom = Vector2.zero;

    public void Fly() =>
      StartCoroutine(FlyingAnimation(Target, MaxRange, MinRange));

    private IEnumerator FlyingAnimation(RectTransform target, Vector2 maxRange, Vector2 minRange)
    {
      foreach (RectTransform icon in MoneyIcons)
      {
        Sequence toCenter = DOTween.Sequence();
        Vector3 random = GetRandom(maxRange, minRange);

        Activate(icon);
        MoveAndScaleToCenter(toCenter, icon, random);

        yield return new WaitForSecondsRealtime(SpawnInterval);
      }

      ResetScale(ResourceText);

      yield return new WaitForSecondsRealtime(PresentDelay);

      foreach (RectTransform icon in MoneyIcons)
      {
        Sequence flyAnimation = DOTween.Sequence();

        MoveAndScaleToWalletIcon(target, flyAnimation, icon);

        yield return new WaitForSecondsRealtime(ToWalletInterval);
      }

      yield return new WaitUntil(AllIconsIsDeactivated);

      ResetScale(WalletText);
    }

    private Vector3 GetRandom(Vector2 maxRange, Vector2 minRange)
    {
      float rangeX = Random.Range(minRange.x, maxRange.x);
      float rangeY = Random.Range(minRange.y, maxRange.y);

      rangeX = UpdateRandomRangeXIfItLikePrevious(maxRange, minRange, rangeX);
      rangeY = UpdateRandomRangeYIfItLikePrevious(maxRange, minRange, rangeY);

      Vector3 random = new Vector2(rangeX, rangeY);

      _lastRandom = random;

      return random;
    }

    private float UpdateRandomRangeXIfItLikePrevious(Vector2 maxRange, Vector2 minRange, float rangeX)
    {
      while (Mathf.Approximately(rangeX, _lastRandom.x))
        rangeX = Random.Range(minRange.x, maxRange.x);

      return rangeX;
    }

    private float UpdateRandomRangeYIfItLikePrevious(Vector2 maxRange, Vector2 minRange, float rangeY)
    {
      while (Mathf.Approximately(rangeY, _lastRandom.y))
        rangeY = Random.Range(minRange.y, maxRange.y);

      return rangeY;
    }

    private static void Activate(RectTransform icon) =>
      icon.gameObject.SetActive(true);

    private bool AllIconsIsDeactivated() =>
      !MoneyIcons[^1].gameObject.activeSelf;

    private void MoveAndScaleToCenter(Sequence flyAnimation, RectTransform icon, Vector3 random)
    {
      flyAnimation.Append(icon.DOMove(icon.position + random, SpawnDuration).SetEase(Ease.InCubic));
      flyAnimation.Append(icon.DOScale(icon.localScale * 2f, SpawnDuration / 3).SetEase(Ease.InCubic));
      ResourceText.DOScale(Random.Range(0.7f, 1.2f), SpawnInterval);
    }

    private void ResetScale(RectTransform item) =>
      item.localScale = Vector3.one;

    private void MoveAndScaleToWalletIcon(RectTransform target, Sequence flyAnimation, RectTransform icon)
    {
      flyAnimation.Append(icon.DOScale(Vector3.one, ToWalletInterval).SetEase(Ease.InCubic));
      flyAnimation.Append(icon.DOMove(target.position, ToWalletDuration))
        .OnKill(() => Deactivate(icon));
      flyAnimation.Append(WalletText.DOScale(Random.Range(0.5f, 1.6f), ToWalletDuration));
    }

    private void Deactivate(RectTransform icon) =>
      icon.gameObject.SetActive(false);
  }
}