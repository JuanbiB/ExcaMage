using System.Collections;
using UnityEngine;

// The type of tile that will be laid in a specific position. Floor is laid everywhere with others on top.
public enum TileType
{
	Wall, Floor, Hole, Enemy
}
	
public class BoardCreator : MonoBehaviour
{
	void Update()
	{
		Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 cameraPos = new Vector3 (playerPos.x, playerPos.y, -8);
		Camera.main.transform.position = cameraPos;
	}
		
	// Map size
	public int columns = 100;                                 // The number of columns on the board (how wide it will be).
	public int rows = 100;                                    // The number of rows on the board (how tall it will be).

	// Room and Corridor dimentions 
	public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
	public IntRange roomWidth = new IntRange (3, 10);         // The range of widths rooms can have.
	public IntRange roomHeight = new IntRange (3, 10);        // The range of heights rooms can have.
	public IntRange corridorLength = new IntRange (6, 10);    // The range of lengths corridors between rooms can have.

	// Prefabs
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
	public GameObject[] holeTiles;    						  // An array of hole prefabs.
	public GameObject[] Baddies;						      // An array of enemies.
	public GameObject player;								  // The player prefab.

	// Where we hold things
	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private Room[] rooms;                                     // All the rooms that are created for this board.
	private Corridor[] corridors;                             // All the corridors that connect the rooms.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

	private void Start ()
	{
		// Create the board holder.
		boardHolder = new GameObject("BoardHolder");

		// Make board.
		SetupTilesArray ();

		// Section off everything.
		CreateRoomsAndCorridors ();

		// Put stuff down
		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		// Make it show up in game.
		InstantiateTiles ();
		InstantiateOuterWalls ();

		// Add player.
		createPlayer (); 
	}
		
	void createPlayer ()
	{
		Vector3 playerPos = new Vector3 (rooms[0].xPos, rooms[0].yPos, -3);
		Instantiate(player, playerPos, Quaternion.identity);
	}

	void SetupTilesArray ()
	{
		tiles = new TileType[columns][];
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[rows];
		}
	}
		
	void CreateRoomsAndCorridors ()
	{
		// Create the rooms array with a random size.
		rooms = new Room[numRooms.Random];

		// There should be one less corridor than there is rooms.
		corridors = new Corridor[rooms.Length - 1];

		// Create the first room and corridor.
		rooms[0] = new Room ();
		corridors[0] = new Corridor ();

		// Setup the first room, there is no previous corridor so we do not use one.
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		// Setup the first corridor using the first room.
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		for (int i = 1; i < rooms.Length; i++)
		{
			// Create a room.
			rooms[i] = new Room ();

			// Setup the room based on the previous corridor.
			rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			// If we haven't reached the end of the corridors array...
			if (i < corridors.Length)
			{
				// ... create a corridor.
				corridors[i] = new Corridor ();

				// Setup the corridor based on the room that was just created.
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}
		}
	}
		
	void SetTilesValuesForRooms ()
	{
		// Go through all the rooms...
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];


			currentRoom.SetupRoom (tiles, i);


			// ... and for each room go through it's width.
			for (int x = 0; x < currentRoom.roomWidth; x++)
			{
				int xCoord = currentRoom.xPos + x;
				// For each horizontal tile, go up vertically through the room's height.
				for (int y = 0; y < currentRoom.roomHeight; y++)
				{
					int yCoord = currentRoom.yPos + y;
					// The coordinates in the jagged array are based on the room's position and it's width and height.
					int pitProb = UnityEngine.Random.Range(0, 16);
					int rand1 = UnityEngine.Random.Range(0, 25);
					if (pitProb == 5) {
						tiles [xCoord] [yCoord] = TileType.Hole;
					} else if (rand1 == 4) {
						tiles [xCoord] [yCoord] = TileType.Enemy;
					} else {
						tiles [xCoord] [yCoord] = TileType.Floor;
					}
				}
			}
		}
	}
		
	void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
				case Direction.North:
					yCoord += j;
					break;
				case Direction.East:
					xCoord += j;
					break;
				case Direction.South:
					yCoord -= j;
					break;
				case Direction.West:
					xCoord -= j;
					break;
				}
				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}


	void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array...
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				// Floors go everywhere.
				InstantiateFromArray (floorTiles, i, j);

				// Wall
				if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					InstantiateFromArray (wallTiles, i, j);
				}
			
				// Hole
				if (tiles[i][j] == TileType.Hole)
				{
					// ... instantiate a hole over the top.
					InstantiateFromArray (holeTiles, i, j);
				}

				if (tiles[i][j] == TileType.Enemy)
				{
					// ... instantiate a hole over the top.
					InstantiateFromArray (Baddies, i, j);
				}
			}
		}
	}
		
	void InstantiateOuterWalls ()
	{
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}
		
	void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		float currentY = startingY;
		while (currentY <= endingY)
		{
			InstantiateFromArray(outerWallTiles, xCoord, currentY);
			currentY++;
		}
	}

	void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		float currentX = startingX;
		while (currentX <= endingX)
		{
			InstantiateFromArray (outerWallTiles, currentX, yCoord);
			currentX++;
		}
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
}