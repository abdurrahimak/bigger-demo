using System;
using BiggerDemo.Data;
using BiggerDemo.Game;
using CoreProject.Pool;
using CoreProject.Singleton;
using UnityEngine;

namespace BiggerDemo.Creation
{
    public class PieceFactory : SingletonClass<PieceFactory>
    {
        public PieceData CreatePieceData()
        {
            return new PieceData() { PieceColor = Color.white };
        }

        public Piece CreatePiece(PieceData pieceData, Func<Vector2Int, Vector3, Vector3> FuncTileGridPosition, Transform parent = null)
        {
            Piece piece = PoolerManager.Instance.GetPoolObject("Piece", parent).GetComponent<Piece>();
            piece.InitPiece(pieceData, (boardPos) => FuncTileGridPosition(boardPos, piece.transform.position));
            return piece;
        }
    }
}