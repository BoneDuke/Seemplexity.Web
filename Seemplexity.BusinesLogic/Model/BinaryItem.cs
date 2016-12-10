using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.BusinesLogic.Model
{
    public class BinaryItem
    {
        public BinaryItem() { }

        public BinaryItem(BinaryItemType type, string fileName, byte[] data)
        {
            Type = type;
            Data = data;
            FileName = fileName;
        }

        public int Id { get; set; }
        public BinaryItemType Type { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }

        public Image GetImage()
        {
            var ms = new MemoryStream(Data);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
