using TimiShared.UI;

namespace Game.UI {

    public class UIGameView : DialogViewBase {

        private System.Action _onLeaveButtonCallback;

        public void Configure(System.Action onLeaveGameButtonCallback) {
            this._onLeaveButtonCallback = onLeaveGameButtonCallback;
        }

        public void OnLeaveButtonClicked() {
            if (this._onLeaveButtonCallback != null) {
                this._onLeaveButtonCallback.Invoke();
            }
        }

    }
}