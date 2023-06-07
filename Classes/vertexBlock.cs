using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_3dviewport.Classes
{
    public class vertexBlock
    {
        public int offset { get; set; }
        private byte[]? dataByte { get;set; } = new byte[] { };
        private int[]? dataInt { get; set; } = new int[] { };

        public vertexBlock(int offset,)
        {
            this.offset = offset;
            findVertex(string path)
        }

        public void addVertex(byte val)
        {
            dataByte.Append(val);
        }
        public void addVertex(int val)
        {
            dataInt.Append(val);
        }

        public void addVertex(byte[] val, bool replace)
        {
            if (replace)
            {
                dataByte = val;
            }
            else
            {
                dataByte.Union(val);
            }
        }
        public void addVertex(int[] val, bool replace)
        {
            if(replace)
            {
                dataInt = val;
            }
            else
            {
                dataInt.Union(val);
            }
        }

        public string writeToObj()
        {
            string vertices="";
            if(dataByte.Length>dataInt.Length)
            {
                //implementare scrittura dei dati byte
                for(int i = 0; i < dataByte.Length; i++)
                {
                    vertices+= " " + dataByte[i];

                    if (i % 3 == 0&&i!=0&&i!=dataByte.Length-1)
                    {
                        vertices+="\nv"
                    }
                }

            }
            else
            {
                //implementare scrittura dei dati int
            }


            return vertices;
        }

        public void findVertex()
        {

        }

    }
}
