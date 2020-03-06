using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Resources;

namespace Swipe_Brick_Breaker
{
    public partial class Form1 : Form
    {
        static int n = 70;
        Point p1, p2, MouseLocation;
        Timer t = new Timer();
        Button[] btn = new Button[n];
        List<PictureBox> listPic = new List<PictureBox>();
        List<Timer> listTime = new List<Timer>();
        List<Point> PPic = new List<Point>();
        List<bool> xb = new List<bool>(), yb = new List<bool>(), kxb = new List<bool>(), kyb = new List<bool>();
        List<int> x1 = new List<int>(), y1 = new List<int>(),kx=new List<int>(),ky=new List<int>();
        private int[] btncount = new int[n];
        private Random rn = new Random();
        private int i = 0, k = -1, bw, bh, button = 0, runcatch, picw = 20, pich = 20, stacount = 0, sanoq = 0,fastcount=0, last = 0, index = 0, startcount = 0, falledcount = 0;

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Programmer prog = new Programmer();
            prog.ShowDialog();
        }

        private string fpath = "C:\\Program Files/Swipe Brick Breaker/Score.txt";
        SoundPlayer sound = new SoundPlayer();
        private void btnContinue_Click(object sender, EventArgs e)
        {
            isover = false;
            falledcount = 0;
            Startpage.SendToBack();
            btnContinue.Visible = false;
            lblResult.Visible = false;
            btnOk.Visible = false;
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape&&!btnStart.Visible)
            {
                isover = true;
                btnContinue.Visible = true;
                btnContinue.BringToFront();
                for (int w = 0; w < listTime.Count; w++)
                {
                    listTime[w].Tick -= new EventHandler(timer1_Tick);
                    listTime[w].Stop();
                }
                btnStart.BringToFront();
                btnQuit.BringToFront();
                btnContinue.Visible = true;
                lblResult.Visible = true;
                lblResult.Text = "PAUSE";
                lblResult.BringToFront();
                btnOk.Visible = true;
                btnOk.Text = "Stop";
                btnOk.BringToFront();
                if (File.ReadAllText(fpath) != "")
                {
                    string[] str = File.ReadAllText(fpath).Split('#');
                    if (Convert.ToInt32(str[1]) < Convert.ToInt32(lblPoint_.Text))
                    {
                        File.WriteAllText(fpath, lblBall_.Text + "#" + lblPoint_.Text);
                        lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                    }
                }
                else
                {
                    File.WriteAllText(fpath, lblBall_.Text + "#" + lblPoint_.Text);
                    lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics gr = pictureBox1.CreateGraphics();
            gr.DrawLine(new Pen(pictureBox1.BackColor, 10), MouseLocation, listPic[0].Location);
            if (sanoq <= 1 &&!listTime[0].Enabled)
            {
                Pen pen = new Pen(Color.Red, 5);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                gr.DrawLine(pen, e.Location, listPic[0].Location);
            }
            else
            {
                gr.Clear(pictureBox1.BackColor);
            }
            MouseLocation = e.Location;
        }

        private bool isadded = false, isover = false;
        private void btnOk_Click(object sender, EventArgs e)
        {
            sound = new SoundPlayer(MyResource.back);
            sound.PlayLooping();
            btnOk.Visible = false;
            lblResult.Visible = false;
            Startpage.Visible = true;
            btnStart.Visible = true;
            btnAbout.Visible = true;
            btnQuit.Visible = true;
            listPic.Clear();
            listTime.Clear();
            PPic.Clear();
            xb.Clear();
            yb.Clear();
            kxb.Clear();
            kyb.Clear();
            x1.Clear();
            y1.Clear();
            kx.Clear();
            ky.Clear();
            t.Tick -= new EventHandler(timer2_Tick);
            this.Controls.Clear();
            this.Controls.Add(Startpage);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnAbout);
            this.Controls.Add(btnQuit);
            this.Controls.Add(pictureBox1);
            this.Controls.Add(lblBall_);
            this.Controls.Add(lblPoint_);
            this.Controls.Add(lblBall);
            this.Controls.Add(lblPoint);
            this.Controls.Add(btnOk);
            this.Controls.Add(lblResult);
            this.Controls.Add(lblScore1);
            this.Controls.Add(lblScore);
            this.Controls.Add(btnContinue);
            Startpage.BringToFront();
            btnStart.BringToFront();
            btnAbout.BringToFront();
            btnQuit.BringToFront();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            listTime[startcount].Start();
            listTime[startcount].Tick += new EventHandler(timer1_Tick);
            runcatch = button;
            if (startcount < listTime.Count-1)
            {
                startcount++;
            }
            else
            {
                t.Stop();
            }
        }
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Width = this.Width-16;
            for (int j = 0; j < n; j++)
            {
                btn[j] = new Button();
            }
            Startpage.BringToFront();
            btnStart.BringToFront();
            btnAbout.BringToFront();
            btnQuit.BringToFront();
            try
            {
                    string[] str = File.ReadAllText(fpath).Split('#');
                    lblScore.Text = "Ball: " + str[0] + "    Point: " + str[1];
                
            }
            catch
            {
                Directory.CreateDirectory("C:\\Program Files/Swipe Brick Breaker");
                File.Create(fpath);
            }
            sound = new SoundPlayer(MyResource.back);
            sound.PlayLooping();
        }
        private void btnQuit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnStart_Click_1(object sender, EventArgs e)
        {

            sound = new SoundPlayer(MyResource.start);
            sound.PlayLooping();
            k = -1;
            isover = false; 
            lblBall_.Text = "0";
            lblPoint_.Text = "0";
            falledcount = 0;
            Startpage.SendToBack();
            btnStart.Visible = false;
            btnAbout.Visible = false;
            btnQuit.Visible = false;
            bw = pictureBox1.Width / 10;
            bh = bw;
            for (int j = 0; j < n; j++)
            {
                if (j % 10 == 0)
                    k++;
                btn[j].SetBounds(j % 10 * bw, k * bh, bw, bh);
                btn[j].Tag = j;
                btn[j].Font = new Font("Algerian", 18F, FontStyle.Bold);
                btn[j].ForeColor = Color.White;
                try
                {
                    btn[j].Image = MyResource.btnimage;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                btn[j].FlatStyle=FlatStyle.Standard;
                btn[j].UseVisualStyleBackColor = true;
                btncount[j] = 0;
                btn[j].Text = "";
                btn[j].Name = "btn" + j.ToString();
                this.Controls.Add(btn[j]);
                btn[j].MouseDown += new MouseEventHandler(button_MouseDown);
                btn[j].BringToFront();
            }
            listPic.Add(new PictureBox());
            listPic[0].BackgroundImage=MyResource.ballimage;
            listPic[0].BackgroundImageLayout = ImageLayout.Stretch;
            listPic[0].Width = picw;
            listPic[0].Height = pich;
            listPic[0].Location = new Point(0, pictureBox1.Height - pich);
            this.Controls.Add(listPic[0]);
            listPic[0].BringToFront();
            listTime.Add(new Timer());
            listTime[0].Interval = 1;
            t.Interval = 1;
            t.Tick += new EventHandler(timer2_Tick);
            i++;
            PPic.Add(new Point(listPic[0].Location.X, listPic[0].Location.Y));
            button = 0;
            xb.Add(true);
            yb.Add(false);
            kxb.Add(false);
            kyb.Add(false);
            x1.Add(1);
            y1.Add(1);
            kx.Add(1);
            ky.Add(1);
            stacount = 0;
            for (int j = 0; j < n; j++)
            {
                if (j < 10)
                {
                    btncount[j] = 1;
                    btn[j].Text = "1";
                    btn[j].Visible = true;
                }
                else
                {
                    btn[j].Visible = false;
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isover)
            {
                goto M;
            }
            sanoq = 0;
            bool HaveEnabled = false;
            for (int q = 0; q < listTime.Count; q++)
            {
                if (listTime[q].Enabled)
                {
                    HaveEnabled = true;
                    sanoq++;
                    index = q;
                }
                if (listPic[q].Location.Y < pictureBox1.Height - pich)
                {
                    fastcount++;
                }
            }
            if (sanoq == 1 && falledcount == listPic.Count - 1)
            {
                last = index;
            }
            else
            {
                if (fastcount > 10)
                {
                    if (!isadded)
                    {
                        isadded = true;
                        for (int w = 0; w < listTime.Count; w++)
                        {
                            for (int q = 0; q < sanoq*(sanoq/5.0); q++)
                            {
                                listTime[w].Tick += new EventHandler(timer1_Tick);
                            }
                        }
                    }
                }
                else
                {
                    if (isadded)
                    {
                        for (int w = 0; w < listTime.Count; w++)
                        {
                            for (int q = 0; q < sanoq * (sanoq / 5.0); q++)
                            {
                                listTime[w].Tick -= new EventHandler(timer1_Tick);
                            }
                        }
                        isadded = false;
                    }
                }
                last = listPic.Count;
            }
            do
            {
                if (stacount < PPic.Count - 1)
                {
                    stacount++;
                }
                else
                {
                    stacount = 0;
                }
            } while (HaveEnabled && !listTime[stacount].Enabled);
            int butCount = 0;
            for (int j = 0; j < n; j++)
            {
                if (btn[j].Visible)
                {
                    if (PPic[stacount].X == btn[j].Location.X + bw && PPic[stacount].Y <= btn[j].Location.Y + bh  && PPic[stacount].Y + pich >= btn[j].Location.Y - pich)
                    {
                        xb[stacount] = true;
                        btncount[j]--;
                        btn[j].Text = btncount[j].ToString();
                        button++;
                    }
                    if (PPic[stacount].X + picw == btn[j].Location.X && PPic[stacount].Y <= btn[j].Location.Y + bh && PPic[stacount].Y + pich >= btn[j].Location.Y)
                    {
                        xb[stacount] = false;
                        btncount[j]--;
                        btn[j].Text = btncount[j].ToString();
                        button++;
                    }
                    if (PPic[stacount].Y == btn[j].Location.Y + bh && PPic[stacount].X <= btn[j].Location.X + bw && PPic[stacount].X >= btn[j].Location.X - picw)
                    {
                        yb[stacount] = true;
                        btncount[j]--;
                        btn[j].Text = btncount[j].ToString();
                        button++;
                    }
                    if (PPic[stacount].Y + pich == btn[j].Location.Y && PPic[stacount].X <= btn[j].Location.X + bw && PPic[stacount].X >= btn[j].Location.X - picw)
                    {
                        yb[stacount] = false;
                        btncount[j]--;
                        btn[j].Text = btncount[j].ToString(); 
                        button++;
                    }
                    lblPoint_.Text = Convert.ToString(button);
                }
                else
                {
                    butCount++;
                }
                if (btncount[j] <= 0)
                {
                    btn[j].Visible = false;
                }
            }
            if (butCount == n)
            {
                isover = true;
                for (int w = 0; w < listTime.Count; w++)
                {
                    listTime[w].Tick -= new EventHandler(timer1_Tick);
                    listTime[w].Stop();
                }
                sound = new SoundPlayer(MyResource.win);
                sound.Play();
                lblResult.Visible = true;
                lblResult.Text = "YOU WIN";
                lblResult.BringToFront();
                btnOk.Visible = true;
                btnOk.Text = "OK";
                btnOk.BringToFront();
                if (File.ReadAllText(fpath) != "")
                {
                    string[] str = File.ReadAllText(fpath).Split('#');
                    if (Convert.ToInt32(str[1]) < Convert.ToInt32(lblPoint_.Text))
                    {
                        File.WriteAllText(fpath,lblBall_.Text + "#" + lblPoint_.Text);
                        lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                    }
                }
                else
                {
                    File.WriteAllText(fpath, lblBall_.Text + "#" + lblPoint_.Text);
                    lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                }
                goto M;
            }
            listPic[stacount].Location = new Point(PPic.ToArray()[stacount].X, PPic.ToArray()[stacount].Y);

            if (PPic[stacount].X <= 0)
            {
                xb[stacount] = true;
            }
            if (PPic[stacount].X >= pictureBox1.Width - picw)
            {
                xb[stacount] = false;
            }
            if (PPic[stacount].Y <= 0)
            {
                yb[stacount] = true;
            }
            if (listPic[stacount].Location.Y > pictureBox1.Height - pich)
            {
                falledcount++;
                listTime[stacount].Stop();
                listTime[stacount].Tick -= new EventHandler(timer1_Tick);
                yb[stacount] = false;
                PPic[stacount] = new Point(PPic[0].X, pictureBox1.Height - pich);
                listPic[stacount].Location = new Point(PPic[stacount].X, PPic[stacount].Y);

                if (stacount == last)
                {
                    falledcount = 0;
                    for (int q = 0; q < listPic.Count; q++)
                    {
                        listPic[q].Location = new Point(listPic[0].Location.X, listPic[0].Location.Y);
                        PPic[q] = new Point(listPic[0].Location.X, listPic[0].Location.Y);
                    }
                    kx.Add(1);
                    ky.Add(1);
                    kxb.Add(false);
                    kyb.Add(false);
                    x1.Add(1);
                    y1.Add(1);
                    for (int q = 0; q < kxb.Count; q++)
                    {
                        kxb[q] = false;
                        kyb[q] = false;
                    }
                    listPic.Add(new PictureBox());
                    listPic[stacount+1].BackgroundImage = MyResource.ballimage;
                    listPic[stacount+1].BackgroundImageLayout = ImageLayout.Stretch;
                    listPic[stacount+1].Width = picw;
                    listPic[stacount+1].Height = pich;
                    listPic[stacount+1].Location = new Point(PPic[0].X, PPic[0].Y);
                    this.Controls.Add(listPic[stacount+1]);
                    listPic[stacount+1].BringToFront();
                    listTime.Add(new Timer());
                    listTime[stacount + 1].Interval = 1;
                    PPic.Add(new Point(listPic[0].Location.X, listPic[0].Location.Y));
                    xb.Add(xb[stacount]);
                    yb.Add(yb[stacount]);
                    for (int j = n - 1; j >= 0; j--)
                    {
                        int r = rn.Next(0, 2);
                        if (j >= 10)
                        {
                            if (btn[j - 10].Visible)
                            {
                                btncount[j] = btncount[j - 10];
                                btn[j].Text = btncount[j].ToString();
                                btn[j].Visible = true;
                            }
                            btn[j - 10].Visible = false;
                            btncount[j] = btncount[j - 10];
                            btn[j].BackColor = btn[j - 10].BackColor;
                            btn[j].UseVisualStyleBackColor = true;
                        }
                        else
                        {
                            if (r == 1)
                            {
                                btncount[j] = listPic.Count;
                                btn[j].Text = btncount[j].ToString();
                                 btn[j].Visible = true;
                            }
                        }
                    }
                    for (int j = n - 1; j >= n-10; j--)
                    {
                        if (btn[j].Visible)
                        {
                            isover = true;
                            for (int w = 0; w < listTime.Count; w++)
                            {
                                listTime[w].Tick -= new EventHandler(timer1_Tick);
                                listTime[w].Stop();
                            }
                            sound = new SoundPlayer(MyResource.lose);
                            sound.Play();
                            lblResult.Visible = true;
                            lblResult.Text = "GAME OVER";
                            lblResult.BringToFront();
                            btnOk.Visible = true;
                            btnOk.Text = "OK";
                            btnOk.BringToFront();
                            if (File.ReadAllText(fpath) != "")
                            {
                                string[] str = File.ReadAllText(fpath).Split('#');
                                if (Convert.ToInt32(Convert.ToInt32(str[1])) < Convert.ToInt32(lblPoint_.Text))
                                {
                                    File.WriteAllText(fpath, lblBall_.Text + "#" + lblPoint_.Text);
                                    lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                                }
                            }
                            else
                            {
                                File.WriteAllText(fpath, lblBall_.Text + "#" + lblPoint_.Text);
                                lblScore.Text = "Ball: " + lblBall_.Text + "    Point: " + lblPoint_.Text;
                            }
                            goto M;
                        }
                    }
                    stacount = 0;
                }
                else
                {
                }

            }
            while (x1[stacount] != 1 || y1[stacount] != 1)
            {
                if (x1[stacount] > 1)
                {
                    kyb[stacount] = false;
                    kxb[stacount] = true;
                    kx[stacount]++;
                    x1[stacount]--;
                }
                else
                {
                    if (y1[stacount] > 1)
                    {
                        kxb[stacount] = false;
                        kyb[stacount] = true;
                        ky[stacount]++;
                        y1[stacount]--;
                    }
                    else
                    {
                        kxb[stacount] = false;
                        x1[stacount] = kx[stacount];
                        kx[stacount] = 1;
                        kyb[stacount] = false;
                        y1[stacount] = ky[stacount];
                        ky[stacount] = 1;
                    }
                }
                if (kxb[stacount])
                {
                    PPic[stacount] = xb[stacount] ? new Point(PPic[stacount].X + 1, PPic[stacount].Y) : new Point(PPic[stacount].X - 1, PPic[stacount].Y);

                }
                else
                {
                    if (kyb[stacount])
                    {
                        PPic[stacount] = yb[stacount] ? new Point(PPic[stacount].X, PPic[stacount].Y + 1) : new Point(PPic[stacount].X, PPic[stacount].Y - 1);
                    }
                    else
                    {
                        PPic[stacount] = xb[stacount] ? new Point(PPic[stacount].X + 1, PPic[stacount].Y) : new Point(PPic[stacount].X - 1, PPic[stacount].Y);
                        PPic[stacount] = yb[stacount] ? new Point(PPic[stacount].X, PPic[stacount].Y + 1) : new Point(PPic[stacount].X, PPic[stacount].Y - 1);
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    if (btn[j].Visible)
                    {
                        if (PPic[stacount].X == btn[j].Location.X + bw && PPic[stacount].Y <= btn[j].Location.Y + bh && PPic[stacount].Y + pich >= btn[j].Location.Y - pich)
                        {
                            xb[stacount] = true;
                            btncount[j]--;
                            btn[j].Text = btncount[j].ToString();
                            button++;
                        }
                        if (PPic[stacount].X + picw == btn[j].Location.X && PPic[stacount].Y <= btn[j].Location.Y + bh && PPic[stacount].Y + pich >= btn[j].Location.Y)
                        {
                            xb[stacount] = false;
                            btncount[j]--;
                            btn[j].Text = btncount[j].ToString();
                            button++;
                        }
                        if (PPic[stacount].Y == btn[j].Location.Y + bh && PPic[stacount].X <= btn[j].Location.X + bw && PPic[stacount].X >= btn[j].Location.X - picw)
                        {
                            yb[stacount] = true;
                            btncount[j]--;
                            btn[j].Text = btncount[j].ToString();
                            button++;
                        }
                        if (PPic[stacount].Y + pich == btn[j].Location.Y && PPic[stacount].X <= btn[j].Location.X + bw && PPic[stacount].X >= btn[j].Location.X - picw)
                        {
                            yb[stacount] = false;
                            btncount[j]--;
                            btn[j].Text = btncount[j].ToString();
                            button++;
                        }
                        lblPoint_.Text = Convert.ToString(button);
                    }
                    else
                    {
                        butCount++;
                    }
                    if (btncount[j] <= 0)
                    {
                        btn[j].Visible = false;
                    }
                }
            }
            PPic[stacount] = xb[stacount] ? new Point(PPic[stacount].X + 1, PPic[stacount].Y) : new Point(PPic[stacount].X - 1, PPic[stacount].Y);
            PPic[stacount] = yb[stacount] ? new Point(PPic[stacount].X, PPic[stacount].Y + 1) : new Point(PPic[stacount].X, PPic[stacount].Y - 1);
            kxb[stacount] = false;
            x1[stacount] = kx[stacount];
            kx[stacount] = 1;
            kyb[stacount] = false;
            y1[stacount] = ky[stacount];
            ky[stacount] = 1;
            M:;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bool CanStart = false;
            for(int q = 0; q < n; q++)
            {
                if (btn[q].Visible)
                {
                    CanStart=true;
                }
            }
            if (!btnOk.Visible&&CanStart)
            {
                p1.X = listPic[0].Location.X + 10;
                p1.Y = listPic[0].Location.Y + 10;
                if (sender == pictureBox1)
                {
                    p2 = e.Location;
                }
                if (!listTime[stacount].Enabled)
                {
                    lblBall_.Text = listPic.Count.ToString();
                    for (int q = 0; q < listPic.Count; q++)
                    {
                        x1[q] = 1;
                        y1[q] = 1;
                        kx[q] = 1;
                        ky[q] = 1;
                        listPic[q].Location = new Point(listPic[0].Location.X, listPic[0].Location.Y);
                        PPic[q] = new Point(listPic[0].Location.X, listPic[0].Location.Y);
                    }
                    if (p1.X > p2.X && p1.Y > p2.Y)
                    {
                        for (int q = 0; q < xb.Count; q++)
                        {
                            xb[q] = false;
                            yb[q] = false;
                        }
                        if (p1.X - p2.X > p1.Y - p2.Y)
                        {
                            double d = Convert.ToDouble(p1.X - p2.X) / (p1.Y - p2.Y);
                            for (int q = 0; q < x1.Count; q++)
                            {
                                if (Math.Round(d) < 10)
                                {
                                    x1[q] = 9;
                                    y1[q] = 9/Convert.ToInt32(Math.Round(d));
                                }
                                else
                                {
                                    x1[q] = 10;
                                    y1[q] = 1;
                                }
                            }
                        }
                        else
                        {
                            double d = Convert.ToDouble(p1.Y - p2.Y) / (p1.X - p2.X);
                            for (int q = 0; q < x1.Count; q++)
                            {
                                if (Math.Round(d) < 10)
                                {
                                    y1[q] = 9;
                                    x1[q] = 9/Convert.ToInt32(Math.Round(d));
                                }
                                else
                                {
                                    y1[q] = 10;
                                    x1[q] = 0;
                                }
                            }
                        }
                    }
                    if (p1.X < p2.X && p1.Y > p2.Y)
                    {
                        for (int q = 0; q < xb.Count; q++)
                        {
                            xb[q] = true;
                            yb[q] = false;
                        }
                        if (p2.X - p1.X > p1.Y - p2.Y)
                        {
                            double d = Convert.ToDouble(p2.X - p1.X) / (p1.Y - p2.Y);
                            for (int q = 0; q < x1.Count; q++)
                            {
                                if (Math.Round(d) < 10)
                                {
                                    x1[q] = 9;
                                    y1[q] = 9 / Convert.ToInt32(Math.Round(d));
                                }
                                else
                                {
                                    x1[q] = 10;
                                    y1[q] = 1;
                                }
                            }
                        }
                        else
                        {
                            double d = Convert.ToDouble(p1.Y - p2.Y) / (p2.X - p1.X);
                            for (int q = 0; q < x1.Count; q++)
                            {
                                if (Math.Round(d) < 10)
                                {
                                    y1[q] = 9;
                                    x1[q] = 9 / Convert.ToInt32(Math.Round(d));
                                }
                                else
                                {
                                    y1[q] = 10;
                                    x1[q] = 1;
                                }
                            }
                        }
                    }
                    startcount = 0;
                    t.Interval = 50;
                    t.Start();
                }

            }
        }
        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (!btnOk.Visible)
            {
                Button b = (Button)sender;
                for(int q = 0; q < btn.Length; q++)
                {
                    if (b.Name==btn[q].Name)
                    {
                        p2.X = btn[q].Location.X + bw/2;
                        p2.Y = btn[q].Location.Y + bh/2;
                    }
                }
                pictureBox1_MouseDown(sender, e);
            }
        }
    }
}
