using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Manager.Contents
{
    public class MapManager
    {
        private static readonly string GRID_NAME = "@Grid";

        private Dictionary<Define.TileType, Tilemap> _tilemaps = new Dictionary<Define.TileType, Tilemap>();
        private Dictionary<string, TileBase> tileBases = new Dictionary<string, TileBase>();
        
        private TileBase tile;
        public Define.Theme Theme { get; private set; } = Define.Theme.None;
        
        private Grid _grid;

        public void Initialize()
        {
            _grid = Util.GetOrNewComponent<Grid>(GRID_NAME);
            
            InitTilemap();
            InitTile();
        }

        private void InitTile()
        {
            foreach (string tileName in Enum.GetNames(typeof(Define.TileType)))
            {
                foreach (string themeName in Enum.GetNames(typeof(Define.Theme)))
                {
                    string path = $"Tiles/{themeName}/{tileName}";
                    TileBase tileBase = Managers.Resource.Load<TileBase>(path);

                    if (tileBase == null)
                        Debug.Log($"해당 경로에 알맞은 타일이 없습니다. PATH : {path}");
                    else
                        tileBases.Add(path, tileBase);
                }
            }
        }

        public void Clear()
        {
            _tilemaps?.Clear();
        }
        
        private void InitTilemap()
        {
            foreach (Define.TileType tileType in Enum.GetValues(typeof(Define.TileType)))
            {
                if(tileType == Define.TileType.None) continue;
                _tilemaps.Add(tileType, AddTileMap(tileType));
            }
        }
        
        private Tilemap AddTileMap(Define.TileType tileType)
        {
            GameObject go = null;
            string typeName = tileType.ToString();


            Transform tile = _grid.transform.Find(typeName);

            if (tile != null)
                go = tile.gameObject;
            
            if (go == null)
                go = new GameObject {name = typeName, transform = {parent = _grid.transform}}; 
            
            go.GetOrAddComponent<TilemapRenderer>();
            
            Tilemap tilemap = go.GetOrAddComponent<Tilemap>();
            
            return tilemap;
        }


    }
}