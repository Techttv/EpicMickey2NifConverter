using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace prova_3dviewport.Classes
{
    public class Nif
    {
        private string path;
        private List<vertexBlock> vertex = new List<vertexBlock>();
        private List<faceBlock> face = new List<faceBlock>();
        private string filename = "";
        private static string[] hex;
        public Point3D centerPoint;

        public Nif(string path)
        {

                Path.GetFullPath(path);
                this.path = path;
                if (path.Contains("toon"))
                {
                    Trace.WriteLine("è toon");
                    return;
                }

            /*catch (Exception)
            {
                throw ;
            }*/
        }

        public string toModel()
        {
            if (filename.Length == 0 || !filename.Contains("toon"))
            {

                    FileStream fs = File.Create(@"temp.obj");
                    fs.Close();

                    filename = fs.Name;
                    createMesh(filename);

                /*
                catch (Exception e)
                {

                    if (e.Message.Contains("temp.obj"))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        File.Delete(@"temp.obj");
                        return "";
                    }
                    MessageBox.Show(e.Message);
                    return "";
                }*/
            }

            return filename;
        }

        private void createMesh(string filename)
        {
            
            StreamWriter writer = new StreamWriter(filename);
            writer.AutoFlush = true;
            byte[] data;

            try
            {
                data = File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            string temphex = BitConverter.ToString(data);
            string[] hex = temphex.Split('-');
            bool skipV = false;
            for (int i = 0; i < hex.Length; i++)
            {
                if (hex[i].Equals("15") && hex[i + 1] == "02" && hex[i + 2] == "01")
                {
                    if(face.Count-vertex.Count==0)
                    face.Add(new faceBlock(i + 4, hex));
                }
                if(i== 222.056||i== 232449||i==243878)
                {

                }
                 if (hex[i].Equals("37") && hex[i + 1] == "04" && hex[i + 2] == "03")
                {
                    //ricerca dei blocchi dei vertici FUNZIONA

                    if (skipV == false)
                    {
                        vertex.Add(new vertexBlock(i + 4, hex));
                        skipV = true;
                    }
                    else { skipV=false; }
                }
            }
            float x = 0, y = 0, z = 0;
            int dividendo = 0;
            for (int k = 0; k < vertex.Count; k++)
            {
                for (int i = 0; i < vertex.ElementAt(k).vertex.Count; i += 3)
                {
                    string v1, v2, v3;
                    v1 = vertex.ElementAt(k).vertex.ElementAt(i).ToString().Replace(',', '.');
                    v2 = vertex.ElementAt(k).vertex.ElementAt(i + 1).ToString().Replace(',', '.');
                    v3 = vertex.ElementAt(k).vertex.ElementAt(i + 2).ToString().Replace(',', '.');
                    writer.Write("v " + v1 + " " + v2 + " " + v3 + "\n");
                    x += vertex.ElementAt(k).vertex.ElementAt(i);
                    y += vertex.ElementAt(k).vertex.ElementAt(i + 1);
                    z += vertex.ElementAt(k).vertex.ElementAt(i + 2);
                }
                dividendo += vertex.ElementAt(k).vertexAmount;
            }
            x = x / dividendo;
            y = y / dividendo;
            z = z / dividendo;
            centerPoint = new Point3D(x, y, z);
            int totalIndex = 1;
            if (vertex.Count() == 1)
            {
                while (face.Count() != vertex.Count())
                {
                    vertex.Add(vertex.ElementAt(0));
                }
            }
            for (int k = 0; k < face.Count; k++)
            {
                for (int i = 0; i < face.ElementAt(k).face.Count; i += 3)
                {
                    string f1, f2, f3;
                    f1 = (face.ElementAt(k).face.ElementAt(i) + totalIndex).ToString();
                    f2 = (face.ElementAt(k).face.ElementAt(i + 1) + totalIndex).ToString();
                    f3 = (face.ElementAt(k).face.ElementAt(i + 2) + totalIndex).ToString();
                    writer.Write("f " + f1 + " " + f2 + " " + f3 + "\n");
                }
                totalIndex += vertex.ElementAt(k).vertex.Count / 3;
            }

            writer.Close();
        }

        public bool isEmpty()
        {
            if (vertex.Count > 0)
            {
                return false;
            }
            return true;
        }

        public void ToObj(string path)
        {
            try
            {
                FileStream fs = File.Create(path);
                fs.Close();

                createMesh(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }
    }
}