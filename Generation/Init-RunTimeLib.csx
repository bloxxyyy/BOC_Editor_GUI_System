#! "netcoreapp3.1"
#r "nuget: NetStandard.Library, 1.6.1"
using System.Runtime.Loader;

var projectPath = Directory.GetCurrentDirectory();
var assemblyPath = projectPath + "\\bin\\Debug\\netcoreapp3.1\\Koko.RunTimeGui.dll";
Console.WriteLine(assemblyPath);
AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
