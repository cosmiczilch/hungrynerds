using TimiShared.UI;
using UnityEngine;

namespace Game.UI {

    public class UIGameView : DialogViewBase {

        [SerializeField] private UIDragLauncher _dragLauncher = null;

        public class Config {
            public System.Action onLeaveButtonCallback;
            public System.Action onDragLaunchStarted;
            public System.Action<Vector2> onDragLaunchMoved;
            public System.Action<Vector2> onDragLaunchEnded;
        }
        private Config _config;

        public void Configure(Config config) {
            this._config = config;
            this._dragLauncher.Initialize(this._config.onDragLaunchStarted,
                                          this._config.onDragLaunchMoved,
                                          this._config.onDragLaunchEnded);
        }

        public void OnLeaveButtonClicked() {
            if (this._config.onLeaveButtonCallback != null) {
                this._config.onLeaveButtonCallback.Invoke();
            }
        }
    }
}