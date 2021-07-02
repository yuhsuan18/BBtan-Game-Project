using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        int n = 1;
        int num = 0;
        int record;
        float vx = 0;
        float vy = 0;
        int lv = 1;
        double ratio = 1;
        bool f = false;
        bool ff = false;
        float location;
        Point cursor = Control.MousePosition;
        Random R = new Random();
        float[] xi = new float[200];
        float[] yi = new float[200];
        float[] righti = new float[100] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200 };
        float[] topi = new float[100] { 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500 };
        float[] bxi = new float[200];
        float[] byi = new float[200];
        float[] vxi = new float[200];
        float[] vyi = new float[200];
        float[] vxx = new float[200];
        float[] vyy = new float[200];

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e) //遊戲初始化
        {
            if (vx == 0 && vy == 0)
                for (int i = 0; i < 7; i++)  ///橫向7格
                {
                    int r = R.Next(10) + 1;
                    if (r % 3 == 1)
                        this.Controls.Add(Brick(i * 50 + 13, 68, lv)); // (i=0 時) 產生磚塊左上點(13,68)和邊界稍微隔開,方塊上初值設為1
                    else if (r == 2)
                        this.Controls.Add(Pluse(i * 50 + 30, 80));
                }
            timer1.Interval = 16;
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < n; i++)
            {
                bxi[i] = xi[i];
                byi[i] = yi[i];
            }
            for (int i = 0; i < n; i++)
            {
                righti[i] += vxi[i];
                topi[i] += vyi[i];
            }
            for (int i = 0; i < n; i++)    //遇到邊框碰撞返彈
            {                            //vxi[i],vy[i] 球心速度
                if (xi[i] < 20)  //左框
                    vxi[i] = Math.Abs(vxi[i]);
                if (xi[i] > 340)  //右框
                    vxi[i] = -Math.Abs(vxi[i]);
                if (yi[i] < 75)  //上框
                    vyi[i] = Math.Abs(vyi[i]);
                if (vyi[i] > 0 && yi[i] > 500)   //速度方向朝下 & 到達底線前
                {
                    if (ff == false)  //第一顆掉落
                    {
                        location = righti[i]; topi[i] = 500; vxi[i] = 0; vyi[i] = 0; vxx[i] = 0; vyy[i] = 0; ff = true; record = i; break;
                    }
                    if (ff == true)  //非第一顆掉落     
                    {
                        if (yi[i] > 500)
                            righti[i] = location; topi[i] = 500; vxi[i] = 0; vyi[i] = 0; vxx[i] = 0; vyy[i] = 0;
                    }
                }
            }
            foreach (var c in this.Controls)
            {
                if (c is Label)
                {
                    Label Q = (Label)c;
                    if (Q.Visible && Q.BackColor == Color.Green)
                        for (int i = 0; i < n; i++)
                            if (chkhitplus(Q, i))
                                num++;  //球數增加
                }
            }

            int count = 0;
            for (int i = 0; i < n; i++)   //判斷每個球 到底線了 count++
                if (xi[i] == location)
                    count++;
            if (count == n && f == false && ff == true)  //若count==n 代表全部球都到底線了
            {
                vx = 0; vy = 0;
                lv++; f = true; //lv: 代表累積層數
                n += num; num = 0;  //n:計算總球數
                for (int i = 0; i < n; i++)
                    righti[i] = location;   //每個球都給予第一顆掉落球的位子
                ff = false;
            }
            foreach (var c in this.Controls)
            {
                if (c is Label)
                {
                    Label Q = (Label)c;  //方塊顏色變化
                    if (Q.Width == 44)
                    {
                        int ii = System.Convert.ToInt16(Q.Text);

                        if (ii < 6)
                            Q.BackColor = Color.Yellow;
                        if (ii < 11 && ii > 5)
                        {
                            Q.BackColor = Color.Blue;
                        }
                        if (ii > 10)
                            Q.BackColor = Color.Red;
                    }
                }
            }
            foreach (var c in this.Controls)
            {
                if (c is Label)  //判斷被撞擊數字就減一
                {
                    Label Q = (Label)c;
                    if (Q.Visible && Q.BackColor == Color.Red || Q.BackColor == Color.Yellow || Q.BackColor == Color.Blue)
                    {
                        int k = System.Convert.ToInt32(Q.Text);
                        for (int i = 0; i < n; i++)
                            if (chkhit(Q, k, i))
                                k--; Q.Text = System.Convert.ToString(k);
                    }
                }
            }

            foreach (var c in this.Controls)
            {
                if (c is Label) // 方塊判斷後往下移動一層
                {
                    Label Q = (Label)c;
                    if (Q.Visible && Q.BackColor == Color.Red || Q.BackColor == Color.Green || Q.BackColor == Color.Yellow || Q.BackColor == Color.Blue)
                    {
                        if ((Q.Top + 44) > 510)
                        {
                            f = false;
                            lv = 0;
                        }
                        if (f)    //f 判斷是否可下移了~~ true:代表未下移
                            Q.Top += 50;
                    }
                }
            }
            if (f && lv > 1)    //隨機產生新方塊
                for (int i = 0; i < 7; i++)
                {
                    int r = R.Next(15) + 1;
                    if (r % 3 == 1)
                        this.Controls.Add(Brick(i * 50 + 13, 68, lv));
                    else if (r == 2)
                        this.Controls.Add(Pluse(i * 50 + 30, 80));
                }

            f = false;
            this.Invalidate();
        }

        private Boolean chkhit(Label Q, int j, int i) //判斷是否撞擊大方塊  j=方塊上數字
        {
            if (yi[i] - 5 > Q.Bottom) return false;
            if (xi[i] + 15 < Q.Left) return false;
            if (xi[i] - 5 > Q.Right) return false;
            if (yi[i] + 15 < Q.Top) return false;
            if (j == 1)
                Q.Dispose();
            if (yi[i] - 5 <= Q.Bottom && yi[i] - vyi[i] - 5 > Q.Bottom)
            {
                vyi[i] = Math.Abs(vyi[i]);
                return true;
            }
            if (xi[i] + 15 >= Q.Left && xi[i] - vxi[i] + 15 < Q.Left)
            {
                vxi[i] = -Math.Abs(vxi[i]);
                return true;
            }
            if (xi[i] - 5 <= Q.Right && xi[i] - vxi[i] - 5 > Q.Right)
            {
                vxi[i] = Math.Abs(vxi[i]);
                return true;
            }
            if (yi[i] + 15 >= Q.Top && yi[i] - vyi[i] + 15 < Q.Top)
            {
                vyi[i] = -Math.Abs(vyi[i]);
                return true;
            }
            return false;
        }
        private Boolean chkhitplus(Label Q, int i)  //判斷是否撞到綠(加球)方塊
        {
            if (yi[i] - 5 > Q.Bottom) return false;
            if (xi[i] + 10 < Q.Left) return false;
            if (xi[i] - 5 > Q.Right) return false;
            if (yi[i] + 10 < Q.Top) return false;
            Q.Dispose();
            if (yi[i] - 5 <= Q.Bottom) return true;
            if (xi[i] + 10 >= Q.Left) return true;
            if (xi[i] - 5 <= Q.Right) return true;
            if (yi[i] + 10 >= Q.Top) return true;
            return false;
        }
        private Label Brick(int a, int b, int j) //磚塊
        {
            Label Q = new Label();
            Q.Width = 44;
            Q.Height = 44;
            Q.Text = System.Convert.ToString(j);
            Q.Font = new System.Drawing.Font("新細明體", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(100)));
            Q.BackColor = Color.Red;
            Q.Left = a;
            Q.Top = b;
            return Q;
        }
        private Label Pluse(int a, int b) //加球綠色小方塊
        {
            Label Q = new Label();
            Q.Width = 10;
            Q.Height = 10;
            Q.Text = System.Convert.ToString("+");
            Q.BackColor = Color.Green;
            Q.Left = a;
            Q.Top = b;
            return Q;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (lv != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    xi[i] = righti[i] - (vxx[i]) * 10 * i;
                    yi[i] = topi[i] - (vyy[i]) * 10 * i;
                }
                label1.Text = System.Convert.ToString(lv);

                cursor = Control.MousePosition;
                cursor = this.PointToClient(cursor);

                if (vx == 0 && vy == 0)  //目前球速為0 (看第一顆球)
                {
                    if (Control.MouseButtons == System.Windows.Forms.MouseButtons.Left)  //按下左鍵
                    {

                        if (cursor.X >= 10 && cursor.X <= 360 && cursor.Y >= 65 && cursor.Y <= 510) //確保滑鼠左鍵按在視窗內才有效
                        {
                            float m1, m2, m3, m4;
                            m1 = (500 - 400) / (location - 10);
                            m2 = (500 - 450) / (360 - location);
                            m3 = (cursor.Y - 400) / (cursor.X - 10);
                            m4 = (cursor.Y - 450) / (360 - cursor.X);

                            if ((m3 < m1 && cursor.X < location) || (m2 > m4 && cursor.X > location))
                            {
                                f = false;
                                ff = false;
                                ratio = Math.Sqrt(Math.Pow((cursor.X - xi[0]), 2) + Math.Pow((cursor.Y - yi[0]), 2)) / 5;
                                vx = (float)((cursor.X - xi[0]) / ratio);
                                vy = (float)((cursor.Y - yi[0]) / ratio);
                                for (int i = 0; i < n; i++)
                                {
                                    vxx[i] = (float)((cursor.X - xi[0]) / ratio);  //加速度
                                    vyy[i] = (float)((cursor.Y - yi[0]) / ratio);
                                }
                                for (int i = 0; i < n; i++) //每顆球給予第一顆球的速度
                                {
                                    vxi[i] = 3 * vx;
                                    vyi[i] = 3 * vy;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < n; i++)  //畫球 直徑10
                    if (yi[i] <= 500)
                    {
                        Pen whitePen = new Pen(Color.FromArgb(255, 255, 255, 255), 10);
                        e.Graphics.DrawEllipse(whitePen, xi[i], yi[i], 10, 10);
                    }

                    e.Graphics.DrawLine(Pens.White, 10, 65, 360, 65);  //畫外框
                    e.Graphics.DrawLine(Pens.White, 10, 510, 360, 510);
                    e.Graphics.DrawLine(Pens.White, 10, 65, 10, 510);
                    e.Graphics.DrawLine(Pens.White, 360, 65, 360, 510);
            }
            else
            {
                foreach (var c in this.Controls)
                {
                    if (c is Label)
                    {
                        Label Q = (Label)c;
                        Q.Dispose();
                    }
                }
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawImage(Properties.Resources.RMIs0gk, 0, 50, 400, 400);

            }

           

        }
    }
}

