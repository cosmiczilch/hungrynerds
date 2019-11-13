using TimiShared.Debug;
using TimiShared.Extensions;
using UnityEngine;
using Utilities;

namespace Game {
    public class GameView : MonoBehaviour {

        [SerializeField] private CameraFlip _gameCameraFlipper = null;

        [SerializeField] private Transform _player1Anchor = null;
        public Transform Player1Anchor {
            get { return this._player1Anchor; }
        }

        [SerializeField] private Transform _player2Anchor = null;
        public Transform Player2Anchor {
            get { return this._player2Anchor; }
        }

        [SerializeField] private Transform _destructiblePile1Anchor = null;
        public Transform DestructiblePile1Anchor {
            get {
                return this._destructiblePile1Anchor;
            }
        }

        [SerializeField] private Transform _destructiblePile2Anchor = null;
        public Transform DestructiblePile2Anchor {
            get {
                return this._destructiblePile2Anchor;
            }
        }

        public void FlipCameraHorizontal() {
            this._gameCameraFlipper.AssertNotNull("Game Camera Flipper");
            this._gameCameraFlipper.FlipCamera(CameraFlip.FlipDirection.Horizontal);
        }

        private const float kMinTimeBeforeCheckingWinCOnditionSeconds = 15.0f;
        private float _startTime = float.MaxValue;

        private void Awake() {
            this._startTime = Time.time;
        }

        private void Update() {
            if (GameController.Instance.IsGameOver()) {
                return;
            }

            if ((Time.time - this._startTime) <= kMinTimeBeforeCheckingWinCOnditionSeconds) {
                return;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int numOursAlive = 0;
            int numTheirsAlive = 0;
            if (enemies != null) {
                foreach (GameObject enemy in enemies) {
                    DestructibleEnemy destructibleEnemy = enemy.GetComponent<DestructibleEnemy>();
                    if (destructibleEnemy != null) {
                        if (!destructibleEnemy.IsDead) {
                            if (destructibleEnemy.IsOurs) {
                                ++numOursAlive;
                            } else {
                                ++numTheirsAlive;
                            }
                        }
                    }
                }
            }

            if (numOursAlive <= 0) {
                GameController.Instance.HandleGameOver(true);
            } else if (GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                       numTheirsAlive <= 0) {
                GameController.Instance.HandleGameOver(false);
            }
        }

    }
}