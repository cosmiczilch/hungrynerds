using SharedBrawl.Debug;
using SharedBrawl.Loading;
using SharedBrawl.Utils;
using UnityEditor;

namespace SharedBrawl.Menu {
    
    public static class SharedBrawlMenu {

        [MenuItem("SharedBrawl/Clear Persistent AppData")]
        static void ClearPersistentAppData() {
            SharedBrawlURI appDataURI = new SharedBrawlURI(FileBasePathType.LocalPersistentDataPath, "AppData");
            if (FileUtils.DoesDirectoryExist(appDataURI)) {
                FileUtils.DeleteDirectory(appDataURI);
                DebugLog.LogColor("Cleared persistent app data", LogColor.grey);
            }
            else {
                DebugLog.LogColor("No persistent app data exists", LogColor.grey);
            }
        }

    }
}