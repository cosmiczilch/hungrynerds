using Game.UI;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Instance;
using TimiShared.Loading;
using UnityEngine;

namespace Game {
    public class GameController : IInstance {

        public static GameController Instance {
            get {
                return InstanceLocator.Instance<GameController>();
            }
        }

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

        public DestructiblePile DestructiblePileUs {
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

        // Required for IInstance
        public GameController() {
            InstanceLocator.RegisterInstance<GameController>(this);
        }

        public GameController(Config config) {
            InstanceLocator.RegisterInstance<GameController>(this);

            this._config = config;

            this.PlayerUs = new Player(this, Player.PlayerType_t.Us);
            if (this._config.gameType == GameType_t.MULTI_PLAYER) {
                this.PlayerOther = new Player(this, Player.PlayerType_t.Other);
            }

            if (this.GameType == GameType_t.SINGLE_PLAYER) {
                this.LoadGameSceneObjects();

            } else {

                MultiPlayerManager.Instance.StartGameInRoom();
                MultiPlayerManager.OnOtherPlayerLeftRoom += this.HandleOtherPlayerLeftRoom;

                // TODO: Hack: This is to give enough time for the other clients to load their game scene as well
                // Ideally we should be using messages / custom properties on room to do this
                CoroutineHelper.Instance.RunAfterDelay(1.0f, () => {
                    this.LoadGameSceneObjects();
                });
            }
        }

        private void LoadGameSceneObjects() {

            DebugLog.LogColor("Creating game view", LogColor.blue);
            this.CreateView();

            Transform playerUsAnchor = null;
            if (this.GameType == GameType_t.SINGLE_PLAYER) {
                playerUsAnchor = this.View.Player1Anchor;
            } else {
                playerUsAnchor = MultiPlayerManager.Instance.AreWePlayer1() ? this.View.Player1Anchor
                                                                            : this.View.Player2Anchor;
            }
            DebugLog.LogColor("Creating player view", LogColor.blue);
            this.PlayerUs.CreateView(playerUsAnchor);
            // Don't create view for other player as it will be synced over the network

            if (this.GameType == GameType_t.SINGLE_PLAYER) {
                this.View.Player2Anchor.gameObject.SetActive(false);
            }

            DebugLog.LogColor("Creating our destructible pile", LogColor.blue);
            this.CreateDestructiblePile();
        }

        private bool _isGameOver = false;
        public bool IsGameOver() {
            return this._isGameOver;
        }

        public void LeaveGame() {
            if (this.GameType == GameType_t.MULTI_PLAYER) {
                MultiPlayerManager.Instance.LeaveRoom();
            }
            AppSceneManager.Instance.LoadLobbyScene();
        }

        public void HandleGameOver(bool weWon, bool isFromDisconnect = false) {
            // Don't process this more than once
            if (this._isGameOver) {
                return;
            }

            this._isGameOver = true;

            DebugLog.LogWarningColor("Game over | isFromDisconnect: " + isFromDisconnect.ToString(), LogColor.blue);

            float delaySeconds = isFromDisconnect ? 1.0f : 2.0f;
            CoroutineHelper.Instance.RunAfterDelay(delaySeconds, () => {
                if (this != null && this.View != null) {
                    UIGameOverController gameOverController = new UIGameOverController(weWon, () => {
                        this.LeaveGame();
                    });
                    gameOverController.PresentDialog();
                }
            });
        }

        private void HandleOtherPlayerLeftRoom() {
            this.HandleGameOver(true, true);
        }


        private const string kLayerNamePlayer1 = "GameScenePlayer1";
        private const string kLayerNamePlayer2 = "GameScenePlayer2";

        public int GetLayer(bool isUnderLocalControl) {
            string layerName = kLayerNamePlayer1;
            if (this.GameType == GameType_t.MULTI_PLAYER) {
                if (isUnderLocalControl != MultiPlayerManager.Instance.AreWePlayer1()) {
                    layerName = kLayerNamePlayer2;
                }
            }

            return LayerMask.NameToLayer(layerName);
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

        private void CreateDestructiblePile() {
            // Create our destructible pile (other player's will be created by them and synced over the network)

            string prefabPath = "Prefabs/GameScene/NetworkResources/Resources/DestructiblePile1";
            GameObject destructiblePileGo = null;
            if (this.GameType == GameType_t.SINGLE_PLAYER) {
                destructiblePileGo = PrefabLoader.Instance.InstantiateSynchronous(prefabPath, this.View.DestructiblePile1Anchor);
            } else {
                Transform anchor = MultiPlayerManager.Instance.AreWePlayer1() ? this.View.DestructiblePile1Anchor
                                                                              : this.View.DestructiblePile2Anchor;
                destructiblePileGo = MultiPlayerManager.Instance.InstantiatePrefab(prefabPath, anchor);
            }
            destructiblePileGo.AssertNotNull("Destructible pile game object");

            // Do this to make sure that the destructible piles are mirrors of each other
            if (this.GameType == GameType_t.MULTI_PLAYER &&
                !MultiPlayerManager.Instance.AreWePlayer1()) {
                destructiblePileGo.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }

            DestructiblePile destructiblePile = destructiblePileGo.GetComponent<DestructiblePile>();
            destructiblePile.AssertNotNull("Destructible pile component");
        }

    }
}