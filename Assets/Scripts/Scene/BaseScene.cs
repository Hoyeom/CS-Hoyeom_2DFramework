using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Manager.Core
{
    public abstract class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.None;
        private static string ES_NAME = "@EventSystem";

        private void Awake()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
                Managers.Resource.Instantiate("UI/EventSystem").name = ES_NAME;

            name = $"@{this.GetType().Name}";
            
            Initialize();
        }

        protected virtual void Initialize()
        {
            
        }
        
        public abstract void Clear();
    }
}   