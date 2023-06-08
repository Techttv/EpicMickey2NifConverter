using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace prova_3dviewport.Classes
{

    public class Nif
    {
        private string path;
        private static List<vertexBlock> vertex = new List<vertexBlock>();
        private static List<faceBlock> face = new List<faceBlock>();
        private static Queue<int> faceIndex = new Queue<int>();
        private static Queue<int> vertexIndex = new Queue<int>();
        private List<Thread> threads = new List<Thread>();
        private string filename="";
        private static string[] hex;
        public Point3D centerPoint;
        public Nif(string path)
        {
            try
            {
                Path.GetFullPath(path);
                this.path = path;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string toModel()
        {
            if (filename.Length==0)
            {
                FileStream fs = File.Create(@"temp.obj");
                fs.Close();
                filename = fs.Name;

                createMesh(filename);
            }


            return filename;
        }

        private void createMesh(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.AutoFlush = true;
            byte[] data = File.ReadAllBytes(path);
            string temphex = BitConverter.ToString(data);
            hex = temphex.Split('-');

            for (int i = 0; i < hex.Length; i++)
            {
                if (hex[i].Equals("15") && hex[i + 1] == "02" && hex[i + 2] == "01")
                {
                    faceIndex.Enqueue(i + 4);
                }
                if ((hex[i].Equals("37") && hex[i + 1] == "04" && hex[i + 2] == "03") && (hex[i + 4] != "38" && hex[i + 5] != "04" && hex[i + 6] != "04"))
                {
                    //ricerca dei blocchi dei vertici FUNZIONA
                    vertexIndex.Enqueue(i + 4);
                }
            }
            int IndexTotal = faceIndex.Count;
            for (int i =0;i<IndexTotal;i++)
            {
                threads.Add(new Thread(new ThreadStart(findFace)));
                threads.Last().Start();
            }
            IndexTotal = vertexIndex.Count;
            for (int i = 0; i<IndexTotal;i++)
            {
                threads.Add(new Thread(new ThreadStart(findVertex)));
                threads.Last().Start();
            }
            
            float x=0, y=0, z=0;
            int dividendo=0;
            for(int k =0;k<vertex.Count;k++)
            {
                for(int i = 0; i < vertex.ElementAt(k).vertex.Count; i+=3)
                {
                    string v1, v2,v3;
                    v1 = vertex.ElementAt(k).vertex.ElementAt(i).ToString().Replace(',', '.');
                    v2 = vertex.ElementAt(k).vertex.ElementAt(i+1).ToString().Replace(',', '.');
                    v3 = vertex.ElementAt(k).vertex.ElementAt(i+2).ToString().Replace(',', '.');
                    writer.Write("v " + v1 + " " + v2 + " " + v3+"\n");
                    x += vertex.ElementAt(k).vertex.ElementAt(i);
                    y += vertex.ElementAt(k).vertex.ElementAt(i + 1);
                    z += vertex.ElementAt(k).vertex.ElementAt(i + 2);
                }
                dividendo+= vertex.ElementAt(k).vertexAmount;
            }
            x = x / dividendo;
            y = y / dividendo;
            z = z / dividendo;
            centerPoint = new Point3D(x, y, z);

            int totalIndex = 1;
            for(int k =0;k<face.Count;k++)
            {
                
                for(int i =0;i<face.ElementAt(k).face.Count;i+=3)
                {
                    string f1, f2, f3;
                    f1 = (face.ElementAt(k).face.ElementAt(i) + totalIndex).ToString();
                    f2 = (face.ElementAt(k).face.ElementAt(i+1) + totalIndex).ToString();
                    f3 = (face.ElementAt(k).face.ElementAt(i+2) + totalIndex).ToString();
                    writer.Write("f " + f1+" "+f2+" "+f3+"\n");
                }
                totalIndex += vertex.ElementAt(k).vertex.Count/3;
            }
            
            writer.Close();
        }

        private static void findFace()
        {
            face.Add(new faceBlock(faceIndex.Dequeue(), hex));

        }

        private static void findVertex()
        {
            vertex.Add(new vertexBlock(vertexIndex.Dequeue(), hex));
        }
        public bool isEmpty()
        {
            if (vertex.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
