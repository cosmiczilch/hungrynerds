using Photon.Pun;
using TimiMultiPlayer;
using UnityEngine;

namespace Game {
    public class Projectile : NetworkedObjectBase {

        private const float kLifetimeDurationSeconds = 10.0f;

        [SerializeField] private PhotonView _photonView = null;
        protected override PhotonView PhotonView {
            get {
                return this._photonView;
            }
        }

        [SerializeField] private Transform _artTransform;

        private float _startTime = float.MaxValue;

        protected override void Awake() {
            base.Awake();

            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            foreach (Transform child in this.gameObject.transform) {
                child.gameObject.layer = layer;
            }

            if (GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                (
                    (!MultiPlayerManager.Instance.AreWePlayer1() && this._photonView.IsMine) ||
                    (MultiPlayerManager.Instance.AreWePlayer1() && !this._photonView.IsMine)
                )
                ) {
                this._artTransform.Rotate(Vector3.up, 180.0f);
            }
        }

        public void MarkAsLaunched() {
            this._startTime = Time.time;
        }

        private void Update() {
            if ((Time.time - this._startTime) > kLifetimeDurationSeconds) {
                GameObject.Destroy(this.gameObject);
                return;
            }
        }

    }
}