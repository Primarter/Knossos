# Knossos

This is the repository for the *Knossos* game made for Rafale25 and my Master's project. It will be developped over the course of our 3rd semester in Heriot-Watt University's Games Design & Development MDes.

This game will be an exploration game centered around discovering a labyrinth and escaping it. It will work in a 3D pseudo-isometric view with a 3D pixel-art style (similar to Cloud Gardens).

The player will be dropped in a safe zone at the center of the maze. Their goal will be to leave that zone and try and find an exit to the labyrinth. They will gather whatever gear they can from the safe zone and explore the maze to find the exit. While they run through the labyrinth, they will encounter both enemies and allies. They will have to fight through those enemies or escape them to continue exploring. The allies they meet might help them enhance their weapon or their gear in order to more easily get through the maze. They will also find special rooms in which they will solve small puzzles or face special enemies to unlock more of the labyrinth or more informations about it.

The maze will shift regularly presenting itself in 3 configurations. The different configurations will give access to different resources and different parts of the labyrinth. In order to exit the labyrinth, the player will need to us those shifts to their advantage.

# Development parts

## Labyrinth

### Layout

We will need to design the different configurations for the map. As of now, we plan to have 3 different configurations. These will need to link in the proper way in the proper places to allow for passage between each region properly. We will need to include in the design what differentiates the three configurations. This could be resources, enemy types, puzzle mechanics.

We will need to make sure players need to go through the 3 different configurations to get out of the labyrinth. One way of achieving this is having "levels" in the configurations. Say we name the configurations A, B and C; with levels being a number associated with the letters, e.g. A1, A2, C3, B1, etc. We can make it so that A2 is only accessible from B1 or C1, that C3 is only accessible from B2 or A2. When going up a level, you access part of a configuration you couldn't get to using this configuration's lower level. In order to make it slightly simpler and actually forcing players to go through the 3 different configurations, we would force them to go a certain way:

- A1 -> B2 -> C3
- B1 -> C2 -> A3
- C1 -> A2 -> B3

This only leaves a limited amount of ways to escape the labyrinth and simplifies its design.

![SCHEMATIC](schematic.png)

### Special zones:

- Puzzle zones (water puzzle, lever puzzle, crates puzzle): gives access to specific reward or new zone
- Fight zones: Similar to combat rooms in Isaac (take the reward to trigger enemies) or just filled with enemies and stealth zones to get through it

### Layout shifting zones:

If you stay in those rooms overnight, the level layout changing gives you access to new parts of the labyrinth. You need to get through 2 of those to get through to the exit. In order to stay in the room safely and pass the night, the player will need to pull a lever and beat the enemy wave it triggers.

### End zone and key zone:

We plan on having a key made of 4 parts that the player will need to gather to actually get out of the labyrinth. This means that we will have to add in the labyrinth the different zones needed for that. Combining them with the layout shifting zones might be a good incentive to get the player to try and stay in the zones.

### Start zone:

- NPCs:
    - Improve melee weapon
    - Improve armour
    - Improve special weapon?
- Item refills
- Respawn device (the reason you get back there when you lose)
- Day/Night door (timeskip device)

## Enemies

### M. X

This enemy will draw inspiration from the design of M. X from Resident Evil: invincible and unrelenting. The goal is to have it be triggered by specific player actions and patrol around during night (making the maze much more dangerous during the night). Pulling a lever, detonating a grenade, using a shotgun, letting a specific enemy detonate rather than killing it... This would all play in the "forbidden action" trope.

When it is triggered, the enemy will try and search for the player. The player will have to hide and if they manage to stay out of detection long enough, the enemy will go into a predictable patrol mode, allowing the player to safely escape. If the player is detected, they will be able to fight the enemy to slow it down and try and hide or get away.

### Regular enemies

While M. X can be a great antagonist, we will still need smaller enemies for the player to face more regularly. Those enemies will work the same way the big antagonist does: patrol, attack, search. On the other hand, they will be perfectly killable and it will be up to the player whether they want to try and avoid them or not.

Hades (2018, Supergiant Games) had wonderful enemies balance wise and it gave us inspiration for different archetypes we could use and adapt to our project:

- small, fast but very low health; dashes towards the player to attack and does so with relatively low cooldown
- big, big health and slow; dashes quickly and far towards the player with a higher cooldown than the smaller enemies
- big, big health and slow; attacks from close range with medium cooldown
- small, low health; slow ranged attacks from far away

## Player

The player will only move horizontally on the map, no verticality. They will also be able to roll. The roll will be primarily used in combat to avoid enemy attacks and will make the player invincible. The camera will use a point of view similar to Tunic or Hades: high angle with a wide field of view. It won't move much so the player gets an instinctive feeling of up, down, left or right. This will help them navigate the maze without getting lost as they will have constant bearings.

Going back on the combat, the minimum for the combat will be to have a basic melee combo performed with successive presses of a single button. The combat will follow a simple hack and slash style. We would like to introduce maybe different weapons to the player so they can switch their play style; however, keeping melee combat as the main focus will allow us to make ranged combat significantly different. Ranged combat could appear in the form of a firearm. As mentioned earlier, the player doesn't have a lot of resources. This means firing will be expensive to the player. Also, firing a firearm being particularly loud, it would attract the "M. X" enemy to the player. It could alternatively take the form of a grenade, in order to make it more powerful. We will need to see whether this can fit in our scope later.

During our outside the combat, the player will probably need to heal in some way. This will be introduced via using an item such as bandages. If we make the system properly, this would also allow us to introduce different items later on if both scope and design allow us to.

## Map

We will need the map to reflect the different configurations and allow good visualisation of each. This could mean overlaying the three configurations, having three separate maps, having good landmarks for the player to know where they're going, etc.

The player will need to fill in the map by exploring the labyrinth. The initial idea was that going back to the center of the maze allows the player to save the exploration progression and that if they lose, they also lose part of what they managed to explore. This would put some more pressure on the player, making death more punitive and thus more menacing.

The map would also display the player's position and the way they are facing at all times. This will be very important to make the maze less a hassle to get through, especially if players get lost. It should also display symbols where the special zones are and what kind of zone they are. Since players need to go through 3 special zones, the zones could change according to the cycle in order to give the player something to choose from when planning their route out. On the same screen could also be displayed player info, such as health, inventory, etc.

## Art

We will be using 3D pixel-art. Using software like Blender and Blockbench, making the models will not be too hard. We need to devise a way to identify the three different configurations: symbols, colours, etc.

Sound design will be very important as we want the music not to be the biggest focus of the game. This means we will try to have sound design for pretty much any interaction with the environment: triggers, walking, running, enemies, fighting and environmental sounds.

## Polish

UI & menus

Control mapping/remapping

Music:
- ambiance: start, labyrinth, end, being searched, near patrolling enemy
- fighting: regular enemies, events, big enemy

Save system

[Accessibility options](https://gameaccessibilityguidelines.com/)