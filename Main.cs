using System;
using System.ComponentModel;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using ROS;

namespace Whynot
{
    public class Main : Form
    {
        public static float X, Y, Z = 0;
        public static int height = -30;
        public static bool nuke = false;
        public string WINDOW_NAME = "null";
        public static int range = 200;
        public static bool Scopes48 = false;
        public static int speed = 2;
        public static int fov = 30;
        public static int num1;
        public static int w = 0;//310;
        public static Vector3 MyPosition = new Vector3();
        public static Pen greenPen = new Pen(Color.Green, 2);
        public static Pen Dpen = new Pen((Brush)new SolidBrush(colour.newco));
        public static bool lockaim;
        public static bool aimbot = true;
        public static int plus = 5;
        public static int jarak = 150;
        Pen blackPen = new Pen(Color.Black, 3);
        public Font font = new Font(Control.DefaultFont, FontStyle.Bold);
        private IContainer components = (IContainer)null;
        private Main.RECT rect;
        private System.Windows.Forms.Timer Loop;
        public static bool hideh = false;
        private Label label1;
        private Label label2;
        private Label label3;
        int res = WinAPI.SystemParametersInfo(113, 0, 20, 0);
        public static Point pointxx(Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }
        public static Point pointxxx(Vector2 vec)
        {
            return new Point((int)vec.X - 5, (int)vec.Y - plus);
        }

        public static Point pointxxxxx(Vector2 vec)
        {
            return new Point((int)vec.X + w, (int)vec.Y + height);
        }
        public static Point closeaf(Vector2 vec)
        {
            return new Point((int)vec.X - 5, (int)vec.Y - 30);
        }
        public static Point closeaf2(Vector2 vec)
        {
            return new Point((int)vec.X - 5, (int)vec.Y - 70);
        }
        public static Point pointxxxx(Vector2 vec)
        {
            return new Point((int)vec.X - 5, (int)vec.Y - 3);
        }
        public Main()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.ProcessName == "ros")
                    {
                        WINDOW_NAME = process.MainWindowTitle;
                    }
                }
            }
            this.InitializeComponent();

        }
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hwnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hwnd = pList.MainWindowHandle;
                }
            }
            return hwnd;
        }
        public const UInt32 SPI_GETMOUSESPEED = 0x0070;

        [DllImport("User32.dll")]
        static extern Boolean SystemParametersInfo(
            UInt32 uiAction,
            UInt32 uiParam,
            IntPtr pvParam,
            UInt32 fWinIni);
        [DllImport("User32.dll")]
        private static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Main.RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private void Main_Load(object sender, EventArgs e)
        {
            IntPtr aaaa = WinGetHandle(WINDOW_NAME);
            if (MessageBox.Show("Enable Fullscreen? Note: Aimbot works good on fullscreen only", "Fullscreen", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                const int GWL_STYLE = (-16);
                const int WS_VISIBLE = 0x10000000;
                SetWindowLong(aaaa, GWL_STYLE, (WS_VISIBLE));
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                MoveWindow(aaaa, resolution.Top, resolution.Left, resolution.Width, resolution.Height, true);

            }
          
           
            //ShowWindowAsync(aaaa, 5); //Fullscreenbug
            this.TopMost = true;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Mem.Initialize("ros");
            if (Mem.m_pProcessHandle == IntPtr.Zero)
            {
                int num = (int)MessageBox.Show("Game Not Found", "Please Start Rules of Survival");
                this.Close();
            }
            else
            {
                int num = (int)MessageBox.Show("ROS MultiHack v99 by ~Ashesh [unknowncheats]\n------------------------\nO & P - adjurst aimbot height \n F1 Show Players + esp \n F2 Only Aimbot, No ESP \n F3 -  SMART_AIM!  \nLEFT ALT - Toggle Aimbot \n U - Show FOV \n UP and DOWN arrow - Aimbot Range  \n LEFT and RIGHT arrow - Aimbot FOV change \n Y - toggle aimbot mode (4x8x, 2x,noscope) \n------------------------\nToggle Noclip: J\n Controls: RCTRL+Arrow Keys, Backspace,RShift , f4,f5 - speed of noclip");
                Main.SetWindowLong(this.Handle, -20, Main.GetWindowLong(this.Handle, -20) | 524288 | 32);
            }
            new Thread(new ThreadStart(this.thread)).Start();
        }
        private void Loop_Tick(object sender, EventArgs e)
        {
            Main.GetWindowRect(Main.FindWindow((string)null, WINDOW_NAME), out this.rect);
            this.Size = new Size(this.rect.right - this.rect.left, this.rect.bottom - this.rect.top);
            this.Top = this.rect.top;
            this.Left = this.rect.left;
        }

        public void thread()
        {
            Graphics graphics = this.CreateGraphics();
            int numnya;
            int Adress1, Adress2, num2;
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            Graphics drawnawbrudda = Graphics.FromImage((Image)bitmap);
            Point bottom = new Point()
            {
                X = this.ClientRectangle.Width / 2,
                Y = this.ClientRectangle.Height / 2 + 200
            };
            int Adress3, num6, name, aa, a2, n;
            string str;
            double num4, num5;
            float dist;
            bool Player, Bot, Vehicle, Item, SuplyBox, Plane, weapon;
            Vector2 center = new Vector2();

            while (true)
            {
                try {
                    MethodInvoker inv = delegate
                    {
                        SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
                        this.TopMost = true;

                    };
                    this.Invoke(inv);


                    center.X = this.Width / 2;
                    center.Y = (this.Height / 2) + 20;
                    drawnawbrudda.Clear(Color.Black);
                    num1 = Mem.ReadMemory<int>(Mem.BaseAddress + Offsets.LocalPlayer);
                    numnya = Mem.ReadMemory<int>(Mem.BaseAddress + 34);
                    MyPosition.X = Mem.ReadMemory<float>(num1 + 16);
                    MyPosition.Y = Mem.ReadMemory<float>(num1 + 16 + 4);
                    MyPosition.Z = Mem.ReadMemory<float>(num1 + 16 + 8);
                    if (Z == 0)
                    {
                        X = MyPosition.X;
                        Y = MyPosition.Y;
                        Z = MyPosition.Z;
                    }
                    Adress1 = Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.BaseAddress + Offsets.Client) + Offsets.m_ppObjects);
                    Adress2 = Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Adress1))));
                    num2 = Mem.ReadMemory<int>(Adress1 + 4);
                    this.DrawMenu(drawnawbrudda);
                    while (Adress2 != num2 && Adress2 != num1)
                    {
                        MethodInvoker invx = delegate
                        {
                            SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
                            this.TopMost = true;
                        };
                        this.Invoke(invx);
                        Adress3 = Mem.ReadMemory<int>(Adress2 + 12);
                        str = Mem.ReadString(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Adress3 + 4) + 12), 20);
                        Mem.ReadMemory<int>(Adress3);
                        Mem.ReadMemory<int>(Adress3 + 12);
                        Vector3 EnemyPos = Mem.ReadMemory<Vector3>(Adress3 + 16);

                        num4 = (double)Mem.ReadMemory<float>(Adress3 + 56);
                        num5 = (double)Mem.ReadMemory<float>(Adress3 + 60);
                        num6 = Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Adress3 + 256) + 20) + 152) + 8);
                        name = Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Mem.ReadMemory<int>(Adress3 + 34) + 0) + 0) + 0);

                        Player = str.Contains("Avatar");
                        Bot = str.Contains("Robot");
                        Vehicle = str.Contains("Land");
                        Item = str.Contains("DtsProp");
                        SuplyBox = str.Contains("DtsPlayerHeritage");
                        Plane = str.Contains("Plane");
                        weapon = str.Contains("ClientWeaponEntity");
                        Adress2 = Mem.ReadMemory<int>(Adress2);
                        Vector2 screen;
                        aa = (aimbot) ? 1 : 0;
                        n = (Settings.NoClip) ? 1 : 0;
                        int nk = (nuke) ? 1 : 0;
                        a2 = (Settings.SmartHeight) ? 1 : 0;
                        string typee;
                        if (Scopes48)
                        {
                            typee = "8x and 4x";
                        }
                        else
                        {
                            typee = "Normal (2x , red dot, no scope)";
                        }
                        MethodInvoker inv3 = delegate
                        {
                            this.label3.Text = "Nuke: "+nk+" \nRange: " + range + " m \n" + "AIMBOT:" + aa + "\n" + "TYPE:" + typee + "\n NOCLIP: " + n + "\nSmartAim: " + a2 + " \n Height: " + height + "\n NoClip Speed: " + speed;
                        };
                        this.Invoke(inv3);

                        if (Settings.DEBUG)
                        {

                            Rectangle rect = new Rectangle((int)center.X - fov, ((int)center.Y) - fov, fov * 2, fov * 2);
                            drawnawbrudda.DrawEllipse(greenPen, rect);



                        }
                        if (Settings.NoClip)
                        {
                            Mem.WriteMemory<float>(num1 + 16, X);
                            Mem.WriteMemory<float>(num1 + 16 + 4, Y);
                            Mem.WriteMemory<float>(num1 + 16 + 8, Z);
                        }
                        if ((Maths.WorldToScreen(EnemyPos, out screen, this.Width, this.Height)))
                        {
                            if ((Player ? 1 : (!Bot ? 0 : ((uint)num6 > 0U ? 1 : 0))) != 0)
                            {
                                if (nuke)
                                    Mem.WriteMemory<Vector3>(Adress3 + 16, MyPosition);
                                if (Settings.ESP == true)
                                {
                                    if (Settings.PlayerESP && num6 != 0)
                                    {
                                        try
                                        {
                                            drawnawbrudda.DrawString("Player", this.font, Brushes.Red, screen.X, screen.Y - 20f);
                                            drawnawbrudda.DrawLine(Dpen, bottom, pointxx(screen));

                                        } catch (Exception)
                                        {

                                        }

                                    }


                                    if (Settings.PlayerHealth)
                                    {

                                        {
                                            drawnawbrudda.DrawString("[ " + (object)num6 + "HP ]", this.font, Brushes.Aqua, screen.X - 15f, screen.Y + 15f);
                                        }
                                    }


                                    if (Settings.PlayerDistance)

                                        if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 100)
                                        {
                                            drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.White, screen.X - 10f, screen.Y + 30f);
                                        }
                                        else
                                        {
                                            drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.Red, screen.X - 10f, screen.Y + 30f);

                                        }
                                }
                                dist = Helper.GetDistance(MyPosition, EnemyPos, 10);
                                if (dist < range && aimbot && num6 > 0 && (Maths.InsideCircle((int)center.X, (int)center.Y, fov, (int)screen.X, (int)screen.Y)))
                                {

                                    Vector2 aim;
                                    aim.X = screen.X + w;
                                    if (Settings.SmartHeight)
                                    {
                                       if(Scopes48)
                                        {
                                            if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 400)
                                            {
                                                height = -15;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 350)
                                            {
                                                height = -15;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 300)
                                            {
                                                height = -15;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 250)
                                            {
                                                height = -18;
                                                fov = 55;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 200)
                                            {
                                                height = -25;
                                                fov = 60;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 150)
                                            {
                                                height = -35;
                                                fov = 65;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 100)
                                            {
                                                height = -38;
                                                fov = 70;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 50)
                                            {
                                                height = -40;
                                                fov = 75;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 45)
                                            {
                                                height = -42;
                                                fov = 85;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 40)
                                            {
                                                height = -45;
                                                fov = 90;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 35)
                                            {
                                                height = -47;
                                                fov = 100;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 30)
                                            {
                                                height = -60;
                                                fov = 110;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 25)
                                            {
                                                height = -75;
                                                fov = 120;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 20)
                                            {
                                                height = -100;
                                                fov = 150;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 15)
                                            {
                                                height = -120;
                                                fov = 200;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 10)
                                            {
                                                height = -150;
                                                fov = 350;
                                            }
                                        }
                                        else
                                        {
                                            if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 400)
                                            {
                                                height = -11;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 350)
                                            {
                                                height = +5;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 300)
                                            {
                                                height = 0;
                                                fov = 50;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 250)
                                            {
                                                height = 0;
                                                fov = 55;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 220)
                                            {
                                                height = -6;
                                                fov = 60;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 200)
                                            {
                                                height = -7;
                                                fov = 60;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 160)
                                            {
                                                height = -10;
                                                fov = 60;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 150)
                                            {
                                                height = -11;
                                                fov = 65;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 100)
                                            {
                                                height = -12;
                                                fov = 70;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 75)
                                            {
                                                height = -14;
                                                fov = 70;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 50)
                                            {
                                                height = -25;
                                                fov = 75;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 45)
                                            {
                                                height = -35;
                                                fov = 85;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 40)
                                            {
                                                height = -38;
                                                fov = 90;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 35)
                                            {
                                                height = -42;
                                                fov = 100;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 30)
                                            {
                                                height = -45;
                                                fov = 110;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 25)
                                            {
                                                height = -50;
                                                fov = 120;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 20)
                                            {
                                                height = -100;
                                                fov = 150;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 15)
                                            {
                                                height = -120;
                                                fov = 200;
                                            }
                                            else if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 10)
                                            {
                                                height = -150;
                                                fov = 350;
                                            }
                                        }
                                    }
                                    aim.Y = screen.Y + height;

                                    Cursor.Position = new Point((int)aim.X, (int)aim.Y); ;

                                }
                            }
                            if (Bot && Settings.ESP == true)
                            {
                                drawnawbrudda.DrawString("ROBOT", this.font, Brushes.Chartreuse, screen.X, screen.Y - 20f);
                                drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.Chartreuse, screen.X - 10f, screen.Y + 30f);
                                drawnawbrudda.DrawLine(new Pen((Brush)new SolidBrush(colour.newco)), bottom, pointxx(screen));
                            }

                            if (Item && Settings.ESP == true)
                            {
                                if (Settings.ItemESP)
                                    drawnawbrudda.DrawString("Item", this.font, Brushes.Green, screen.X - 10f, screen.Y);
                                if (Settings.ItemDistance)
                                    drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.White, screen.X - 10f, screen.Y + 15f);
                            }
                            if (weapon && Settings.ESP == true)
                            {
                                if (Settings.ItemESP)
                                    drawnawbrudda.DrawString("Weapon", this.font, Brushes.Blue, screen.X - 10f, screen.Y);
                                if (Settings.ItemDistance)
                                    drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.White, screen.X - 10f, screen.Y + 15f);
                            }

                            if (SuplyBox && Settings.ESP == true)
                            {
                                if (Settings.BodyESP)
                                {
                                    if (Helper.GetDistance(MyPosition, EnemyPos, 10) > 500)
                                    {
                                        drawnawbrudda.DrawString("SUPPLY BOX", this.font, Brushes.Pink, screen.X - 10f, screen.Y);

                                    }

                                }
                            }
                            if (Vehicle && Settings.ESP == true)
                            {
                                if (Settings.VehicleESP)
                                    drawnawbrudda.DrawString("Vehicle", this.font, Brushes.Gold, screen.X - 10f, screen.Y);
                                if (Settings.VehicleDistance)
                                    drawnawbrudda.DrawString("[ " + (object)Helper.GetDistance(MyPosition, EnemyPos, 10) + " Meter]", this.font, Brushes.White, screen.X - 10f, screen.Y + 15f);
                            }
                            if (Plane && Settings.ESP == true)
                            {
                                drawnawbrudda.DrawString("PLANE", this.font, Brushes.Chocolate, screen.X, screen.Y - 20f);
                            }

                        }

                    }
                    graphics.DrawImage((Image)bitmap, 0, 0);
                    Thread.Sleep(10);
                }
                catch (Exception)
                {

                }
            }
        }

        public void DrawMenu(Graphics g2)
        {



            if (Main.GetAsyncKeyState(Keys.F1))
            {

                Settings.PlayerESP = true;
                Settings.ESP = true;
                Settings.PlayerHealth = true;
                Settings.PlayerDistance = true;
                Settings.ItemESP = true;
                Settings.ItemDistance = true;
                Settings.BodyESP = true;
                Settings.BodyDistance = true;
                Settings.VehicleESP = true;
                Settings.VehicleDistance = true;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.F4))
            {
                speed++;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.F5))
            {
                speed--;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.F2))
            {
                Settings.PlayerESP = false;
                Settings.PlayerHealth = false;
                Settings.PlayerDistance = false;
                Settings.ItemESP = false;
                Settings.ESP = true;
                Settings.ItemDistance = false;
                Settings.BodyESP = false;
                Settings.BodyDistance = false;
                Settings.VehicleESP = false;
                Settings.VehicleDistance = false;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.F3))
            {
                if (!Settings.SmartHeight)
                    height = 0;
                else
                    height = -30;
                Settings.SmartHeight = !Settings.SmartHeight;
                Thread.Sleep(100);

            }

            if (Main.GetAsyncKeyState(Keys.O))
            {
                height += 1;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.P))
            {

                height -= 1;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.Up))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    X += speed;
                else
                    range = range + 25;


            }

            if (Main.GetAsyncKeyState(Keys.Left))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    Z -= speed;
                else
                    fov -= 1;



            }
            if (Main.GetAsyncKeyState(Keys.Right))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    Z += speed;
                else
                    fov += 1;


            }
            if (Main.GetAsyncKeyState(Keys.Back))
            {
                Y += speed;

            }
            if (Main.GetAsyncKeyState(Keys.J))
            {
                Z = 0;
                Settings.NoClip = !Settings.NoClip;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.RShiftKey))
            {
                Y -= speed;

            }

            if (Main.GetAsyncKeyState(Keys.Delete))
            {
                nuke = !nuke;

            }
            if (Main.GetAsyncKeyState(Keys.Down))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    X -= speed;
                else
                    range = range - 25;
                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.LMenu))
            {
                aimbot = !aimbot;
            }

            if (Main.GetAsyncKeyState(Keys.K))
            {
                for (int i = 0; i < 5; i++)
                    ++w;

                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.U))
            {
                Settings.DEBUG = !Settings.DEBUG;
            }
            if (Main.GetAsyncKeyState(Keys.Y))
            {

                Scopes48 = !Scopes48;

            }
            if (Main.GetAsyncKeyState(Keys.L))
            {
                for (int i = 0; i < 5; i++)
                    --w;


                Thread.Sleep(100);

            }

        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Loop = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Loop
            // 
            this.Loop.Enabled = true;
            this.Loop.Tick += new System.EventHandler(this.Loop_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(275, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Range: Nan m";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(528, 308);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "ROS Exsceleton Rel";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}
