using UnityEngine;
using UnityEngine.Events;

namespace BiggerDemo.Game
{
    [RequireComponent(typeof(Collider2D))]
    public class PoleMouseEventSender : MonoBehaviour
    {
        public UnityEvent MouseDownEvent;
        public UnityEvent MouseDragEvent;
        public UnityEvent MouseUpEvent;

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
