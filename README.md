# Spirit-In-Me
Click [here](https://jt5519.github.io/Spirit-In-Me-Game/index.html) to play the game on a GitHub static page!

# Game Details
* Survival/Action Horror
* Hyper realistic mechanics  
* A complex and challenging enemy AI 
* Exploration driven
* Complete story (20 - 25 minutes of story time)

<img src="Snaps/1.png" width="500" alt="boo">
<img src="Snaps/2.png" width="500" alt="boo">
<img src="Snaps/3.png" width="500" alt="boo">
<img src="Snaps/4.png" width="500" alt="boo">

# Enemy AI Behavior
The enemy AI behavior is implemented by a 2 layer behavior system. Layer 1 is the state (similar to emotional state in humans) layer and is less volatile and does not change moment to moment. Layer 2 is the behavior layer that controls the moment to moment actions of the AI, and this layer is influenced by layer 1. Before explaining how the layers work, the actions that the AI can commit are mentioned below under AI Mechanics:

## Enemy AI Mechanics
### MOVEMENT
* Chase: Chases the player to attack at close range
* Hover: Hover in position
* Back-off: Try to distance itself from the player

### ATTACKS
* Ranged Attack: Enemy shoots a projectile that can curve to a certain degree to hit the player
* Melee Attacks: A horn attack with moderate range, A close range melee attack, A special attack 

### DISAPPEARING
Demon can disappear and reappear as it sees fit to escape player and to carry out special attacks.

## Layer 1 - State Layer
* Three states: Aggressive, Balanced, Distanced
* Does not change moment to moment, changes with change in factors affecting it.

Hierarchical factors affecting enemy STATE: (if two factors are true together, the factor higher in the list decides the state)
* Low Health
* High Damage taken
* Player play-style
* High Health gap
* Balancing requirement
* Player state
* Random (every 30 seconds, less volatile)

Some factors have long term effects. For example if the high damage taken factor flag is set to true but the next moment it is set to false, the AI should not just forget that it had taken high damage just a moment ago. So while the 'factor' may become false, the 'effect' once set true, only becomes false after a decided time has passed. Consequently the state of the AI is what the effect demands it to be for that entire duration. 

## Layer 2 - Behavior Layer
* Changes moment to moment
* Influenced by layer 1 STATE, similar to emotions influencing behavior

<img src="Snaps/BehaviorTree.png" width="1000" alt="boo">

That explains the enemy AI working in detial.

## Some other games by me
* [FPPPS - An FPS game](https://github.com/JT5519/First-FPS)
* [H.A.B.R.O.S.I.P - HyperActive Ball Rolls On a Seizure Inducing Platform](https://github.com/JT5519/Roller-Madness)
* [Solar System Model](https://github.com/JT5519/Solar-System)
