using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsAppLab3
{
    public partial class Form2 : Form
    {
        public Form1 parent;
        public bool flagChanged = false;

        public int prntValLine;
        public Size prntValPointSize;

        public Form2(Form1 parentForm)
        {
            InitializeComponent();
            parent = parentForm;
            numericUpDown1.Value = parentForm.LineWidth;
            numericUpDown2.Value = parentForm.PointSize.Width;
            numericUpDown2.Value = parentForm.PointSize.Height;
            prntValLine = parentForm.LineWidth;
            prntValPointSize = parentForm.PointSize;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagChanged)
            {
                parent.LineWidth = (int)numericUpDown1.Value;
                parent.PointSize = new Size((int)numericUpDown2.Value, (int)numericUpDown2.Value);
                parent.Refresh();
            }
            else
            {
                parent.LineWidth = prntValLine;
                parent.PointSize = prntValPointSize;
                parent.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 3;
            numericUpDown2.Value = 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flagChanged = true;
            parent.LineWidth = (int)numericUpDown1.Value;
            parent.PointSize = new Size((int)numericUpDown2.Value, (int)numericUpDown2.Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            parent.LineWidth = (int)numericUpDown1.Value;
            parent.Refresh();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            parent.PointSize = new Size((int)numericUpDown2.Value, (int)numericUpDown2.Value);
            parent.Refresh();

        }
    }
}
