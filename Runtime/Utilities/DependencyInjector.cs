namespace Framework.Update
{
    public static class DependencyInjector
    {
        private static IResourceLoader _resourceLoader;
        private static IHotUpdateExecutor _hotUpdateExecutor;

        // 注册资源加载器
        public static void RegisterResourceLoader(IResourceLoader loader)
        {
            if (loader == null) throw new ArgumentNullException(nameof(loader));
            _resourceLoader = loader;
        }

        // 注册热更新执行器 
        public static void RegisterHotUpdateExecutor(IHotUpdateExecutor executor)
        {
            if (executor == null) throw new ArgumentNullException(nameof(executor));
            _hotUpdateExecutor = executor;
        }

        // 获取已注册模块（带空值检查）
        public static IResourceLoader GetResourceLoader()
        {
            if(_resourceLoader == null) 
                throw new InvalidOperationException("未注册资源加载器!");
            return _resourceLoader;
        }

        public static IHotUpdateExecutor GetHotUpdateExecutor()
        {
            if(_hotUpdateExecutor == null)
                throw new InvalidOperationException("未注册热更新执行器!");
            return _hotUpdateExecutor;
        }
    }
}