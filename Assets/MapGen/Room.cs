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
		Standard ();
//		// First 5 are ez.
//		if (level < 5) {
//			Standard();
//		}
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
		
	public void Standard()
	{
		width = UnityEngine.Random.Range(8, 16);
		height = UnityEngine.Random.Range(8, 16);

		SetupTilesArray (width, height);
		PutFloor();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int probHole = UnityEngine.Random.Range(0, 30);
				int probBasicEnemy = UnityEngine.Random.Range(0, 20);
				if(probHole == 1)
					roomTiles[x][y] = TileType.Hole; 
				if (probBasicEnemy == 1) {
					roomTiles [x] [y] = TileType.BasicEnemy;
					enemyCount++;
				}
			}
		}
		addSpikes (30);
	}

//	public void Bats(TileType[][] tiles)
//	{
//	}
//	public void Turrets(TileType[][] tiles)
//	{
//	} 

	public void addSpikes(int prob){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int probSpike = UnityEngine.Random.Range (0, prob);
				if (x == 0 && probSpike == 1) {
				//	Debug.Log("spiker");
					this.roomTiles [x] [y] = TileType.SpikeR;
				} else if (y == 0 && probSpike == 1) {
				//	Debug.Log("spikeu");
					this.roomTiles [x] [y] = TileType.SpikeU;
				} else if (x == width - 1 && probSpike == 1) {
				//	Debug.Log("spikel");
					this.roomTiles [x] [y] = TileType.SpikeL;
				} else if (y == height - 1 && probSpike == 1) {
				//	Debug.Log("spiked");
					this.roomTiles [x] [y] = TileType.SpikeD;
				}
			}
		}
	}
}