using Photon.Pun;
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

        private float _startTime;

        protected override void Awake() {
            base.Awake();

            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            foreach (Transform child in this.gameObject.transform) {
                child.gameObject.layer = layer;
            }

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