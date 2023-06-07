using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace prova_3dviewport.Classes
{

    public class Nif
    {
        private ModelVisual3D? visualModel;
        private string path;

        public Nif(string path)
        {
            try
            {
                Path.GetFullPath(path);
                this.path = path;
                visualModel = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ModelVisual3D toModel()
        {
            if (visualModel == null)
            {
                string filename = Path.GetTempFileName();
                FileInfo fileInfo = new FileInfo(filename);
                fileInfo.Attributes = FileAttributes.Temporary;

                createMesh(filename);
            }


            return visualModel;
        }

        private void createMesh(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);

            byte[] data = File.ReadAllBytes(path);
            string temphex = BitConverter.ToString(data);
            int numeroOggetti = 0;
            string[] hex = temphex.Split('-');
            for (int i = 0; i < hex.Length; i++)
            {
                if (hex[i].Equals("15") && hex[i + 1] == "02" && hex[i + 2] == "01")
                {
                    numeroOggetti++;
                }
            }
            Trace.Write(numeroOggetti);
        }

        private void findVertex()
        {

        }
    }
}
