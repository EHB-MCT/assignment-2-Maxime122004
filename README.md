# Unity Race Game Project
## Project Overview
This Unity project implements a first-person movement system with jump mechanics and integrates Unity Analytics for tracking player events, such as jump counts. It also includes interactive features like a finish line that displays UI feedback when triggered.

## How to Run
1. Open the project in Unity (version 2022.1 or later).
2. Open the `Scenes/Main.unity` scene.
3. Press "Play" to start the game.

## Features
- **First-Person Controller**: Smooth movement and mouse-controlled camera.
- **Jump Mechanics**: Tracks jump counts and records data to Unity Analytics.
- **Finish Interaction**: A finish pole triggers a UI message when touched.
- **Unity Analytics Integration**: Tracks and sends custom events to the Unity Analytics dashboard.


## Folder Structure
- `Assets/`: All assets such as prefabs, scripts, models, and UI elements.
  - `Animations`: Contains all animations.
  - `Art`: Contains fonts, materials, models sprites and textures.
  - `Audio`: Contains music and sound effects.
  - `Level/`: Contains Unity scene files and prefabs.
  - `Post Processing`: Has post processing profiles used for cameras.
  - `Scripts/`: Game logic scripts.
- `Packages`: Contains all packages used in project.

source: [Unity-Folder-Structure](https://unity.com/how-to/organizing-your-project)

## Key Scripts
- `FirstPersonMovement`: Manages player movement, camera control, jumping, and finish line interactions.
- `AnalyticsScript`: Implements Unity Analytics functionality to track custom player events, such as jump counts.

## How Unity Analytics Is Used
Unity Analytics is used to record and monitor custom events. The player’s jump count is tracked and sent to the analytics dashboard with the event name "jump" and a `jump_index` parameter indicating the current jump count.

### Viewing Analytics Data
1. Log in to the [Unity Dashboard](https://cloud.unity.com/home/organizations/1374483532186/projects/f280becc-6a37-4842-b207-9507da68ee32/environments/b37097e3-0e87-4859-b56e-391c917411c5/analytics/v2/dashboards/game-performance).
2. Navigate to Analytics > Events.
3. Verify that the "jump" event appears along with the jump count data.

## References
- **Unity Analytics Implementation**: Video tutorial by Code Monkey
- **Code Assistance**: ChatGPT was used to help debug and refine the code during development.

## Attribution & License
This project is developed by **Maxime Bastien**.

All code is MIT licensed. Please review any third-party assets' licenses in the `Assets/` folder for
proper attribution.