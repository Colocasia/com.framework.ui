using System.Reflection;

namespace Framework.Update
{
    public interface IHotUpdateExecutor
    {
        // 初始化热更环境
        void Initialize();
    
        // 加载热更DLL
        Assembly LoadHotUpdateDLL(byte[] dllBytes);
    
        // 执行热更入口方法
        void InvokeHotUpdateEntry(Assembly hotUpdateAssembly);
    
        // 补充元数据（如AOT泛型）
        void SupplyMetadata(byte[] metadataBytes);
    }
}