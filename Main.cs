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
            const int GWL_STYLE = (-16);
            const int WS_VISIBLE = 0x10000000;
            //  SetWindowLong(aaaa, GWL_STYLE, (WS_VISIBLE));
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            // MoveWindow(aaaa, resolution.Top, resolution.Left, resolution.Width, resolution.Height, true);
            //ShowWindowAsync(aaaa, 3);

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
                int num = (int)MessageBox.Show("ROS MultiHack v99 by ~Ashesh [unknowncheats]\n------------------------\nO & P - adjurst aimbot height \n F1 Show Players + esp \n F2 Only Aimbot, No ESP \n F3 -  SMART_AIM!  \nLEFT ALT - Toggle Aimbot \n U - Show FOV \n UP and DOWN arrow - Aimbot Range  \n LEFT and RIGHT arrow - Aimbot FOV change \n------------------------\nToggle Noclip: J\n Controls: RCTRL+Arrow Keys, Backspace,RShift , f4,f5 - speed of noclip");
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
                    a2 = (Settings.SmartHeight) ? 1 : 0;
                    int nkk = (nuke) ? 1 : 0;
                    MethodInvoker inv = delegate
                    {
                        this.label3.Text = "NUKE: "+nkk+"Range: " + range + " m \n" + "AIMBOT:" + aa + "\n" + "FOV:" + fov + "\n NOCLIP: " + n + "\nSmartAim: " + a2 + " \n Height: " + height + "\n NoClip Speed: " + speed;
                    };
                    this.Invoke(inv);

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
                                    drawnawbrudda.DrawString("Player", this.font, Brushes.Red, screen.X, screen.Y - 20f);
                                    //   drawnawbrudda.DrawLine(Dpen, bottom, pointxx(screen));

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
                                    height = 0;
                                    if (dist > 100 && dist < 200)
                                        aim.Y = (screen.Y - 20 + height + (-1 * (dist / 15)));
                                    else if (dist < 100)
                                        aim.Y = (screen.Y - 40 + height + (-1 * (dist / 15)));
                                    else
                                        aim.Y = (screen.Y + height + (-1 * (dist / 15)));
                                }
                                else
                                {
                                    aim.Y = screen.Y + height;
                                }
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

                Thread.Sleep(100);

            }

            if (Main.GetAsyncKeyState(Keys.Left))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    Z -= speed;
                else
                    fov -= 1;

                Thread.Sleep(100);


            }
            if (Main.GetAsyncKeyState(Keys.Right))
            {
                if (Main.GetAsyncKeyState(Keys.RControlKey))

                    Z += speed;
                else
                    fov += 1;

                Thread.Sleep(100);

            }
            if (Main.GetAsyncKeyState(Keys.Back))
            {
                Y += speed;
                Thread.Sleep(100);

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
                Thread.Sleep(100);

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
