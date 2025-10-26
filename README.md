Shapez Shifter is an official mod developed by tobspr, open to community contributions, aimed to facilitate mod creation for shapez 2. Check our example mods [here](https://github.com/tobspr-games/shapez2-mod-samples)



We recommend using and contributing to the Shapez Shifter project for sharing common mod functionality. The API is responsible for:

- Facilitating methods patching
- Offering an API that sits in-between the shapez 2 source code and the mods. This allows supporting a backwards-compatible API where mods have compatibility with the game for longer and source changes do not immediately break existing mods
- Offering meaningful methods for adding extra content to the game



## 📏 Architecture

The Shapez Shifter API is split in three layers: `Sharp Detours`, `Hijack`, and `Flow`:

### 🏹 Sharp Detours

The `Sharp Detour` layer adds safety on top of `MonoMod.RuntimeDetours` allowing you to hook into any method or field in the game and modify it in any preferred way. Methods can be replaced, deleted, prefixed or postfixed. This makes this layer the most powerful and close to source one, but requires a deeper understanding of the game structure. It is also not recommended to be used to remain compatible with other mods.

### 💻 Hijack

The `Hijack` layer allows you to intercept and modify game structures and behaviors. It sits directly on top of the low-level Sharp Detour hooks and exposes a structured way to take control of game logic.

Think of it as a two-step process:

1. **Intercept**: catch existing game execution, objects or events mid-flight.
2. **Rewire**: reroute, modify or extend the intercepted behavior or data for your purposes

### 🌊 Flow

The `Flow` layer offers a convenient and refined way to easily setup common features such as adding a new building (including entity, rendering, toolbar, research, placement, simulation). The layer is the easiest one to use, but greatly limits what is possible to do because many assumptions are made to provide a meaningful API

### 🪛 Kit

There is an extra namespace `Kit` present in the Shapez Shifter API separated from the architecture layers. It contains miscellaneous convenience on top of the game code and methods related to modding
