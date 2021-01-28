using System;
using UnityEngine;

namespace BiggerDemo.Data
{
    [Serializable]
    public class LevelParameters
    {
        public Vector2Int BoardSize;
        public int PieceCount;

        public static LevelParameters CreatePrimitive()
        {
            return new LevelParameters { BoardSize = new Vector2Int(4, 4), PieceCount = 5 };
        }
    }
}