using UnityEngine;

public class Room
{
	public int level;										  // What level
	public TileType[][] roomTiles;							  // All the tiles in the room.
	public int height;
	public int width;
	public GameObject[] Baddies;						      // An array of enemies.
	public int enemyCount = 0;

	public void genRoom (int level)
	{
		this.level = level;
		Standard (level);
	}
		
	public void Standard(int level)
	{
		width = UnityEngine.Random.Range(9, 11);
		height = UnityEngine.Random.Range(9, 11);

		SetupTilesArray (width, height);
		PutFloor();

		int numHoles = 0;
		int numEnemyMax = (int) Mathf.Round((2+level*1));
		int numFlyingEnemy = 0;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int probHole = UnityEngine.Random.Range(0, 35);

				int probBasicEnemy = UnityEngine.Random.Range(0, 20);
				int probFlyingEnemy = UnityEngine.Random.Range(0, 35);

				if (probHole == 1 && roomTiles[x][y] == TileType.Floor) {
					roomTiles [x] [y] = TileType.Hole; 
					numHoles++;
				}
				if (probBasicEnemy == 1 && enemyCount <= numEnemyMax && roomTiles[x][y] == TileType.Floor) {
					roomTiles [x] [y] = TileType.BasicEnemy;
					enemyCount++;
				}
				if (probFlyingEnemy == 1 && enemyCount <= numEnemyMax && roomTiles[x][y] == TileType.Floor) {
					roomTiles [x] [y] = TileType.FlyingEnemy;
					enemyCount++;
				}
			}
		}
		if (numHoles == 0) {
			this.roomTiles [(Mathf.RoundToInt(width/2))] [(Mathf.RoundToInt(height/2))] = TileType.Hole;
		}
		addSpikes (1,4);
		roomTiles [1] [1] = TileType.Rock;
	}

	void SetupTilesArray (int width, int height)
	{
		roomTiles = new TileType[width][];
		for (int i = 0; i < roomTiles.Length; i++)
		{
			roomTiles[i] = new TileType[height];
		}
	}

	void PutFloor ()
	{
		for (int x = 0; x < roomTiles.Length; x++) {
			for (int y = 0; y < roomTiles[x].Length; y++) {
				roomTiles[x][y] = TileType.Floor; 
			}
		}
	}
		
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