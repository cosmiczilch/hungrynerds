using System.Collections;
using SharedBrawl.Extensions;
using SharedBrawl.Instance;
using SharedBrawl.Loading;
using SharedBrawl.Multiplayer;
using UnityEngine;

namespace SharedBrawl.Init {
    public class SharedInit : MonoBehaviour, IInitializable {

        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private bool _useMultiplayer;
        
        private MatchmakerClient _matchmakerClient;
        private RoomServerClient _roomServerClient;

        #region IInitializable
        public void StartInitialize() {
            this.StartCoroutine(this.InitializationSequence(() => {
                this.IsFullyInitialized = true;
            }));
        }

        public bool IsFullyInitialized {
            get; private set;
        }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }
        #endregion

        public void Cleanup() {
            if (!this.IsFullyInitialized) {
                return;
            }

            if (this._matchmakerClient != null) {
                this._matchmakerClient.Cleanup();
            }
        }
        
        private IEnumerator InitializationSequence(System.Action callback) {

            InitializableSerialGroup initializables = new InitializableSerialGroup("Shared Init");

            this._prefabLoader.AssertNotNull("Prefab loader");
            initializables.AddInitializable(this._prefabLoader);

            this._sceneLoader.AssertNotNull("Scene loader");
            initializables.AddInitializable(this._sceneLoader);

            initializables.StartInitialize();
            while (!initializables.IsFullyInitialized) {
                yield return null;
            }
            
            if (this._useMultiplayer) {
                this._matchmakerClient = new MatchmakerClient();
                InstanceLocator.RegisterInstance<MatchmakerClient>(this._matchmakerClient);
                
                this._roomServerClient = new RoomServerClient();
                InstanceLocator.RegisterInstance<RoomServerClient>(this._roomServerClient);
            }

            callback.Invoke();
        }

    }
}