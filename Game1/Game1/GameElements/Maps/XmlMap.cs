using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//Source https://www.codeproject.com/Articles/1789/Object-Serialization-using-C
namespace DowerTefenseGame.GameElements
{
    [Serializable()]
    class XmlMap: ISerializable
    {
        public Tile[,] map;
        public int tileSize;
        public int height;
        public int width;
        public XmlMap(Tile[,] Tiles, int _tileSize, int _height, int _width)
        {
            this.map = Tiles;
            this.height = _height;
            this.width = _width;
            this.tileSize = _tileSize;
        }

        //Get the values from info and assign them to the appropriate properties
        public XmlMap(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            map = (Tile[,])info.GetValue("map", typeof(Tile[,]));
            height = (int)info.GetValue("height", typeof(int));
            width = (int)info.GetValue("widtht", typeof(int));
            tileSize = (int)info.GetValue("tileSize", typeof(int));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("map", map);
            info.AddValue("height", height);
            info.AddValue("width", width);
            info.AddValue("height", height);

        }
    }
}
