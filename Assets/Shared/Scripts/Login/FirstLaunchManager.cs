using System.Collections;
using System.Collections.Generic;
using SharedBrawl.Debug;
using SharedBrawl.Init;
using SharedBrawl.Loading;
using SharedBrawl.Utils;
using UnityEngine;

namespace SharedBrawl.Login {
    
    public class FirstLaunchManager : MonoBehaviour, IInitializable {
        
        #region IInitializable
        public void StartInitialize() {
            this.StartCoroutine(DetectAndHandleFirstLaunch());
        }

        public bool IsFullyInitialized {
            get; private set;
        }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }
        #endregion

        [System.Serializable]
        private struct TextAssetInfo {
            public string fileName;
            public TextAsset textAsset;
        }
        [SerializeField] private List<TextAssetInfo> _bootstrapTextAssetInfos;

        private IEnumerator DetectAndHandleFirstLaunch() {
            if (this.IsFirstLaunch()) {
                
                if (FileUtils.DoesDirectoryExist(this.FirstLaunchDestinationURI)) {
                    DebugLog.LogColor("Deleting previous first launch data because app version doesn't match", LogColor.green);
                    FileUtils.DeleteDirectory(this.FirstLaunchDestinationURI);
                }
                
                DebugLog.LogColor("Copying first launch data", LogColor.green);
                yield return this.CopyFirstLaunchTextAssets();
                
                PlayerPrefs.SetString(kAppVersionKey, kAppVersion);
            }

            this.IsFullyInitialized = true;
            yield break;
        }

        private IEnumerator CopyFirstLaunchTextAssets() {
            FileUtils.CreateDirectory(this.FirstLaunchDestinationURI);
            
            var enumerator = this._bootstrapTextAssetInfos.GetEnumerator();
            while (enumerator.MoveNext()) {
                TextAsset textAsset = enumerator.Current.textAsset;
                SharedBrawlURI destinationURI = SharedBrawlURI.Combine(this.FirstLaunchDestinationURI, new SharedBrawlURI(FileBasePathType.LocalPersistentDataPath, enumerator.Current.fileName));

                FileUtils.WriteFile(destinationURI, textAsset.text);
            }
            enumerator.Dispose();
            
            yield break;
        }

        private SharedBrawlURI FirstLaunchDestinationURI {
            get {
                return new SharedBrawlURI(FileBasePathType.LocalPersistentDataPath, "AppData/DataModels");
            }
        }
        
        private const string kAppVersionKey = "app_version";
        private const string kAppVersion = "1.1.2";
        private bool IsFirstLaunch() {
            string appVersion = PlayerPrefs.GetString(kAppVersionKey, "");
            return !FileUtils.DoesDirectoryExist(this.FirstLaunchDestinationURI) || 
                appVersion != kAppVersion;
        }
    }
}