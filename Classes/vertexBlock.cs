using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace prova_3dviewport.Classes
{
    public class vertexBlock
    {
        public int offset { get; set; }
        public List<float> vertex = new List<float>();
        private int vertexAmount;

        public vertexBlock(int offset, string[] hex)
        {
            this.offset = offset;
            findVertex(hex);
        }



        public void findVertex(string[] hex)
        {
            string temp = hex[offset - 12];
            if (hex[offset - 11] != "00")
            {
                string t = temp;
                temp = hex[offset - 11] + t;
            }
            vertexAmount = Convert.ToInt32(temp, 16);
            Trace.WriteLine(vertexAmount);
            string hexstring;
            for(int i = offset; i < offset+ (vertexAmount * 12); i+=4)
            {
                hexstring = hex[i + 3] + hex[i + 2] + hex[i + 1] + hex[i];
                uint num = uint.Parse(hexstring, System.Globalization.NumberStyles.AllowHexSpecifier);

                byte[] floatVals = BitConverter.GetBytes(num);
                float f = BitConverter.ToSingle(floatVals, 0);
                string temp1 = f.ToString();
                int temp2 = temp1.IndexOf(',');
                int Stringlenght = temp1.Length - 1;
                if (Stringlenght - temp2 > 4)
                    temp1 = temp1.Substring(0, temp2 + 5);
                Trace.WriteLine(float.Parse(temp1));
                vertex.Add(float.Parse(temp1));

            }
        }
        public string writeToObj()
        {

            //TODO
            string s = "";

            return s;
        }
    }
}
