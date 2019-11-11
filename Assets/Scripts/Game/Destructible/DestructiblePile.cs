using Photon.Pun;
using UnityEngine;

namespace Game {

    public class DestructiblePile : MonoBehaviour {

        [SerializeField] private PhotonView _photonView = null;

        private void Awake() {
            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            foreach (Transform child in this.gameObject.transform) {
                child.gameObject.layer = layer;
            }
        }
    }
}