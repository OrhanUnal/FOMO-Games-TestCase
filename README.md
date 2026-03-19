# Color Blocks 3D - Slide Puzzle

A Unity implementation of the Color Blocks 3D gameplay, built as an engineering case study.

## How to Play

Swipe colored blocks in their allowed directions to guide them into matching-color exit gates. Clear all blocks from the board to win. Some levels have a move limit — run out of moves and you lose.

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs        — Singleton, game state, win/lose logic
│   │   ├── BoardLoader.cs        — Level deserialization, board spawning
│   │   ├── InputManager.cs       — Swipe detection, block selection
│   │   └── MaterialFactory.cs    — Runtime material creation with caching
│   ├── Blocks/
│   │   ├── BlockBase.cs          — Base block: movement, raycasting, animations
│   │   └── BlockType1x2.cs       — 1x2 block with extended ray origins
│   │   └── ExitGates.cs          — Exit gate: color and direction matching
│   ├── UI/
│   │   └── UIManager.cs          — Move counter, level display, win/lose panels
│   ├── Data/
│   │   ├── LevelData.cs          — JSON data model classes
│   │   └── Enums.cs              — Shared enums (Directions, BlockColor)
│   └── ScriptableObjects/
│       └── BlockShapeSO.cs       — Reusable prefab registry
├── Resources/
│   └── Textures/                 — Block textures loaded at runtime
│   └── Models/
├── LevelsJson/                   — Level JSON files (4 levels)
└── Prefabs/                      — Block, exit gate prefabs
```

## Architecture
 
### Event-Driven Communication
 
Scripts communicate through C# events rather than direct references, keeping systems decoupled:
 
- `BlockBase.OnBlockMoved` → GameManager decrements move counter
- `BlockBase.OnBlockCountChanged(+1)` → GameManager increments block count on spawn
- `BlockBase.OnBlockCountChanged(-1)` → GameManager decrements block count on exit, triggers win if zero
- `GameManager.OnLevelFinished(bool)` → UIManager shows win or lose panel based on the bool value
- `GameManager.UpdateCounters(int, int)` → UIManager updates move count and level number display
- `UIManager.OnRetryClicked` → BoardLoader clears and reinitializes current level
- `UIManager.OnNextLevelClicked` → BoardLoader increments level number, clears and initializes next level

### Raycast-Based Movement

Instead of maintaining a grid array, blocks use Physics.Raycast to detect obstacles. Each block shoots rays from its edges in the movement direction and stops at the nearest hit. This approach:

- Eliminates grid state synchronization
- Makes new block shapes trivial to add (override `GetRayOrigins()`)
- Leverages Unity's optimized physics engine

### Extensible Block System

`BlockBase` provides virtual `GetRayOrigins()` which child classes override to define ray positions based on their shape. Adding a new block type (e.g., L-shape) requires only a new child class — no changes to movement, input, or spawning logic.

### Runtime Material Pipeline

`MaterialFactory` creates materials at runtime by loading textures from Resources following a naming convention (`Cube{type}{color}{orientation}TextureMap`). Materials are cached to prevent duplicate creation. Adding a new color requires only adding textures and an enum entry — no editor configuration needed.

## Levels

The game includes 4 levels parsed from JSON files:

- **Level 1** (2x2) — No move limit, 3 blocks
- **Level 2** (3x2) — No move limit, 5 blocks
- **Level 3** (3x2) — Move limited, 5 blocks
- **Level 4** (4x4) — Move limited, includes 1x2 blocks

## Third-Party Libraries

- **Newtonsoft.Json** — JSON deserialization for level data

## Unity Version

Built with Unity 6 using the Universal Render Pipeline (URP).