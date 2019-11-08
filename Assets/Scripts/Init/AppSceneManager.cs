using Init;
using Lobby;
using TimiShared.Debug;
using TimiShared.Instance;
using TimiShared.Loading;
using UnityEngine.SceneManagement;

public class AppSceneManager : IInstance {

    public static AppSceneManager Instance {
        get {
            return InstanceLocator.Instance<AppSceneManager>();
        }
    }

    public enum AppScene {
        LOBBY_SCENE,
        GAME_SCENE
    }

    private const string kLobbySceneName = "LobbyScene";
    private const string kGameSceneName = "GameScene";

    #region Public API
    public void LoadLobbyScene(System.Action callback) {
        this.LoadScene(AppScene.LOBBY_SCENE, (success) => {
            if (success) {
                UILobbyController lobbyController = new UILobbyController();
                lobbyController.PresentDialog();
            }
        });
    }

    public void LoadGameScene() {
        LoadingScreenManager.Instance.ShowLoadingScreen(true, false);

        this.LoadScene(AppScene.GAME_SCENE, (success) => {
            if (success) {
            }
        });

    }
    #endregion

    private void LoadScene(AppScene appScene, System.Action<bool /* success */> callback = null) {
        LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        string sceneName = null;
        switch (appScene) {

            case AppScene.LOBBY_SCENE:
                sceneName = kLobbySceneName;
                loadSceneMode = LoadSceneMode.Single;
                break;

            case AppScene.GAME_SCENE:
                sceneName = kGameSceneName;
                loadSceneMode = LoadSceneMode.Single;
                break;

            default:
                DebugLog.LogErrorColor("Invalid scene: " + appScene, LogColor.red);
                return;
        }

        SceneLoader.Instance.LoadSceneAsync(sceneName, loadSceneMode, (loadedSceneName, success) => {
            if (!success) {
                DebugLog.LogErrorColor("Failed to load scene: " + appScene, LogColor.red);
                if (callback != null) {
                    callback.Invoke(false);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(true);
                }
            }
        });
    }

}