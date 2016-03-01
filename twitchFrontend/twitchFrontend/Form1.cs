using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using twitch.Models;
using twitchFrontend.Controllers;

namespace twitchFrontend
{
    public partial class Form1 : Form
    {
        List<Streamer> _datasource = new List<Streamer>();
        //AppService _serviceHandler;
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
            var dataRows = dataGridView1.SelectedRows;
            var selectedRows = new List<Streamer>();
            for (int i = 0; i < dataRows.Count; i++)
            {
                var streamer = _datasource.Single(o => o.name == dataRows[i].Cells[0].Value.ToString());
                if (streamer.online == false)
                {
                    MessageBox.Show("Streamer is not online:  " + streamer.name, "Offline",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    selectedRows.Add(streamer);
            }
            _controller.Button_Launch(selectedRows);
        }
    }
}
