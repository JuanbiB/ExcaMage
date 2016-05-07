using UnityEngine;

public class Room
{
	public int level;										  // What level
	public TileType[][] roomTiles;							  // All the tiles in the room.
	public int height;										  // How tall
	public int width;										  // How wide
	public GameObject[] Baddies;						      // An array of enemies.
	public int enemyCount = 0;								  // How many guys in the room - to be replaced by Baddies

	// What is called by BoardCreator
	public void genRoom (int level, int height, int width)
	{
		this.height = height;
		this.width = width;
		this.level = level;
		Standard (level);
	}

	public void genSecondFloorRoom(int level, int height, int width)
	{
		this.height = height;
		this.width = width;
		this.level = level;
		StandardFloorTwo (level);
	}

	public void genBossRoom (int height, int width)
	{
		this.height = height;
		this.width = width;
		Boss();
		enemyCount++;
	}

	public void Boss()
	{
		SetupTilesArray (width, height);
		PutFloor();
		roomTiles[width/2][height-1] = TileType.Boss; 
	}

	// Example level
	public void Standard(int level)
	{
		// Need these
		SetupTilesArray (width, height);
		PutFloor();

		// Optional
		addBasicEnemies (1+level,2+level);
		addFlyingEnemies (1,2+level);
		addHoles (1,4);
		addSpikes (1,4);
		addRocks (1, 1);
	}

	public void StandardFloorTwo(int level)
	{
		// Need these
		SetupTilesArray (width, height);
		PutFloor();

		// Optional
		addBasicEnemies (1+level,2+level);
		addFlyingEnemies (1,2+level);
		addHoles (3,6);
		addSpikes (4,7);
		//addRocks (1, 1);
	}

	// Build the array of tiles
	void SetupTilesArray (int width, int height)
	{
		roomTiles = new TileType[width][];
		for (int i = 0; i < roomTiles.Length; i++)
		{
			roomTiles[i] = new TileType[height];
		}
	}

	// Put floor everywhere first
	void PutFloor ()
	{
		for (int x = 0; x < roomTiles.Length; x++) {
			for (int y = 0; y < roomTiles[x].Length; y++) {
				roomTiles[x][y] = TileType.Floor; 
			}
		}
	}

	public void addRocks(int min, int max)
	{
		int numRocks = UnityEngine.Random.Range (min, max+1);
		int rocksAdded = 0;
		while (rocksAdded < numRocks) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int probRock = UnityEngine.Random.Range (0, 100);
					if (probRock == 1 && roomTiles [x] [y] == TileType.Floor) {
						roomTiles [x] [y] = TileType.Rock; 
						rocksAdded++;
					}
					if (rocksAdded >= numRocks)
						break;
				}
			}
		}
	}

	// Flying dudes
	public void addFlyingEnemies(int min, int max)
	{
		int numEnemies = UnityEngine.Random.Range (min, max+1);
		int enemiesAdded = 0;
		while (enemiesAdded < numEnemies) {
			// If this bugs out it's probably the start values for x and y - a fix for the deaths on start
			for (int x = 3; x < width; x++) {
				for (int y = 3; y < height; y++) {
					int probFlying = UnityEngine.Random.Range (0, 50);
					if (probFlying == 1 && roomTiles [x] [y] == TileType.Floor) {
						roomTiles [x] [y] = TileType.FlyingEnemy; 
						enemiesAdded++;
						enemyCount++;
					}
					if (enemiesAdded >= numEnemies)
						break;
				}
			}
		}
	}

	// Regualar dudes
	public void addBasicEnemies(int min, int max)
	{
		int numEnemies = UnityEngine.Random.Range (min, max+1);
		int enemiesAdded = 0;
		while (enemiesAdded < numEnemies) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int probBasic = UnityEngine.Random.Range (0, 50);
					if (probBasic == 1 && roomTiles [x] [y] == TileType.Floor) {
						roomTiles [x] [y] = TileType.BasicEnemy; 
						enemiesAdded++;
						enemyCount++;
					}
					if (enemiesAdded >= numEnemies)
						break;
				}
			}
		}
	}

	// You know
	public void addHoles(int min, int max)
	{
		int numHoles = UnityEngine.Random.Range (min, max+1);
		int holesAdded = 0;
		while (holesAdded < numHoles) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int probHole = UnityEngine.Random.Range (0, 50);
					if (probHole == 1 && roomTiles [x] [y] == TileType.Floor) {
						roomTiles [x] [y] = TileType.Hole; 
						holesAdded++;
					}
					if (holesAdded >= numHoles)
						break;
				}
			}
		}
	}
		
	// Will only show up on walls
	public void addSpikes(int min, int max)
	{
		int numSpikes = UnityEngine.Random.Range (min, max+1);
		int spikesAdded = 0;
		while (spikesAdded < min) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int probSpike = UnityEngine.Random.Range (0, 50);
					if (x == 0 && probSpike == 1 && this.roomTiles [x] [y] == TileType.Floor) {
						this.roomTiles [x] [y] = TileType.SpikeR;
						spikesAdded++;
						if (spikesAdded >= numSpikes)
							break;
					} else if (y == 0 && probSpike == 1 && this.roomTiles [x] [y] == TileType.Floor) {
						this.roomTiles [x] [y] = TileType.SpikeU;
						spikesAdded++;
						if (spikesAdded >= numSpikes)
							break;
					} else if (x == width - 1 && probSpike == 1 && this.roomTiles [x] [y] == TileType.Floor) {
						this.roomTiles [x] [y] = TileType.SpikeL;
						spikesAdded++;
						if (spikesAdded >= numSpikes)
							break;
					} else if (y == height - 1 && probSpike == 1 && this.roomTiles [x] [y] == TileType.Floor) {
						this.roomTiles [x] [y] = TileType.SpikeD;
						spikesAdded++;
						if (spikesAdded >= numSpikes)
							break;
					}
				}
			}
		}
	}
}