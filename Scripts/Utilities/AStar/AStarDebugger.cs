using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    private TileScript start, goal;
    [SerializeField] private GameObject arrowPrefab, debugTilePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
 //       ClickTile();

 //       // not error-prone -> start must be set first or else a NullReferenceException error will be thrown
 //       if (Input.GetKeyDown(KeyCode.Space))
 //       {
 //           AStar.GetPath(start.GridPosition, goal.GridPosition);
 //       }
	//}

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))    // 1 - right mouse
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        CreateDebugTile(start.WorldPosition, new Color32(255, 165, 0, 255));  // orange color
                    }
                    else if (goal == null)
                    {
                        goal = tmp;
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));  // red color
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
    {
        foreach (Node node in openList)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.cyan, node);
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in path)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);
            }
        }
    }

    private void PointToParent(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity) as GameObject;
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 2;

            // Parent to the right
            if ((node.GridPosition.x < node.Parent.GridPosition.x) && (node.GridPosition.y == node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            // Parent to the top right
            else if ((node.GridPosition.x < node.Parent.GridPosition.x) && (node.GridPosition.y > node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            // Parent to the top
            else if ((node.GridPosition.x == node.Parent.GridPosition.x) && (node.GridPosition.y > node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            // Parent to the top left
            else if ((node.GridPosition.x > node.Parent.GridPosition.x) && (node.GridPosition.y > node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            // Parent to the left
            else if ((node.GridPosition.x > node.Parent.GridPosition.x) && (node.GridPosition.y == node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            // Parent to the bottom left
            else if ((node.GridPosition.x > node.Parent.GridPosition.x) && (node.GridPosition.y < node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            // Parent to the bottom
            else if ((node.GridPosition.x == node.Parent.GridPosition.x) && (node.GridPosition.y < node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            // Parent to the bottom right
            else if ((node.GridPosition.x < node.Parent.GridPosition.x) && (node.GridPosition.y < node.Parent.GridPosition.y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
        }
    }

    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos, Quaternion.identity) as GameObject;

        if (node != null)
        {
            DebugTile tmp = debugTile.GetComponent<DebugTile>();

            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
