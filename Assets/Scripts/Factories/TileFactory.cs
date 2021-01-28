using BiggerDemo.Data;
using BiggerDemo.Game;
using CoreProject.Pool;
using CoreProject.Resource;
using CoreProject.Singleton;
using UnityEngine;

namespace BiggerDemo.Creation
{
    public class TileFactory : SingletonClass<TileFactory>
    {
        public TileData CreateTileData(Vector2Int boardPos)
        {
            return new TileData() { TileBoardPosition = boardPos };
        }

        public Tile CreateTile(TileData tileData, Color color, Transform parent = null)
        {
            Tile tile = PoolerManager.Instance.GetPoolObject("Tile", parent).GetComponent<Tile>();
            tile.SetActivePoles(tileData.IncludedPoles);
            tile.ChangeColor(color);
            return tile;
        }
    }
}
