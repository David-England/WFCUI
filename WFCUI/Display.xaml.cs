using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WFCUI
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display : Window
    {
        SaveFileDialog saveDialog = new SaveFileDialog() { DefaultExt = ".png", Filter = ".png files|*.png" };

        public Display(string title, BitmapSource b)
        {
            InitializeComponent();
            this.Title = title;
            this.Output.Source = b;
            saveDialog.FileOk += SaveImage;
        }

        public void Save_Click(object sender, RoutedEventArgs rea)
        {
            saveDialog.FileName = Title + DateTime.Now.ToString("-yyyyMMdd-HHmmss");
            saveDialog.ShowDialog();
        }

        public void SaveImage(object sender, CancelEventArgs cea)
        {
            using (var stream = new FileStream(saveDialog.FileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Output.Source as BitmapSource));
                encoder.Save(stream);
            }
        }
    }
}
