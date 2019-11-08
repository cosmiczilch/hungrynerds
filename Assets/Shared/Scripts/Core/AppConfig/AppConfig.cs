using System.Collections;
using System.IO;
using SharedBrawl.Debug;
using SharedBrawl.Init;
using SharedBrawl.Loading;
using SharedBrawl.Instance;
using UnityEngine;

public class AppConfig : MonoBehaviour, IInitializable, IInstance {

    [SerializeField] private TextAsset _appConfigTextAsset;

    public static AppConfig Instance {
        get {
            return InstanceLocator.Instance<AppConfig>();
        }
    }

    public enum Environment {
        LOCAL,
        DEV,
        STAGING,
        PROD
    }

    private AppConfigData _appConfigData;


    #region Public API
    public int GetAppID() {
        return _appConfigData.appID;
    }
    
    public Environment GetCurrentEnvironment() {
        if (_appConfigData == null) {
            DebugLog.LogErrorColor("AppConfig not set", LogColor.red);
            return Environment.LOCAL;
        }
        return _appConfigData.currentEnvironment;
    }

    public string GetLobbyServerUrl() {
        switch (this.GetCurrentEnvironment()) {
            case Environment.LOCAL: return "ws://localhost:8000/" + _appConfigData.lobbyServerEndpoint + "/";
            case Environment.DEV: return "ws://34.93.33.242:8000/" + _appConfigData.lobbyServerEndpoint + "/";    // TODO: avi: Update this
            case Environment.STAGING: return "ws://10.197.37.146:8000/" + _appConfigData.lobbyServerEndpoint + "/";    // TODO: avi: Update this
            case Environment.PROD: return "" + _appConfigData.lobbyServerEndpoint + "/";
        }

        return null;
    }
    
    public string GetRoomServerUrl() {
        switch (this.GetCurrentEnvironment()) {
            case Environment.LOCAL: return "ws://localhost:8001/" + _appConfigData.roomServerEndpoint + "/";
            case Environment.DEV: return "ws://34.93.33.242:8001/" + _appConfigData.roomServerEndpoint + "/";    // TODO: avi: Update this
            case Environment.STAGING: return "ws://10.197.37.146:9000/" + _appConfigData.roomServerEndpoint + "/";    // TODO: avi: Update this
            case Environment.PROD: return "" + _appConfigData.roomServerEndpoint + "/";
        }

        return null;
    }


    public string GetDebugDeviceID() {
        if (UnityEngine.Debug.isDebugBuild) {
            return _appConfigData.debugDeviceId;
        }
        return null;
    }
    #endregion

    #region IInitializable
    public void StartInitialize() {
        InstanceLocator.RegisterInstance<AppConfig>(this);
        this.StartCoroutine(this.InitializeAsync(() => {
            this.IsFullyInitialized = true;
        }));
    }

    public string GetName {
        get {
            return this.GetType().Name;
        }
    }

    public bool IsFullyInitialized {
        get; private set;
    }

    private IEnumerator InitializeAsync(System.Action callback) {
        this._appConfigData = AppConfigHelper.LoadAppConfigDataFromTextAsset(this._appConfigTextAsset);
        callback.Invoke();
        yield break;
    }
    #endregion

}
