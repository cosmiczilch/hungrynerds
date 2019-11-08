using UnityEngine;

namespace SharedBrawl.Loading {
    public abstract class AsyncRequest : CustomYieldInstruction {
        public abstract void StartRequest();
    }
}