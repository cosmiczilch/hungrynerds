using TimiShared.UI;
using UnityEngine;

namespace Game.UI {

    public class UIGameOverView : DialogViewBase {

        [SerializeField] private Transform _winContainer;
        [SerializeField] private Transform _loseContainer;

        private System.Action _onOkButtonClickedCallback;

        public void Configure(bool win, System.Action onOkButtonClickedCallback) {
            this._winContainer.gameObject.SetActive(win);
            this._loseContainer.gameObject.SetActive(!win);
            this._onOkButtonClickedCallback = onOkButtonClickedCallback;
        }

        public void OnOkButtonClicked() {
            if (this._onOkButtonClickedCallback != null) {
                this._onOkButtonClickedCallback.Invoke();
            }
        }

    }
}