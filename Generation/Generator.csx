#! "netcoreapp1.1"
#r "nuget: NetStandard.Library, 1.6.1"
#r "nuget: System.Xml.ReaderWriter, 4.3.1"
#r "nuget: System.Runtime.Serialization.Xml, 4.3.0"
#r "nuget: MonoGame.Framework.DesktopGL, 3.0.8"
#r "nuget: MonoGame.Extended, 3.8.0"
#r "../bin/Debug/netcoreapp3.1/Koko.RunTimeGui.dll"

// dotnet tool install -g dotnet-script

// cd C:\Users\mnnop\Documents\BOC\Koko.RunTimeGui\Generation
// dotnet script Generator.csx

using System;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Xml;
using Koko.RunTimeGui;


var projectPath = Directory.GetCurrentDirectory();

/// <summary>
///  The file location where your xml files are located.
/// </summary>
var xmlFilesLocation = projectPath + "\\XML";

/// <summary>
///  The file location where your generated files are stored.
/// </summary>
var generatedFilesLocation = projectPath + "\\Generated";

if (Directory.Exists(xmlFilesLocation))
{
    ProcessDirectory(xmlFilesLocation, generatedFilesLocation);
}
else
{
    throw new ArgumentException($"Path \"{xmlFilesLocation}\" does not exist, please check your current working directory");
}

/// <summary>
/// To check for all xml files in directory plus sub directories.
/// </summary>
/// <param name="targetDirectory"></param>
public void ProcessDirectory(string targetDirectory, string generatedFilesLocation) {
    foreach (string fileName in Directory.EnumerateFiles(targetDirectory, "*.xml", SearchOption.AllDirectories)) {
        Console.WriteLine("");
        Console.WriteLine("-----------------------------------------------------------------------------");
        Console.WriteLine("");
        ProcessFile(fileName, generatedFilesLocation);
    }
}

public void ProcessFile(string path, string generatedFilesLocation) {
    var fileName = new DirectoryInfo(path).Name;
    Console.WriteLine("Processed file '{0}'.", path);
    Console.WriteLine("Found file '{0}'.", fileName);
    fileName = System.IO.Path.GetFileNameWithoutExtension(path);

    Console.WriteLine("Converting file name: '{0}'...", fileName);
    fileName = "Gen_" + fileName.Replace(" ", "_");
    Console.WriteLine("Converted to: '{0}'.", fileName);

    Console.WriteLine("Check if generation file exists...");
    var check = CheckIfGeneratedFileExists(fileName, generatedFilesLocation);

    var cSharpFile = check;
    Console.WriteLine("Creating new file: " + cSharpFile);

    CreateNewFile(cSharpFile, fileName, path);
}

public void CreateNewFile(string fileName, string csharpName, string xmlpath) {
    var path = fileName;
    try {
        using (FileStream fs = File.Create(path)) {
            GenerateFileInformation(fs, csharpName, xmlpath);
        }
    } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
    }
}

public string CheckIfGeneratedFileExists(string fileName, string generatedFilesLocation) {
    Console.WriteLine("Looking in Folder '{0}'...", generatedFilesLocation);

    var path = generatedFilesLocation + "\\" + fileName + ".cs";

    if (File.Exists(path)) {
        Console.WriteLine(path + " exists... Re-generate anyways.");
    }

    return path;
}

private Type FindType(string qualifiedTypeName) {
    Type t = Type.GetType(qualifiedTypeName);

    if (t != null) {
        return t;
    } else { // TODO dont go through all assemblies. Just get my own.
        foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
            /*Console.WriteLine("Assembly:" + asm);*/
            t = asm.GetType(qualifiedTypeName);
            if (t != null)
                return t;
        }
        return null;
    }
}

private Type GetType(string name) {
    var text = "Koko.RunTimeGui." + name;
    Console.WriteLine("to Check: " + text);

    var type = FindType("Koko.RunTimeGui." + name);
    Console.WriteLine("type: " + type);

    return type;
}

public void GenerateFileInformation(FileStream fs, string name, string xmlPath) {
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.IgnoreWhitespace = true;

    Console.WriteLine("Checking xml for path: " + xmlPath);

    using (var fileStream = File.OpenText(xmlPath)) {

        using (XmlReader reader = XmlReader.Create(fileStream, settings)) {

            var currentParent = "GUI.Gui";

            var start = "using Microsoft.Xna.Framework;\nusing Koko.RunTimeGui;\nusing Koko.RunTimeGui.Gui.Initable_Components;\n\nnamespace Koko.Generated { \npublic class " + name + " : IInitable { \npublic void Init() {\n";
            var end = "}\n}\n}\n";
            var writestart = new UTF8Encoding(true).GetBytes(start);
            var writeend = new UTF8Encoding(true).GetBytes(end);
            fs.Write(writestart, 0, writestart.Length);

            while (reader.Read()) {
                var line = "";

                switch (reader.NodeType) {
                    case XmlNodeType.Element:
                        try {
                            var instance = Activator.CreateInstance(GetType(reader.Name)) as IComponent;

                            Console.WriteLine($"Start Element: {reader.Name}. Has Attributes? : {reader.HasAttributes}");
                            line += Elements(reader, instance);

                        } catch (System.Exception) {
                            Console.WriteLine("Couldn't find XML TYPE: " + reader.Name + " Create Component first!");
							throw;
						}

                        break;

                    case XmlNodeType.Text:
                        Text = reader.Value;
                        if (Text is null)
                            Text = "";

                        if (Text != "") {
                            line += Text + "\";\n" +
                                "component.ChildComponents.Add(temp);\n";
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (Text == "") {
                            line += "\";\n" +
                                "component.ChildComponents.Add(temp);\n";
                        }

                        try {
                            var instance = Activator.CreateInstance(GetType(reader.Name)) as IComponent;

                            Console.WriteLine($"End Element: {reader.Name}");
                            line += EndTag(reader, instance);

                        } catch (System.Exception) {
                            Console.WriteLine("Couldn't find XML TYPE: " + reader.Name + " Create Component first!");
                            throw;
                        }

                        break;

                    default:
                        Console.WriteLine($"Unknown: {reader.NodeType}");
                        break;
                }

                var write = new UTF8Encoding(true).GetBytes(line);
                fs.Write(write, 0, write.Length);
            }

            fs.Write(writeend, 0, writeend.Length);
        }
    }
}

private string Text = "";
string Elements(XmlReader reader, IComponent componentType) {

    if (reader.Name == "GUI") { // special case.
        return "IParent component = GUI.Gui;\nBaseComponent temp;\n";
    }

    var tag = "\"" + reader.GetAttribute("Tag") + "\"";
    var margin = reader.GetAttribute("Margin");
    var border = reader.GetAttribute("Border");
    int convertedMargin = GetIntergerValue(margin);
    int convertedBorder = GetIntergerValue(border);

    if (componentType is IParent) {

        var background = reader.GetAttribute("BackGround-Color");
        if (background is null) {
            background = "null";
        } else if (background.StartsWith("#")) {
            var rx = new Regex(@"^#(?<alpha>[0-9a-f]{2})?(?<red>[0-9a-f]{2})(?<green>[0-9a-f]{2})(?<blue>[0-9a-f]{2})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var groups = rx.Matches(background)[0].Groups;
            var alpha = byte.Parse(groups[1].Value != "" ? groups[1].Value : "ff", NumberStyles.HexNumber);
            var red = byte.Parse(groups[2].Value, NumberStyles.HexNumber);
            var green = byte.Parse(groups[3].Value, NumberStyles.HexNumber);
            var blue = byte.Parse(groups[4].Value, NumberStyles.HexNumber);
            background = $"new Color({red}, {green}, {blue}, {alpha})";
        } else {
            background = "Color." + background;
        }

        return "component = new " + reader.Name + "() { Parent = component, Tag = " + tag + ", BorderSpace = new Margin(" + convertedBorder + "), MarginalSpace = new Margin(" + convertedMargin + ") " + ", BackgroundColor = " + background + " };\n";
    }

    return "temp = new " + reader.Name + "() { Parent = component, Tag = " + tag + ", BorderSpace = new Margin(" + convertedBorder + "),  MarginalSpace = new Margin(" + convertedMargin + ") " + " };\n" +
        "temp.Text = \"";
   
}

string EndTag(XmlReader reader, IComponent componentType) {
    if (reader.Name == "GUI") { // special case.
        return "";
    }

    if (componentType is IParent) {
        return "((BaseComponent)component).Parent.ChildComponents.Add((BaseComponent)component);\ncomponent = ((BaseComponent)component).Parent;\n";
    }

    return "";
}

int GetIntergerValue(string margin) {
    if (margin != null) {
        try {
            return Int32.Parse(margin);
        } catch (FormatException e) {
            Console.WriteLine(e.Message);
        }
    }
    return 0;
}
