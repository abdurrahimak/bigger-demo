using System;
using System.Collections.Generic;
using BiggerDemo.Creation;
using BiggerDemo.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BiggerDemo.Data
{
    public class RandomLevelCreator : IRandomLevelCreator
    {
        #region usefull private class
        private class PathNode
        {
            public bool Avaible;
            public bool Collected;
            public Vector2Int BoardPosition;
            public TilePole TilePole;
            public List<PathNode> Neighbors;

            public PathNode(Vector2Int boardPos, TilePole pole)
            {
                BoardPosition = boardPos;
                TilePole = pole;
                Avaible = true;
            }
        }

        private class Path
        {
            public List<PathNode> PathNodes;

            public Path()
            {
                PathNodes = new List<PathNode>();
            }
        }
        #endregion

        #region parameters
        private Vector2Int _boardSize;
        private int _pieceCount;
        #endregion

        #region interface implementation
        public LevelData GenerateRandomLevel(LevelDifficult levelDifficult)
        {
            LevelParameters levelParameters = LevelFactory.Instance.GetLevelParameters(levelDifficult);
            _boardSize = levelParameters.BoardSize;
            _pieceCount = levelParameters.PieceCount;
            List<PathNode> pathNodes = CreatePathNodes(_boardSize);
            List<Path> paths = CreatePaths(pathNodes, _pieceCount);
            LevelData levelData = CreateLevelDataWithPath(paths);
            pathNodes.Clear();
            pathNodes = null;
            paths.Clear();
            paths = null;
            return levelData;
        }
        #endregion

        #region  private methods
        private List<PathNode> CreatePathNodes(Vector2Int boardSize)
        {
            List<PathNode> pathNodes = new List<PathNode>(_boardSize.x * _boardSize.y);
            for (int i = 0; i < _boardSize.x; i++)
            {
                for (int j = 0; j < _boardSize.y; j++)
                {
                    foreach (TilePole pole in Enum.GetValues(typeof(TilePole)))
                    {
                        PathNode pathNode = new PathNode(new Vector2Int(i, j), pole);
                        pathNodes.Add(pathNode);
                    }
                }
            }
            SetNeighBors(pathNodes, boardSize);
            return pathNodes;
        }

        private void SetNeighBors(List<PathNode> pathNodes, Vector2Int boardSize)
        {
            foreach (PathNode pathNode in pathNodes)
            {
                pathNode.Neighbors = FindNeighbors(pathNode, pathNodes, boardSize);
            }
        }

        private List<PathNode> FindNeighbors(PathNode pathNode, List<PathNode> pathNodes, Vector2Int boardSize)
        {
            List<PathNode> pathNeighbors = new List<PathNode>();
            switch (pathNode.TilePole)
            {
                case TilePole.Top:
                    {
                        if (pathNode.BoardPosition.x > 0)
                        {
                            PathNode upNode = FindPathNode(new Vector2Int(pathNode.BoardPosition.x - 1, pathNode.BoardPosition.y), TilePole.Bottom);
                            if (upNode != null)
                            {
                                pathNeighbors.Add(upNode);
                            }
                        }
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Left));
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Right));
                        break;
                    }
                case TilePole.Right:
                    {
                        if (pathNode.BoardPosition.y < boardSize.y - 1)
                        {
                            PathNode upNode = FindPathNode(new Vector2Int(pathNode.BoardPosition.x, pathNode.BoardPosition.y + 1), TilePole.Left);
                            if (upNode != null)
                            {
                                pathNeighbors.Add(upNode);
                            }
                        }
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Top));
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Bottom));
                        break;
                    }
                case TilePole.Bottom:
                    {
                        if (pathNode.BoardPosition.x < boardSize.x - 1)
                        {
                            PathNode upNode = FindPathNode(new Vector2Int(pathNode.BoardPosition.x + 1, pathNode.BoardPosition.y), TilePole.Top);
                            if (upNode != null)
                            {
                                pathNeighbors.Add(upNode);
                            }
                        }
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Left));
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Right));
                        break;
                    }
                case TilePole.Left:
                    {
                        if (pathNode.BoardPosition.y > 0)
                        {
                            PathNode upNode = FindPathNode(new Vector2Int(pathNode.BoardPosition.x, pathNode.BoardPosition.y - 1), TilePole.Right);
                            if (upNode != null)
                            {
                                pathNeighbors.Add(upNode);
                            }
                        }
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Top));
                        pathNeighbors.Add(FindPathNode(pathNode.BoardPosition, TilePole.Bottom));
                        break;
                    }

            }
            return pathNeighbors;

            PathNode FindPathNode(Vector2Int pos, TilePole pole)
            {
                return pathNodes.Find(p => (p.BoardPosition == pos) && (p.TilePole == pole));
            }
        }

        private List<Path> CreatePaths(List<PathNode> pathNodes, int pieceCount)
        {
            List<Path> paths = new List<Path>(pieceCount);
            InitializePaths();
            MoveAroundNeighbors();
            return paths;

            void InitializePaths()
            {
                do
                {
                    PathNode randomPathNode = pathNodes.GetRandom();
                    if (randomPathNode.Avaible)
                    {
                        randomPathNode.Avaible = false;
                        Path path = new Path();
                        path.PathNodes.Add(randomPathNode);
                        paths.Add(path);
                    }
                }
                while (paths.Count < pieceCount);
            }

            void MoveAroundNeighbors()
            {
                Queue<PathNode> tempQueue = new Queue<PathNode>();
                while (pathNodes.Find(p => p.Avaible) != null)
                {
                    foreach (Path path in paths)
                    {
                        tempQueue.Clear();
                        foreach (PathNode pathNode in path.PathNodes.FindAll(p => !p.Collected))
                        {
                            List<PathNode> avaiblePathNodes = pathNode.Neighbors.FindAll(n => n.Avaible);
                            if (avaiblePathNodes == null || avaiblePathNodes.Count == 0)
                            {
                                continue;
                            }
                            foreach (PathNode neighbors in avaiblePathNodes)
                            {
                                tempQueue.Enqueue(neighbors);
                            }
                            pathNode.Collected = true;
                        }

                        while (tempQueue.Count > 0)
                        {
                            PathNode pathNode = tempQueue.Dequeue();
                            pathNode.Avaible = false;
                            path.PathNodes.Add(pathNode);
                        }
                    }
                }
            }
        }

        private LevelData CreateLevelDataWithPath(List<Path> paths)
        {
            LevelData levelData = new LevelData() { BoardSize = _boardSize };
            foreach (Path path in paths)
            {
                PieceData pieceData = new PieceData();
                pieceData.PieceColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
                pieceData.Tiles = new List<TileData>();
                do
                {
                    List<PathNode> pathNodes = path.PathNodes.FindAll(p => p.BoardPosition == path.PathNodes[0].BoardPosition);
                    TileData tileData = new TileData();
                    tileData.TileBoardPosition = pathNodes[0].BoardPosition;
                    tileData.IncludedPoles = new List<TilePole>();
                    foreach (var pathNode in pathNodes)
                    {
                        if (!tileData.IncludedPoles.Contains(pathNode.TilePole))
                            tileData.IncludedPoles.Add(pathNode.TilePole);
                    }
                    pieceData.Tiles.Add(tileData);
                    path.PathNodes.RemoveAll(p => p.BoardPosition == tileData.TileBoardPosition);
                } while (path.PathNodes.Count > 0);
                levelData.Pieces.Add(pieceData);
            }
            return levelData;
        }
        #endregion
    }
}