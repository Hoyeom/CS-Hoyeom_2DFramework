using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Object = UnityEngine.Object;

namespace UI
{
    public abstract class UI_Base : MonoBehaviour
    {
        protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();
        public abstract void Initialize();
        private void Start() => Initialize();

        protected void Bind<T>(Type type) where T : Object
        {
            string[] names = Enum.GetNames(type);
            Object[] objects = new Object[names.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true);
                if (objects[i] == null)
                    throw new Exception($"Failed To Bind ({names[i]})");
            }
        }

        protected T Get<T>(int index) where T : Object
        {
            Object[] objects = null;
            if (!(_objects.TryGetValue(typeof(T), out objects))) return null;

            return objects[index] as T;
        }

        public static void BindEvent(GameObject go, Action<PointerEventData> action,
            Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler += action;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}