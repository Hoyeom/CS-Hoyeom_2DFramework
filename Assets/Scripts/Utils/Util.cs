using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class Util
    {
        
        /// <summary>
        /// 해당 이름의 컴포넌트를 반환 또는
        /// 새로운 오브젝트를 생성하여 컴포넌트를 추가한다 
        /// </summary>
        /// <param name="name">찾을이름</param>
        /// <typeparam name="T">컴포넌트</typeparam>
        /// <returns></returns>
        public static T GetOrNewComponent<T>(string name) where T : Component
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
                go.AddComponent(typeof(T));
            }
            
            return go.GetComponent<T>();
        }

        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null)
                return null;
            return transform.gameObject;
        }

        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
        {
            if (go == null)
                return null;

            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
            
            return null;
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out T component))
                component = go.AddComponent<T>();
            return component;
        }

        public static void SetPrivateData<T>(Object obj, string[] fieldNames, string[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                FieldInfo info = typeof(T).GetField(fieldNames[i],
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);
               
                if (info == null)
                    throw new Exception($"Field Name: {fieldNames[i]} / Data: {datas[i]}");

                info.SetValue(obj,
                    info.FieldType.IsEnum
                        ? Enum.Parse(info.FieldType, datas[i])
                        : Convert.ChangeType(datas[i], info.FieldType));
            }
        }


    }
}