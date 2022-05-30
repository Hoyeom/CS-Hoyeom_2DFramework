using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Manager.Core
{
    public class ResourceManager
    {
        private string prefabsPath = "Prefabs";
        
        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = Managers.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }
            
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"{prefabsPath}/{path}");
            
            if(original == null) throw new Exception($"Failed To Load Prefab : {path}");
            
            GameObject go = InstantiateOrPop(original, parent);
            
            go.name = original.name;
            
            return go;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"{prefabsPath}/{path}");
            
            if(original == null) throw new Exception($"Failed To Load Prefab : {path}");
            
            GameObject go = InstantiateOrPop(original,position,rotation, parent);
            
            go.name = original.name;
            
            return go;
        }

        private GameObject InstantiateOrPop(GameObject original,Transform parent)
        {
            if (original.GetComponent<Poolable>() != null)
                return Managers.Pool.Pop(original, parent).gameObject;

            return Object.Instantiate(original, parent);
        }
        
        private GameObject InstantiateOrPop(GameObject original,Vector3 position,Quaternion rotation,Transform parent)
        {
            if (original.GetComponent<Poolable>() != null)
                return Managers.Pool.Pop(original, position, rotation, parent).gameObject;

            return Object.Instantiate(original, position, rotation, parent);
        }

        public void Destroy(GameObject go)
        {
            if(go == null)
                return;
            
            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable != null)
            {
                Managers.Pool.Push(poolable);
                return;
            }
            
            Object.Destroy(go);
        }

    }

}