// Refer : https://novemberfive.co/blog/windows-jenkins-cake-tutorial/
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"

var target = Argument("target","Default");
var buildConfiguration = Argument("buildConfiguration", "Release");

var solutionfile="TestBuild.sln";
var supportedPlatforms = new PlatformTarget[]
    {
        //PlatformTarget.ARM,
        //PlatformTarget.x64,
        //PlatformTarget.x86,
        PlatformTarget.MSIL, //Any CPU
    };

Task("Default")
.IsDependentOn("Tests");

Task("Clean")
.Does(()=> 
{
    CleanDirectories("./*/bin");
    CleanDirectories("./*/obj");
    Information("Cleaning has been done...");
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
    NuGetRestore(solutionfile);
});

Task("Build")
.IsDependentOn("NugetPackageRestore")
.Does(()=>
{
    MSBuildSettings settings = new MSBuildSettings() {
        Configuration = buildConfiguration
    };
    foreach(var platform in supportedPlatforms)
    {
        MSBuild(solutionfile, settings);
    }
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

Task("UnitTests")
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