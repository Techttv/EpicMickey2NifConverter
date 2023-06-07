using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX.Elements2D;
using prova_3dviewport.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prova_3dviewport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> strings = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Nif nif = new Nif(@"C:\EpicMickey2_Definitive\Export\unpacked\characters\mickey\mickey.nif");
            nif.toModel();
            /*ObjReader obj = new ObjReader();
            Model3DGroup model = null;
            model = obj.Read(@"oggetto.obj");
            ModelVisual3D modelVisual3D = new ModelVisual3D();
            modelVisual3D.Content = model;
            viewport.Children.Add(modelVisual3D);*/
        }

        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            lb_files.Items.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.ShowNewFolderButton = true;
            dialog.ShowDialog();



            String path = dialog.SelectedPath;
            try
            {
                var txtFiles = Directory.EnumerateFiles(path, "*.nif");

                foreach (string currentFile in txtFiles)
                {
                    strings.Add(currentFile);
                    string fileName = currentFile.Substring(path.Length + 1);
                    lb_files.Items.Add(fileName);
                }
            }
            catch (Exception )
            {
            }

        }

        private void lb_files_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = this.lb_files.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Model3DGroup MdlGrp = null;
                ModelVisual3D device3D = new ModelVisual3D();
                ObjReader ObjRed = new ObjReader();
                MdlGrp = ObjRed.Read(strings[index]);
                device3D.Content = MdlGrp;
               // viewport.Children.Add(device3D);
            }
        }
    }
}
