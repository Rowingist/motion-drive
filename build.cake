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
		Console.WriteLine("Build will be here");
});

RunTarget(target);