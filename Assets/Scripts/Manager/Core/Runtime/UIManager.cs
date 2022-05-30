using System.Collections.Generic;
using UI;
using UI.Popup;
using UI.UI_Scene;
using UnityEngine;
using Utils;

namespace Manager.Core
{
    public class UIManager
    {
        private int _order = 10;

        private static string NAME = "@UI";
        
        private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
        private UI_Scene _sceneUI = null;

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find(NAME);
                if (root == null)
                    root = new GameObject {name = NAME};
                return root;
            }
        }

        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = 0;
            }
        }

        public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
        {
            EmptyStringToTypeName<T>(ref name);

            GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
            
            if(parent != null)
                go.transform.SetParent(parent);

            Canvas canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            return Util.GetOrAddComponent<T>(go);
        }

        public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
        {
            EmptyStringToTypeName<T>(ref name);

            GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
            
            if(parent != null)
                go.transform.SetParent(parent);

            return Util.GetOrAddComponent<T>(go);
        }

        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            EmptyStringToTypeName<T>(ref name);

            GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
            T sceneUI = Util.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;
            
            go.transform.SetParent(Root.transform);

            return sceneUI;
        }
        
        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            EmptyStringToTypeName<T>(ref name);

            GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

            T popup = Util.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            go.transform.SetParent(Root.transform);

            return popup;
        }

        public void ClosePopupUI(UI_Popup popup)
        {
            if(_popupStack.Count == 0)
                return;

            if (_popupStack.Peek() != popup)
            {
                Debug.Log("Close Popup Failed");
                return;
            }
            
            ClosePopupUI();
        }

        public void ClosePopupUI()
        {
            if (_popupStack.TryPop(out UI_Popup popup))
            {
                Managers.Resource.Destroy(popup.gameObject);
                _order--;
            }
        }

        public void CloseAllPopup()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();
        }

        public void Clear()
        {
            CloseAllPopup();
            _sceneUI = null;
        }
        
        private void EmptyStringToTypeName<T>(ref string name)
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
        }
    }
}