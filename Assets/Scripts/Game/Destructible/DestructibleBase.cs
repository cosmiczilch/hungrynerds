using System.Collections.Generic;
using Photon.Pun;
using TimiShared.Extensions;
using UnityEngine;

namespace Game {
    public class DestructibleBase : NetworkedObjectBase, IPunObservable {

        private const float kMinImpulseForFiltering = 1.0f;

        [SerializeField] private PhotonView _photonView;
        protected override PhotonView PhotonView {
            get {
                return this._photonView;
            }
        }

        [SerializeField] private DestructiblePropertiesHelper.DestructibleType _destructibleType;

        // TODO: Remove serialized after testing
        [SerializeField] protected float _startingHealth;
        [SerializeField] protected float _currentHealth;


        [System.Serializable]
        protected class DamageState {
            public float normalizedHealth;
            public Transform damageStateContainer;
        }
        [SerializeField] private List<DamageState> _damageStates;

        protected Rigidbody2D _rigidbody2D;

        protected virtual void Start() {
            if (this._damageStates != null) {
                this._damageStates.Sort((a, b) => {
                    if (a.normalizedHealth < b.normalizedHealth) {
                        return 1;
                    } else if (a.normalizedHealth > b.normalizedHealth) {
                        return -1;
                    } else {
                        return 0;
                    }
                });
            }

            if (GameController.Instance != null &&
                GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !this.PhotonView.IsMine) {
                // This object will be controlled by some other client
            } else {
                this._rigidbody2D = this.GetComponent<Rigidbody2D>();
                this._rigidbody2D.AssertNotNull("RigidBody2D component on destructible");
            }

            this._startingHealth = DestructiblePropertiesHelper.GetStartingHealthForType(this._destructibleType);
            this._currentHealth = this._startingHealth;

            if (this._rigidbody2D != null) {
                this._rigidbody2D.mass = DestructiblePropertiesHelper.GetMassForType(this._destructibleType);
            }
        }

        protected virtual void Update() {
            if (this._currentHealth <= 0) {
                this.MarkAsDead();
                return;
            }

            if (this._damageStates != null && this._damageStates.Count > 0) {
                float normalizedHealth = this._currentHealth / this._startingHealth;

                // Turn off all the damage state containers
                {
                    var enumerator = this._damageStates.GetEnumerator();
                    while (enumerator.MoveNext()) {
                        enumerator.Current.damageStateContainer.gameObject.SetActive(false);
                    }
                    enumerator.Dispose();
                }

                // Turn on the first matching one (descending order)
                {
                    var enumerator = this._damageStates.GetEnumerator();
                    while (enumerator.MoveNext()) {
                        if (enumerator.Current.normalizedHealth < normalizedHealth) {
                            enumerator.Current.damageStateContainer.gameObject.SetActive(true);
                            break;
                        }
                    }
                    enumerator.Dispose();
                }
            }
        }

        private bool _isDead = false;
        private void MarkAsDead() {
            if (this._isDead) {
                return;
            }
            this._isDead = true;

            if (this._damageStates != null && this._damageStates.Count > 0) {
                // Turn off all the damage state containers
                {
                    var enumerator = this._damageStates.GetEnumerator();
                    while (enumerator.MoveNext()) {
                        enumerator.Current.damageStateContainer.gameObject.SetActive(false);
                    }
                    enumerator.Dispose();
                }
            }

            Rigidbody2D[] rigidbody2Ds = this.gameObject.GetComponentsInChildren<Rigidbody2D>();
            if (rigidbody2Ds != null) {
                for (int i = 0; i < rigidbody2Ds.Length; ++i) {
                    GameObject.Destroy(rigidbody2Ds[i]);
                }
            }

            Collider2D[] collider2Ds = this.gameObject.GetComponentsInChildren<Collider2D>();
            if (collider2Ds != null) {
                for (int i = 0; i < collider2Ds.Length; ++i) {
                    GameObject.Destroy(collider2Ds[i]);
                }
            }


        }

        private void OnCollisionEnter2D(Collision2D col) {
            if (GameController.Instance.GameType == GameController.GameType_t.MULTI_PLAYER &&
                !this._photonView.IsMine) {
                return;
            }

            if (col.gameObject == null) {
                return;
            }

            float impulseMagnitude;
            if (col.contactCount <= 0) {
                impulseMagnitude = col.relativeVelocity.magnitude;
            } else {
                impulseMagnitude = Vector2.Dot(col.relativeVelocity, col.GetContact(0).normal);
            }

            if (col.rigidbody != null && col.otherRigidbody != null) {
                Rigidbody2D otherRigidBody = this._rigidbody2D == col.rigidbody ? col.otherRigidbody : col.rigidbody;
                impulseMagnitude *= otherRigidBody.mass / (otherRigidBody.mass + this._rigidbody2D.mass);
            }

            this.ApplyCollisionImpulse(impulseMagnitude);
        }

        private void ApplyCollisionImpulse(float impulseMagnitude) {
            if (impulseMagnitude <= kMinImpulseForFiltering) {
                return;
            }
            this._currentHealth -= impulseMagnitude;
        }

        #region Multiplayer Synchronization
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(this._currentHealth);
            } else {
                this._currentHealth = (float)stream.ReceiveNext();
            }
        }
        #endregion
    }
}