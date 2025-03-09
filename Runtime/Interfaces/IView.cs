using UnityEngine;

namespace Framework.UI
{
    public enum Priority : byte
    {
        HUD = 0,
        NORMAL = 1,
        LAUNCH = 10,
        RUNTIMEINFO = 100,
    }

    public enum ViewStyle : byte
    {
        NORMAL = 0,
        BLURBG = 1,
        FULL = 2,
    }

    public interface IView
    {
        public GameObject UIObj { get; }
        public string AssetPath { get; }
        public Priority Priority { get; }
        public ViewStyle ViewStyle { get; }
        public void OnEnter(GameObject uiObj);
        public void OnPause();
        public void OnResume();
        public void OnExit();
        public void OnUpdate();
    }
}