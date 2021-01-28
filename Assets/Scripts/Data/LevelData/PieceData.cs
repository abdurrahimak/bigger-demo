using System.Collections.Generic;
using UnityEngine;
using System;

namespace BiggerDemo.Data
{
    [Serializable]
    public class PieceData
    {
        public Color PieceColor;
        public List<TileData> Tiles;

        public override string ToString()
        {
            string text = $"--Piece has {Tiles.Count} Tiles.\n";
            foreach (TileData tileData in Tiles)
            {
                text += tileData.ToString();
            }
            return text;
        }
    }
}
