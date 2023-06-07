using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_3dviewport.Classes
{
    public class vertexBlock
    {
        public int offset { get; set; }
        private List<float> vertex = new List<float>();
        private int vertexAmount;

        public vertexBlock(int offset, string[] hex)
        {
            this.offset = offset;
            findVertex(hex);
        }

        public string writeToObj()
        {

            //TODO
            string s="";

            return s;
        }

        public void findVertex(string[] hex)
        {
            string temp = hex[offset - 13] + hex[offset - 12];
            vertexAmount = Convert.ToInt32(temp, 16);
            Trace.Write(vertexAmount);
            string hexstring;
            for(int i = offset; i < offset+ (vertexAmount * 12); i+=4)
            {
                hexstring = hex[i]+ hex[i+1]+ hex[i+2] + hex[i + 3];
                uint num = uint.Parse(hexstring, System.Globalization.NumberStyles.AllowHexSpecifier);

                byte[] floatVals = BitConverter.GetBytes(num);
                float f = BitConverter.ToSingle(floatVals, 0);
                vertex.Add(f);
            }
        }

    }
}
