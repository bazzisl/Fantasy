#if FANTASY_UNITY
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasy.Unity.Download
{
    public sealed class DownloadAudioClip : AUnityDownload
    {
        public DownloadAudioClip(Scene scene, Download download) : base(scene, download)
        {
        }

        public UniTask<AudioClip> StartDownload(string url, AudioType audioType, bool monitor, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<AudioClip>.Create();
            var unityWebRequestAsyncOperation = Start(UnityWebRequestMultimedia.GetAudioClip(Uri.EscapeUriString(url), audioType), monitor);
            
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
                        var audioClip = DownloadHandlerAudioClip.GetContent(UnityWebRequest);
                        task.TrySetResult(audioClip);
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
