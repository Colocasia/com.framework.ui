using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public class UIManager
    {
        private const int IDX_DEFAULT = -2;
        private const int IDX_INVALID = -1;
        
        private List<IView> m_ViewList = new List<IView>();

        private IResourceLoader m_ResourceLoader;
        public Transform UIRoot { get; private set; }
        
        public static UIManager Instance { get; private set; }
        
        public UIManager(IResourceLoader resourceLoader, Transform root)
        {
            m_ResourceLoader = resourceLoader;
            UIRoot = root;
            Instance = this;
        }

        private static int SortView(IView a, IView b)
        {
            if (a.Priority > b.Priority) return 1;
            else if (a.Priority < b.Priority) return -1;
            else return 0;
        }

        public void OpenUI(ILuaTable talbe)
        {
            OpenUI(new UILuaView(talbe));
        }

        public void OpenUI(IView view)
        {
            if (null == view) return;

            if (m_ViewList.Count > 0)
            {
                IView top = m_ViewList[m_ViewList.Count - 1];
                top.OnPause();
            }

            m_ViewList.Add(view);
            m_ViewList.Sort(SortView);

            if (null != view.UIObj)
                InitUI(view, view.UIObj);
            else
                InitUI(view, m_ResourceLoader.LoadAssetSync<GameObject>(view.AssetPath));
        }

        public void InitUI(IView view, GameObject prefab)
        {
            view.OnEnter(GameObject.Instantiate(prefab));
            view.UIObj.transform.localPosition = Vector3.zero;
            view.UIObj.transform.localRotation = Quaternion.identity;
            view.UIObj.transform.localScale = Vector3.one;
            view.UIObj.transform.SetParent(UIRoot, false);
            UpdateSiblingIndex();
            UpdateStyle();
            view.OnResume();
        }

        private void UpdateSiblingIndex()
        {
            for (int i = 0; i < m_ViewList.Count; i++)
            {
                var view = m_ViewList[i];
                if (null != view.UIObj)
                    view.UIObj.transform.SetSiblingIndex(i);
            }
        }

        private void UpdateStyle(int curIndex = IDX_DEFAULT, int tagIndex = IDX_DEFAULT)
        {
            curIndex = (IDX_DEFAULT == curIndex) ? m_ViewList.Count - 1 : curIndex;
            if (curIndex <= IDX_INVALID || curIndex >= m_ViewList.Count) return;
            IView curView = m_ViewList[curIndex];

            if (null != curView.UIObj) curView.UIObj.SetActive(tagIndex < 0);

            var style = curView.ViewStyle;
            if (ViewStyle.FULL == style || ViewStyle.BLURBG == style) tagIndex = curIndex;

            UpdateStyle(--curIndex, tagIndex);
        }

        private void Update()
        {
            for (int i = 0; i < m_ViewList.Count; i++)
            {
                var view = m_ViewList[i];
                if (null != view) view.OnUpdate();
            }
        }

        public void CloseUI(ILuaTable table = null)
        {
            UILuaView view = null;
            if (null != table)
            {
                for (int i = 0; i < m_ViewList.Count; i++)
                {
                    var v = m_ViewList[i] as UILuaView;
                    if (null == v || v.Table.GetHashCode() != table.GetHashCode()) continue;
                    view = v;
                    break;
                }
            }

            CloseUI(view);
        }

        public void CloseUI(IView view = null)
        {
            if (m_ViewList.Count <= 0) return;
            if (null == view) view = m_ViewList[m_ViewList.Count - 1];
            view.OnExit();
            m_ViewList.Remove(view);
            Object.Destroy(view.UIObj);
            m_ResourceLoader.ReleaseAsset(view);
            UpdateStyle();

            if (m_ViewList.Count <= 0) return;
            IView under = m_ViewList[m_ViewList.Count - 1];
            under.OnResume();
        }

        public void CloseAllUI()
        {
            while (m_ViewList.Count > 0)
                CloseUI((IView) null);
        }

        private void OnDestroy()
        {
            CloseAllUI();
        }
    }
}