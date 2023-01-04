using System;
using System.Collections.Generic;
using System.Text;
using JPN = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace ZLMediaKitTest
{
    public class StreamKeyData
    {
        [JPN("key")]
        public string Key { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    public class FlagData
    {
        /// <summary>
        /// 成功或失败
        /// </summary>
        [JPN("flag")]
        public bool Value { get; set; }
    }

    /// <summary>
    /// ssrc rtp信息
    /// </summary>
    public class RtpInfoResponse : ApiResponse
    {
        ///<summary>是否存在</summary>
        [JPN("exist")]
        public bool Exist { get; set; }
        ///<summary></summary>
        [JPN("local_ip")]
        public string LocalIp { get; set; }
        ///<summary></summary>
        [JPN("local_port")]
        public int LocalPort { get; set; }
        ///<summary></summary>
        [JPN("peer_ip")]
        public string PeerIp { get; set; }
        ///<summary></summary>
        [JPN("peer_port")]
        public int PeerPort { get; set; }
    }

    /// <summary>
    /// 创建 GB28181 RTP 接收端口返回值
    /// </summary>
    public class OpenRtpResult:ApiResponse
    {
        ///<summary>接收端口，方便获取随机端口号</summary>
        [JPN("port")]
        public int Port { get; set; }
    }

    /// <summary>
    /// 关闭 GB28181 RTP 接收端口返回值
    /// </summary>
    public class CloseRtpResponse : ApiResponse
    {
        ///<summary>是否找到记录并关闭</summary>
        [JPN("hit")]
        public int Hit { get; set; }
    }

    /// <summary>
    /// 启动 GB28181 RTP 推流返回值
    /// </summary>
    public class SendRtpResponse : ApiResponse
    {
        ///<summary>使用的本地端口号</summary>
        [JPN("local_port")]
        public int LocalPort { get; set; }
    }

    /// <summary>
    /// 获取openRtpServer接口创建的所有RTP服务器返回值
    /// </summary>
    public class ListRtpData
    {
        ///<summary>绑定的端口号</summary>
        [JPN("port")]
        public int Port { get; set; }

        ///<summary>绑定的流ID</summary>
        [JPN("stream_id")]
        public string Stream { get; set; }
    }
}
