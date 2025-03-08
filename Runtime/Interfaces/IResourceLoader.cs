using System;
using System.Collections;

namespace Framework.Update
{
    public interface IResourceLoader
    {
        // 同步加载资源
        T LoadAsset<T>(string assetKey) where T : UnityEngine.Object;
    
        // 异步加载资源
        IEnumerator LoadAssetAsync<T>(string assetKey, Action<T> onComplete) where T : UnityEngine.Object;
    
        // 获取资源版本号
        string GetResourceVersion();
    
        // 检测更新
        IEnumerator CheckUpdate(Action<bool> onResult);
    }
}