using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using TimiShared.Debug;
using TimiShared.Extensions;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Game {
    public class DestructibleBase : NetworkedObjectBase, IPunObservable {

        [SerializeField] private PhotonView _photonView;
        protected override PhotonView PhotonView {
            get {
                return this._photonView;
            }
        }

        [SerializeField] protected float _startingHealth;
        // TODO: Remove serialized after testing
        [SerializeField] protected float _currentHealth;


        [System.Serializable]
        protected class DamageState {
            public float normalizedHealth;
            public Transform damageStateContainer;
        }
        [SerializeField] private List<DamageState> _damageStates;

        protected Rigidbody2D _rigidbody2D;

        protected virtual void Start() {
            if (this._startingHealth == 0) {
                this._startingHealth = 1;
            }
            this._currentHealth = this._startingHealth;

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
        }

        protected virtual void Update() {
            if (this._currentHealth <= 0) {
                GameObject.Destroy(this.gameObject);
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

        private void OnCollisionEnter2D(Collision2D col) {
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