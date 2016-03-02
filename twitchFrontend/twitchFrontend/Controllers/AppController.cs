using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using twitch.Models;
using twitchFrontend.Service;

namespace twitchFrontend.Controllers
{
    class AppController
    {
        readonly List<Streamer> _datasourceStreamer;
        readonly AppService _serviceHandler;

        public AppController(List<Streamer> _datasource)
        {
            _datasourceStreamer = _datasource;
            _serviceHandler = new AppService(_datasource);
        }

        public void Button_Refresh()
        {
            Task.Run(() => _serviceHandler.UpdateStreamers()).Wait();
        }

        public void Button_Launch(DataGridViewSelectedRowCollection dataRows)
        {
            var selectedRows = new List<Streamer>();
            for (int i = 0; i < dataRows.Count; i++)
            {
                var streamer = _datasourceStreamer.Single(o => o.name == dataRows[i].Cells[0].Value.ToString());
                if (streamer.online == false)
                {
                    MessageBox.Show("Streamer is not online:  " + streamer.name, "Offline",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    selectedRows.Add(streamer);
                }
            }
            Task.Run(() => _serviceHandler.DisplayStream(selectedRows));
        }
    }
}
