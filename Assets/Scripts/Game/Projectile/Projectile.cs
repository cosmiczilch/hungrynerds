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

    }
}