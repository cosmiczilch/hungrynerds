using SharedBrawl.Debug;
using SharedBrawl.Extensions;
using UnityEngine;

namespace SharedBrawl.UI {
    public class UIRoot : MonoBehaviour {

        #region Singleton
        private static UIRoot _instance;
        public static UIRoot Instance {
            get {
                return _instance;
            }
        }
        #endregion

        [SerializeField] private Canvas _mainCanvas;
        public Canvas MainCanvas {
            get {
                return this._mainCanvas;
            }
        }

        [SerializeField] private Camera _uiCamera;
        public Camera UICamera {
            get {
                return this._uiCamera;
            }
        }

        [SerializeField] private DialogFactory _dialogFactory;
        public DialogFactory DialogFactory {
            get {
                return this._dialogFactory;
            }
        }

        [SerializeField] private DialogViewPool _dialogViewPool;
        public DialogViewPool DialogViewPool {
            get {
                return this._dialogViewPool;
            }
        }

        #region Unity LifeCycle
        private void Awake() {
            if (UIRoot.Instance != null) {
                DebugLog.LogWarningColor("There should never be more than one UIRoot in the scene!", LogColor.orange);
            }
            UIRoot._instance = this;

            this._mainCanvas.AssertNotNull("Main Canvas");
            this._uiCamera.AssertNotNull("UI Camera");
            this._dialogFactory.AssertNotNull("Dialog Factory");
            this._dialogViewPool.AssertNotNull("Dialog View Pool");
        }
        #endregion
    }
}