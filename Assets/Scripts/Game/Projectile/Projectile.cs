using Photon.Pun;
using UnityEngine;

namespace Game {
    public class Projectile : NetworkedObjectBase {

        [SerializeField] private PhotonView _photonView;
        protected override PhotonView PhotonView {
            get {
                return this._photonView;
            }
        }

        protected override void Awake() {
            base.Awake();

            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            foreach (Transform child in this.gameObject.transform) {
                child.gameObject.layer = layer;
            }

        }

    }
}