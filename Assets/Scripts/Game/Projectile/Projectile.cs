using Photon.Pun;
using UnityEngine;

namespace Game {
    public class Projectile : MonoBehaviour {

        [SerializeField] private PhotonView _photonView;

        private void Awake() {

            // In multiplayer games, don't simulate physics on objects that are not controlled by us
            if (GameController.Instance != null &&
                GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !this._photonView.IsMine) {
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