using System;
using System.Collections.Generic;
using BiggerDemo.Creation;
using BiggerDemo.Data;
using BiggerDemo.Extensions;
using CoreProject.Pool;
using UnityEngine;

namespace BiggerDemo.Game
{
    public class Piece : PoolObject
    {
        #region Private fields
        private PieceData _pieceData;
        private Vector3 _startPosition;
        private Vector3 _offsetToMouse;
        private List<Tile> _tiles;
        private Vector3 _centerPosition;
        #endregion

        #region Public fields
        public static int SortOrder = 10;
        public bool IsAttached = false;
        public List<Tile> Tiles => _tiles;
        public Vector3 CenterPosition => _centerPosition;
        #endregion

        #region Events
        public event Action<Piece> MouseUpEvent;
        public event Action<Piece> MouseDownEvent;
        #endregion

        public void InitPiece(PieceData pieceData, Func<Vector2Int, Vector3> FuncTileGridPosition)
        {
            IsAttached = false;
            SortOrder = 10;
            _tiles = new List<Tile>();
            _pieceData = pieceData;

            Vector3 sumPositionOfTiles = Vector3.zero;
            foreach (TileData tileData in pieceData.Tiles)
            {
                Tile tile = TileFactory.Instance.CreateTile(tileData, pieceData.PieceColor, transform);
                tile.transform.position = FuncTileGridPosition(tileData.TileBoardPosition);
                tile.MouseDownEvent += OnMouseDown;
                tile.MouseDragEvent += OnMouseDrag;
                tile.MouseUpEvent += OnMouseUp;
                _tiles.Add(tile);
                sumPositionOfTiles += tile.transform.position;
            }
            _centerPosition = sumPositionOfTiles / pieceData.Tiles.Count;
        }

        public override void ToPool()
        {
            transform.position = Vector3.zero;
            _centerPosition = Vector3.zero;
            IsAttached = false;
            foreach (Tile tile in _tiles)
            {
                tile.MouseDownEvent -= OnMouseDown;
                tile.MouseDragEvent -= OnMouseDrag;
                tile.MouseUpEvent -= OnMouseUp;
                PoolerManager.Instance.SetPoolObjectToPool(tile);
            }
            _tiles.Clear();
        }

        public void OnMouseDown()
        {
            SortOrder++;
            transform.SetAsLastSibling();
            foreach (var item in transform.GetComponentsInChildren<SpriteRenderer>())
            {
                item.sortingOrder = SortOrder;
            }
            _startPosition = transform.position;
            _offsetToMouse = _startPosition - ExtensionMethods.GetMouseWorldPosition();
            MouseDownEvent?.Invoke(this);
        }

        public void OnMouseDrag()
        {
            transform.position = ExtensionMethods.GetMouseWorldPosition() + _offsetToMouse;
        }

        public void OnMouseUp()
        {
            _offsetToMouse = Vector3.zero;
            MouseUpEvent?.Invoke(this);
        }
    }
}
