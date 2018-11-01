#tool "nuget:?package=xunit.runner.console"

var target = Argument("target","Default");
var configuration = Argument("Configuration", "Debug");

Task("Default")
.IsDependentOn("Tests");

Task("Build")
.Does(()=>
{
    MSBuild("TestBuild.sln");
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
                arguments: $"-configuration {configuration} -diagnostics -stoponfail"
            );
        }
    });
RunTarget(target);