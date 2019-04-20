using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.IO;


namespace GS_MonitoringSite


{
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = sites.getFileList();
        }


        private SitesFile sites = new SitesFile();

        




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sites.UpdateFileSites();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sites.CheckSites();
            dataGridView1.Refresh();
        }
    }
}
