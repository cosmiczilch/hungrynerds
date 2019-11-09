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

        public void FlipCameraHorizontal() {
            this._gameCameraFlipper.AssertNotNull("Game Camera Flipper");
            this._gameCameraFlipper.FlipCamera(CameraFlip.FlipDirection.Horizontal);
        }

    }
}