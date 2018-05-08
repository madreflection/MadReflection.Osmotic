#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/MadReflection.Osmotic/bin") + Directory(configuration);

// Solution file.
var solutionFile = "./src/MadReflection.Osmotic.sln";
var nuspecFile = "./src/MadReflection.Osmotic/Properties/MadReflection.Osmotic.nuspec";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
	{
		CleanDirectory(buildDir);
	});

Task("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		NuGetRestore(solutionFile);
	});

Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
	{
		if (IsRunningOnWindows())
		{
			// Use MSBuild
			MSBuild(solutionFile, settings => settings.SetConfiguration(configuration));
		}
		else
		{
			// Use XBuild
			XBuild(solutionFile, settings => settings.SetConfiguration(configuration));
		}
	});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
	{
		NUnit3("./src/**/bin/" + configuration + "/MadReflection.Osmotic.Tests.dll", new NUnit3Settings()
		{
			NoResults = true
		});
	});

Task("Build-NuGet-Package")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
	{
		if (configuration == "Release")
		{
			NuGetPack(nuspecFile, new NuGetPackSettings());
		}
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
