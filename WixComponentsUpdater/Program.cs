// See https://aka.ms/new-console-template for more information

using System.Xml.Linq;

if (args.Length != 2)
{
    Console.WriteLine("Usage: WixComponentsGenerator.exe componentName pathToOutput");
    return;
}

var componentName = args[0];
var pathToOutput = args[1];
var xmlFilePath = $"{componentName}.wxs";

try
{
    XDocument doc;

    if (File.Exists(xmlFilePath))
    {
        doc = XDocument.Load(xmlFilePath);
        
    }
    else
    {
        var template = $@"
<Wix xmlns=""http://wixtoolset.org/schemas/v4/wxs"">
  <Fragment>
    <ComponentGroup Id=""{componentName}"" Directory=""INSTALLFOLDER"">
      
    </ComponentGroup>
  </Fragment>
</Wix>";

        doc = XDocument.Parse(template);
        doc.Save(xmlFilePath);
        Console.WriteLine($"Created template file: {xmlFilePath}");
    }

    if (doc.Root == null)
    {
        throw new Exception($"Cannot read the file {xmlFilePath}. Ensure the template exists");
    }

    var ns = doc.Root.Name.Namespace;

    var files = Directory.GetFiles(pathToOutput, "*.exe")
        .Concat(Directory.GetFiles(pathToOutput, "*.dll"))
        .ToArray();

    foreach (var file in files)
    {
        var fileName = Path.GetFileName(file);
        var componentExists = doc.Descendants(ns + "Component")
            .Any(c => (bool)c.Element(ns + "File")?.Attribute("Source")?.Value.EndsWith(fileName));

        if (!componentExists)
        {
            var component = new XElement(ns + "Component",
                new XElement(ns + "File", new XAttribute("Source", Path.Combine(pathToOutput,fileName))));

            doc.Descendants(ns + "ComponentGroup")
                .FirstOrDefault()?.Add(component);

            Console.WriteLine($"Added component for file: {fileName}");
        }
    }

    doc.Save(xmlFilePath);
    Console.WriteLine("XML file updated successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
