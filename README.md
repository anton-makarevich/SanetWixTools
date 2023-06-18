# Wix Components Updater

Wix Components Updater is a .NET tool that simplifies the process of generating WiX component entries for your project's output files. It is designed to be used in conjunction with the WiX toolset for creating Windows Installer packages.

## Overview

When building a WiX project, it can be challenging to manually add all the output files, especially when there are many dependencies or when new files are added to the project. Wix Components Updater automates this process by scanning a specified folder and generating WiX component entries for any DLL or EXE files that are not already present in the WiX source file.
Wix's heat harvester tool can help with it as well, but it's a complex tool that require a lot of configurations. For ci/cd scenarios I needed something very simple.  

## Installation

Wix Components Updater can be installed as a .NET tool via NuGet. Make sure you have the .NET CLI installed.

To install Wix Components Updater (`sanet-wix-tools`), use the following command:

```bash
dotnet tool install sanet-wix-tools --global
```

## Usage

To use Wix Components Updater, follow these steps:

1. Create a WiX source file (`<componentName>.wxs`) for a component and add the necessary component entries for the main files in your project. You can specify additional parameters as needed:
```xml
<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">
  <Fragment>
    <ComponentGroup Id="<componentName>" Directory="INSTALLFOLDER">
      <Component>
        <File Source="${pathToOutput}/<YouApp>.exe" />
      </Component>
    </ComponentGroup>
  </Fragment>
</Wix>
```
2. If the file doesn't exist, the tool will create it.                

3. Build your project to generate the output files. For example:

```bash
dotnet build
```
or 
```bash
dotnet publish -c Release
```

4. Run Wix Components Updater, providing the `componentName` (which should match your WiX source file name without the extension) and the `pathToOutput` where the output files are located. For example:

```bash
dotnet sanet-wix-tools MyComponentName ../../Myapp/bin/Debug/net7.0/publish
```

Wix Components Updater will scan the specified folder for DLL and EXE files. For each file that doesn't already have a corresponding component entry in the WiX source file, it will generate a new component entry and add it to the ComponentGroup in the XML.

5. Build your WiX project as usual:

```bash
wix build <your wix source files > -o <your wix bundle>
```

## Example

The example of how Wix Components Updater can be used in a real typical workflow could be found in the `MagicalYatzyXF` repo, where it used in the `publish-windows.yml` GitHub Action.

## License

Wix Components Updater is released under the [MIT License](LICENSE.md). See the `LICENSE.md` file for more details.
```
