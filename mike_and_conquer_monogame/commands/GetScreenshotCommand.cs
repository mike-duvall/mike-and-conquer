

using System;
using ScreenCapture = mike_and_conquer.main.ScreenCapture;
using MemoryStream = System.IO.MemoryStream;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

using String = System.String;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands
{
    internal class GetScreenshotCommand : AsyncViewCommand
    {



        private void FullScreenshot(String filepath, String filename, ImageFormat format)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            // using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            // using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb))
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format64bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                // Pickup here
                // Test then debug contents here
                // http://10rem.net/blog/2011/02/08/capturing-screen-images-in-wpf-using-gdi-win32-and-a-little-wpf-interop-help
                string fullpath = filepath + "\\" + filename;

                bitmap.Save(fullpath, format);
            }
        }


        private static class Win32Native
        {
            public const int DESKTOPVERTRES = 0x75;
            public const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);
        }



        private void DoIt()
        {
            int width, height;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hDC = g.GetHdc();
                width = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPHORZRES);
                height = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPVERTRES);

                width = width / 2;
                height = height / 2;

                g.ReleaseHdc(hDC);
            }

            using (var img = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.CopyFromScreen(0, 0, 0, 0, img.Size, CopyPixelOperation.SourceCopy);
                    // g.CopyFromScreen(0, 0, width, height, img.Size);
                }

                for(int x = 0; x < width -2; x++)
                {
                    for (int y = 0; y < height - 2; y++)
                    {
                        Color originalPixelColor = img.GetPixel(x, y);
                        // Color newPixelColor = Color.FromArgb(50, originalPixelColor.R, originalPixelColor.G,
                        //     originalPixelColor.B);

                        // Color newPixelColor = Color.Red;

                        //
                        // img.SetPixel(x,y,newPixelColor);
                        if (originalPixelColor.R != 240 || originalPixelColor.G != 240 || originalPixelColor.B != 240)
                        {
                            int yyy = 3;
                            int yy = 4;
                        }
                    }

                }

                img.Save(@"C:\buildoutput\temp5.jpg", ImageFormat.Jpeg);
                Color aPixel1 = img.GetPixel(10, 10);
                Color aPixel2 = img.GetPixel(10, 12);
                int xx = 3;
            }
        }
        protected override void ProcessImpl()
        {

            // DoIt();
            ScreenCapture sc = new ScreenCapture();
            MemoryStream stream = new MemoryStream();
            //
            // String buildOutputDirectory = "C:\\buildoutput";
            //
            //
            //
            // FullScreenshot(buildOutputDirectory, "temp4.jpeg", ImageFormat.Jpeg);
            sc.CaptureScreenToFile("C:\\buildoutput\\temp3.png", ImageFormat.Png);

            // TODO does this leak the stream?
            result = sc.CaptureScreenToMemoryStream(stream, ImageFormat.Png);
        }

        internal MemoryStream GetMemoryStream()
        {
            return (MemoryStream)GetResult();
        }


    }
}
