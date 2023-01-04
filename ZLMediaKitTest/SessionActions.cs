using System;
using System.Collections.Generic;
using System.Text;
using JPN = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace ZLMediaKitTest
{
    /// <summary>
    /// 客户端会话
    /// </summary>
    public class Session
    {
        /// <summary>该tcp链接唯一id</summary>
        [JPN("id")] public string Id { get; set; }
        /// <summary>本机网卡ip</summary>
        [JPN("local_ip")] public string LocalIp { get; set; }
        /// <summary>本机端口号</summary>
        [JPN("local_port")] public string LocalPort { get; set; }
        /// <summary>客户端ip</summary>
        [JPN("peer_ip")] public string PeerIp { get; set; }
        /// <summary>客户端端口号</summary>
        [JPN("peer_port")] public string PeerPort { get; set; }
        /// <summary>客户端TCPSession typeid</summary>
        [JPN("typeid")] public string TypeId { get; set; }
    }
}
