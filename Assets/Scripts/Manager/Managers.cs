using System;
using Manager.Contents;
using Manager.Core;
using UnityEngine;
using Utils;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private static Managers Instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }

    #region Contents

    private MapManager _map = new MapManager();
    public static MapManager Map => Instance._map;

    #endregion
    
    private static readonly string NAME = "@Managers";

    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private AudioManager _audio = new AudioManager();
    private PoolManager _pool = new PoolManager();
    private DataManager _data = new DataManager();
    private UIManager _ui = new UIManager();
    public static ResourceManager Resource => Instance._resource;
    public static SceneManagerEx Scene => Instance._scene;
    public static AudioManager Audio => Instance._audio;
    public static PoolManager Pool => Instance._pool;
    public static DataManager Data => Instance._data;
    public static UIManager UI => Instance._ui;
    
    
    private void Awake() => name = NAME;
    private void Start() => Initialize();

    #region ManagerMethod

    private static void Initialize()
    {
        if (_instance != null) return;
        
        MakeInstance(out _instance);
        
        ManagersInit();
    }
    
    private static void MakeInstance(out Managers managers)
    {
        managers = Util.GetOrNewComponent<Managers>(NAME);
        DontDestroyOnLoad(managers);
    }
    
    private static void ManagersInit()
    {
        _instance._audio.Initialize();
        _instance._data.Initialize();
        _instance._map.Initialize();
        _instance._pool.Initialize();
    }
    
    public static void ManagersClear()
    {
        Pool.Clear();
        Audio.Clear();
        UI.Clear();
        Map.Clear();
        Scene.Clear();
    }

    #endregion

}