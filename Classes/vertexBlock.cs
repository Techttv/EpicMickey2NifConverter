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
        public int vertexAmount;

        public vertexBlock(int offset, string[] hex)
        {
            this.offset = offset;
            findVertex(hex);
        }



        public void findVertex(string[] hex)
        {
            //prendo la stringa hex e cerco il punto cui è scritto il numero di vertici
            //siccome è in little endian se sono due devo invertirli
            string temp = hex[offset - 12];
            if (hex[offset - 11] != "00")
            {
                string t = temp;
                temp = hex[offset - 11] + t;
            }
            //li converto in dec
            vertexAmount = Convert.ToInt32(temp, 16);
            

            //dichiarare l'hexstring mi permette di ottimizzare memoria
            string hexstring;
            for(int i = offset; i < offset+ (vertexAmount * 12); i+=4)
            {
                //parsing
                hexstring = hex[i + 3] + hex[i + 2] + hex[i + 1] + hex[i];
                uint num = uint.Parse(hexstring, System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] floatVals = BitConverter.GetBytes(num);
                float f = BitConverter.ToSingle(floatVals, 0);
                string temp1 = f.ToString();

                //taglio alla 4 cifra
                int virgolaIndex = temp1.IndexOf(',');
                int Stringlenght = temp1.Length - 1;
                if (Stringlenght - virgolaIndex > 4)
                    temp1 = temp1.Substring(0, virgolaIndex + 5);
                
                if (temp1.Contains('E'))
                {
                    temp1 =temp1.Substring(0, temp1.IndexOf("E")-1);
                }
                vertex.Add(float.Parse(temp1));

            }
        }
    }
}
