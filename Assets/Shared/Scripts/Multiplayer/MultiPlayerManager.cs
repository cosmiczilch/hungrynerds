using Photon.Pun;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

namespace TimiMultiPlayer {

    public class MultiPlayerManager : MonoBehaviourPunCallbacks, IInstance, IInitializable {

        public static MultiPlayerManager Instance {
            get {
                return InstanceLocator.Instance<MultiPlayerManager>();
            }
        }

        #region Constants
        private const string kGameMultiplayerVersion = "1.0";
        private const byte kMaxPlayersPerRoom = 2;
        #endregion

        private bool _isConnecting = false;

        public void StartInitialize() {
            InstanceLocator.RegisterInstance<MultiPlayerManager>(this);

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = SystemInfo.operatingSystem;

            IsFullyInitialized = true;
        }

        public bool IsFullyInitialized {
            get; private set;
        }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }
    }
}