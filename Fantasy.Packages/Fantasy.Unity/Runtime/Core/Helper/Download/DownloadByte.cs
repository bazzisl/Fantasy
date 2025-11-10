#if FANTASY_UNITY
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine.Networking;

namespace Fantasy.Unity.Download
{
    public sealed class DownloadByte : AUnityDownload
    {
        public DownloadByte(Scene scene, Download download) : base(scene, download)
        {
        }

        public UniTask<byte[]> StartDownload(string url, bool monitor, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<byte[]>.Create();
            var unityWebRequestAsyncOperation = Start(UnityWebRequest.Get(url), monitor);

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
                        var bytes = UnityWebRequest.downloadHandler.data;
                        task.TrySetResult(bytes);
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