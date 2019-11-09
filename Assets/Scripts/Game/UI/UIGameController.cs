using TimiShared.UI;

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
            this.View.Configure(this.HandleLeaveGameButtonClicked);
        }

        private void HandleLeaveGameButtonClicked() {
            if (this._gameController != null) {
                this._gameController.LeaveGame();
            }
            this.RemoveDialog();
        }
    }
}