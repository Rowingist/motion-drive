#addin nuget:?package=Cake.Unity&version=0.9.0

var target = Argument("target", "Build-Android");

Task("Clean-Artifacts")
    .Does(() =>
{
    CleanDirectory($"./artifacts");
});

Task("Build-Android")
    .IsDependentOn("Clean-Artifacts")
    .Does(() =>
{
		UnityEditor(
		new UnityEditorArguments
		{
			ProjectPath = "./src/motion-drive",
			ExecuteMethod = "CodeBase.Editor.Builder.BuildAndroid",
			BuildTarget = BuildTarget.Android,
			LogFile = "./artifacts/unity.log"
		},
		new UnityEditorSettings
		{
			RealTimeLog = true
		});
});

RunTarget(target);