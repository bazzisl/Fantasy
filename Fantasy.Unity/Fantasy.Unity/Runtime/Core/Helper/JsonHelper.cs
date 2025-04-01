using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#pragma warning disable CS8603

namespace Fantasy.Helper
{
    /// <summary>
    /// 提供操作 JSON 数据的辅助方法。
    /// </summary>
    public static partial class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为 JSON 字符串。
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型。</typeparam>
        /// <param name="t">要序列化的对象。</param>
        /// <returns>表示序列化对象的 JSON 字符串。</returns>
        public static string ToJson<T>(this T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 反序列化 JSON 字符串为指定类型的对象。
        /// </summary>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <param name="type">目标对象的类型。</param>
        /// <param name="reflection">是否使用反射进行反序列化（默认为 true）。</param>
        /// <returns>反序列化后的对象。</returns>
        public static object Deserialize(this string json, Type type, bool reflection = true)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 反序列化 JSON 字符串为指定类型的对象。
        /// </summary>
        /// <typeparam name="T">目标对象的类型。</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 克隆对象，通过将对象序列化为 JSON，然后再进行反序列化。
        /// </summary>
        /// <typeparam name="T">要克隆的对象类型。</typeparam>
        /// <param name="t">要克隆的对象。</param>
        /// <returns>克隆后的对象。</returns>
        public static T Clone<T>(T t)
        {
            return t.ToJson().Deserialize<T>();
        }

        /// <summary>
        /// 压缩JSON字符串，移除不必要的空白字符。
        /// </summary>
        /// <param name="json">要压缩的JSON字符串。</param>
        /// <returns>压缩后的JSON字符串。</returns>
        public static string CompressJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return json;
            }

            try
            {
                var obj = JToken.Parse(json);
                return obj.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return json; // 如果解析失败，返回原始字符串
            }
        }

        /// <summary>
        /// 美化JSON字符串，添加适当的缩进和换行。
        /// </summary>
        /// <param name="json">要美化的JSON字符串。</param>
        /// <returns>美化后的JSON字符串。</returns>
        public static string PrettyJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return json;
            }

            try
            {
                var obj = JToken.Parse(json);
                return obj.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch
            {
                return json; // 如果解析失败，返回原始字符串
            }
        }

        /// <summary>
        /// 将JSON字符串转换为二进制数据。
        /// </summary>
        /// <param name="json">要转换的JSON字符串。</param>
        /// <returns>转换后的二进制数据。</returns>
        public static byte[] JsonToBinary(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return Array.Empty<byte>();
            }

            // 先压缩JSON字符串
            string compressedJson = CompressJson(json);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(compressedJson);

            // 使用GZip进一步压缩
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzip.Write(jsonBytes, 0, jsonBytes.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将二进制数据转换回JSON字符串。
        /// </summary>
        /// <param name="binaryData">二进制数据。</param>
        /// <returns>转换后的JSON字符串。</returns>
        public static string BinaryToJson(byte[] binaryData)
        {
            if (binaryData == null || binaryData.Length == 0)
            {
                return string.Empty;
            }

            using (MemoryStream ms = new MemoryStream(binaryData))
            {
                using (MemoryStream decompressedMs = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(decompressedMs);
                    }
                    return Encoding.UTF8.GetString(decompressedMs.ToArray());
                }
            }
        }

        /// <summary>
        /// 将对象直接序列化为二进制数据。
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型。</typeparam>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化后的二进制数据。</returns>
        public static byte[] ToBinary<T>(T obj)
        {
            if (obj == null)
            {
                return Array.Empty<byte>();
            }
            
            string json = ToJson(obj);
            return JsonToBinary(json);
        }

        /// <summary>
        /// 从二进制数据反序列化为指定类型的对象。
        /// </summary>
        /// <typeparam name="T">目标对象的类型。</typeparam>
        /// <param name="binaryData">二进制数据。</param>
        /// <returns>反序列化后的对象。</returns>
        public static T FromBinary<T>(byte[] binaryData)
        {
            if (binaryData == null || binaryData.Length == 0)
            {
                return default;
            }
            
            string json = BinaryToJson(binaryData);
            return Deserialize<T>(json);
        }
    }
}