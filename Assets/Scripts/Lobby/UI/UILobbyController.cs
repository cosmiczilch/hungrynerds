using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.UI;

namespace Lobby {
    public class UILobbyController : DialogControllerBase<UILobbyView> {

        private const string kPrefabPath = "Prefabs/UI/UILobbyView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        protected override void ConfigureView() {
            this.View.Configure(this.HandlePlayButtonClicked);
        }

        private void HandlePlayButtonClicked() {
            MultiPlayerManager.Instance.CreateOrJoinRandomRoom(this.HandleRoomJoined, this.HandleRoomJoinFailed);
        }

        private void HandleRoomJoined() {
            DebugLog.LogColor("Room joined", LogColor.purple);
        }

        private void HandleRoomJoinFailed() {
            DebugLog.LogColor("Room join failed", LogColor.red);
        }
    }
}