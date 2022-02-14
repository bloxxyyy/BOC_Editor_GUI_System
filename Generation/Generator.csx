#! "netcoreapp1.1"
#r "nuget: NetStandard.Library, 1.6.1"
#r "nuget: System.Xml.ReaderWriter, 4.3.1"
#r "nuget: System.Runtime.Serialization.Xml, 4.3.0"
#r "nuget: MonoGame.Framework.DesktopGL, 3.0.8"
#r "nuget: MonoGame.Extended, 3.8.0"
#r "../bin/Debug/netcoreapp3.1/Koko.RunTimeGui.dll"

// dotnet tool install -g dotnet-script

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
using System.Linq;

var projectPath = Directory.GetCurrentDirectory();

/// <summary>
///  The file location where your xml files are located.
/// </summary>
var xmlFilesLocation = projectPath + "\\XML";

/// <summary>
///  The file location where your generated files are stored.
/// </summary>
var generatedFilesLocation = projectPath + "\\Generated";

var longName = "Koko.RunTimeGui, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";
var asm = Assembly.Load(longName);

if (!Directory.Exists(xmlFilesLocation)) 
    throw new ArgumentException($"Path \"{xmlFilesLocation}\" does not exist, please check your current working directory");

var files = GetAllXmlFiles(xmlFilesLocation);

files.ForEach(xmlFileLocation => {
    var fileName = "Gen_" + System.IO.Path.GetFileNameWithoutExtension(xmlFileLocation).Replace(" ", "_");
    var generatedfilesLocation = generatedFilesLocation + "\\" + fileName + ".cs";
    CreateNewFile(generatedfilesLocation, fileName, xmlFileLocation);
});

/// <summary>
/// To check for all xml files in directory plus sub directories.
/// </summary>
/// <param name="targetDirectory"></param>
private List<String> GetAllXmlFiles(string targetDirectory) {
    var files = new List<String>();
    foreach (string fileName in Directory.EnumerateFiles(targetDirectory, "*.xml", SearchOption.AllDirectories)) {
        files.Add(fileName);
    }
    return files;
}

private void CreateNewFile(string GeneratedFileLocation, string filename, string xmlFileLocation) {
    try {
        using (FileStream fs = File.Create(GeneratedFileLocation)) {
            GenerateFileInformation(fs, filename, xmlFileLocation);
        }
    } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
    }
}

public void GenerateFileInformation(FileStream fs, string filename, string xmlPath) {
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.IgnoreWhitespace = true;

    using (var fileStream = File.OpenText(xmlPath)) {
        using (XmlReader reader = XmlReader.Create(fileStream, settings)) {

            var currentParent = "GUI.Gui";

            var start = "using Microsoft.Xna.Framework;\nusing Koko.RunTimeGui;\nusing Koko.RunTimeGui.Gui.Initable_Components;\n\nnamespace Koko.Generated { \npublic class " + filename + " : IInitable { \npublic void Init() {\n";
            var end = "}\n}\n}\n";
            var writestart = new UTF8Encoding(true).GetBytes(start);
            var writeend = new UTF8Encoding(true).GetBytes(end);
            fs.Write(writestart, 0, writestart.Length);

            while (reader.Read()) {
                var line = "";

                switch (reader.NodeType) {
                    case XmlNodeType.Element:
                        try {
                            var instance = Activator.CreateInstance(asm.GetType("Koko.RunTimeGui." + reader.Name)) as IComponent;
                            line += Elements(reader, instance);
                        } catch (System.Exception) {
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
                            var type = asm.GetType("Koko.RunTimeGui." + reader.Name);
                            var instance = Activator.CreateInstance(type) as IComponent;

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

    var setparentToGui = "IParent component = GUI.Gui;";
    var createTempComponent = "BaseComponent temp;";

    if (reader.Name == "GUI") // special case.
        return $"{setparentToGui}\n{createTempComponent}\n";

    var tagVal = "\"" + reader.GetAttribute("Tag") + "\"";
    var marginVal = GetIntergerValue(reader.GetAttribute("Margin"));
    var borderVal = GetIntergerValue(reader.GetAttribute("Border"));

    var setnew = $"new {reader.Name}() {{ Parent = component";
    var tag = $"Tag = {tagVal}";
    var margin = $"MarginalSpace = new Margin({marginVal})";
    var border = $"BorderSpace = new Margin({borderVal})";

    if (componentType is IParent) {
        var backgroundColor = $"BackgroundColor = {GetBackgroundVal(reader)}";
        var columns = $"Columns = {GetColumnsVal(reader)}";

        if (componentType is GridPanel)
            return $"component = {setnew}, {tag}, {border}, {margin}, {backgroundColor}, {columns} }};\n";

        return $"component = {setnew}, {tag}, {border}, {margin}, {backgroundColor} }};\n";
    }

    return $"temp = {setnew}, {tag}, {border}, {margin} }};\n temp.Text = \"";
   
}

private string EndTag(XmlReader reader, IComponent componentType) {
    if (reader.Name == "GUI") return "";

    var addComponentToParent = "((BaseComponent)component).Parent.ChildComponents.Add((BaseComponent)component);";
    var setComponent = "component = ((BaseComponent)component).Parent;";

    if (componentType is IParent) // besides GUI
        return $"{addComponentToParent}\n{setComponent}\n";

    return "";
}

private int GetIntergerValue(string margin) {
    if (margin != null) {
        try {
            return Int32.Parse(margin);
        } catch (FormatException e) {
            Console.WriteLine(e.Message);
        }
    }
    return 0;
}

private int GetColumnsVal(XmlReader reader) {
    var columnsVal = GetIntergerValue(reader.GetAttribute("Columns"));
    return (columnsVal == 0) ? 2 : columnsVal;
}

private string GetBackgroundVal(XmlReader reader) {
    var background = reader.GetAttribute("BackGround-Color");
    if (background is null) {
        return "null";
    } else if (background.StartsWith("#")) {
        var rx = new Regex(@"^#(?<alpha>[0-9a-f]{2})?(?<red>[0-9a-f]{2})(?<green>[0-9a-f]{2})(?<blue>[0-9a-f]{2})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var groups = rx.Matches(background)[0].Groups;
        var alpha = byte.Parse(groups[1].Value != "" ? groups[1].Value : "ff", NumberStyles.HexNumber);
        var red = byte.Parse(groups[2].Value, NumberStyles.HexNumber);
        var green = byte.Parse(groups[3].Value, NumberStyles.HexNumber);
        var blue = byte.Parse(groups[4].Value, NumberStyles.HexNumber);
        return $"new Color({red}, {green}, {blue}, {alpha})";
    } else {
        return "Color." + background;
    }
}
