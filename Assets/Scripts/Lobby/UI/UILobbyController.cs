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
            DebugLog.LogErrorColor("Play button clicked", LogColor.red);
        }
    }
}