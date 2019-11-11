using UnityEngine;

namespace Game {

    public class DestructibleEnemy : DestructibleBase {

        public bool IsDead {
            get { return this._isDead; }
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