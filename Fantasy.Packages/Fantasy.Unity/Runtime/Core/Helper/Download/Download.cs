#if FANTASY_UNITY
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine;

namespace Fantasy.Unity.Download
{
    public sealed class Download
    {
        public Scene Scene;
        public ulong DownloadSpeed;
        public ulong TotalDownloadedBytes;
        public readonly HashSet<AUnityDownload> Tasks = new();

        public static Download Create(Scene scene) => new Download(scene);

        private Download(Scene scene)
        {
            Scene = scene;
        }
        
        public void Clear()
        {
            DownloadSpeed = 0;
            TotalDownloadedBytes = 0;
            
            if (Tasks.Count <= 0)
            {
                return;
            }
            
            foreach (var aUnityDownload in Tasks.ToArray())
            {
                aUnityDownload.Dispose();
            }
            
            Tasks.Clear();
        }

        public UniTask<AssetBundle> DownloadAssetBundle(string url, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadAssetBundle(Scene, this).StartDownload(url, monitor, cancellationToken);
        }

        public UniTask<AudioClip> DownloadAudioClip(string url, AudioType audioType, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadAudioClip(Scene, this).StartDownload(url, audioType, monitor, cancellationToken);
        }

        public UniTask<Sprite> DownloadSprite(string url, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadSprite(Scene, this).StartDownload(url, monitor, cancellationToken);
        }
        
        public UniTask<Texture> DownloadTexture(string url, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadTexture(Scene, this).StartDownload(url, monitor, cancellationToken);
        }
        
        public UniTask<string> DownloadText(string url, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadText(Scene, this).StartDownload(url, monitor, cancellationToken);
        }

        public UniTask<byte[]> DownloadByte(string url, bool monitor = false, CancellationTokenSource cancellationToken = null)
        {
            return new DownloadByte(Scene, this).StartDownload(url, monitor, cancellationToken);
        }
    }
}
#endif