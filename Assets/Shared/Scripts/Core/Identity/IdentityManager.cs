using System;
using System.Security.Cryptography;
using System.Text;
using SharedBrawl.Debug;
using SharedBrawl.Init;
using SharedBrawl.Instance;
using UnityEngine;

namespace SharedBrawl.Identity {
    
    public class IdentityManager : MonoBehaviour, IInstance, IInitializable {

        #region Public Api
        public static IdentityManager Instance {
            get {
                return InstanceLocator.Instance<IdentityManager>();
            }
        }

        private User _currentUser;
        public User CurrentUser {
            get {
                return this._currentUser;
            }
        }
        #endregion
        
        
        private void Awake() {
            InstanceLocator.RegisterInstance<IdentityManager>(this);
        }

        #region IInitializable
        public void StartInitialize() {
            string duid = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(duid)) {
                DebugLog.LogErrorColor("Failed to get device unique identifier", LogColor.red);
                return;
            }
            MD5 md5hasher = MD5.Create();
            byte[] duid_bytes = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier));
            int userId = BitConverter.ToInt32(duid_bytes, 0);

            this._currentUser = new User(userId);

            this.IsFullyInitialized = true;
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
    }
}