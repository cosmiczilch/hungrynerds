using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI {
    public class UIDragLauncher : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

        private System.Action _onDragStartedDelegate;
        private System.Action<Vector2> _onDragMovedDelegate;
        private System.Action<Vector2> _onDragEndedDelegate;

        public void Initialize(System.Action onDragStartedDelegate,
                               System.Action<Vector2> onDragMovedDelegate,
                               System.Action<Vector2> onDragEndedDelegate) {
            this._onDragStartedDelegate = onDragStartedDelegate;
            this._onDragMovedDelegate = onDragMovedDelegate;
            this._onDragEndedDelegate = onDragEndedDelegate;
        }

        private Vector2 _startPosition;

        public void OnBeginDrag(PointerEventData eventData) {
            this._startPosition = eventData.position;
            if (this._onDragStartedDelegate != null) {
                this._onDragStartedDelegate.Invoke();
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (this._onDragMovedDelegate != null) {
                this._onDragMovedDelegate.Invoke(eventData.position - this._startPosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (this._onDragEndedDelegate != null) {
                this._onDragEndedDelegate.Invoke(eventData.position - this._startPosition);
            }
        }

    }
}