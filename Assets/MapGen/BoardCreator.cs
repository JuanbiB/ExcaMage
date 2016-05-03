using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
	//Environment
	Wall, 
	Floor, 
	Hole, 
	SpikeR, SpikeL, SpikeU, SpikeD, 
	Rock,

	//Enemies
	BasicEnemy,
	FlyingEnemy
}
	
public class BoardCreator : MonoBehaviour
{
	// Basic board stuff
	public int numRooms = 5;
	public IntRange roomWidth = new IntRange(10,10);
	public IntRange roomHeight = new IntRange(10,10);

	private int boardH = 40;
	private int boardW = 310;
	
	// Prefabs
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] holeTiles;    						  // An array of hole prefabs.
	public GameObject[] Baddies;						      // An array of enemies.
	public GameObject[] Spikes;	
	public GameObject[] Rock;
	public GameObject player;								  // The player prefab.
	public GameObject portal;

	public GameObject multikill;

	// Where we hold things
	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private Room[] rooms;                                     // All the rooms that are created for this board.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

	// How we keep track of progress
	private int curLevel = 0;
	private int curKills = 0;
	private int curStreak = 0;

	private bool killstreak = false;
	private float killstreakTimer = 3.0f;

	// So other scipts can see this
	public static BoardCreator instance = null;

	// Check is the room done
	private void Update()
	{
		//print ("Current Kills " + curKills + ", Kills needed " + +rooms [curLevel].enemyCount + " Current Level " + curLevel);
		if (curKills >= rooms[curLevel].enemyCount) { 
			spawnExit ();
			curKills = 0;
			curLevel++;
		}
	}

	private void Awake ()
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
		rooms = new Room[numRooms];
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i] = new Room ();
			rooms [i].genRoom(i,roomHeight.Random,roomWidth.Random); 
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
				// Floors
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

				//Baddies
				if (tiles[x][y] == TileType.BasicEnemy || tiles[x][y] == TileType.FlyingEnemy)
				{
					InstantiateFromArray (Baddies, x, y);
				}
					
				// Spikes
				if (tiles[x][y] == TileType.SpikeD || tiles[x][y] == TileType.SpikeL 
					|| tiles[x][y] == TileType.SpikeR || tiles[x][y] == TileType.SpikeU)
				{
					InstantiateFromArray (Spikes, x, y);
				}
			
				// Rocks
				if (tiles[x][y] == TileType.Rock)
				{
					InstantiateFromArray (Rock, x, y);
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
		int index = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, -3);

		// Board x and y of prefab
		int x = (int)Mathf.Round(position.x);
		int y = (int)Mathf.Round(position.y);

		// If we are a bad guy put the right one
		if (prefabs == Baddies) {
			if (tiles[x][y] == TileType.BasicEnemy)
				index = 0;
			if (tiles[x][y] == TileType.FlyingEnemy)
				index = 1;
		}

		// Create the prefab.
		GameObject tileInstance = Instantiate(prefabs[index], position, Quaternion.identity) as GameObject;

		if (prefabs == floorTiles) {
			Vector3 pos = new Vector3 (tileInstance.transform.position.x, tileInstance.transform.position.y, 1);
			tileInstance.transform.position = pos;
		}

		if (prefabs == holeTiles || prefabs == wallTiles  || prefabs == Spikes) {
			Vector3 pos = new Vector3 (tileInstance.transform.position.x, tileInstance.transform.position.y, 0);
			tileInstance.transform.position = pos;
		}

		// If its a spike we gotta rotate it properly
		if (prefabs == Spikes) {
			rotato (tileInstance);
		}

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}

	private void rotato(GameObject tileInstance)
	{
		int x = (int)Mathf.Round(tileInstance.transform.position.x);
		int y = (int)Mathf.Round(tileInstance.transform.position.y);
		if (tiles [x] [y] == TileType.SpikeU)
			tileInstance.transform.Rotate (0, 0, 90);
		if (tiles [x] [y] == TileType.SpikeL)
			tileInstance.transform.Rotate (0, 0, 180);
		if (tiles [x] [y] == TileType.SpikeD)
			tileInstance.transform.Rotate (0, 0, 270);
	}

	private void spawnExit ()
	{
		
		Vector3 portalPos = new Vector3 ((30*(curLevel)+10+4), 10+4,-5);
		Instantiate(portal, portalPos, Quaternion.identity);	
	}

	private void kill ()
	{
		curKills++;
		if (!killstreak) {
			StartCoroutine(killcounter());
		} else {
			curStreak++;
		}
	}

	public IEnumerator killcounter()
	{
		killstreak = true;
		curStreak = 1;
		float start = 0.0f;
		while (start < killstreakTimer)
		{
			start += Time.deltaTime;
			if (curStreak >= 3) {
				Vector3 pos = new Vector3 (player.transform.position.x, player.transform.position.y, -4);
				GameObject text = Instantiate(multikill,pos,player.transform.rotation) as GameObject;
				curStreak = 0;
			}
			yield return null;
		}
		killstreak = false;
	}
}