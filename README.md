# mike-and-conquer


This project is my attempt at building a clone of the original 1995 Command and Conquer RTS game.  I'm doing this as a personal project for my own enjoyment and education.  I don't currently have any plans to open this up as an open source project for others to contribute to.

This project is being implemented using Monogame and C# with Visual Studio 2022 as the IDE

Testing is done via an external test project, mike-and-conquer-test

In general, the intent of this project is to exactly re-implement the original game, with few if any changes to gameplay or behavior.   One big exception to that, however, is the AI.  My intent with the AI is to improve the AI and to implement an AI opponent that can reliably beat me in a one-on-one game. (Which seems like a reasonable challenge, since I'm only an average player)

The current state of the code varies.  Some of code is pretty high quality that I'm proud of and is in a good state, while some of it has been temporarily slapped together to work for now but is need of refactoring and improvement.  My planned end state is to get all of the code in pristine state, but it's not there yet


## Current work

You see the project Kanban board here:  https://github.com/users/mike-duvall/projects/1/views/2


Here is screenshot of current state:


![Screenshot 1](/mike_and_conquer_monogame/video-and-screenshots/Screenshot-1.png?raw=true "Screenshot 1")


Here is a video of the current state:

https://user-images.githubusercontent.com/4364791/230379676-e3a82c3c-4f37-4872-9bef-1b4b5769fa60.mp4


## Overall Design


mike-and-conquer consists of two high level projects:  mike_and_conquer_monogame and mike_and_conquer_simulation.


mike_and_conquer_simulation is the non-UI game engine/simulation.  It handles the gameworld and game logic.  

mike_and_conquer_monogame is the UI for the game.  It displays the games graphics and handles input from the mouse and keyboard

Communication with mike_and_conquer_simulation is done via commands and events.  Commands are used to setup game state and to communicate player input.  Sample commands would be: SetGameSpeedCommand and OrderUnitToMoveCommand.
Events are used to publish what changes to the game state.  Example events would be: UnitPositionChange and MinigunnerCreated

mike_and_conquer_monogame listens to these events and updates the graphical game state accordingly.

mike_and_conquer_monogame also translates user input into commands and sends those commands to mike_and_conquer_simulation for execution


![High Level Architecture](/docs/high-level.drawio.png?raw=true "High Level Architecture")


In addition, both mike_and_conquer_simulation and mike_and_conquer_monogame provide REST interfaces that faciliate setting up and running test scenarios via a separate test project: mike_and_conquer_test.  


Threads here...



View vs simulation.  Publishing of events from simulation.  Interface of each.

Both have REST interfaces

Breakdown of UI client vs admin client

Communication via events and commands

Both having a constantly running loop that pulls commands from the command queue

Threading and thread safety

Testing

Running headless

Determinsitic simulation


Timing of the main simulation loop (based on monogame implementation.  Pretty closely matches actual speed of real game)


Logging

