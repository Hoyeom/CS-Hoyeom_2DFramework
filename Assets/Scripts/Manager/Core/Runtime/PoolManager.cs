using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Core
{
    public class PoolManager
    {
        #region Pool

        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }

            private Stack<Poolable> _poolStack = new Stack<Poolable>();

            public void Initialize(GameObject original, int count = 5)
            {
                Original = original;
                Root = new GameObject().transform;
                Root.name = $"{original.name}'s Root";

                for (int i = 0; i < count; i++)
                    Push(Create());
            }
            public Poolable Create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetComponent<Poolable>();
            }
            public void Push(Poolable poolable)
            {
                if(poolable == null)
                    return;

                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.IsUsing = false;

                _poolStack.Push(poolable);
            }

            public Poolable Pop(Transform parent)
            {
                Poolable poolable = CreateOrPop();
                
                poolable.gameObject.SetActive(true);
                
                DonDestroyDisable(poolable, parent);

                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }

            public Poolable Pop(Vector3 position, Quaternion rotation, Transform parent = null)
            {
                Poolable poolable = CreateOrPop();

                poolable.transform.SetPositionAndRotation(position, rotation);
                poolable.gameObject.SetActive(true);

                DonDestroyDisable(poolable, parent);
                
                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }

            
            private void DonDestroyDisable(Poolable poolable,Transform parent)
            {
                if (parent == null)
                    poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            
            private Poolable CreateOrPop()
            {
                if (_poolStack.Count > 0)
                    return _poolStack.Pop();
                else
                    return Create();
            }
        }



        #endregion

        public static string NAME = "@Pools";

        private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
        private Transform _root;
        
        public void Initialize()
        {
            if (_root == null)
            {
                _root = new GameObject {name = NAME}.transform;
                Object.DontDestroyOnLoad(_root);
            }
        }

        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Initialize(original,count);
            pool.Root.parent = _root;

            _pools.Add(original.name, pool);
        }

        public void Push(Poolable poolable)
        {
            string name = poolable.gameObject.name;
            if (_pools.ContainsKey(name) == false)
            {
                Object.Destroy(poolable);
                return;
            }
            
            _pools[name].Push(poolable);
        }

        public Poolable Pop(GameObject original, Transform parent = null)
        {
            if(_pools.ContainsKey(original.name) == false)
                CreatePool(original);

            return _pools[original.name].Pop(parent);
        }

        public Poolable Pop(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if(_pools.ContainsKey(original.name) == false)
                CreatePool(original);

            return _pools[original.name].Pop(position, rotation, parent);
        }
        
        public GameObject GetOriginal(string name)
        {
            if (_pools.ContainsKey(name) == false)
                return null;

            return _pools[name].Original;
        }

        public void Clear()
        {
            foreach (Transform child in _root)
                Object.Destroy(child.gameObject);
            _pools.Clear();
        }
        
    }
}