

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


        protected override void ProcessImpl()
        {

            ScreenCapture screenCapture = new ScreenCapture();
            MemoryStream memoryStream = new MemoryStream();

            // TODO does this leak the stream?
            result = screenCapture.CaptureScreenToMemoryStream(memoryStream, ImageFormat.Png);
            // memoryStream.Dispose();  // Doing this makes it fail when returning the File in the rest call
            // May need further investigation but this says that it may not be a resource leak:
            // https://stackoverflow.com/questions/234059/is-a-memory-leak-created-if-a-memorystream-in-net-is-not-closed

        }

        internal MemoryStream GetMemoryStream()
        {
            return (MemoryStream)GetResult();
        }


    }
}
