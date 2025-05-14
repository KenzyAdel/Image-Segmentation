using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class Bonus2: Form
    {
        public Bonus2(Image imageFromForm1)
        {
            InitializeComponent();
            pictureBox1_bonus2.Image = imageFromForm1;
        }

        private void pictureBox1_bonus2_Click(object sender, EventArgs e)
        {

        }
    }
}
