using UnityEngine;

namespace Game {
    public class DebugShortcuts : MonoBehaviour {

        private void Update() {
            if (Input.GetKey(KeyCode.W)) {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemies != null) {
                    foreach (GameObject enemy in enemies) {
                        GameObject.Destroy(enemy);
                    }
                }
            }
        }
    }
}