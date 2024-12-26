# Parkour Simulator
## Project Overview
This Unity project is a parkour simulator where players try to get the best time possible. Players can start a new game or view their statistics on the scoreboard by entering their username. The game tracks player data such as completion times and the positions of where the player died. The data is aggregated through a Firebase database.

## How to Run
1. Open the project in Unity (version 2022.1 or later).
2. Open the `Scenes/Main.unity` scene.
3. Press "Play" to start the game.

## How to Develop
Follow these steps to contribute to or modify the project:
### Prerequisites
 - Unity 2022.1 or later.
- Basic knowledge of C# and Unity workflows.
- Firebase account.
### Development Setup
1. Clone the [**repository**](https://github.com/EHB-MCT/assignment-2-Maxime122004/tree/feature/firebase-database).
2. **Scene Navigation**: Work on gameplay on the `Scenes/`
### Key Development Tasks
- **Add Parkour Obstacles**:
   - Create new obstacle prefabs under `Level/Prefabs`.
   - Add the prefab to the `Scenes/Level.unity` and set up colliders and scripts as necessary.
- **Modify or Improve Gameplay Mechanics**:
   - Update player movement and mechanics in the `PlayerMovement` script located in `Assets/Scripts`.
- **Enhance UI**:
   - Update home screen or scoreboard UI by editing sprites in the `Assets/Art/Sprites` folder.
- **Implement New Features**:
   - Write new Scripts in the `Assets/Scripts` and attach them to relevant GameObjects.
- **Debugging**:
   - Use Unity's Play mode and the console to test changes.
   - Chech Firebase logs for database integration debugging.
### Testing
1. **Playtest Changer**:
   - Run the game in Unity's Play mode to verify changes.
2. **Verify Firebase Integration**:
   - Check the Firebase console for new data entries when testing scoreboard functionality.
### Additional Contribution
For any further contribution, please check the `CONTRIBUTING.md`.

## Gameplay Features
- **First-Person Controller**: Smooth movement and mouse-controlled camera.
- **Stopwatch**: Tracks the time it takes the player to finish the parkour.
- **Scoreboard**: Allows players to enter their username and view their stats.

## Folder Structure
- `Assets/`: All assets such as prefabs, scripts, models, and UI elements.
  - `Animations/`: Contains all animations.
  - `Art/`: Contains fonts, materials, models, sprites and textures.
  - `Audio/`: Contains music and sound effects.
  - `DatabaseAssets/`: Contains all Firebase assets.
  - `Level/`: Contains Unity scene files and prefabs.
  - `Shaders/`: Contains Renderer files and post processing.
  - `Scripts/`: Game logic scripts.
  - `StreamingAssets/`: Has json file to ensure connection with database.
- `Packages/`: Contains all Unity packages used in project.

source: [Unity-Folder-Structure](https://unity.com/how-to/organizing-your-project)

## Documentation
All logic concerning conventions, folder structure, data flowchart and database structure are documented in `docs/`.

## Key Scripts
- `PlayerScript`: Manages player movement, camera control, jumping, and finish line interactions.
- `Stopwatch`: Contains start and stop functions to make a stopwatch for the parkour.
- `DatabaseManager`: Saves and gets player data in firebase database.
- `HomescreenManager`: Manages connection for inputfields and text between homescreen and databasemanager.
- `GameManager`: Contains script to open Homescreen scene for button onClick event.

## References
- **Code Assistance**: ChatGPT was used to help debug and refine the code during development.^[ChatGPT conversation](https://chatgpt.com/share/67434de9-c530-8007-9322-1a3c632d0040)
- **Firbase Database Implementation**: [Video tutorial](https://youtu.be/59RBOBbeJaA?si=Awg7M49yIwssi3Pe) helped implementing Firebase database into the Unity project.
- **Stopwatch Implementation**: [Video tutorial](https://youtu.be/b1ONIoDfUes?si=D8reLyBQeEi6FeNz)

## Attribution & License
This project is developed by **Maxime Bastien**.

This project is licensed under the MIT License. See the full license text in the LICENSE.md file.