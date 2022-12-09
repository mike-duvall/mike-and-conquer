using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;


namespace mike_and_conquer_monogame.humancontroller
{
    class MouseInputHandler
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        struct INPUT
        {
            public int type;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }


        private const int MOUSEEVENTF_MOVE = 0x01;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        public enum ClickType { RIGHT_CLICK, LEFT_CLICK };


        public static void DoLeftMouseClick(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            DoMouseClick(ClickType.LEFT_CLICK, mouseX, mouseY, screenWidth, screenHeight);
        }

        public static void DoLeftMouseClickAndHold(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            DoMouseClickAndHold(ClickType.LEFT_CLICK, mouseX, mouseY, screenWidth, screenHeight);
        }

        public static void DoReleaseLeftMouseClick(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            DoReleaseMouseClick(ClickType.LEFT_CLICK, mouseX, mouseY, screenWidth, screenHeight);
        }



        public static void DoRightMouseClick(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            DoMouseClick(ClickType.RIGHT_CLICK, mouseX, mouseY, screenWidth, screenHeight);
        }



        public static void MoveMouseToCoordinates(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            INPUT mouseInput = CreateBaseMouseInput(mouseX, mouseY, screenWidth, screenHeight);
            int mouseInputStructSize = Marshal.SizeOf(mouseInput);

            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            System.Threading.Thread.Sleep(1000);

            uint y3 = SendInput(1, ref mouseInput, mouseInputStructSize);

            System.Threading.Thread.Sleep(1000);
        }


        public static void DoMouseClick(ClickType clickType, uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {


            INPUT mouseInput = CreateBaseMouseInput(mouseX, mouseY, screenWidth, screenHeight);
            int mouseInputStructSize = Marshal.SizeOf(mouseInput);

            uint mouseDownFlag;
            uint mouseUpFlag;

            if (clickType == ClickType.LEFT_CLICK)
            {
                mouseDownFlag = MOUSEEVENTF_LEFTDOWN;
                mouseUpFlag = MOUSEEVENTF_LEFTUP;
            }
            else
            {
                mouseDownFlag = MOUSEEVENTF_RIGHTDOWN;
                mouseUpFlag = MOUSEEVENTF_RIGHTUP;
            }

            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            uint y3 = SendInput(1, ref mouseInput, mouseInputStructSize);

            mouseInput.mkhi.mi.dwFlags = mouseDownFlag;
            // mouseInput.mkhi.mi.time = (uint)DateTime.Now.Ticks;
            mouseInput.mkhi.mi.time = 0;
            uint y = SendInput(1, ref mouseInput, mouseInputStructSize);
            
            System.Threading.Thread.Sleep(1000);
            
            mouseInput.mkhi.mi.dwFlags = mouseUpFlag;
            uint y2 = SendInput(1, ref mouseInput, mouseInputStructSize);

            System.Threading.Thread.Sleep(200);

        }


        public static void DoMouseClickAndHold(ClickType clickType, uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            INPUT mouseInput = CreateBaseMouseInput(mouseX, mouseY, screenWidth, screenHeight);
            int mouseInputStructSize = Marshal.SizeOf(mouseInput);

            uint mouseDownFlag;

            if (clickType == ClickType.LEFT_CLICK)
            {
                mouseDownFlag = MOUSEEVENTF_LEFTDOWN;
            }
            else
            {
                mouseDownFlag = MOUSEEVENTF_RIGHTDOWN;
            }

            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            uint y3 = SendInput(1, ref mouseInput, mouseInputStructSize);

            mouseInput.mkhi.mi.dwFlags = mouseDownFlag;
            // mouseInput.mkhi.mi.time = (uint)DateTime.Now.Ticks;
            mouseInput.mkhi.mi.time = 0;

            uint y = SendInput(1, ref mouseInput, mouseInputStructSize);

            System.Threading.Thread.Sleep(1000);

        }

        public static void DoReleaseMouseClick(ClickType clickType, uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            INPUT mouseInput = CreateBaseMouseInput(mouseX, mouseY, screenWidth, screenHeight);
            int mouseInputStructSize = Marshal.SizeOf(mouseInput);


            uint mouseUpFlag;

            if (clickType == ClickType.LEFT_CLICK)
            {
                mouseUpFlag = MOUSEEVENTF_LEFTUP;
            }
            else
            {
                mouseUpFlag = MOUSEEVENTF_RIGHTUP;
            }

            mouseInput.mkhi.mi.dwFlags = mouseUpFlag;
            uint y2 = SendInput(1, ref mouseInput, mouseInputStructSize);

            System.Threading.Thread.Sleep(1000);

        }

        private static INPUT CreateBaseMouseInput(uint mouseX, uint mouseY, int screenWidth, int screenHeight)
        {
            // Must normalize mouse coordinates.
            // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx
            double normalizedMouseX = mouseX * (65535.0f / screenWidth);
            double normalizedMouseY = mouseY * (65535.0f / screenHeight);

            INPUT mouseInput = new INPUT();
            mouseInput.type = 0;
            mouseInput.mkhi.mi.mouseData = 0;

            // Setting time to Ticks seems to sometimes activate screensaver and/or turn monitor off
            // Not sure why
            // See https://comp.os.ms-windows.programmer.win32.narkive.com/QSgTykc6/simulating-mouse-buton-press-isung-sendinput-activates-screensaver-and-turns-offmonitor
            // And https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput
            // It seems that setting time to zero let's windows provide it's own time
            //            mouseInput.mkhi.mi.time = (uint)DateTime.Now.Ticks;
            mouseInput.mkhi.mi.time = 0;

            mouseInput.mkhi.mi.dx = (int)normalizedMouseX;
            mouseInput.mkhi.mi.dy = (int)normalizedMouseY;
            return mouseInput;
        }
    }
}
