using System.Threading;
using Game.UI;
using Photon.Pun;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Game {
    public class GameController {

        public enum GameType_t {
            SINGLE_PLAYER,
            MULTI_PLAYER
        }

        public class Config {
            public GameType_t gameType;
        }

        public GameView View {
            get; private set;
        }

        public UIGameController UIController {
            get; private set;
        }

        private Config _config;

        public Player PlayerUs {
            get; private set;
        }
        public Player PlayerOther {
            get; private set;
        }

        public GameType_t GameType {
            get {
                if (this._config != null) {
                    return this._config.gameType;
                } else {
                    DebugLog.LogErrorColor("Game controller config not set", LogColor.red);
                    return GameType_t.SINGLE_PLAYER;
                }
            }
        }

        private const string kGameViewPrefabPath = "Prefabs/GameScene/RootGameView";

        public GameController(Config config) {
            this._config = config;

            this.PlayerUs = new Player(this, Player.PlayerType_t.Us);
            if (this._config.gameType == GameType_t.MULTI_PLAYER) {
                this.PlayerOther = new Player(this, Player.PlayerType_t.Other);
            }

            this.CreateView();

            Transform playerUsAnchor = null;
            if (this.GameType == GameType_t.SINGLE_PLAYER) {
                playerUsAnchor = this.View.Player1Anchor;
            } else {
                playerUsAnchor = MultiPlayerManager.Instance.AreWePlayer1() ? this.View.Player1Anchor
                                                                            : this.View.Player2Anchor;
            }
            this.PlayerUs.CreateView(playerUsAnchor);
            // Don't create view for other player as it will be synced over the network
        }

        public void LeaveGame() {
            if (this.GameType == GameType_t.MULTI_PLAYER) {
                MultiPlayerManager.Instance.LeaveRoom();
            }
            AppSceneManager.Instance.LoadLobbyScene();
        }

        private void CreateView() {
            GameObject go = PrefabLoader.Instance.InstantiateSynchronous(kGameViewPrefabPath);
            go.AssertNotNull("Game View Prefab");

            this.View = go.GetComponent<GameView>();
            this.View.AssertNotNull("Game View Component");

            // If we are player 2 in the game, we flip the camera horizontally
            // This way, each player thinks they are the player on the left
            if (this.GameType == GameType_t.MULTI_PLAYER && !MultiPlayerManager.Instance.AreWePlayer1()) {
                this.View.FlipCameraHorizontal();
            }

            this.UIController = new UIGameController(this);
            this.UIController.PresentDialog();
        }

    }
}