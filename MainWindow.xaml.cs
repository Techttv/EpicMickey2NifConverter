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
        }

        private void btn_openFolder_Click(object sender, RoutedEventArgs e)
        {
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
                System.Windows.MessageBox.Show(index.ToString());
            }
        }
    }
}
