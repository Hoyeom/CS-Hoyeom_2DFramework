using System.Collections.Generic;
using UnityEditor;

namespace Manager.Core
{
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    
    public class DataManager
    {
        public void Initialize()
        {
            
        }

        
        private Loader LoadCsv<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            return default;
        }

    }
}