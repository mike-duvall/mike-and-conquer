﻿
using System.Collections.Generic;
using mike_and_conquer.pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AStar = mike_and_conquer.pathfinding.AStar;
using Path = mike_and_conquer.pathfinding.Path;
using Node = mike_and_conquer.pathfinding.Node;

using Point = Microsoft.Xna.Framework.Point;

namespace unit_tests
{
    [TestClass]
    public class AStarUnitTests
    {



        [TestMethod]
        public void BasicPathfindingTest1()
        {
            // given
            AStar aStar = new AStar();

            // and
//            int[,] nodeArray = new int[3, 3];
//          S, 0, 0
//          0, 0, 0
//          0, 0, E


//            NavigationGraph navigationGraph = new NavigationGraph(nodeArray);
            NavigationGraph navigationGraph = new NavigationGraph(3, 3);

            // when

            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(2, 2);

//            Path foundPath = aStar.FindPath(navigationGraph,0,8);
            Path foundPath = aStar.FindPath(navigationGraph, startPoint, endPoint);

            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 3);

            Assert.IsTrue(foundPath.nodeList[0].id == 0);
            Assert.IsTrue(foundPath.nodeList[1].id == 4);
            Assert.IsTrue(foundPath.nodeList[2].id == 8);


        }


        [TestMethod]
        public void BasicPathfindingTest2()
        {
            // given
            AStar aStar = new AStar();

            // and
//            int[,] nodeArray = new int[3, 3];
//          0, 0, 0
//          0, 1, 0
//          0, 0, 0

//            nodeArray[1, 1] = 1;

//            NavigationGraph navigationGraph = new NavigationGraph(nodeArray);
            NavigationGraph navigationGraph = new NavigationGraph(3, 3);

            navigationGraph.MakeNodeBlockingNode(1,1);
            navigationGraph.RebuildAdajencyGraph();

            // when
            Path foundPath = aStar.FindPath(navigationGraph, 0, 8);

            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 4);

            Assert.IsTrue(foundPath.nodeList[0].id == 0);
            Assert.IsTrue(foundPath.nodeList[1].id == 1);
            Assert.IsTrue(foundPath.nodeList[2].id == 5);
            Assert.IsTrue(foundPath.nodeList[3].id == 8);


        }


        [TestMethod]
        public void BasicPathfindingTest3()
        {
            // given
            AStar aStar = new AStar();

            // and
//          0, 0, 1
//          0, 1, 1
//          0, 0, 0

//            int[,] nodeArray = new int[3, 3];
//
//            nodeArray[2, 0] = 1;
//            nodeArray[1, 1] = 1;
//            nodeArray[2, 1] = 1;


//            NavigationGraph navigationGraph = new NavigationGraph(nodeArray);
            NavigationGraph navigationGraph = new NavigationGraph(3, 3);
            navigationGraph.MakeNodeBlockingNode(2, 0);
            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.RebuildAdajencyGraph();

            // when
            Path foundPath = aStar.FindPath(navigationGraph, 0, 8);

            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 4);

            Assert.IsTrue(foundPath.nodeList[0].id == 0);
            Assert.IsTrue(foundPath.nodeList[1].id == 3);
            Assert.IsTrue(foundPath.nodeList[2].id == 7);
            Assert.IsTrue(foundPath.nodeList[3].id == 8);


        }

        [TestMethod]
        public void AsymmetricMapMapPathfindingTest1()
        {
            // given
            AStar aStar = new AStar();

            // and
            //  0, 0, S, 0, 0
            //  0, 1, 1, 1, 0
            //  0, 0, E, 0, 0


            NavigationGraph navigationGraph = new NavigationGraph(5,3);

            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.MakeNodeBlockingNode(3, 1);
            navigationGraph.RebuildAdajencyGraph();
            
            // when
            Point startPoint = new Point(2, 0);
            Point endPoint = new Point(2, 2);
            Path foundPath = aStar.FindPath(navigationGraph, startPoint, endPoint);
            
            // then
            Assert.IsNotNull(foundPath);
            
            // and
            Assert.IsTrue(foundPath.nodeList.Count == 5);
            
            Assert.IsTrue(foundPath.nodeList[0].id == 2);
            Assert.IsTrue(foundPath.nodeList[1].id == 1);
            Assert.IsTrue(foundPath.nodeList[2].id == 5);
            Assert.IsTrue(foundPath.nodeList[3].id == 11);
            Assert.IsTrue(foundPath.nodeList[4].id == 12);


        }

        [TestMethod]
        public void AsymmetricMapMapPathfindingTest2()
        {
            // given
            AStar aStar = new AStar();

            // and
            //  0, 0, S, 0, 0, 0
            //  0, 1, 1, 1, 0, 0
            //  0, 0, E, 0, 0, 0
            //  .
            //  .

//            int[,] nodeArray = new int[26, 24];
//
//            nodeArray[1, 1] = 1;
//            nodeArray[2, 1] = 1;
//            nodeArray[3, 1] = 1;



            //            NavigationGraph navigationGraph = new NavigationGraph(nodeArray);
            NavigationGraph navigationGraph = new NavigationGraph(26, 24);
            navigationGraph.MakeNodeBlockingNode(1, 1);
            navigationGraph.MakeNodeBlockingNode(2, 1);
            navigationGraph.MakeNodeBlockingNode(3, 1);
            navigationGraph.RebuildAdajencyGraph();

            // when
            Point startPoint = new Point(2, 0);
            Point endPoint = new Point(2, 2);
            Path foundPath = aStar.FindPath(navigationGraph, startPoint, endPoint);

            // then
            Assert.IsNotNull(foundPath);

            // and
            Assert.IsTrue(foundPath.nodeList.Count == 5);

            Assert.IsTrue(foundPath.nodeList[0].id == 2);
            Assert.IsTrue(foundPath.nodeList[1].id == 1);
            Assert.IsTrue(foundPath.nodeList[2].id == 26);
            Assert.IsTrue(foundPath.nodeList[3].id == 53);
            Assert.IsTrue(foundPath.nodeList[4].id == 54);


        }


    }

}
