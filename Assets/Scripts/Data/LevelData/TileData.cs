using System.Collections.Generic;
using UnityEngine;
using System;

namespace BiggerDemo.Data
{
    [Serializable]
    public class TileData
    {
        public Vector2Int TileBoardPosition;
        public List<TilePole> IncludedPoles;

        public override string ToString()
        {
            string text = $"----Tile Board Pos: {TileBoardPosition}, Has {IncludedPoles.Count} poles.\n";
            foreach (TilePole tilePole in IncludedPoles)
            {
                text += $"------{tilePole}\n";
            }
            return text;
        }
    }
}
