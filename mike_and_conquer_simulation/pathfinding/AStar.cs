using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;
// using Microsoft.Xna.Framework;
// using mike_and_conquer.gameworld;
// using mike_and_conquer.main;
using Boolean = System.Boolean;

using Point = System.Drawing.Point;

namespace mike_and_conquer_simulation.pathfinding
{

    public class Path
    {
        public List<Node> nodeList;

        public Path()
        {
            nodeList = new List<Node>();
        }

    }

    public class NavigationGraph
    {

        public List<Node> nodeList;
        public int width;
        public int height;
        private int currentNodeId = 0;

        private int[,] nodeArray;

        public NavigationGraph(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.nodeArray = new int[width, height];
            this.nodeList = new List<Node>();
            for (int i = 0; i < nodeArray.Length; i++)
            {
                List<int> adjacentNodes = CalculateAdjacentNodes(currentNodeId);
                Node node = new Node(currentNodeId, adjacentNodes);
                nodeList.Add(node);
                currentNodeId++;
            }


        }

  
        public void MakeNodeBlockingNode(int xInMapSquareCoordinates, int yInMapSquareCoordinates)
        {
            this.nodeArray[xInMapSquareCoordinates, yInMapSquareCoordinates] = 1;
        }


        public void RebuildAdajencyGraph()
        {
            this.nodeList = new List<Node>();
            this.currentNodeId = 0;
            for (int i = 0; i < nodeArray.Length; i++)
            {
                List<int> adjacentNodes = CalculateAdjacentNodes(currentNodeId);
                Node node = new Node(currentNodeId, adjacentNodes);
                nodeList.Add(node);
                currentNodeId++;
            }

        }


        List<int> CalculateAdjacentNodes(int nodeId)
        {

            List<int> adjacentNodes = new List<int>();
            int currentNodex = nodeId % width;
            int currentNodey = nodeId / width;

            if (IsLocationOpen(currentNodex, currentNodey))
            {
                for (int y = currentNodey - 1; y <= currentNodey + 1; y++)
                    for (int x = currentNodex - 1; x <= currentNodex + 1; x++)
                    {
                        if (IsValidLocation(x, y) && IsLocationOpen(x, y) && !(currentNodex == x && currentNodey == y))
                        {
                            int adjacentNodeIndex = y * width + x;
                            adjacentNodes.Add(adjacentNodeIndex);
                        }
                    }
            }

            return adjacentNodes;
        }


        bool IsValidLocation(int x, int y)
        {
            bool isValid = (
                x >= 0 &&
                (x <= width - 1) &&
                y >= 0 &&
                (y <= height - 1));

            return isValid;
        }

        bool IsLocationOpen(int x, int y)
        {
            bool isOpen = nodeArray[x, y] == 0;
            return isOpen;
        }

        internal void Reset()
        {
            this.nodeArray = new int[width, height];
            this.nodeList = new List<Node>();
        }


        private double Distance(double dX0, double dY0, double dX1, double dY1)
        {
            return Math.Sqrt((dX1 - dX0) * (dX1 - dX0) + (dY1 - dY0) * (dY1 - dY0));
        }


        public int Cost(int fromNodeId, int toNodeId)
        {
            // TODO:  Possible optimization would be
            // to precalculate the costs up front and store them
            // Might mean Node has to have a list of costed connections rather than
            // a list of connected node ids
            int cost = 0;

            if (toNodeId == (fromNodeId + 1))
            {
                // toNodeId is to the right
                cost = 1;
            }
            else if (toNodeId == (fromNodeId - 1))
            {
                // toNodeId is to the left
                cost = 1;
            }
            else if (toNodeId == (fromNodeId + width) )
            {
                // toNodeId is below
                cost = 1;
            }
            else if (toNodeId == (fromNodeId - width))
            {
                // toNodeId is above
                cost = 1;
            }
            else
            {
                // It's diagonal
                // Presuming it's adjacent
                cost = 2;
            }


            return cost;

        }
    }

    public class Node
    {
        public int id;
        public List<int> connectedNodes;

        public Node(int id, List<int> connectedNodes)
        {
            this.id = id;
            this.connectedNodes = connectedNodes;
        }

    }

    public class AStar
    {
        // This code uses algorithms from here:  https://www.redblobgames.com/pathfinding/a-star/introduction.html
        // Current implementation is actually "Dijkstra’s Algorithm" rather than A *


        public Path FindPath(NavigationGraph navigationGraph, Point startPoint, Point endPoint)
        {
            int startPointIndex = (startPoint.Y * navigationGraph.width) + startPoint.X;
            int endPointIndex = (endPoint.Y * navigationGraph.width) + endPoint.X;
            return this.FindPath(navigationGraph, startPointIndex, endPointIndex);
        }

        public Path FindPath(NavigationGraph navigationGraph, int startLocation, int goalLocation)

        {

            PriorityQueue<Node> frontier = new PriorityQueue<Node>();

            frontier.Enqueue(0, navigationGraph.nodeList[startLocation]);

            Dictionary<int, int> came_from = new Dictionary<int, int>();
            Dictionary<int, int> cost_so_far = new Dictionary<int, int>();

            came_from[startLocation] = -1;
            cost_so_far[startLocation] = -1;

            while (frontier.Count > 0)
            {
                Node current = frontier.Dequeue();

                foreach (int next in current.connectedNodes)
                {
                    int costToNextNode = navigationGraph.Cost(current.id, next);
                    int current_cost_so_far = 0;
                    if (cost_so_far.ContainsKey(current.id))
                    {
                        current_cost_so_far = cost_so_far[current.id];
                    }
                    int new_cost = current_cost_so_far + costToNextNode;

                    if (!cost_so_far.ContainsKey(next) || (new_cost < cost_so_far[next]))
                    {
                        cost_so_far[next] = new_cost;
                        int priority = new_cost;
                        frontier.Enqueue(priority, navigationGraph.nodeList[next]);
                        came_from[next] = current.id;
                    }
                }
            }

            Path thePath = new Path();

            Node nextNode = navigationGraph.nodeList[goalLocation];
            List<Node> nodesInReverseOrder = new List<Node>();
            Boolean moreNodes = true;
            while (moreNodes)
            {
                nodesInReverseOrder.Add(nextNode);
                try
                {
                    if (came_from[nextNode.id] != -1)
                    {
                        nextNode = navigationGraph.nodeList[came_from[nextNode.id]];
                    }
                    else
                    {
                        moreNodes = false;
                    }
                }
                catch (KeyNotFoundException e)
                {

                    SimulationMain.logger.LogInformation("KeyNotFoundException:");
                    SimulationMain.logger.LogInformation("For key:" + nextNode.id);
                    SimulationMain.logger.LogInformation("Start location:" + startLocation + "   goalLocation:" + goalLocation);

                    SimulationMain.logger.LogInformation("Dumping came_from:");
                    Dictionary<int, int>.KeyCollection keyCollection = came_from.Keys;
                    foreach (int key in keyCollection)
                    {
                        int value = came_from[key];
                        SimulationMain.logger.LogInformation("key:" + key + "  value:" + value);
                    }

                    throw e;
                }
            }




            nodesInReverseOrder.Reverse(0, nodesInReverseOrder.Count);
            thePath.nodeList = nodesInReverseOrder;
            return thePath;
        }

    }
}



