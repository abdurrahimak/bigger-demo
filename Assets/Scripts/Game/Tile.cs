using System;
using System.Collections.Generic;
using BiggerDemo.Data;
using CoreProject.Pool;
using UnityEngine;

namespace BiggerDemo.Game
{
    public class Tile : PoolObject
    {
        [SerializeField] private SpriteRenderer BottomTilePiece;
        [SerializeField] private SpriteRenderer TopTilePiece;
        [SerializeField] private SpriteRenderer LeftTilePiece;
        [SerializeField] private SpriteRenderer RightTilePiece;

        private Color _color = Color.white;

        public event Action MouseDownEvent;
        public event Action MouseDragEvent;
        public event Action MouseUpEvent;

        private List<TilePole> _activePoles;
        public List<TilePole> ActivePoles => _activePoles;

        public override void FromPool()
        {
        }

        public override void ToPool()
        {
        }

        public void ChangeColor(Color color)
        {
            _color = color;
            BottomTilePiece.color = _color;
            TopTilePiece.color = _color;
            LeftTilePiece.color = _color;
            RightTilePiece.color = _color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public SpriteRenderer GetTilePiece(TilePole tilePieceType)
        {
            switch (tilePieceType)
            {
                case TilePole.Top:
                    return TopTilePiece;
                case TilePole.Bottom:
                    return BottomTilePiece;
                case TilePole.Right:
                    return RightTilePiece;
                case TilePole.Left:
                    return LeftTilePiece;
                default:
                    return null;
            }
        }

        public void SetActivePoles(List<TilePole> tilePoles)
        {
            _activePoles = tilePoles;
            BottomTilePiece.gameObject.SetActive(false);
            TopTilePiece.gameObject.SetActive(false);
            LeftTilePiece.gameObject.SetActive(false);
            RightTilePiece.gameObject.SetActive(false);
            foreach (TilePole tilePiecePosition in tilePoles)
            {
                SpriteRenderer renderer = GetTilePiece(tilePiecePosition);
                renderer.gameObject.SetActive(true);
                renderer.sortingOrder = 10;
            }
        }

        public void OnMouseDown()
        {
            MouseDownEvent?.Invoke();
        }

        public void OnMouseDrag()
        {
            MouseDragEvent?.Invoke();
        }

        public void OnMouseUp()
        {
            MouseUpEvent?.Invoke();
        }
    }
}
