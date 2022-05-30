using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager.Core
{
    public class SceneManagerEx
    {
        private BaseScene currentScene;
        public BaseScene CurrentScene
        {
            get
            {
                if(currentScene == null)
                    currentScene = Object.FindObjectOfType<BaseScene>();
                return currentScene;
            }
        }

        public void LoadScene(Define.Scene type)
        {
            Managers.ManagersClear();
            SceneManager.LoadScene(GetSceneName(type));
        }
        
        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }
        
        public void Clear()
        {
            currentScene = null;
            CurrentScene.Clear();
        }
    }
}