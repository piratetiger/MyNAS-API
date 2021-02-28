
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTorrent;
using MonoTorrent.Client;
using MonoTorrent.Client.PiecePicking;

namespace MyNAS.Util
{
    public class TorrentDownload
    {
        private ClientEngine _engine;
        private Dictionary<string, TorrentManager> _managers = new Dictionary<string, TorrentManager>();

        public TorrentDownload()
        {
            EngineSettings settings = new EngineSettings();
            settings.PreferEncryption = true;
            settings.MaximumUploadSpeed = 200 * 1024;

            _engine = new ClientEngine(settings);
        }

        public TorrentManager LoadTorrent(string path, string savePath)
        {
            Torrent torrent = Torrent.Load(path);

            // Set all the files to not download
            foreach (TorrentFile file in torrent.Files)
                file.Priority = Priority.DoNotDownload;

            // Set the first file as high priority and the second one as normal
            if (torrent.Files.Any())
            {
                torrent.Files[0].Priority = Priority.Highest;
            }
            // torrent.Files[1].Priority = Priority.Normal;

            TorrentManager manager = new TorrentManager(torrent, savePath, new TorrentSettings());
            _managers.Add(Guid.NewGuid().ToString(), manager);
            _engine.Register(manager);

            // PiecePicker picker = new StandardPicker();
            // picker = new PriorityPicker(picker);
            // manager.ChangePickerAsync(picker);

            return manager;
        }

        public void StartAll()
        {
            _engine.StartAll();
        }

        public void StopAll()
        {
            _engine.StopAll();
        }

        public double[] GetState()
        {
            return _managers.Select(t => t.Value.Progress).ToArray();
        }

        public void Start(params string[] torrentIds)
        {
            var torrents = _managers.Where(t => torrentIds.Contains(t.Key));
        }
    }
}