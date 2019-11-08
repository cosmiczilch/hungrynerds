
namespace SharedBrawl.Init {
    public interface IInitializable {
        void StartInitialize();
        bool IsFullyInitialized { get; }
        string GetName { get; }
    }
}