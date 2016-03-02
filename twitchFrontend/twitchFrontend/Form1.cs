using System.Collections.Generic;
using System.Windows.Forms;
using twitch.Models;
using twitchFrontend.Controllers;

namespace twitchFrontend
{
    public partial class Form1 : Form
    {
        List<Streamer> _datasource = new List<Streamer>();
        AppController _controller;

        public Form1()
        {
            InitializeComponent();

            //_datasource.Add(new Streamer { name = "hi", url = "/stream", online = false });
            //_datasource.Add(new Streamer { name = "bye", url = "/sss", online = false });
            //_datasource.Add(new Streamer { name = "hello", url = "/stream2", online = false });
            _datasource.Add(new Streamer { name = "winter", url = "/stream2", online = false });
            _datasource.Add(new Streamer { name = "test_channel", url = "/stream", online = false });


            dataGridView1.DataSource = _datasource;
            _controller = new AppController(_datasource);

            comboBox1.DataSource = new string[] { "Source", "High", "Medium", "Low" };
            comboBox1.SelectedIndex = 0;
        }

        void cellFormattingImage(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.Columns[e.ColumnIndex].Name == "online")
            {
                e.Value = (bool)e.Value ? (System.Drawing.Image)Properties.Resources.green : (System.Drawing.Image)Properties.Resources.red;
                e.FormattingApplied = true;
            }
        }

        void button_Refresh_Click(object sender, System.EventArgs e)
        {
            button1.Enabled = false;
            dataGridView1.SuspendLayout();
            _controller.Button_Refresh();
            dataGridView1.ResumeLayout();
            dataGridView1.Refresh();
            button1.Enabled = true;
        }

        void button_Launch_Click(object sender, System.EventArgs e)
        {
            _controller.Button_Launch(dataGridView1.SelectedRows);
        }
    }
}
