using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Manager.Core
{
    public class Parser : EditorWindow
    {
        private static readonly string DataPath = $"{Application.dataPath}/Scripts/Data/";
        private static readonly string LoadPath = $"{Application.dataPath}/ScriptableObjects/";
        private static readonly string SavePath = $"Assets/ScriptableObjects/";
        
        private static readonly string[] TableNames = Enum.GetNames(typeof(Define.Table));
        
        private static readonly string Format = ".csv";

        private static readonly string VariantName = "sOData";
        
        [MenuItem("Parser/LoadChatacterStat")]
        private static void LoadStat()
        {
            LoadData<CharacterStatus>(Define.Table.CharacterStatus);
        }
        
        private static void LoadData<T>(Define.Table type = Define.Table.None) where T : ScriptableObject, ITableSetter
        {
            CreatePath();
            
            string tableName = TableNames[(int) type];

            StreamReader streamReader = new StreamReader($"{DataPath}{tableName}{Format}");

            // string[] fieldTypes = streamReader.ReadLine().Split(',');
            string[] fieldNames = streamReader.ReadLine().Split(',');
            
            while (!streamReader.EndOfStream)
            {
                string[] datas = streamReader.ReadLine().Split(',');
                T sO = ScriptableObject.CreateInstance<T>();
                sO.SetData(fieldNames,datas);
                
                string path = $"{SavePath}{tableName}/{sO.ID.ToString()}.asset";
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.CreateAsset(sO, path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreatePath()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
            
            if (!Directory.Exists(LoadPath))
                Directory.CreateDirectory(LoadPath);

            foreach (var name in TableNames)
            {
                if (!Directory.Exists($"{LoadPath}{name}"))
                    Directory.CreateDirectory($"{LoadPath}{name}");
            }
        }

        
        
    }
}