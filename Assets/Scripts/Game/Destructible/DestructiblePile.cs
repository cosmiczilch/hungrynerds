using Photon.Pun;
using UnityEngine;

namespace Game {

    public class DestructiblePile : MonoBehaviour {

        [SerializeField] private PhotonView _photonView = null;

        private void Awake() {
            int layer = GameController.Instance.GetLayer(this._photonView.IsMine);
            this.gameObject.layer = layer;
            Transform[] childs = this.gameObject.GetComponentsInChildren<Transform>();
            if (childs != null) {
                foreach (Transform child in childs) {
                    child.gameObject.layer = layer;
                }
            }
        }
    }
}