using TimiShared.UI;

namespace Lobby {

    public class UILobbyView : DialogViewBase {

        private System.Action _onPlayButtonCallback;

        public void Configure(System.Action onPlayButtonCallback) {
            this._onPlayButtonCallback = onPlayButtonCallback;
        }

        public void OnPlayButtonClicked() {
            if (this._onPlayButtonCallback != null) {
                this._onPlayButtonCallback.Invoke();
            }
        }
    }
}
