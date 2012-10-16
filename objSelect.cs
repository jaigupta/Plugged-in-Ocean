using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoCommunication
{
    public partial class objSelect : Form
    {
        public static int SelectedIndex = -1;
        public objSelect(String title, List<string> names)
        {
            InitializeComponent();
            this.selectCombo.DataSource = names;
            this.Text = title;
        }
        private void selectCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = this.selectCombo.SelectedIndex;
        }
    }
}
