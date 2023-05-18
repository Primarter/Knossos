# Untitled Maze Game

This is the repository for the untitled maze game made for Rafale25 and my Master's project. It will be developped over the course of our 3rd semester in Heriot-Watt University's Games Design & Development MDes.

This game will be an exploration game centered around discovering a labyrinth and escaping it. It will work in a 3D pseudo-isometric view with a 3D pixel-art style (similar to Cloud Gardens).

The player will be dropped in a safe zone at the center of the maze. Their goal will be to leave that zone and try and find an exit to the labyrinth. They will gather whatever gear they can from the safe zone and explore the maze to find the exit. While they run through the labyrinth, they will encounter both enemies and allies. They will have to fight through those enemies or escape them to continue exploring. The allies they meet might help them enhance their weapon or their gear in order to more easily get through the maze. They will also find special rooms in which they will solve small puzzles or face special enemies to unlock more of the labyrinth or more informations about it.

The maze will shift regularly presenting itself in 3 configurations. The different configurations will give access to different resources and different parts of the labyrinth. In order to exit the labyrinth, the player will need to us those shifts to their advantage.

# Development parts

## Labyrinth

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

We will need to design the different configurations for the map. As of now, we plan to have 3 different configurations. These will need to link in the proper way in the proper places to allow for passage between each region properly.
