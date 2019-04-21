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
            this.FormClosing += Form1_FormClosing;
            dataGridView1.DataSource = sites.getSitesList();
        }


        private SitesFile sites = new SitesFile();


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sites.Add(textBox1.Text);
            dataGridView1.Refresh();
            dataGridView1.Update();
            textBox1.Clear();
            UpdateDataSource();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sites.CheckSites();
            dataGridView1.Refresh();
            UpdateDataSource();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sites.UpdateFileSites();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            int index = dataGridView1.SelectedCells[0].RowIndex;
            sites.Delete(index);
            UpdateDataSource();
        }

        private void UpdateDataSource()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = sites.getSitesList();
        }
    }
}
