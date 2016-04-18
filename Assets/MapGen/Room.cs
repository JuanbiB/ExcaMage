using UnityEngine;

public class Room
{
	public int level;										  // What level
	public TileType[][] roomTiles;								  // All the tiles in the room.
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
		int width = UnityEngine.Random.Range(8, 16);
		int height = UnityEngine.Random.Range(8, 16);

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
	}

//	public void Bats(TileType[][] tiles)
//	{
//	}
//	public void Turrets(TileType[][] tiles)
//	{
//	} 
}