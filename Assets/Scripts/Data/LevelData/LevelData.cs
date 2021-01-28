using System;
using System.Collections.Generic;
using UnityEngine;

namespace BiggerDemo.Data
{
    [Serializable]
    public class LevelData
    {
        public Vector2Int BoardSize;
        public List<PieceData> Pieces;

        public LevelData()
        {
            Pieces = new List<PieceData>();
            TileData tileData = new TileData()
            {
                IncludedPoles = new List<TilePole>() { TilePole.Bottom, TilePole.Left, TilePole.Right, TilePole.Top }
            };
        }

        public override string ToString()
        {
            string text = $"Level has {Pieces.Count} pieces.\n";
            foreach (PieceData pieceData in Pieces)
            {
                text += pieceData.ToString();
            }
            return text;
        }
    }
}
