using System.Threading.Tasks;

namespace Framework.UI
{
    public interface IResourceLoader
    {
        T LoadAssetSync<T>(string path);
        
        Task<T> LoadAssetAsync<T>(string path);
        
        void ReleaseAsset<T>(T asset);
    }
}