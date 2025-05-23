using System;
using Cysharp.Threading.Tasks;
using Fantasy.Async;
using Fantasy.Helper;
using Fantasy.Network.Interface;

#pragma warning disable CS8625
#pragma warning disable CS8618

namespace Fantasy.Scheduler
{
    /// <summary>
    /// 网络消息发送者的类。
    /// </summary>
    public struct MessageSender : IDisposable
    {
        /// <summary>
        /// 获取或设置 RPC ID。
        /// </summary>
        public uint RpcId { get; private set; }
        /// <summary>
        /// 获取或设置路由 ID。
        /// </summary>
        public long RouteId { get; private set; }
        /// <summary>
        /// 获取或设置创建时间。
        /// </summary>
        public long CreateTime { get; private set; }
        /// <summary>
        /// 获取或设置消息类型。
        /// </summary>
        public Type MessageType { get; private set; }
        /// <summary>
        /// 获取或设置请求消息。
        /// </summary>
        public IMessage Request { get; private set; }
        /// <summary>
        /// 获取或设置任务。
        /// </summary>
        public AutoResetUniTaskCompletionSourcePlus<IResponse> Tcs { get; private set; }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            RpcId = 0;
            RouteId = 0;
            CreateTime = 0;
            Tcs = null;
            Request = null;
            MessageType = null;
        }

        /// <summary>
        /// 创建一个 <see cref="MessageSender"/> 实例。
        /// </summary>
        /// <param name="rpcId">RPC ID。</param>
        /// <param name="requestType">请求消息类型。</param>
        /// <param name="tcs">任务。</param>
        /// <returns>创建的 <see cref="MessageSender"/> 实例。</returns>
        public static MessageSender Create(uint rpcId, Type requestType, AutoResetUniTaskCompletionSourcePlus<IResponse> tcs)
        {
            var routeMessageSender = new MessageSender();
            routeMessageSender.Tcs = tcs;
            routeMessageSender.RpcId = rpcId;
            routeMessageSender.MessageType = requestType;
            routeMessageSender.CreateTime = TimeHelper.Now;
            return routeMessageSender;
        }

        /// <summary>
        /// 创建一个 <see cref="MessageSender"/> 实例。
        /// </summary>
        /// <param name="rpcId">RPC ID。</param>
        /// <param name="request">请求消息。</param>
        /// <param name="tcs">任务。</param>
        /// <returns>创建的 <see cref="MessageSender"/> 实例。</returns>
        public static MessageSender Create(uint rpcId, IRequest request, AutoResetUniTaskCompletionSourcePlus<IResponse> tcs)
        {
            var routeMessageSender = new MessageSender();
            routeMessageSender.Tcs = tcs;
            routeMessageSender.RpcId = rpcId;
            routeMessageSender.Request = request;
            routeMessageSender.CreateTime = TimeHelper.Now;
            return routeMessageSender;
        }

        /// <summary>
        /// 创建一个 <see cref="MessageSender"/> 实例。
        /// </summary>
        /// <param name="rpcId">RPC ID。</param>
        /// <param name="routeId">路由 ID。</param>
        /// <param name="request">路由消息请求。</param>
        /// <param name="tcs">任务。</param>
        /// <returns>创建的 <see cref="MessageSender"/> 实例。</returns>
        public static MessageSender Create(uint rpcId, long routeId, IRouteMessage request, AutoResetUniTaskCompletionSourcePlus<IResponse> tcs)
        {
            var routeMessageSender = new MessageSender();
            routeMessageSender.Tcs = tcs;
            routeMessageSender.RpcId = rpcId;
            routeMessageSender.RouteId = routeId;
            routeMessageSender.Request = request;
            routeMessageSender.CreateTime = TimeHelper.Now;
            return routeMessageSender;
        }
    }
}