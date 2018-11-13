#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"

var target = Argument("target","Default");
var configuration = Argument("buildConfiguration", "Debug");

var solutionfile="TestBuild.sln"

Task("Default")
.IsDependentOn("Tests");

Task("Clean")
.Does(()=> 
{
foreach(var platform in supportedPlatforms)
{
    MSBuild(solutionfile, configurator =>
    configurator.SetConfiguration(buildConfiguration)
    .SetVerbosity(Verbosity.Quiet)
    .SetMSBuildPlatform(MSBuildPlatform.x86)
    .WithTarget("Clean")
});

Task("Versioning")
.IsDependentOn("Clean")
.Does(()=>
{
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo=true
    });
});

Task("NugetPackageRestore")
.IsDependentOn("Versioning")
.Does(()=>
{
    NugetRestore(solutionfile)
});

Task("Build")
.IsDependentOn("NugetPackageRestore")
.Does(()=>
{
    MSBuild(solutionfile);
});

Task("Tests")
 .IsDependentOn("Build")
    .Does(() =>
    {
         Information("Start running on Unit Tests");
        var projects = GetFiles("./TesBuild.Tests/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTool(
                projectPath: project.FullPath, 
                command: "xunit", 
                arguments: $"-configuration {buildConfiguration} -diagnostics -stoponfail"
            );
        }
    });
RunTarget(target);