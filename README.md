# Unlit

A top-down 2D survival game made in Unity. You're trapped in a dark dungeon with a torch that's slowly burning out. Collect three keys and reach the exit before the monsters lurking outside your light catch you.

## How to Play

| Key | Action |
|---|---|
| **W A S D** | Move |
| **F** | Use the flash powerup (once picked up) |

**Goal:** collect all **3 keys**, then reach the **exit door** in the top wall.

**Your torch is your lifeline:**
- It burns down over time, and its light shrinks and dims as it does.
- Monsters move **slower inside your light**, but get faster as the torch dies.
- Pick up **fuel flasks** to refill it. If it goes out, you're in total darkness.
- The **flash powerup** creates a burst of light that **stuns every monster inside your light** for a few seconds. It can only be used once.

You have **3 hearts**. Touching a monster costs a heart, followed by a brief moment of invincibility.

## Monsters

- **Chaser:** Constantly on your tail, never backs off
- **Ambusher:** stalks you from just outside the edge of your light. When its **eyes glow red and the warning sound plays**, it's about to lunge where it thinks you'll be. Change direction to dodge it.

## Replayability

Every run is different:
- One of **4 arena layouts** is picked at random (Three Cells, Ruins, Keep, Pillared Hall).
- **Keys, fuel, the flash powerup, and monsters** spawn at random points within that layout.

## Features

- Dynamic 2D torch lighting with flicker, smooth fuel changes, and shadows cast by walls and obstacles
- NavMesh pathfinding so monsters navigate around obstacles
- Animated HUD: draining torch bar with a flame that burns out at zero, hearts, key icons, and flash indicator
- Win and lose screens with Play Again and Quit
- Sound effects for pickups, damage, the flash, the ambusher's warning, footsteps, the torch crackle (fades with fuel), and its extinguishing

## Running the Project

- **Unity version:** 6000.5.9f1 (Unity 6), Universal Render Pipeline (2D)
- Open the project in Unity Hub, then open `Assets/Scenes/SampleScene` and press Play.
- **Required packages:** Input System, AI Navigation, and NavMeshPlus (installed from `https://github.com/h8man/NavMeshPlus.git`).

## Scripts

| Script | Purpose |
|---|---|
| `Player_Controller` | Movement, sprite flipping, footsteps |
| `TorchLight` | Fuel, light size and brightness, flicker, torch sounds |
| `PlayerHealth` | Hearts, invincibility blink, death |
| `KeyCollector`, `KeyPickup` | Key counting and collection |
| `FuelBehavior` | Fuel pickups |
| `FlashAbility`, `FlashPickup` | Flash powerup and stunning monsters |
| `ChaserAI` | Chaser movement, light/dark speeds |
| `AmbusherAI` | Stalk, warn, and lunge cycle |
| `EnemyDamage`, `EnemyStun`, `EnemyAnimations` | Shared monster behavior |
| `SpawnManager` | Random layout, NavMesh rebuild, random spawns |
| `ExitDoor` | Win condition |
| `HUD` | Torch bar, hearts, keys, flash icon |
| `EndScreens` | Win/lose panels, Play Again, Quit |
| `SoundManager` | Plays sound effects from any script |

## Credits

**Code libraries**
- [NavMeshPlus](https://github.com/h8man/NavMeshPlus) by h8man: 2D NavMesh support

**Art**
- 2D Pixel Dungeon Asset Pack v2.0 by [Zerie]: tileset, player, items
- [Pixel UI](https://lizcheong.itch.io/pixel-ui) by Liz Cheong: torch bar and flame (recolored to orange)
- DungeonFont by [vrtxrry]: UI font

**Sound**
- `pickupCoin.wav`, `hitHurt.wav`: made with jsfxr
- Flash, torch loop, extinguish, footsteps, warning, win and lose sounds: [PixaBay]

## AI Usage Notes

I used Claude (an AI assistant) throughout development as a tutor and debugging partner.

**How it was used:**
- **Step-by-step guidance:** for most systems, Claude explained the concepts and the code piece by piece, and I typed and assembled the scripts myself. This covered the torch fuel and light system, pickups, the chaser and ambusher AI, player health and invincibility, the HUD, win/lose screens, the random layout and spawn system, sound, and animations.
- **Concepts explained:** coroutines, Lerp and smoothing, trigger vs. collision events, NavMesh pathfinding, arrays and loops, the Fisher-Yates shuffle, Animator state machines, the singleton pattern (SoundManager), and OnEnable/OnDisable.
- **Design suggestions:** the ambusher stalking at the light's edge and leading/overshooting its lunge, the second chaser intercepting, invincibility frames, layout designs, and difficulty tuning.
- **Debugging:** Claude read my project files to find bugs, such as a mismatched class name, editor-only `using` lines that would break the build, missing Inspector references, and NavMesh baking problems.

**Where Claude wrote or edited directly:**
- Grouped comments added to all scripts, following the style of my existing comments
- Art edits: recoloring the Pixel UI art to orange, splitting the bar into empty/full/flame images, and removing a leftover pixel from the burn-out animation
- Layout diagrams used as a reference while painting the tilemaps
- This README (drafted by Claude, reviewed and edited by me)
