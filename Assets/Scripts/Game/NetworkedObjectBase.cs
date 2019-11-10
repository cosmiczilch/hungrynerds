using Photon.Pun;
using UnityEngine;

namespace Game {
    public abstract class NetworkedObjectBase : MonoBehaviour {

        protected abstract PhotonView PhotonView { get; }

        protected virtual void Awake() {
            // In multiplayer games, don't simulate physics on objects that are not controlled by us
            if (GameController.Instance != null &&
                GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !this.PhotonView.IsMine) {
                Rigidbody2D[] rigidbody2Ds = this.gameObject.GetComponentsInChildren<Rigidbody2D>();
                if (rigidbody2Ds != null) {
                    for (int i = 0; i < rigidbody2Ds.Length; ++i) {
                        GameObject.Destroy(rigidbody2Ds[i]);
                    }
                }
            }
        }

    }
}