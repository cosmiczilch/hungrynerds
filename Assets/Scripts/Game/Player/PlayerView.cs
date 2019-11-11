using TimiMultiPlayer;
using UnityEngine;

namespace Game {
    public class PlayerView : MonoBehaviour {

        [SerializeField] private Transform _launchPosition = null;
        public Transform LaunchPosition {
            get {
                return this._launchPosition;
            }
        }

        [SerializeField] private Transform _launchIndicatorArrow = null;

        private void Awake() {
            this.HideLaunchIndicatorArrow();

            if (GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !MultiPlayerManager.Instance.AreWePlayer1()) {
                this._launchPosition.Translate(2.19f * 2, 0, 0);
                this._launchIndicatorArrow.Translate(2.19f * 2, 0, 0);
            }
        }

        public void ShowLaunchArrow(float normalizedStrength = 0.0f, float angle = 0.0f) {
            this._launchIndicatorArrow.gameObject.SetActive(true);
            this._launchIndicatorArrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            this._launchIndicatorArrow.localScale = new Vector3(
                Mathf.Lerp(0.8f, 2.0f, normalizedStrength),
                Mathf.Lerp(1.5f, 2.0f, normalizedStrength),
                this._launchIndicatorArrow.localScale.z
                );
        }

        public void HideLaunchIndicatorArrow() {
            this._launchIndicatorArrow.gameObject.SetActive(false);
        }

    }
}