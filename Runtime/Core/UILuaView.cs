using System;
using UnityEngine;

namespace Framework.UI
{
    public class UILuaView : IView
    {
        public GameObject UIObj { get; private set; }
        public ILuaTable Table { get; }
        public string Name { get; }
        public string AssetPath { get; }
        public Priority Priority { get; }
        public ViewStyle ViewStyle { get; }

        private Action<ILuaTable> _mUpdate;
        private Action<ILuaTable, GameObject> _mEnter;
        private Action<ILuaTable> _mPause;
        private Action<ILuaTable> _mResume;
        private Action<ILuaTable> _mExit;

        public UILuaView(ILuaTable table)
        {
            Table = table;
            Name = table.Get<string>("__cname");

            try
            {
                AssetPath = table.Get<string>("AssetPath");
            }
            catch
            {
                throw new Exception($"UI name->{Name} need init 'AssetPath' member!");
            }

            try
            {
                ViewStyle = table.Get<ViewStyle>("ViewStyle");
            }
            catch
            {
                throw new Exception($"UI name->{Name} need init 'ViewStyle' member!");
            }

            try
            {
                Priority = table.Get<Priority>("Priority");
            }
            catch
            {
                Priority = Priority.NORMAL;
            }

            _mEnter = table.Get<Action<ILuaTable, GameObject>>("OnEnter");
            _mPause = table.Get<Action<ILuaTable>>("OnPause");
            _mResume = table.Get<Action<ILuaTable>>("OnResume");
            _mExit = table.Get<Action<ILuaTable>>("OnExit");
            _mUpdate = table.Get<Action<ILuaTable>>("OnUpdate");
        }

        public void OnEnter(GameObject uiObj)
        {
            UIObj = uiObj;
            _mEnter?.Invoke(Table, uiObj);
        }

        public void OnPause()
        {
            _mPause?.Invoke(Table);
        }

        public void OnResume()
        {
            _mResume?.Invoke(Table);
        }

        public void OnExit()
        {
            try
            {
                _mExit?.Invoke(Table);
            }
            catch (Exception e)
            {
                Debug.LogError($"UILuaView: {Name}, OnExit error: {e.Message}");
            }
            finally
            {
                _mUpdate = null;
                _mEnter = null;
                _mPause = null;
                _mResume = null;
                _mExit = null;
            }
        }

        public void OnUpdate()
        {
            _mUpdate?.Invoke(Table);
        }
    }
}