#! "netcoreapp1.1"
#r "nuget: NetStandard.Library, 1.6.1"
#r "nuget: System.Xml.ReaderWriter, 4.3.1"
#r "nuget: System.Runtime.Serialization.Xml, 4.3.0"
#r "C:\Users\mnnop\Documents\BOC\Koko.RunTimeGui\bin\Debug\netcoreapp3.1\Koko.RunTimeGui.dll"
#r "nuget: MonoGame.Framework.DesktopGL, 3.0.8"
#r "nuget: MonoGame.Extended, 3.8.0"

// dotnet tool install -g dotnet-script

// cd C:\Users\mnnop\Documents\BOC\Koko.RunTimeGui\Generation
// dotnet script Generator.csx

using System;
using System.IO;
using System.Collections;
using System.Xml;
using Koko.RunTimeGui;
using System.Reflection;

/// <summary>
///  The file location where your xml files are located.
/// </summary>
var xmlFilesLocation = "C:\\Users\\mnnop\\Documents\\BOC\\BOC_Editor\\XML";

/// <summary>
///  The file location where your generated files are stored.
/// </summary>
var generatedFilesLocation = "C:\\Users\\mnnop\\Documents\\BOC\\BOC_Editor\\Generated";

if (Directory.Exists(xmlFilesLocation)) {
    ProcessDirectory(xmlFilesLocation, generatedFilesLocation);
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
    } else {
        foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
            Console.WriteLine("Assembly:" + asm);
            t = asm.GetType(qualifiedTypeName);
            if (t != null)
                return t;
        }
        return null;
    }
}

public void GenerateFileInformation(FileStream fs, string name, string xmlPath) {
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.IgnoreWhitespace = true;

    Console.WriteLine("Checking xml for path: " + xmlPath);

    using (var fileStream = File.OpenText(xmlPath)) {

        using (XmlReader reader = XmlReader.Create(fileStream, settings)) {

            var currentParent = "GUI.Gui";

            var start = "using Koko.RunTimeGui;\nusing Koko.RunTimeGui.Gui.Initable_Components;\n\nnamespace Koko.Generated { \npublic class " + name + " : IInitable { \npublic void Init() {\n";
            var end = "}\n}\n}\n";
            var writestart = new UTF8Encoding(true).GetBytes(start);
            var writeend = new UTF8Encoding(true).GetBytes(end);
            fs.Write(writestart, 0, writestart.Length);

            while (reader.Read()) {
                var line = "";

                switch (reader.NodeType) {
                    case XmlNodeType.Element:

                        var text = "Koko.RunTimeGui." + reader.Name;
                        Console.WriteLine("to Check: " + text);

                        var type = FindType("Koko.RunTimeGui." + reader.Name);
                        Console.WriteLine("type: " + type);

                        try {
                            var instance = Activator.CreateInstance(type) as IComponent;

                            Console.WriteLine($"Start Element: {reader.Name}. Has Attributes? : {reader.HasAttributes}");
                            line += Elements(reader, instance);

                        } catch (System.Exception) {
                            Console.WriteLine("Couldn't find: " + text + " Create it first!");
							throw;
						}

                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine($"Inner Text: {reader.Value}");
                        line += InnerText(reader);
                        break;
                    case XmlNodeType.EndElement:
                        Console.WriteLine($"End Element: {reader.Name}");
                        line += EndTag(reader);
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

string Elements(XmlReader reader, IComponent componentType) {
    if (reader.Name == "GUI") {
        return "IParent parent = GUI.Gui;\n IParent previousParent = null;\n";
    }

    if (componentType is IParent) {
        return "previousParent = parent;\n" +
           "parent = new " + reader.Name + "();\n";
    }

    //if (reader.Name == "Panel" || reader.Name == "FlexPanel") {
     //   return "previousParent = parent;\n" +
	//		"parent = new " + reader.Name + "();\n";
    //}

    if (reader.Name == "Label") {
        return "parent.ChildComponents.Add(new " + reader.Name + "());\n";
    }

    if (reader.Name == "Button") {
        return "parent.ChildComponents.Add(new " + reader.Name + "());\n";
    }

    return "";
}

string InnerText(XmlReader reader) {
    return "";
}

string EndTag(XmlReader reader) {

    if (reader.Name == "Panel" || reader.Name == "FlexPanel") {
        return "previousParent.ChildComponents.Add((IComponent)parent);\n" +
            "parent = previousParent;\n";
    }

    return "";
}