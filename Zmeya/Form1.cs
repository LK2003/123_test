using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zmeya
{
    public partial class Form1 : Form
    {
        Graphics g;        
        List<Rectangle> zmeya = new List<Rectangle>();      
        public enum Course { UP, DOWN, LEFT, RIGHT };
        public int lengthZmeya; 
        public bool existsFood; 
        Rectangle food = new Rectangle(); 
        List<Rectangle> empty_filed = new List<Rectangle>(); 
        public Random rnd = new Random();
        public Course course;     
        public int X, Y; 

        public Form1()
        {
            InitializeComponent();
            course = Course.LEFT; 
            zmeya.Add(new Rectangle(200, 0, 20, 20));
            zmeya.Add(new Rectangle(220, 0, 20, 20));
            zmeya.Add(new Rectangle(240, 0, 20, 20));
            timer1.Interval = 200;           
            lengthZmeya = 3;
            existsFood = false;
            g = this.CreateGraphics();
            DrawZmeya();             
            DrawFood(); 
            timer1.Enabled = true;         
        }  
        
        private void Form1_TickTimer(object sender, EventArgs e)
        {
            Refresh(); 
            timer1.Enabled = false; 

            if (course == Course.UP)
            {
              X = 0;
              Y = -20;
            }

            if (course == Course.DOWN)
            {
              X = 0;
              Y = 20;
            }

            if (course == Course.LEFT)
            {
              X = -20;
              Y = 0;
            }

            if (course == Course.RIGHT)
            {
              X = 20;
              Y = 0;
            }

            Rectangle prev_segment;
            Rectangle next_segment;

            prev_segment = zmeya[0]; 
            for (int i = 0; i < zmeya.Count - 1; i++)
            {

                if (i == 0)
                {

                    zmeya[i] = new Rectangle(zmeya[i].X + X, zmeya[i].Y + Y, 20, 20);
                }
                if (!(zmeya[i + 1].IsEmpty))
                {
                    next_segment = zmeya[i + 1];
                    zmeya[i + 1] = prev_segment;
                    prev_segment = next_segment;
                }
            }
  
            if (zmeya[0] == food)
            {
                zmeya.Add(food); 
                lengthZmeya++; 
                existsFood = false; 
            
                if ((lengthZmeya % 7 == 0) && (timer1.Interval > 50))
                {
                    timer1.Interval -= 30;
                }
            }
           
            if ((zmeya[0].X < 0 || zmeya[0].X > 240 || zmeya[0].Y < 0 || zmeya[0].Y > 240) || EatMySelf())
            {
                MessageBox.Show("WASTED\n ZMEYA: " + lengthZmeya.ToString());
                this.Close();
                return;
            }

            DrawZmeya();  
            DrawFood();             

            timer1.Enabled = true; 

        } 

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && course != Course.DOWN)
            {
                course = Course.UP;
            }
            if (e.KeyCode == Keys.Down && course != Course.UP)
            {
                course = Course.DOWN;
            }
            if (e.KeyCode == Keys.Left && course != Course.RIGHT)
            {
                course = Course.LEFT;
            }
            if (e.KeyCode == Keys.Right && course != Course.LEFT)
            {
                course = Course.RIGHT;
            }

        } 

        private void DrawZmeya()
        {
            for (int i = 0; i < zmeya.Count; i++)
            {
                if (i == 0)  
                    g.FillRectangle(Brushes.Green, zmeya[i]);
                else 
                    g.FillRectangle(Brushes.Black, zmeya[i]);
                g.DrawRectangle(Pens.Green, zmeya[i]);
            }
        } 

        private void DrawFood()
        {
            if (!existsFood)
            {
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        Rectangle temp = new Rectangle(i * 20, j * 20, 20, 20);
                        if (zmeya.IndexOf(temp) == -1)
                        {
                            empty_filed.Add(temp);
                        }
                    }
                }
              
                food = empty_filed[rnd.Next(0, empty_filed.Count - 1)];
                empty_filed.Clear(); 
                g.FillRectangle(Brushes.Red, food); 
                existsFood = true;  
            }
            else
            {  
                g.FillRectangle(Brushes.Red, food);
            }
        }
      
        private bool EatMySelf()
        {
            int count = 0; 
            foreach (Rectangle t in zmeya)
            {
                if (t == zmeya[0]) count++;
            }
           
            if (count > 1 && food != zmeya[0])
                return true;
            else
                return false;
        }             
    }
}
