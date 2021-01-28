using System.Collections.Generic;
using BiggerDemo.Creation;
using BiggerDemo.Data;
using BiggerDemo.Extensions;
using CoreProject.Pool;
using UnityEngine;
using DG.Tweening;

namespace BiggerDemo.Game
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float _boardViewportReferanceY = 0.75f;
        [SerializeField] private float _piecesViewportReferanceY = 0.25f;

        private SpecialGrid<TileHolder> specialGrid;
        private List<PoolObject> _tileHolderObject;
        private LevelData _levelData;
        private List<Piece> _pieces;
        private Vector2Int _boardSize;

        public void Initialize(LevelData levelData)
        {
            _levelData = levelData;
            _boardSize = _levelData.BoardSize;
            _pieces = new List<Piece>();
            _tileHolderObject = new List<PoolObject>();
            CreateGrid();
            CreatePieces();
        }

        private void CreateGrid()
        {
            Camera.main.orthographicSize = ExtensionMethods.GetCameraOrthographicSize(_boardSize);
            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, _boardViewportReferanceY, 0f));
            specialGrid = new SpecialGrid<TileHolder>(_boardSize.x, _boardSize.y, 1f, new Vector3(0, pos.y, 0f), transform, () => new TileHolder());

            for (int i = 0; i < _boardSize.x; i++)
            {
                for (int j = 0; j < _boardSize.y; j++)
                {
                    PoolObject poolObj = PoolerManager.Instance.GetPoolObject("TileHolder", transform);
                    _tileHolderObject.Add(poolObj);
                    poolObj.gameObject.transform.localPosition = specialGrid.GetWorldPosition(new Vector2Int(i, j));
                }
            }
        }

        private void CreatePieces()
        {
            Vector3 posDown = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, _piecesViewportReferanceY, 0f));
            foreach (PieceData pieceData in _levelData.Pieces)
            {
                Piece piece = PieceFactory.Instance.CreatePiece(pieceData, (Vector2Int boardPos, Vector3 originPosition) => specialGrid.GetWorldPositionWithOrigin(originPosition, boardPos), transform);
                piece.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.2f, 0f));

                Vector3 targetPos = new Vector3(UnityEngine.Random.Range(-1f, 1f), posDown.y, 0f) - piece.CenterPosition;
                piece.transform.DOMove(targetPos, 1f).Play();
                piece.MouseUpEvent += OnPieceMouseUp;
                piece.MouseDownEvent += OnPieceMouseDown;
                _pieces.Add(piece);
            }
        }

        private void OnPieceMouseUp(Piece piece)
        {
            PieceAttachProcess(piece);
        }

        private void OnPieceMouseDown(Piece piece)
        {
            if (piece.IsAttached)
            {
                ClearHolders(piece);
            }
        }

        private void PieceAttachProcess(Piece piece)
        {
            if (CanAttachToBoard(piece))
            {
                Vector3 offsetPos = GetPieceOffset(piece);
                piece.transform.position += offsetPos;
                FillHolders(piece);
                CheckLevelFinish();
            }
        }

        private void CheckLevelFinish()
        {
            foreach (Piece piece in _pieces)
            {
                if (!piece.IsAttached)
                {
                    return;
                }
            }
            GenerateNewLevel();
        }

        private void GenerateNewLevel()
        {
            Destroy(gameObject);
            ObjectsGiveBackToPool();
            GameController.Instance.NextLevel();
        }

        private void ObjectsGiveBackToPool()
        {
            foreach (PoolObject pObj in _tileHolderObject)
            {
                PoolerManager.Instance.SetPoolObjectToPool(pObj);
            }
            foreach (Piece piece in _pieces)
            {
                piece.MouseUpEvent -= OnPieceMouseUp;
                piece.MouseDownEvent -= OnPieceMouseDown;
                PoolerManager.Instance.SetPoolObjectToPool(piece);
            }
            _pieces.Clear();
        }

        private void ClearHolders(Piece piece)
        {
            piece.IsAttached = false;
            foreach (Tile tile in piece.Tiles)
            {
                TileHolder tileHolder = FindNearestTileHolder(tile);
                tileHolder.CleanTilePosition(tile.ActivePoles);
            }
        }

        private void FillHolders(Piece piece)
        {
            piece.IsAttached = true;
            foreach (Tile tile in piece.Tiles)
            {
                TileHolder tileHolder = FindNearestTileHolder(tile);
                tileHolder.FillTilePosition(tile.ActivePoles);
            }
        }

        private Vector3 GetPieceOffset(Piece piece)
        {
            Tile tile = piece.Tiles[0];
            Vector3 worldPosition = specialGrid.GetWorldPosition(specialGrid.GetXY(tile.transform.position));
            return worldPosition - tile.transform.position;
        }

        private bool CanAttachToBoard(Piece piece)
        {
            bool isAttachableToBoard = true;
            foreach (Tile tile in piece.Tiles)
            {
                TileHolder tileHolder = FindNearestTileHolder(tile);
                if (tileHolder == null || tileHolder.IsFilled(tile.ActivePoles))
                {
                    isAttachableToBoard = false;
                    break;
                }
            }
            return isAttachableToBoard;
        }

        private TileHolder FindNearestTileHolder(Tile tile)
        {
            return specialGrid.GetGridObject(tile.transform.position);
        }
    }
}
