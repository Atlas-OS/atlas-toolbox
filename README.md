# AtlasOS Toolbox

<a href="https://github.com/Atlas-OS/.github/blob/main/profile/CODE_OF_CONDUCT.md"><img alt="Code of Conduct" src="https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg?style=for-the-badge&color=1A91FF" /></a>

This repository contains the AtlasOS Toolbox made with C# and WinAppSDK.

**Want to contribute?** Please check out the [Atlas Contribution Guidelines](https://docs.atlasos.net/contributions) for more information.

**If you want to contribute to the translations, please do it here: [Crowdin Project](https://crowdin.com/project/atlas-toolbox-test-1).**

## How do I contribute?

The AtlasOS Toolbox uses C#, which is best programmed using [Visual Studio](https://visualstudio.microsoft.com/vs/) with the WinUI application development workload. You can also build it with the .NET 8 SDK alone:

```
dotnet build AtlasToolbox\AtlasToolbox.csproj -p:Platform=x64
```

1. Once you've cloned the repository, you can run the `AtlasToolbox-WinUI3.sln` file to launch the solution and code.

You can now make all your changes and view them live! The Toolbox needs a PC with Atlas 0.6 installed, so run it in a VM.

## How the Toolbox works with Atlas

The Toolbox supports Atlas 0.6 and refuses other versions. Everything it knows about Atlas comes from three files Atlas installs:

| File | Used for |
| --- | --- |
| `C:\Windows\AtlasModules\Toggles\catalog.json` | Every toggle and its states: the default, the warning to show, whether a restart is needed, and whether a state changes the signed-in user's own settings |
| `C:\Windows\AtlasOS\state.json` | The installed Atlas version and the choice recorded for each toggle |
| `C:\Windows\AtlasModules\Scripts\Entry\Invoke-AtlasToggleLauncher.cmd` | Applying a toggle state, by name, as the launchers in the Atlas folder do |

The Toolbox runs as administrator. A state that only changes the PC runs silently. A state that changes the signed-in user's own settings opens in a window as that user, without administrator rights, because Atlas applies those only there; Atlas asks for administrator rights itself for the rest. Actions such as repairing Windows components also open in a window, where they show their questions and results.

The items each page shows are listed in `AtlasToolbox\HostBuilder\AddViewModelsHostBuilderExtensions.cs`, by toggle name. An item whose toggle isn't in the installed Atlas's catalog is left out and logged.

The Toolbox keeps its profiles and logs in `C:\ProgramData\AtlasOS\Toolbox`, and its settings in `HKLM\SOFTWARE\AtlasOS\Toolbox`. It never writes to Atlas's files or to Atlas's toggle records.

## Releases

Atlas installs the latest stable release during setup, and the Toolbox checks for it in Settings. A release must have:

- a tag `vX.Y.Z` that matches `MyAppVersion` in `Installer\setup.iss` (the installer records it under `HKLM\SOFTWARE\AtlasOS\Toolbox`);
- exactly one asset named `AtlasToolbox-Setup.exe`, which installs silently with `/verysilent /install` and exits 0.

## Credits
- [WinUI 3 Gallery](https://apps.microsoft.com/detail/9P3JFPWWDZRC) for many aspects of the Toolbox and for the home page tile galery
- [Windows Community Toolkit Gallery](https://apps.microsoft.com/detail/9NBLGGH4TLCQ) for aspects of the app such as settings cards and expanders

## 💙 Contributors
<a href="https://github.com/Atlas-OS/atlas-toolbox/graphs/contributors" target="_blank"><img src="https://contrib.rocks/image?repo=Atlas-OS/atlas-toolbox&columns=18" alt="Avatars of all contributors"></a>
