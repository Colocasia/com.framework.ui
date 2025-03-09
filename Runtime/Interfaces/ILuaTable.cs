namespace Framework.UI
{
    public interface ILuaTable
    {
        TValue Get<TValue>(string filed);
    }
}