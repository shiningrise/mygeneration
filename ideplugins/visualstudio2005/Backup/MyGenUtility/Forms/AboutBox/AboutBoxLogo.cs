using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyGeneration
{
    public partial class AboutBoxLogo : Control
    {
        private static Random rand = new Random();
        private List<Ball> balls = new List<Ball>();
        private List<Rectangle> pics = new List<Rectangle>();
        private List<int> picindeces = new List<int>();
        private List<Image> images = new List<Image>();
        private Point ip;
        private int countMoves = 0;

        public AboutBoxLogo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void Start()
        {
            // Add other developer pics here if desired.
            images.Add(Properties.Resources.dev01tiny);
            images.Add(Properties.Resources.dev02tiny);

            for (int i = 0; i < 5; i++)
            {
                Ball b = new Ball(this);
                b.MaxLifeInMoves = rand.Next(250) + 250;
                balls.Add(b);
            }
            
            ip = new Point((this.ClientSize.Width - Properties.Resources.mygenlogo1.Width) / 2,
                (this.ClientSize.Height - Properties.Resources.mygenlogo1.Height) / 2);

            this.timerRepaint.Interval = 25;
            this.timerRepaint.Enabled = true;
        }


        private void AboutBoxLogo_MouseUp(object sender, MouseEventArgs e)
        {
            pics.Add(new Rectangle(e.Location.X, 0, 4, 5));
            picindeces.Add(rand.Next(images.Count));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            this.timerRepaint.Enabled = false;

            Graphics g = pe.Graphics;

            for (int i = 0; i < pics.Count; i++)
            {
                g.DrawImage(images[picindeces[i]], pics[i]);
            }

            foreach (Ball b in balls)
            {
                b.Paint(g);
            }

            g.DrawImage(Properties.Resources.mygenlogo1, ip);

            // Calling the base class OnPaint
            base.OnPaint(pe);

            this.timerRepaint.Enabled = true;
        }

        private void timerRepaint_Tick(object sender, EventArgs e)
        {
            countMoves++;
            List<Ball> ballsToAdd = null;
            List<Ball> ballsToRemove = null;
            foreach (Ball b in balls)
            {
                if (b.IsExpired)
                {
                    if (ballsToRemove == null) ballsToRemove = new List<Ball>();
                    ballsToRemove.Add(b);

                    if (!b.IsParticle)
                    {
                        if (ballsToAdd == null) ballsToAdd = new List<Ball>();
                        ballsToAdd.AddRange(b.Explode());
                    }
                }
                else
                {
                    b.Move();
                }
            }
            if (ballsToRemove != null)
            {
                foreach (Ball b in ballsToRemove) balls.Remove(b);
            }
            if (ballsToAdd != null)
            {
                foreach (Ball b in ballsToAdd) balls.Add(b);
            }

            if (countMoves == 50 && this.balls.Count < 300)
            {
                countMoves = 0;
                Ball b = new Ball(this);
                b.MaxLifeInMoves = rand.Next(500) + 100;
                balls.Add(b);
            }

            List<int> picsToRemove = null;
            for (int i=0; i < pics.Count; i++)
            {
                Rectangle r = pics[i];
                if (r.Top > this.Height)
                {
                    if (picsToRemove == null) picsToRemove = new List<int>();
                    picsToRemove.Add(i);
                }
                else
                {
                    r.Y = r.Y + 1;
                    r.X = r.X - 1;
                    r.Width = r.Width + 2;
                    r.Height = r.Height + 2;
                    pics[i] = r;
                }
            }
            if (picsToRemove != null)
            {
                foreach (int i in picsToRemove)
                {
                    pics.RemoveAt(i);
                    picindeces.RemoveAt(i);
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Inner class Ball.
        /// </summary>
        public class Ball
        {
            public Ball(Control c, Pen pen)
            {
                this.Control = c;
                this.Brush = pen.Brush;
                this.GenerateOffsets();
                this.GenerateLocation();
            }

            public Ball(Control c)
            {
                this.Control = c;
                this.GenerateOffsets();
                this.GenerateLocation();
                this.GenerateColor();
            }

            public Ball(Control c, Point location)
            {
                this.Control = c;
                this.Location = location;
                this.GenerateOffsets();
                this.GenerateLocation();
                this.GenerateColor();
            }

            public Ball(Control c, Point location, int offsetX, int offsetY, Pen pen)
            {
                this.Control = c;
                this.Location = location;
                this.Brush = pen.Brush;
                this.OffsetX = offsetX;
                this.OffsetY = offsetY;
            }
            
            private Ball(Control c, Point location, int radius, Brush brush, int maxLives)
            {
                this.Control = c;
                this.Location = location;
                this.Brush = brush;
                this.Radius = radius;
                this.MaxLifeInMoves = maxLives;
                if (radius <= 2) this.IsParticle = true;
                this.GenerateOffsets();
            }

            public Ball[] Explode()
            {
                int maxLives = 40;
                if (this.MaxLifeInMoves <= maxLives && this.MaxLifeInMoves > 0) maxLives = MaxLifeInMoves / 2;
                return new Ball[] {
                    new Ball(this.Control, this.Location, this.Radius/2, this.Brush, maxLives),
                    new Ball(this.Control, this.Location, this.Radius/2, this.Brush, maxLives),
                    new Ball(this.Control, this.Location, this.Radius/2, this.Brush, maxLives),
                    new Ball(this.Control, this.Location, this.Radius/2, this.Brush, maxLives)
                };
            }

            public bool IsExpired
            {
                get
                {
                    if (IsParticle)
                    {
                        return (hasHitWallSinceExpire && IsLifeExpired);
                    }
                    else
                    {
                        return IsLifeExpired;
                    }
                }
            }

            private bool IsLifeExpired
            {
                get
                {
                    if (this.MaxLifeInMoves == -1) return false;
                    else return (this.LifeInMoves > this.MaxLifeInMoves);
                }
            }


            private void GenerateColor()
            {
                int r = rand.Next(255);
                int b = rand.Next(255 - r) + r;
                int a = rand.Next(128) + 127;
                Pen p = new Pen(Color.FromArgb(a, r, r, b));
                this.Brush = p.Brush;
            }

            private void GenerateLocation()
            {
                int x = rand.Next(Control.Width - (Radius * 2)) + Radius, 
                    y = rand.Next(Control.Height - (Radius * 2)) + Radius;

                this.Location = new Point(x, y);
            }

            private void GenerateOffsets() 
            {
                int ox = 0, oy = 0;

                while (ox == 0) ox = rand.Next(MaxSpeed * 2) - MaxSpeed;
                while (oy == 0) oy = rand.Next(MaxSpeed * 2) - MaxSpeed;

                this.OffsetX = ox;
                this.OffsetY = oy;
            }

            public void Move()
            {
                LifeInMoves++;

                int x = OffsetX + Location.X;
                int y = OffsetY + Location.Y;

                if ((x < 0 && OffsetX < 0) || (x > (this.Control.ClientSize.Width - Radius) && OffsetX > 0))
                {
                    OffsetX = -1 * OffsetX;
                    x = OffsetX + Location.X;
                    if (this.IsParticle && this.IsLifeExpired) this.hasHitWallSinceExpire = true;
                }

                if ((y < 0 && OffsetY < 0) || (y > (this.Control.ClientSize.Height - Radius) && OffsetY > 0))
                {
                    OffsetY = -1 * OffsetY;
                    y = OffsetY + Location.Y;
                    if (this.IsParticle && this.IsLifeExpired) this.hasHitWallSinceExpire = true;
                }

                Location.X = x;
                Location.Y = y;
            }

            public void Paint(Graphics g)
            {
                if (IsExpired) return;
                g.FillEllipse(this.Brush, new Rectangle(Location.X - Radius,
                    Location.Y - Radius,
                    Radius * 2,
                    Radius * 2));
            }

            private bool hasHitWallSinceExpire = false;

            public static int MaxSpeed = 6;

            public bool IsParticle = false;
            public Point Location;
            public int Radius = 8;
            public Brush Brush;
            public int OffsetX = 1;
            public int OffsetY = 1;
            public Control Control;
            public int LifeInMoves = 0;
            public int MaxLifeInMoves = -1;
        }
    }
}
