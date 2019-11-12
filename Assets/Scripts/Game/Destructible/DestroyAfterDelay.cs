using UnityEngine;

namespace Game {
    public class DestroyAfterDelay : MonoBehaviour {

        [SerializeField] private float _delaySeconds = 2.0f;

        private void Awake() {
            CoroutineHelper.Instance.RunAfterDelay(this._delaySeconds, () => {
                if (this != null && this.gameObject != null) {
                    GameObject.Destroy(this.gameObject);
                }
            });
        }
    }
}