using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using twitch.Models;
using twitchFrontend.Service;

namespace twitchFrontend
{
    public partial class Form1 : Form
    {
        List<Streamer> _datasource = new List<Streamer>();
        AppService _serviceHandler;

        public Form1()
        {
            InitializeComponent();

            _datasource.Add(new Streamer { name = "hi", url = "/stream", online = false });
            _datasource.Add(new Streamer { name = "bye", url = "/sss", online = false });
            _datasource.Add(new Streamer { name = "hello", url = "/stream2", online = false });


            dataGridView1.DataSource = _datasource;
            _serviceHandler = new AppService(_datasource);

            //Task llamaq = RunAsync2();

            dataGridView1.SuspendLayout();
            Task.Run(() => _serviceHandler.UpdateStreamers()).Wait();
            dataGridView1.ResumeLayout();
            dataGridView1.Refresh();
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
    }
}
