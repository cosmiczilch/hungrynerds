using UnityEngine;

namespace Game {

    public class DestructibleEnemy : DestructibleBase {

        public bool IsOurs {
            get {
                return GameController.Instance.GameType == GameController.GameType_t.SINGLE_PLAYER ||
                       this.PhotonView.IsMine;
            }
        }

        public bool IsDead {
            get { return this._isDead; }
        }

        protected override string GetVfxPuffPrefabPath() {
            return "Prefabs/Vfx/ExplosionBig";
        }

        protected override float GetImpulseMultiplier(GameObject collidingObject) {
            float baseMultiplier = base.GetImpulseMultiplier(collidingObject);
            if (collidingObject.GetComponent<Projectile>() != null) {
                return 3.0f * baseMultiplier;
            }
            return baseMultiplier;
        }

    }
}