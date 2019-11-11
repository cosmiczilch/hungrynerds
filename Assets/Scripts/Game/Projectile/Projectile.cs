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
        [SerializeField] private Transform _artMine;
        [SerializeField] private Transform _artOther;

        private float _startTime = float.MaxValue;

        protected override void Awake() {
            base.Awake();

            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            foreach (Transform child in this.gameObject.transform) {
                child.gameObject.layer = layer;
            }

            this._artMine.gameObject.SetActive(GameController.Instance.GameType == GameController.GameType_t.SINGLE_PLAYER ||
                                               this._photonView.IsMine);
            this._artOther.gameObject.SetActive(GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                                                !this._photonView.IsMine);
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
                if (GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                    this._photonView.IsMine) {
                    PhotonNetwork.Destroy(this.gameObject);
                } else {
                    GameObject.Destroy(this.gameObject);
                }
                return;
            }
        }

    }
}