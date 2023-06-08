using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace prova_3dviewport
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        string exportFolder = "";
        Window f1;
        public ExportWindow(Window f1)
        {
            InitializeComponent();
            this.f1 = f1;
            foreach(string var in MainWindow.strings)
            {
                txt_files.Text += var;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            f1.Show();
            this.Close();
        }
        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.ShowNewFolderButton = true;
            dialog.ShowDialog();



            String path = dialog.SelectedPath;
            txt_path.Text = path;
            exportFolder = path;

        }
    }
}
