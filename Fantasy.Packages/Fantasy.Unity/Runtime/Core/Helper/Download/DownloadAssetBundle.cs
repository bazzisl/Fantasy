#if FANTASY_UNITY
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasy.Unity.Download
{
    public sealed class DownloadAssetBundle : AUnityDownload
    {
        public DownloadAssetBundle(Scene scene, Download download) : base(scene, download)
        {
        }

        public UniTask<AssetBundle> StartDownload(string url, bool monitor, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<AssetBundle>.Create();
            var unityWebRequestAsyncOperation = Start(UnityWebRequestAssetBundle.GetAssetBundle(Uri.EscapeUriString(url)), monitor);
            
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
                        var assetBundle = DownloadHandlerAssetBundle.GetContent(UnityWebRequest);
                        task.TrySetResult(assetBundle);
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