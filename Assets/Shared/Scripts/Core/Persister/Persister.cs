using System.IO;
using SharedBrawl.Debug;
using SharedBrawl.Extensions;
using SharedBrawl.Loading;
using SharedBrawl.Utils;

namespace SharedBrawl.Persister {

    public class Persister {

        private IPersistable _target;

        public Persister(IPersistable target) {
            this._target = target;
        }

        public bool Save() {
            if (this._target == null) {
                DebugLog.LogErrorColor("No persistable target set", LogColor.red);
                return false;
            }

            SharedBrawlURI baseURI = this.BaseURI;
            SharedBrawlURI fileURI = this.FullFileURI;

            if (!FileUtils.DoesDirectoryExist(baseURI)) {
                FileUtils.CreateDirectory(baseURI);
            }

            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Create, FileAccess.ReadWrite)) {
                if (fileStream != null) {
                    SharedBrawlSerializer.Serialize(fileStream, this._target);
                    fileStream.Flush();
                    fileStream.Close();
                    return true;

                } else {
                    DebugLog.LogErrorColor("Unable to get file stream for writing for " + fileURI.GetFullPath(), LogColor.grey);
                }
            }
            return false;
        }

        public bool Restore() {
            SharedBrawlURI fileURI = this.FullFileURI;

            if (!FileUtils.DoesFileExist(fileURI)) {
                return false;
            }

            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Open, FileAccess.Read)) {
                if (fileStream != null) {
                    var obj = SharedBrawlSerializer.DeserializeNonGeneric(this._target.GetType(), fileStream);
                    return this._target.CopyObjectFieldsFrom(obj);

                } else {
                    DebugLog.LogErrorColor("Unable to get file stream for reading for " + fileURI.GetFullPath(), LogColor.grey);
                }
            }

            return false;
        }

        #region Helpers
        public SharedBrawlURI BaseURI {
            get {
                return new SharedBrawlURI(FileBasePathType.LocalPersistentDataPath, this._target.GetBaseFolderName());
            }
        }

        public SharedBrawlURI FullFileURI {
            get {
                SharedBrawlURI relativeURI = new SharedBrawlURI(FileBasePathType.LocalPersistentDataPath, this._target.GetFileName());
                return SharedBrawlURI.Combine(this.BaseURI, relativeURI);
            }
        }
        #endregion

    }
}