using System.IO;
using SharedBrawl.Loading;
using UnityEditor;
using UnityEngine;

public class AppConfigWindow : EditorWindow {

    private int _appID;
    private AppConfig.Environment _currentEnvironment;
    private string _lobbyServerEndpoint;
    private string _roomServerEndpoint;
    private string _deviceId;
    private bool _initialized = false;

    [MenuItem("SharedBrawl/AppConfig")]
    public static void ShowWindow() {
        AppConfigWindow thisWindow = EditorWindow.GetWindow(typeof(AppConfigWindow)) as AppConfigWindow;

        if (!thisWindow._initialized) {
            thisWindow.Initialize();
        }
    }

    private void Initialize() {

        SharedBrawlURI fileURI = AppConfigHelper.AppConfigFileURI;
        using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Open, FileAccess.Read)) {
            TextAsset appConfigTextAsset = Resources.Load<TextAsset>(AppConfigHelper.AppConfigFileURI.GetFullPath());
            AppConfigData appConfigData = AppConfigHelper.LoadAppConfigDataFromTextAsset(appConfigTextAsset);
            if (appConfigData != null) {
                this._appID = appConfigData.appID;
                this._currentEnvironment = appConfigData.currentEnvironment;
                this._lobbyServerEndpoint = appConfigData.lobbyServerEndpoint;
                this._roomServerEndpoint = appConfigData.roomServerEndpoint;
                this._deviceId = appConfigData.debugDeviceId;
            } else {
                this._currentEnvironment = AppConfig.Environment.LOCAL;
            }
            fileStream.Close();
        }

        this._initialized = true;
    }

    void OnGUI() {
        if (!this._initialized) {
            this.Initialize();
        }

        string newAppIDString = EditorGUILayout.TextField("App ID:", this._appID.ToString());
        int newAppID;
        if (!int.TryParse(newAppIDString, out newAppID)) {
            newAppID = 0;
        }
        AppConfig.Environment newEnvironment = (AppConfig.Environment)EditorGUILayout.EnumPopup("Current Environment:", this._currentEnvironment);
        string newLobbyServerEndpoint = EditorGUILayout.TextField("Lobby Server Endpoint", this._lobbyServerEndpoint);
        string newRoomServerEndpoint = EditorGUILayout.TextField("Room Server Endpoint", this._roomServerEndpoint);
        string newDeviceId = EditorGUILayout.TextField("Debug Device ID", this._deviceId);

        if (newAppID != this._appID ||
            newEnvironment != this._currentEnvironment ||
            newLobbyServerEndpoint != this._lobbyServerEndpoint ||
            newRoomServerEndpoint != this._roomServerEndpoint ||
            newDeviceId != this._deviceId) {
            this.SaveEnvironmentSelection(newAppID, newEnvironment, newLobbyServerEndpoint, newRoomServerEndpoint, newDeviceId);
            this._appID = newAppID;
            this._currentEnvironment = newEnvironment;
            this._lobbyServerEndpoint = newLobbyServerEndpoint;
            this._roomServerEndpoint = newRoomServerEndpoint;
            this._deviceId = newDeviceId;
        }
    }

    private void SaveEnvironmentSelection(int appID, AppConfig.Environment environment, string lobbyServerEndpoint, string roomServerEndpoint, string deviceId) {
        AppConfigData newAppConfigData = new AppConfigData();
        newAppConfigData.appID = appID;
        newAppConfigData.currentEnvironment = environment;
        newAppConfigData.lobbyServerEndpoint = lobbyServerEndpoint;
        newAppConfigData.roomServerEndpoint = roomServerEndpoint;
        newAppConfigData.debugDeviceId = deviceId;
        AppConfigHelper.SaveAppConfigData(newAppConfigData);
    }


}
