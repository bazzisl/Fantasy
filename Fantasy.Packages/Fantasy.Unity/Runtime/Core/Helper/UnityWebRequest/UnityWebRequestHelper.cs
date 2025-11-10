#if FANTASY_UNITY
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasy.Unity
{
    /// <summary>
    /// UnityWebRequest的帮助类
    /// </summary>
    public static class UnityWebRequestHelper
    {
        /// <summary>
        /// 获取一个文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<string> GetText(string url, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<string>.Create();
            var unityWebRequest = UnityWebRequest.Get(url);
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var text = unityWebRequest.downloadHandler.text;
                    task.TrySetResult(text);
                }
                else
                {
                    Log.Error(unityWebRequest.error);
                    task.TrySetResult(null);
                }
            };

            return task.Task;
        }
        
        /// <summary>
        /// 获取一个Sprite
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<Sprite> GetSprite(string url, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<Sprite>.Create();
            var unityWebRequest = UnityWebRequestTexture.GetTexture(Uri.EscapeUriString(url));
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(unityWebRequest);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 5, 1f);
                    task.TrySetResult(sprite);
                }
                else
                {
                    Log.Error(unityWebRequest.error);
                    task.TrySetResult(null);
                }
            };

            return task.Task;
        }

        /// <summary>
        /// 获取一个Texture
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<Texture> GetTexture(string url, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<Texture>.Create();
            var unityWebRequest = UnityWebRequestTexture.GetTexture(Uri.EscapeUriString(url));
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(unityWebRequest);
                    task.TrySetResult(texture);
                }
                else
                {
                    Log.Error(unityWebRequest.error);
                    task.TrySetResult(null);
                }
            };

            return task.Task;
        }
        
        /// <summary>
        /// 获取Bytes
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<byte[]> GetBytes(string url, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<byte[]>.Create();
            var unityWebRequest = UnityWebRequest.Get(url);
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var bytes = unityWebRequest.downloadHandler.data;
                    task.TrySetResult(bytes);
                }
                else
                {
                    Log.Error(unityWebRequest.error);
                    task.TrySetResult(null);
                }
            };

            return task.Task;
        }
        
        /// <summary>
        /// 获取AssetBundle
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<AssetBundle> GetAssetBundle(string url, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<AssetBundle>.Create();
            var unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(Uri.EscapeUriString(url));
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }

            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var assetBundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
                    task.TrySetResult(assetBundle);
                    return;
                }

                Log.Error(unityWebRequest.error);
                task.TrySetResult(null);
            };

            return task.Task;
        }

        /// <summary>
        /// 获取AudioClip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="audioType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static UniTask<AudioClip> GetAudioClip(string url, AudioType audioType, CancellationTokenSource cancellationToken = null)
        {
            var task = AutoResetUniTaskCompletionSourcePlus<AudioClip>.Create();
            var unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(Uri.EscapeUriString(url), audioType);
            var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
            
            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(() =>
                {
                    unityWebRequest.Abort();
                    task.TrySetResult(null);
                });
            }
            
            unityWebRequestAsyncOperation.completed += operation =>
            {
                if (unityWebRequest.result == UnityWebRequest.Result.Success)
                {
                    var audioClip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                    task.TrySetResult(audioClip);
                }
                else
                {
                    Log.Error(unityWebRequest.error);
                    task.TrySetResult(null);
                }
            };

            return task.Task;
        }
    }
}
#endif