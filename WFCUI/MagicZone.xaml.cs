using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaveFunctionCollapse;

namespace WFCUI
{
    /// <summary>
    /// Interaction logic for MagicZone.xaml
    /// </summary>
    public partial class MagicZone : UserControl
    {
        public MagicZone()
        {
            InitializeComponent();
        }

        [DllImport("gdi32.dll")]
        private extern static bool DeleteObject(IntPtr pointer);

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            try
            {
                using (DatafileManager dm = new DatafileManager(GrabDataLocation(e)))
                {
                    BitmapSource bi = RunAlgorithm(dm);
                    new Display(dm.FilenameNoExtension, bi).Show();
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "You've got a problem....", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            static string GrabDataLocation(DragEventArgs drag)
            {
                if (!drag.Data.GetDataPresent(DataFormats.FileDrop)) throw new Exception("No data present");

                drag.Effects = DragDropEffects.Copy;
                drag.Handled = true;

                return ((string[])drag.Data.GetData(DataFormats.FileDrop))[0];
            }

            static BitmapSource RunAlgorithm(DatafileManager dfManager)
            {
                if (dfManager.Extension != ".png") throw new Exception($"Dropped file is a \"{dfManager.Extension}\"; we only accept \".png\"s here.");

                Bitmap bmap = Top.Fire(dfManager.FilenameNoExtension, Config.IsOverlapping,
                    heuristic: Config.Heuristic, maxAttempts: Config.MaxAttempts, size: Config.Size,
                    n: Config.N);
                return ConvertOldBitmapToNew(bmap);
            }
        }

        private static BitmapSource ConvertOldBitmapToNew(Bitmap bmap)
        {
            IntPtr pointerToBitmap = bmap.GetHbitmap();

            try
            {
                // ASSUME this method never needs palette, sourceRect or sizeOptions specified.
                return Imaging.CreateBitmapSourceFromHBitmap(pointerToBitmap, palette: IntPtr.Zero,
                    sourceRect: Int32Rect.Empty, sizeOptions: BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(pointerToBitmap);
            }
        }
    }
}