# Progress Log
## Overview
This file tracks the development progress of the Prakour Simulator game project, including milestones, updates, challenges, and solutions.

## Updates
**18-11-2024**
- Set up Unity project with organized folder structure.
- Created a simple scene with a player capsule and camera setup.

**21-11-2024**
- Implemented basic movement controls for the player.
- Added mouse-controlled camera movement.

**23-11-2024**
- Added jumping mechanics using a raycast for ground detection.
- Fixed initial bugs with ray length, which caused isGrounded to always return false.

**24-11-2024**
- Implemented finish pole interaction with a text-based UI on collision.
- Integrated Unity Analytics using a [tutorial](https://www.youtube.com/watch?v=FGTJ3bLCBbA).
- Verified custom event data on the Unity Analytics dashboard.
- Updated documentation (README.md) and attribution for tutorial references and assistance.

**18-12-2024**
- gitignore updated to ensure firebase packages with heavy file size won't be commited.
- Implemented Firebase to replace Unity Analytics using this [tutorial](https://youtu.be/59RBOBbeJaA?si=ED6AXgEqprjxfqhh).
- Tracked jumpAmount with realtime database on Firebase.
- Stopwatch script an UI text created to make a stopwatch to get player completion time of the parkour.
- Added completion time and respawn amount to be tracked in database.
- Homescreen scene added so player can create a user. 

**19-12-2024**
- Scoreboard implemented on homescreen for the player to check their stats (e.g., jumpAmount, time, respawnAmount).
- UI panel created and shown at end of parkour for player to see their stats.

**21-12-2024**
- Environment added to act as respawner when player collides with it.

**22-12-2024**
- SaveBestTime() function implemented to only add the time if it is better that what's currently in the database.
- Every player position on death is tracked in database.

**23-12-2024**
- Homescreen scoreboard updated to show best time and amount of total deaths.
- Connection restored between homescreen and database after player went back to homescreen after finishing parkour.

**24-12-2024**
- Readme documentation updated and MIT licence added in LICENSE.md.

**25-12-2024**
- Contributing.md documentation updated.