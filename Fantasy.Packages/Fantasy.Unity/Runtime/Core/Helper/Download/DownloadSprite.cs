#if FANTASY_UNITY
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasy.Unity.Download
{
    public sealed class DownloadSprite : AUnityDownload
    {
        public DownloadSprite(Scene scene, Download download) : base(scene, download)
        {
        }

        public UniTask<Sprite> StartDownload(string url, bool monitor, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<Sprite>.Create();
            var unityWebRequestAsyncOperation = Start(UnityWebRequestTexture.GetTexture(Uri.EscapeUriString(url)), monitor);
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    Dispose();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                try
                {
                    if (UnityWebRequest.result == UnityWebRequest.Result.Success)
                    {
                        var texture = DownloadHandlerTexture.GetContent(UnityWebRequest);
                        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 5, 1f);
                        task.TrySetResult(sprite);
                    }
                    else
                    {
                        Log.Error(UnityWebRequest.error);
                        task.TrySetResult(null);
                    }
                }
                finally
                {
                    Dispose();
                }
            };

            return task.Task;
        }
    }
}
#endif