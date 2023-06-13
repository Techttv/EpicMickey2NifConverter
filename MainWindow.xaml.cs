using HelixToolkit.Wpf;
using prova_3dviewport.Classes;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;


namespace prova_3dviewport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> strings = new List<string>();
        string basepath="";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            lb_files.Items.Clear();
            strings.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.ShowNewFolderButton = true;
            dialog.ShowDialog();
            
            

            String path = dialog.SelectedPath;
            txt_path.Text = path;
            basepath = path;

        }

        private void lb_files_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewport.Children.Count == 4)
            {
                for (int i = 3; i < viewport.Children.Count; i++)
                {
                    viewport.Children.RemoveAt(i);
                }
            }
            int index = this.lb_files.SelectedIndex;
            if(lb_files.SelectedItems.Count > 1 ) {
                foreach (var item in lb_files.SelectedItems)
                {
                    string path = basepath + "\\" + item.ToString();
                    LoadFile(path);
                }
                return;
            }
            if (index != -1)
            {
                LoadFile(strings[index]);


            }
        }

        private void txt_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            strings.Clear();
            lb_files.Items.Clear();
            String path = txt_path.Text;
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
            catch (Exception)
            {
            }
        }

        private void LoadFile(string strings)
        {
            if(!strings.Contains("toon"))
            {
                Nif nif = new Nif(strings);
                ObjReader obj = new ObjReader();
                Model3DGroup model = null;
                string path = nif.toModel();
                if (path == "")
                {
                    System.Windows.MessageBox.Show("Cannot read the model");
                    return;
                }
                try
                {
                    model = obj.Read(path);
                }
                catch (Exception e )
                {
                    System.Windows.MessageBox.Show(e.Message);
                    return;
                }
                MeshBuilder TestMesh = new MeshBuilder(false, false);
                if (!nif.isEmpty())
                {

                    foreach (var m in model.Children)
                    {
                        var mGeo = m as GeometryModel3D;
                        var mesh = (MeshGeometry3D)((Geometry3D)mGeo.Geometry);
                        if (mesh != null) TestMesh.Append(mesh);
                    }

                    GeometryModel3D geometryModel3D = new GeometryModel3D();
                    geometryModel3D.Geometry = TestMesh.ToMesh();
                    geometryModel3D.Material = Materials.DarkGray;

                    ;

                    //rotate the mesh in correct axis

                    Vector3D axis = new Vector3D(1, 0, 0); //In case you want to rotate it about the x-axis
                    Matrix3D transformationMatrix = geometryModel3D.Transform.Value; //Gets the matrix indicating the current transformation value
                    transformationMatrix.RotateAt(new System.Windows.Media.Media3D.Quaternion(axis, 90), nif.centerPoint); //Makes a rotation transformation over this matrix
                    /*Vector3D translation = new Vector3D(-nif.centerPoint.X, -nif.centerPoint.X, -nif.centerPoint.X);
                    transformationMatrix.Translate(translation);*/
                    geometryModel3D.Transform = new MatrixTransform3D(transformationMatrix);


                    //centra la griglia e la telecamera
                    grid.Center = nif.centerPoint;
                    viewport.Camera.LookAt(nif.centerPoint, 2);
                    Point3D cameraPosition = new Point3D(nif.centerPoint.X + 20, nif.centerPoint.Y + 10, nif.centerPoint.Z + 30);
                    viewport.Camera.Position = cameraPosition;
                    viewport.FixedRotationPoint = nif.centerPoint;


                    ModelVisual3D modelVisual3D = new ModelVisual3D();
                    modelVisual3D.Content = geometryModel3D;
                    viewport.Children.Add(modelVisual3D);
                }
                else
                {
                    System.Windows.MessageBox.Show("Model not found");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot read toon mesh yet!");
            }
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<string> files = new List<string>();
            foreach(string f in lb_files.SelectedItems)
            {
                files.Add(basepath+"\\" +f);
            }
            ExportWindow f1 = new ExportWindow(this, files.ToArray());
            f1.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 3; i < viewport.Children.Count; i++)
            {
                viewport.Children.RemoveAt(i);
            }
        }

        private void btn_OpenSourceFolder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenSourceFolder.IsOpen = true;
        }

        private void btn_OpenSourceFolder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenSourceFolder.IsOpen = false;
        }

        private void btn_ExportAll_Click(object sender, RoutedEventArgs e)
        {

            ExportWindow f1 = new ExportWindow(this, strings.ToArray());
            f1.Show();
            this.Hide();
        }
    }
}
