using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMARTY.Ui
{
    public partial class Splash : Form
    {
        public static Splash spl;
        public Splash()
        {
            InitializeComponent();
            this.Load += Splash_Load1;
            spl = this;
        }

        private void Splash_Load1(object sender, EventArgs e)
        {
            BunifuDragControl drag = new
              BunifuDragControl();
            drag.TargetControl = this;

            BunifuElipse elipse = new
                BunifuElipse();
            elipse.ElipseRadius = 50;
            elipse.TargetControl = this; 

            Timer t = new Timer();
            t.Tick += T_Tick;
            t.Interval = 50;
            t.Enabled = true;
            t.Start();
        }

        int i = 1;
        private void T_Tick(object sender, EventArgs e)
        {
            if (i > 10)
                i = 1;
            else
                i++;
            toggel(i);


        }

        private void toggel(int i)
        {
            allWhite();

            if (i==1)
                label1.ForeColor = Color.SeaGreen;
            if (i==2)
                label2.ForeColor = Color.SeaGreen;
            if (i == 3)
                label3.ForeColor = Color.SeaGreen;
            if (i == 4)
                label4.ForeColor = Color.SeaGreen;
            if (i == 5)
                label5.ForeColor = Color.SeaGreen;
            if (i == 6)
                label6.ForeColor = Color.SeaGreen;
            if (i == 7)
                label7.ForeColor = Color.SeaGreen;
            if (i == 8)
                label8.ForeColor = Color.SeaGreen;
            if (i == 9)
                label9.ForeColor = Color.SeaGreen;
            if (i == 10)
                label10.ForeColor = Color.SeaGreen;

        }

        private void Splash_Load(object sender, EventArgs e)
        {
            BunifuDragControl drag = new
                BunifuDragControl();
            drag.TargetControl = this;
        }

        private void allWhite()
        {
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label5.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            label8.ForeColor = Color.White;
            label7.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label9.ForeColor = Color.White;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
