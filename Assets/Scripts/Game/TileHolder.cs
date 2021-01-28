using System.Collections.Generic;
using BiggerDemo.Data;

namespace BiggerDemo.Game
{
    public class TileHolder
    {
        private List<TilePole> _tilePiecePositions;

        public TileHolder()
        {
            _tilePiecePositions = new List<TilePole>();
        }

        public void FillTilePosition(TilePole tilePiecePosition)
        {
            _tilePiecePositions.Add(tilePiecePosition);
        }

        public void FillTilePosition(List<TilePole> tilePiecePositions)
        {
            foreach (TilePole tilePiecePosition in tilePiecePositions)
            {
                _tilePiecePositions.Add(tilePiecePosition);
            }
        }

        public void CleanTilePosition(TilePole tilePiecePosition)
        {
            _tilePiecePositions.Remove(tilePiecePosition);
        }

        public void CleanTilePosition(List<TilePole> tilePiecePositions)
        {
            foreach (TilePole tilePiecePosition in tilePiecePositions)
            {
                _tilePiecePositions.Remove(tilePiecePosition);
            }
        }

        public bool IsFilled(TilePole tilePiecePosition)
        {
            return _tilePiecePositions.Contains(tilePiecePosition);
        }

        public bool IsFilled(List<TilePole> tilePiecePositions)
        {
            foreach (TilePole tilePiecePosition in tilePiecePositions)
            {
                if (_tilePiecePositions.Contains(tilePiecePosition))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
