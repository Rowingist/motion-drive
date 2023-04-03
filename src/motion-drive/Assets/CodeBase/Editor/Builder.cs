using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using static UnityEditor.BuildPipeline;

namespace CodeBase.Editor
{
  public static class Builder
  {
    [MenuItem("Build/⚽Android")]
    public static void BuildAndroid()
    {
      BuildReport report = BuildPlayer(new BuildPlayerOptions
      {
        target = BuildTarget.Android,
        locationPathName = "../../artifacts/Motion-drive-sa.apk",
        scenes = new[] { "Assets/Scenes/Initial.unity", "Assets/Scenes/Tutorial.unity" }
      });

      if (report.summary.result != BuildResult.Succeeded)
        throw new Exception("Failed to build Android package. See log for details.");
    }
  }
}