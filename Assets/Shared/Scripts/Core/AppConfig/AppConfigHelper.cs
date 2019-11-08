using System;
using System.IO;
using SharedBrawl.Loading;
using SharedBrawl.Utils;
using UnityEngine;

public static class AppConfigHelper {

    private static string kAppConfigFileName = "AppConfig.json";

    private static SharedBrawlURI _appConfigFileURI = null;
    public static SharedBrawlURI AppConfigFileURI {
        get {
            if (_appConfigFileURI == null) {
                _appConfigFileURI = new SharedBrawlURI(FileBasePathType.LocalDataPath, "Resources/BootstrapData/" + kAppConfigFileName);
            }
            return _appConfigFileURI;
        }
    }

    public static AppConfigData LoadAppConfigDataFromTextAsset(TextAsset textAsset) {
        AppConfigData appConfigData = null;

        string appConfigDataJson = textAsset.text;
        if (!string.IsNullOrEmpty(appConfigDataJson)) {
            appConfigData = SharedBrawlSerializer.Deserialize<AppConfigData>(appConfigDataJson);
        }

        return appConfigData;
    }

    public static void SaveAppConfigData(AppConfigData appConfigData) {
        if (!Application.isEditor) {
            throw new NotImplementedException("Not yet supported to edit app config on device");
        }

        string appConfigDataJson = SharedBrawlSerializer.Serialize(appConfigData);
        if (!string.IsNullOrEmpty(appConfigDataJson)) {
            SharedBrawlURI appConfigFileUri = AppConfigHelper.AppConfigFileURI;
            using (Stream appConfigFileStream = FileLoader.GetFileStreamSync(appConfigFileUri, FileMode.Create, FileAccess.Write)) {
                FileUtils.PutStreamContents(appConfigFileStream, appConfigDataJson);
                appConfigFileStream.Close();
            }
        }
    }


}
