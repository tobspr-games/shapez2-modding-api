### Requirements

- Shapez2 Alpha 9
- Visual Studio (recommended) or Rider or VSCode

> For MacOS users, patching with either MonoMod, HarmonyX, MelonLoader, tModLoader, BepInEx, require running the game with Rosetta

### Installation

The projects is configured for very easy installation with Visual Studio and fairly straightforward with other IDEs. It only requires two environment variables:

1. SPZ2_PATH: Pointing to the game folder containing the managed assemblies
2. SPZ2_PERSISTENT: Should point to Unity's `Application.persistentDataPath`

On Windows, these are automatically set when you play the game. You can also add them manually

On Unix, these must be set somehow. My recommendation for MacOS is using the `.zprofile` to export the variables and then opening Visual Studio from the console.

After these variables are set, it is as easy as building the solution and the patchers should already be available in the game. To actually change something, check out the [mods samples](https://github.com/tobspr-games/shapez2-mod-samples)

### Disclaimer

This API is only a proof of concept. We plan on creating an extended API and 99% of these will probably change. They lack dependency handling, unloading, packing and much more. 
