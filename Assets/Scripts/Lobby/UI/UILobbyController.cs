using System.Collections;
using Game;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.UI;
using UnityEngine;

namespace Lobby {
    public class UILobbyController : DialogControllerBase<UILobbyView> {

        private const string kPrefabPath = "Prefabs/UI/UILobbyView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        protected override void ConfigureView() {
            this.View.Configure(this.HandleSinglePlayerButtonClicked, this.HandleMultiPlayerButtonClicked);
        }

        private void HandleSinglePlayerButtonClicked() {
            this.StartGame(GameController.GameType_t.SINGLE_PLAYER);
        }

        private void HandleMultiPlayerButtonClicked() {
            this.View.SetState(UILobbyView.State.Connecting);
            MultiPlayerManager.Instance.CreateOrJoinRandomRoom(this.HandleRoomJoined, this.HandleRoomJoinFailed);
        }

        private void HandleRoomJoined() {
            this.View.SetState(UILobbyView.State.FindingMatch);
            CoroutineHelper.Instance.RunCoroutine(this.WaitForOtherPlayer(() => {
                                                                              this.StartGame(GameController.GameType_t.MULTI_PLAYER);
                                                                          },
                                                                          this.HandleTimeoutWaitingForMatch,
                                                                          AppConstants.kWaitForMatchTimeoutSeconds));
        }

        private void HandleTimeoutWaitingForMatch() {
            this.View.SetState(UILobbyView.State.TimedOut);
            CoroutineHelper.Instance.RunAfterDelay(4.0f, () => {
                this.View.SetState(UILobbyView.State.Idle);
            });
            MultiPlayerManager.Instance.LeaveRoom();

            DebugLog.LogColor("Timed out waiting for match", LogColor.grey);
        }

        private void HandleRoomJoinFailed() {
            // TODO: Show Error dialog
            DebugLog.LogColor("Room join failed", LogColor.red);
        }

        private IEnumerator WaitForOtherPlayer(System.Action successCallback, System.Action failureCallback, float timeoutSeconds) {
            float startTime = Time.time;
            float elapsedTime = 0;
            while ((elapsedTime = Time.time - startTime) <= timeoutSeconds) {
                if (MultiPlayerManager.Instance.NumPlayersInRoom >= 2) {
                    if (successCallback != null) {
                        successCallback.Invoke();
                    }
                    break;
                }

                yield return null;
            }

            if (elapsedTime > timeoutSeconds) {
                if (failureCallback != null) {
                    failureCallback.Invoke();
                }
            }
        }

        private void StartGame(GameController.GameType_t gameType) {
            DebugLog.LogColor("Starting game: " + gameType.ToString(), LogColor.green);
            AppSceneManager.Instance.LoadGameScene(gameType);

            // TODO: Hack: This delay is to make sure the game scenes are created and synced across the different clients
            // Otherwise, we will see some gameobjects of other players "drift" into their initial positions
            CoroutineHelper.Instance.RunAfterDelay(2.5f, () => {
                if (this != null && this.View != null) {
                    this.RemoveDialog();
                }
            });
        }
    }
}