using System;
using System.Collections;
using BiggerDemo.Extensions;
using UnityEngine;

namespace BiggerDemo.Game
{
    public class SpecialGrid<TGridItem>
    {
        public int width;
        public int height;
        private Vector3 absoluteOriginPosition;
        private Vector3 originPosition;
        private float cellSize;
        private TGridItem[,] gridArray;
        private TextMesh[,] debugTextArray;
        private bool showDebug;
        private Vector3 halftCellsize;

        public SpecialGrid(int width, int height, float cellSize, Vector3 originPosition, Transform parent = null, Func<TGridItem> CreateItemFunc = null) : this(new TGridItem[width, height], cellSize, originPosition, parent, CreateItemFunc)
        {
        }

        public SpecialGrid(TGridItem[,] gridObjects, float cellSize, Vector3 originPosition, Transform parent = null, Func<TGridItem> CreateItemFunc = null)
        {
            gridArray = gridObjects;
            this.width = gridArray.GetLength(0);
            this.height = gridArray.GetLength(1);
            this.cellSize = cellSize;
            this.absoluteOriginPosition = originPosition;
            this.halftCellsize = new Vector3(cellSize, cellSize) * 0.5f;
            this.originPosition = absoluteOriginPosition - (new Vector3(width, height) * cellSize / 2f);

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridObjects[x, y] = CreateItemFunc != null ? CreateItemFunc.Invoke() : default(TGridItem);
                }
            }

            showDebug = false;
            if (showDebug)
            {
                Vector3 halfCellSize = new Vector3(cellSize, cellSize) * 0.5f;
                debugTextArray = new TextMesh[width, height];
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = ExtensionMethods.CreateWorldTextTemplate($"{x},{y}", parent, GetWorldPosition(new Vector2Int(x, y)), 5, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    }
                }
            }
        }

        public Vector3 GetWorldPosition(Vector2Int boardPos)
        {
            return new Vector3(boardPos.y, (height - 1) - boardPos.x) * cellSize + originPosition + halftCellsize;
        }

        public Vector3 GetWorldPositionWithOrigin(Vector3 origin, Vector2Int boardPos)
        {
            Vector3 localOrigin = origin - (new Vector3(width, height) * cellSize / 2f);
            return new Vector3(boardPos.y, (height - 1) - boardPos.x) * cellSize + localOrigin + halftCellsize;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = height - 1 - Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        }

        public Vector2Int GetXY(Vector3 worldPosition)
        {
            Vector2Int boardPos = Vector2Int.zero;
            boardPos.x = height - 1 - Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
            boardPos.y = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            return boardPos;
        }

        public void SetGridObject(Vector2Int boardPos, TGridItem value)
        {
            if (boardPos.x >= 0 && boardPos.y >= 0 && boardPos.x < width && boardPos.y < height)
            {
                gridArray[boardPos.x, boardPos.y] = value;
                if (showDebug && debugTextArray[boardPos.x, boardPos.y] != null)
                {
                    debugTextArray[boardPos.x, boardPos.y].text = $"{boardPos.x},{boardPos.y}";
                }
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridItem value)
        {
            Vector2Int boardPos = GetXY(worldPosition);
            SetGridObject(boardPos, value);
        }

        public TGridItem GetGridObject(Vector2Int boardPos)
        {
            if (boardPos.x >= 0 && boardPos.y >= 0 && boardPos.x < width && boardPos.y < height)
            {
                return gridArray[boardPos.x, boardPos.y];
            }
            return default(TGridItem);
        }

        public TGridItem GetGridObject(Vector3 worldPosition)
        {
            Vector2Int boardPos = GetXY(worldPosition);
            return GetGridObject(boardPos);
        }
    }
}
