using UnityEngine;

namespace Framework.UI
{
    public abstract class UICsView : IView
    {
        protected GameObject MuiObj;
        public abstract GameObject UIObj { get; }
        public abstract string AssetPath { get; }
        public abstract Priority Priority { get; }
        public abstract ViewStyle ViewStyle { get; }

        public abstract void OnEnter(GameObject uiObj);
        public abstract void OnPause();
        public abstract void OnResume();
        public abstract void OnExit();
        public abstract void OnUpdate();

        public virtual IView Open()
        {
            UIManager.Instance.OpenUI(this);
            return this;
        }

        public virtual void Close(UIManager manager)
        {
            UIManager.Instance.CloseUI(this);
        }
    }
}