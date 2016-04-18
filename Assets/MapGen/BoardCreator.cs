using System.Collections;
using UnityEngine;

// The type of tile that will be laid in a specific position. Floor is laid everywhere with others on top.
public enum TileType
{
	Wall, Floor, Hole, BasicEnemy
}
	
public class BoardCreator : MonoBehaviour
{
	private IntRange numRooms = new IntRange (15, 20);         
	private int boardH = 40;
	private int boardW = 310;

	// Prefabs
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] holeTiles;    						  // An array of hole prefabs.
	public GameObject[] Baddies;						      // An array of enemies.
	public GameObject player;								  // The player prefab.
	public GameObject portal;

	// Where we hold things
	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private Room[] rooms;                                     // All the rooms that are created for this board.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

	// How we keep track of progress
	private int curLevel = 0;
	private int curKills = 0;


	public static BoardCreator instance = null;

	private void Update()
	{
		if (curKills >= rooms[curLevel].enemyCount) {
			curKills = 0;
			curLevel++;
			spawnExit ();
		}
	}

	private void Start ()
	{
		// Create the board holder.
		instance = this;
		boardHolder = new GameObject("BoardHolder");

		// Create empty board.
		SetupTilesArray ();

		// Puts Walls everywhere.
		SetWalls();

		// Creates rooms.
		createRooms();

		// Adds the rooms to our board
		addRooms ();

		// Displays the tiles.
		InstantiateTiles ();

		//Adds the player.
		createPlayer ();
	}
		
	void SetupTilesArray ()
	{
		tiles = new TileType[boardW][];
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[boardH];
		}
	}

	void SetWalls()
	{
		for (int x = 0; x < tiles.Length; x++) {
			for (int y = 0; y < tiles[x].Length; y++) {
				tiles[x][y] = TileType.Wall; 
			}
		}
	}



	void createRooms()
	{
		rooms = new Room[10];
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i] = new Room ();
			rooms [i].genRoom(i); 
		}
	}

	void addRooms()
	{
		for (int i = 0; i < rooms.Length; i++) {
			Room room = rooms [i];
			for (int x = 0; x < room.roomTiles.Length; x++) {
				for (int y = 0; y < room.roomTiles[x].Length; y++) {
					tiles [x + (i*30) + 10] [y + 10] = room.roomTiles [x] [y];
				}
			}
		}

	}
		
	void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array...
		for (int x = 0; x < tiles.Length; x++)
		{
			for (int y = 0; y < tiles[x].Length; y++)
			{
				// Floors go everywhere.
				InstantiateFromArray (floorTiles, x, y);

				// Wall
				if (tiles[x][y] == TileType.Wall)
				{
					InstantiateFromArray (wallTiles, x, y);
				}		
				// Hole
				if (tiles[x][y] == TileType.Hole)
				{
					InstantiateFromArray (holeTiles, x, y);
				}

				//Basic baddie
				if (tiles[x][y] == TileType.BasicEnemy)
				{
					InstantiateFromArray (Baddies, x, y);
				}
			}
		}
	}

	void createPlayer ()
	{
		Vector3 playerPos = new Vector3 (10, 10);
		Instantiate(player, playerPos, Quaternion.identity);
	}

	void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// If it's not a floor tile we have to put it at a lower z.
		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		if (prefabs != floorTiles) {
			Vector3 pos = new Vector3 (tileInstance.transform.position.x, tileInstance.transform.position.y, -3);
			tileInstance.transform.position = pos;
		}

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}

	private void spawnExit ()
	{
		Vector3 portalPos = new Vector3 ((20+(30*curLevel-1)), 10);
		Instantiate(portal, portalPos, Quaternion.identity);	
	}

	private void kill ()
	{
		curKills++;
	}
}