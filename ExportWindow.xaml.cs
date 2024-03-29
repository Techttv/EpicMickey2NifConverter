﻿using HelixToolkit.Wpf;
using Microsoft.VisualBasic;
using prova_3dviewport.Classes;
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
using System.Windows.Media.Animation;
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
        List<string> files = new List<string>();
        public static int initialVertex = 1;
        public ExportWindow(Window f1, string[] strings)
        {
            InitializeComponent();
            this.f1 = f1;
            foreach(string item in strings)
            {
                files.Add(item);
                txt_files.Text +=( item + "\n");
                
            }
            lbl_items.Content = "Total items: " + files.Count();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //export button

            
            if (exportFolder.Length > 0)
            {
                progressBar.Opacity = 1;
                int s = 0;

                if(chk_merge.IsChecked == true)
                {
                    initialVertex = 1;
                    
                    FileStream fs = File.Create(exportFolder + "\\" + txt_exportFilename.Text + ".obj");
                    fs.Close();
                    StreamWriter writer = new StreamWriter(exportFolder + "\\" + txt_exportFilename.Text + ".obj");
                    foreach (string file in files)
                    {
                        if (!file.Contains("toon"))
                        {
                            writer.WriteLine("o "+ file.Substring(file.LastIndexOf("\\"), file.LastIndexOf('.')- file.LastIndexOf("\\")));
                            Nif nif = new Nif(file);
                            nif.ToObj(writer);
                        }
                        s++;
                        progressBar.Value = s * 100 / files.Count;
                    }
                    writer.Close();
                }
                else
                {
                    initialVertex = 1;
                    foreach (string file in files)
                    {
                        if (!file.Contains("toon"))
                        {
                            Nif nif = new Nif(file);
                            string filename = file.Substring(file.LastIndexOf('\\') + 1);
                            filename.Remove(filename.Length - 4);
                            nif.ToObj(exportFolder + "\\" + filename + ".obj");
                        }
                        s++;
                        progressBar.Value = s * 100 / files.Count;
                    }
                }

                
                label_export.Content = "Completed succesfully";
                Task.Delay(5000).ContinueWith(_ =>
                {
                    
                }
                    );
                label_export.Content = "Export";
            }
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(progressBar.Value == 100)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.To = 0.0;
                doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

                Storyboard storyboard = new Storyboard();

                Storyboard.SetTargetName(doubleAnimation, progressBar.Name);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(System.Windows.Controls.Control.OpacityProperty));

                storyboard.Children.Add(doubleAnimation);

                storyboard.Begin(this);
            }
        
        }

        private void btn_OpenDestFolder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenDestFolder.IsOpen = true;
        }

        private void btn_OpenDestFolder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenDestFolder.IsOpen=false;
        }

        private void btn_OpenSourceFolder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenSourceFolder.IsOpen = true;
        }

        private void btn_OpenSourceFolder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup_OpenSourceFolder.IsOpen = false;
        }

        private void btn_OpenDestFolder_Click(object sender, RoutedEventArgs e)
        {
            if (txt_path.Text.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", txt_path.Text);
            }
        }

        private void chk_merge_Click(object sender, RoutedEventArgs e)
        {
            if(chk_merge.IsChecked==true)
            {
                lbl_destFolder.Visibility = Visibility.Visible;
                txt_exportFilename.Visibility = Visibility.Visible;
            }
            else
            {
                lbl_destFolder.Visibility = Visibility.Collapsed;
                txt_exportFilename.Visibility = Visibility.Collapsed;
            }
        }
    }
}
