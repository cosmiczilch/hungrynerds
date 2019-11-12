using Photon.Pun;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Game {
    public class Player {

        public enum PlayerType_t {
            Us,
            Other
        }

        public PlayerType_t PlayerType {
            get; private set;
        }

        public PlayerView View {
            get; private set;
        }

        private GameController _gameController;
        private Projectile _currentProjectile;

        private const string kPlayerViewPrefabPath = "Prefabs/GameScene/NetworkResources/Resources/PlayerView";
        private const string kProjectilePrefabPath = "Prefabs/GameScene/NetworkResources/Resources/Projectile";

        private const float kForceMultiplierMin = 2500.0f;
        private const float kForceMultiplierMax = 4000.0f;

        private const float kNextProjectileCooldownSeconds = 3.0f;

        public Player(GameController gameController, PlayerType_t playerType) {
            this._gameController = gameController;
            this.PlayerType = playerType;
        }

        public void CreateView(Transform anchor) {
            // Only instantiate prefab if this is our player. Other player's view is synced over the network
            if (this.PlayerType == PlayerType_t.Us) {

                GameObject go = null;
                switch (this._gameController.GameType) {

                    case GameController.GameType_t.SINGLE_PLAYER: {
                        go = PrefabLoader.Instance.InstantiateSynchronous(kPlayerViewPrefabPath, anchor);
                    } break;

                    case GameController.GameType_t.MULTI_PLAYER: {
                        go = MultiPlayerManager.Instance.InstantiatePrefab(kPlayerViewPrefabPath, anchor);
                    } break;

                }
                go.AssertNotNull("Player view game object");
                this.View = go.GetComponent<PlayerView>();
                this.View.AssertNotNull("Player view component");

                this.LoadNextProjectile();
            }
        }

        private void LoadNextProjectile() {
            // Check because this is called from a coroutine
            if (this == null || this.View == null) {
                return;
            }

            // Already have a projectile
            if (this._currentProjectile != null) {
                return;
            }

            GameObject projectileGO = null;
            if (this._gameController.GameType == GameController.GameType_t.SINGLE_PLAYER) {
                projectileGO = PrefabLoader.Instance.InstantiateSynchronous(kProjectilePrefabPath, this.View.LaunchPosition);
            } else {
                projectileGO = MultiPlayerManager.Instance.InstantiatePrefab(kProjectilePrefabPath, this.View.LaunchPosition);
            }
            projectileGO.AssertNotNull("Projectile game object");

            this._currentProjectile = projectileGO.GetComponent<Projectile>();
            this._currentProjectile.AssertNotNull("Projectile component");

            Rigidbody2D rigidbody2D = projectileGO.GetComponent<Rigidbody2D>();
            rigidbody2D.AssertNotNull("RigidBody2D component");
            rigidbody2D.gravityScale = 0.0f;
        }

        private void LaunchCurrentProjectile(float normalizedStrength, float angle) {
            this._currentProjectile.AssertNotNull("Current projectile");
            this._currentProjectile.MarkAsLaunched();

            Rigidbody2D rigidbody2D = this._currentProjectile.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.AssertNotNull("RigidBody2D component");

            rigidbody2D.gravityScale = 1.0f;

            float angleRad = angle * Mathf.PI / 180.0f;

            Vector2 force = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) *
                            Mathf.Lerp(kForceMultiplierMin, kForceMultiplierMax, normalizedStrength);

            rigidbody2D.AddForce(force);

            this._currentProjectile = null;

            CoroutineHelper.Instance.RunAfterDelay(kNextProjectileCooldownSeconds, this.LoadNextProjectile);
        }

        public void HandleDragToLaunchStarted() {
            if (this._currentProjectile == null) {
                return;
            }
            this.View.ShowLaunchArrow();
        }

        public void HandleDragToLaunchMoved(float normalizedStrength, float angle) {
            if (this._currentProjectile == null) {
                return;
            }
            this.View.ShowLaunchArrow(normalizedStrength, angle);
        }

        public void HandleDragToLaunchEnded(float normalizedStrength, float angle) {
            this.View.HideLaunchIndicatorArrow();

            if (this._currentProjectile == null) {
                return;
            }
            this.LaunchCurrentProjectile(normalizedStrength, angle);
        }

    }
}