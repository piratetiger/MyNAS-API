using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MyNAS.Util;

namespace MyNAS.Site.BackendServices
{
    public class TorrentTask
    {
        public string SavePath { get; set; }
        public string Path { get; set; }
    }

    public interface ITorrentDownloadService
    {
        void Enqueue(string savePath, string path);
        double[] Status();
    }

    public class TorrentDownloadService : IHostedService, IDisposable, ITorrentDownloadService
    {
        private static Timer _timer;
        private static ConcurrentQueue<TorrentTask> _taskQueue = new ConcurrentQueue<TorrentTask>();
        private static TorrentDownload _download = new TorrentDownload();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Enqueue(string savePath, string path)
        {
            _taskQueue.Enqueue(new TorrentTask { SavePath = savePath, Path = path });
        }

        public double[] Status()
        {
            return _download.GetState();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            TorrentTask task = null;
            while (_taskQueue.TryDequeue(out task) && task != null)
            {
                _download.LoadTorrent(task.Path, task.SavePath).StartAsync();
            }
        }
    }
}