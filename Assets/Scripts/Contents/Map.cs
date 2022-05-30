using UnityEngine;

namespace Contents
{
    public class Tile
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        public void SetPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3Int GetPos() => new Vector3Int(X, Y);
    }
    
    public class Map
    {
        private int _sizeX;
        private int _sizeY;

        private Tile[,] map;

        public Map(int sizeX,int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            
            InitMap();
        }

        private void InitMap()
        {
            map = new Tile[_sizeX, _sizeY];
            
            for (int x = 0; x < _sizeX; x++)
            for (int y = 0; y < _sizeY; y++)
            {
                map[x,y] = new Tile();
                
            }
        }
        
        
    }
}