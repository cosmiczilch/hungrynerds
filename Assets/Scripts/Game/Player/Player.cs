using Photon.Pun;
using TimiMultiPlayer;
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

        private const string kPlayerViewPrefabPath = "Prefabs/GameScene/NetworkResources/Resources/PlayerView";

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
            }
        }
    }
}