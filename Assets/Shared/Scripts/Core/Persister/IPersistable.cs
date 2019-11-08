
namespace SharedBrawl.Persister {

    public interface IPersistable {
        string GetBaseFolderName();
        string GetFileName();
    }
}