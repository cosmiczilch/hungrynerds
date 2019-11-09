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

        private void HandleDragToLaunchStarted() {
            DebugLog.LogColor("start", LogColor.green);
        }

        private void HandleDragToLaunchMoved(Vector2 delta) {
        }

        private void HandleDragToLaunchEnded(Vector2 delta) {
            DebugLog.LogColor("end: " + delta, LogColor.red);
        }
    }
}