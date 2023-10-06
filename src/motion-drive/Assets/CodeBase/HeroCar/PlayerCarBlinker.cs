namespace CodeBase.HeroCar
{
  public class PlayerCarBlinker : CarBlinker
  {
    public PlayerCarCrashChecker CrashChecker;
    public PlayerCarRespawn CarRespawn;

    private void Start()
    {
      CrashChecker.Crashed += DisableParts;
      CarRespawn.Completed += Blink;
    }

    private void OnDestroy()
    {
      CrashChecker.Crashed -= DisableParts;
      CarRespawn.Completed -= Blink;
    }

    protected override void Blink() => 
      StartCoroutine(Blinking());
  }
}