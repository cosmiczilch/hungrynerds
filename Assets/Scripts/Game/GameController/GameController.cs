using Photon.Pun;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Game {
    public class GameController {

        public enum GameType {
            SINGLE_PLAYER,
            MULTI_PLAYER
        }

        public class Config {
            public GameType gameType;
        }

        private GameView _view;
        private Config _config;

        public Player PlayerUs {
            get; private set;
        }
        public Player PlayerOther {
            get; private set;
        }

        private const string kGameViewPrefabPath = "Prefabs/GameScene/RootGameView";

        public GameController(Config config) {
            this._config = config;

            this.PlayerUs = new Player(Player.PlayerType_t.Us);
            if (this._config.gameType == GameType.MULTI_PLAYER) {
                this.PlayerOther = new Player(Player.PlayerType_t.Other);
            }

            this.CreateView();
        }

        private void CreateView() {
            GameObject go = PrefabLoader.Instance.InstantiateSynchronous(kGameViewPrefabPath);
            go.AssertNotNull("Game View Prefab");

            this._view = go.GetComponent<GameView>();
            this._view.AssertNotNull("Game View Component");
        }

    }
}