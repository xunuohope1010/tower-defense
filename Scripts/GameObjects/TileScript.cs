using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

    public Point GridPosition { get; private set; } // a property, not a reference variable

    public bool IsEmpty { get; set; }

    private Tower myTower;

    private Color32 fullColor = new Color32(255, 118, 118, 255), emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    public bool Walkable { get; set; }

    public bool AStarDebugging { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), 
                transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    // Use this for initialization
    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        Walkable = true;
        IsEmpty = true;
        GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);    // attaches tile prefabs onto a designated folder with a reference to that folder's transform
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()  // executed whenever the mouse pointer is over the tile (remember, the TileScript is attached to the tiles)
    {
        // only instantiate a tower if the user clicks on a tile not behind a button (which is considered a game object by IsPointerOverGameObject())
        // and if an empty tile and tower button has been clicked, in the case where the button clicked has been determined
        if (!EventSystem.current.IsPointerOverGameObject() && Game_Manager.Instance.ClickedBtn != null)
        {  
            //color change (if not doing AStar Debugging) or place a tower 
            if (IsEmpty && !AStarDebugging)
            {
                ColorTile(emptyColor);
            }

            if (!IsEmpty && !AStarDebugging)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))    // 0 - left mouse
            {
                PlaceTower();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && Game_Manager.Instance.ClickedBtn == null && Input.GetMouseButton(0))    // if the mouse is not over another game object and a tower has not been bought and the current tower that the mouse is hovering over has been clicked
        {
            if (myTower != null)    // if clicked on a tower on it
            {
                Game_Manager.Instance.SelectTower(myTower);
            } 
            else  // if clicked on something besides a tower
            {
                Game_Manager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()  // executed whenever the mouse pointer leaves the tile (remember, the TileScript is attached to the tiles)
    {
        if (!AStarDebugging)
        {
            ColorTile(Color.white); // default shading color of sprites
        }
    }

    private void PlaceTower()
    {
        Walkable = false;
        if (AStar.GetPath(LevelManager.Instance.BlueSpawn, LevelManager.Instance.RedSpawn) == null)
        {
            // there is no path for the monster
            Walkable = true;
            return;
        }

        GameObject tower = Instantiate(Game_Manager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.y;
        tower.transform.SetParent(transform);

        myTower = tower.transform.GetChild(0).GetComponent<Tower>();    // need to get the child here because the tower script is sitting on the range, which is the child of each tower

        IsEmpty = false;

        ColorTile(Color.white); // default shading color of sprites

        myTower.Price = Game_Manager.Instance.ClickedBtn.Price;

        Game_Manager.Instance.BuyTower();   // also sets the current button selected null so that the user cannot place an infinite number of towers after selecting a button 

        Walkable = false;
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
