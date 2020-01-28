using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    // this is a property, which retains encapsulation for public fields
    public float TileSize  
    {
        get { return tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    [SerializeField] private GameObject[] tiles;    
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Transform map;

    private Point blueSpawn, redSpawn;
    [SerializeField] private GameObject bluePortalPrefab, redPortalPrefab;

    public Portal BluePortal { get; set; }

    private Point mapSize;

    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));  // a stack of paths for each monster to use
        }
    }

    public Dictionary<Point, TileScript> Tiles { get; set; }

    public Point BlueSpawn
    {
        get
        {
            return blueSpawn;
        }
    }

    public Point RedSpawn
    {
        get
        {
            return redSpawn;
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length, mapY = mapData.Length;

        mapSize = new Point(mapX, mapY);

        Vector3 maxTile = Vector3.zero, worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)); // starts at the top-left corner of the screen

        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);  // places the tile in the world
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;  // using the last tile generated (on the lower right hand corner of the screen) and added into the tiles dictionary property to figure out the camera bound limits

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);    // converts string tileType to integer version

        TileScript newTile = Instantiate(tiles[tileIndex]).GetComponent<TileScript>();
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0f), map);
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("LevelData") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    private void SpawnPortals()
    {
        // instantiate the blue portal
        blueSpawn = new Point(0, 0);    // hard-coded spawn value
        GameObject tmp = Instantiate(bluePortalPrefab, Tiles[BlueSpawn].WorldPosition, Quaternion.identity);    // Quaternion.identity maintains current default rotation
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        // instantiate the red portal
        redSpawn = new Point(11, 6);    // hard-coded spawn value
        Instantiate(redPortalPrefab, Tiles[redSpawn].WorldPosition, Quaternion.identity);    // Quaternion.identity maintains current default rotation
    }

    public bool InBounds(Point position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < mapSize.x && position.y < mapSize.y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(BlueSpawn, RedSpawn);
    }
}