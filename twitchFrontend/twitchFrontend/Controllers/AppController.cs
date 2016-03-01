using System.Collections.Generic;
using System.Threading.Tasks;
using twitch.Models;
using twitchFrontend.Service;

namespace twitchFrontend.Controllers
{
    class AppController
    {
        List<Streamer> _datasourceStreamer;
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

        public void Button_Launch(List<Streamer> selectedStreamers)
        {
            Task.Run(() => _serviceHandler.DisplayStream(selectedStreamers));
        }
    }
}
