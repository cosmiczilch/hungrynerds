using System.Security.Cryptography;
using TimiShared.Debug;
using TimiShared.UI;

namespace Game.UI {
    public class UIGameOverController : DialogControllerBase<UIGameOverView> {

        private const string kPrefabPath = "Prefabs/UI/UIGameOverView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private bool _win;
        private System.Action _onDismissCallback;

        public UIGameOverController(bool win, System.Action onDismissCallback) {
            this._win = win;
            this._onDismissCallback = onDismissCallback;
        }

        protected override void ConfigureView() {
            this.View.Configure(this._win, this.HandleOkButtonClickedCallback);
        }

        private void HandleOkButtonClickedCallback() {
            if (this._onDismissCallback != null) {
                this._onDismissCallback.Invoke();
            }
            this.RemoveDialog();
        }
    }
}