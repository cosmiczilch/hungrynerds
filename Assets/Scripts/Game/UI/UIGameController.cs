using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.UI;
using UnityEngine;

namespace Game.UI {

    public class UIGameController : DialogControllerBase<UIGameView> {

        private const string kPrefabPath = "Prefabs/UI/UIGameView";

        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private GameController _gameController;

        public UIGameController(GameController gameController) {
            this._gameController = gameController;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UIGameView.Config {
                onLeaveButtonCallback = this.HandleLeaveGameButtonClicked,
                onDragLaunchStarted = this.HandleDragToLaunchStarted,
                onDragLaunchMoved = this.HandleDragToLaunchMoved,
                onDragLaunchEnded = this.HandleDragToLaunchEnded
            });
        }

        private void HandleLeaveGameButtonClicked() {
            if (this._gameController != null) {
                this._gameController.LeaveGame();
            }

            this.RemoveDialog();
        }

        #region Drag to launch
        private const float kMinDragDistance = 0.0f;
        private const float kMaxDragDistance = 100.0f;

        private void HandleDragToLaunchStarted() {
            if (this._gameController != null && this._gameController.PlayerUs != null) {
                this._gameController.PlayerUs.HandleDragToLaunchStarted();
            }
        }

        private void HandleDragToLaunchMoved(Vector2 delta) {
            float distance = delta.magnitude;
            float normalized = Mathf.InverseLerp(kMinDragDistance, kMaxDragDistance, distance);
            float angle = Vector2.SignedAngle(Vector2.left, delta.normalized);
            if (this._gameController.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !MultiPlayerManager.Instance.AreWePlayer1()) {
                angle = 180 - angle;
            }

            if (this._gameController != null && this._gameController.PlayerUs != null) {
                this._gameController.PlayerUs.HandleDragToLaunchMoved(normalized, angle);
            }
        }

        private void HandleDragToLaunchEnded(Vector2 delta) {
            float distance = delta.magnitude;
            float normalized = Mathf.InverseLerp(kMinDragDistance, kMaxDragDistance, distance);
            float angle = Vector2.SignedAngle(Vector2.left, delta.normalized);
            if (this._gameController.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !MultiPlayerManager.Instance.AreWePlayer1()) {
                angle = 180 - angle;
            }

            if (this._gameController != null && this._gameController.PlayerUs != null) {
                this._gameController.PlayerUs.HandleDragToLaunchEnded(normalized, angle);
            }
        }
        #endregion
    }
}