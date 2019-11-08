using TimiShared.Init;
using TimiShared.Instance;

public class AppInit : AppInitBase {

    #region Events
    public static System.Action OnAppInitComplete = delegate {};
    #endregion

    #region IInitializable
    private bool _isFullyInitialized = false;

    public override void StartInitialize() {

        AppSceneManager appSceneManager = new AppSceneManager();
        InstanceLocator.RegisterInstance<AppSceneManager>(appSceneManager);

        this._isFullyInitialized = true;
        OnAppInitComplete.Invoke();
    }

    public override bool IsFullyInitialized {
        get {
            return this._isFullyInitialized;
        }
    }
    #endregion

}
