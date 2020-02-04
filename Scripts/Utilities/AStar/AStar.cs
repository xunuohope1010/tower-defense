using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bean;
using UnityEngine;

public static class AStar
{
    /*
     * nodes is dict, includes Point(key), Node(value)
     */
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values) // attach a node to each tile in the game
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }
    /*
     * generate path (main part)
     * giving start point/end point
     */
    public static Stack<Node> GetPath(Point start, Point goal)    // TODO - return something
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>(), closedList = new HashSet<Node>();
        /*
         * finalPath, stack, init
         */
        Stack<Node> finalPath = new Stack<Node>();  // class that adds data values in a stacking fashion, enables backtracking without recursion
        /*
     * nodes is dict, includes Point(key), Node(value)
         * start(Point, key)
         * nodes[starts] means Node
     */
        Node currentNode = nodes[start];
        openList.Add(currentNode);  // add the start node to the open list
        /*
         * AStar
         */
        while (openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    /*
                     * neighborPos(Point)
                     */
                    Point neighborPos = new Point(currentNode.GridPosition.x + x, currentNode.GridPosition.y + y); ;

                    // look at the node's neighbors, ignore unwalkable nodes
                    if (LevelManager.Instance.InBounds(neighborPos) && LevelManager.Instance.Tiles[neighborPos].Walkable && neighborPos != currentNode.GridPosition)
                    {
                        int gCost = 0;

                        if (Math.Abs(x - y) == 1) // neighbor is adjacent to the current node
                        {
                            gCost = 10;
                        }
                        else  // Math.Abs(x-y) == 0 neighbor is not adjacent to (diagonal) the current node
                        {
                            if (!ConnectedDiagonally(currentNode, nodes[neighborPos]))  // skip open diagonals that cross an opposing diagonal of unwalkable tiles (tiles with towers placed on them)
                            {
                                continue;
                            }
                            gCost = 14;
                        }

                        Node neighbor = nodes[neighborPos];

                        if (openList.Contains(neighbor))
                        {
                            if (currentNode.G + gCost < neighbor.G) // if the current node is a better parent (G scores lower if the current node is a parent?)
                            {
                                neighbor.CalcValues(currentNode, nodes[goal], gCost);   // set current node as parent and recalculate all neighboring F, G, and H values in respect to the new parent
                            }
                        }
                        else if (!closedList.Contains(neighbor)) // ignoring nodes on the closed list
                        {
                            openList.Add(neighbor);
                            neighbor.CalcValues(currentNode, nodes[goal], gCost);   // add undiscovered nodes to the open list
                        }
                    }
                }
            }

            // move the current node from the open to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0) // select the node with the lowest F score
            {
                currentNode = openList.OrderBy(n => n.F).First();   // sorts the list by F value in ascending order and selects the first node 
            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    /*
                     * print x,y of currentNode, many currentNodes
                     */
                    // Debug.Log(currentNode.GridPosition.x+" "+currentNode.GridPosition.y);

                    currentNode = currentNode.Parent;
                }
                /*
                 * finalPath, return
                 */
                
                return finalPath;
            }
        }

        /*
         * end of AStar
         */
        return null;
        /****THIS IS ONLY FOR DEBUGGING NEEDS TO BE REMOVED LATER!****/
        // GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPosition - currentNode.GridPosition, first = new Point(currentNode.GridPosition.x + direction.x, currentNode.GridPosition.y), second = new Point(currentNode.GridPosition.x, currentNode.GridPosition.y + direction.y);

        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].Walkable)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].Walkable)
        {
            return false;
        }
        return true;
    }
}
