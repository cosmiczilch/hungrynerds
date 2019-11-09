using TimiShared.UI;
using UnityEngine;

namespace Lobby {

    public class UILobbyView : DialogViewBase {

        [SerializeField] private Transform _loadingContainer = null;
        [SerializeField] private Transform _connectingContainer = null;
        [SerializeField] private Transform _findingMatchContainer = null;
        [SerializeField] private Transform _timedOutContainer = null;

        private System.Action _onPlayButtonCallback;

        public enum State {
            Idle,
            Connecting,
            FindingMatch,
            TimedOut
        }
        private State _state;

        public void Configure(System.Action onPlayButtonCallback) {
            this._onPlayButtonCallback = onPlayButtonCallback;
            this._state = State.Idle;
            this.UpdateState();
        }

        public void SetState(State state) {
            this._state = state;
            this.UpdateState();
        }

        private void UpdateState() {
            switch (this._state) {
                case State.Idle: {
                    this._loadingContainer.gameObject.SetActive(false);
                    this._connectingContainer.gameObject.SetActive(false);
                    this._findingMatchContainer.gameObject.SetActive(false);
                    this._timedOutContainer.gameObject.SetActive(false);
                } break;

                case State.Connecting: {
                    this._loadingContainer.gameObject.SetActive(true);
                    this._connectingContainer.gameObject.SetActive(true);
                    this._findingMatchContainer.gameObject.SetActive(false);
                    this._timedOutContainer.gameObject.SetActive(false);
                } break;

                case State.FindingMatch: {
                    this._loadingContainer.gameObject.SetActive(true);
                    this._connectingContainer.gameObject.SetActive(false);
                    this._findingMatchContainer.gameObject.SetActive(true);
                    this._timedOutContainer.gameObject.SetActive(false);
                } break;

                case State.TimedOut: {
                    this._loadingContainer.gameObject.SetActive(true);
                    this._connectingContainer.gameObject.SetActive(false);
                    this._findingMatchContainer.gameObject.SetActive(false);
                    this._timedOutContainer.gameObject.SetActive(true);
                } break;
            }
        }

        public void OnPlayButtonClicked() {
            if (this._onPlayButtonCallback != null) {
                this._onPlayButtonCallback.Invoke();
            }
        }
    }
}
