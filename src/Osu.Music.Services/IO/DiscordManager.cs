using DiscordRPC;
using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using Prism.Mvvm;
using System;

namespace Osu.Music.Services.IO
{
    public class DiscordManager : BindableBase, IDisposable
    {
        private static readonly string APPLICATION_ID = "910311809179848745";

        private DiscordRpcClient _client;
        public DiscordRpcClient Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public DiscordManager()
        {
            Client = new DiscordRpcClient(APPLICATION_ID);
        }

        public void Initialize()
        {
            if (Client == null)
                Client = new DiscordRpcClient(APPLICATION_ID);

            if (!Client.IsInitialized)
                Client.Initialize();
        }

        public void Deinitialize()
        {
            if (Client == null)
                return;

            if (Client.IsInitialized)
                Client.Deinitialize();
        }

        public void Update(Beatmap beatmap)
        {
            if (Client == null || !Client.IsInitialized)
                return;

            if (beatmap != null)
            {
                // TODO: Imaplement some fun stuff like status with small image
                Client.SetPresence(new RichPresence()
                {
                    State = beatmap.Artist,
                    Details = beatmap.Title,
                    Timestamps = new Timestamps()
                    {
                        StartUnixMilliseconds = DateTime.Now.ToUniversalTime().ToUnix()
                    },
                    Assets = new Assets()
                    {
                        LargeImageKey = "logo",
                        LargeImageText = $"{beatmap.Title} - {beatmap.Artist}"
                    }
                });
            } 
            else
            {
                Client.ClearPresence();
            }
        }

        public void Dispose()
        {
            Deinitialize();
        }
    }
}
