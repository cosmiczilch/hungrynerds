using UnityEngine;
using Utilities;

namespace Game {
    public class GameView : MonoBehaviour {

        [SerializeField] private CameraFlip _gameCameraFlipper;

        [SerializeField] private Transform _player1Anchor;
        public Transform Player1Anchor {
            get { return this._player1Anchor; }
        }

        [SerializeField] private Transform _player2Anchor;
        public Transform Player2Anchor {
            get { return this._player2Anchor; }
        }

    }
}