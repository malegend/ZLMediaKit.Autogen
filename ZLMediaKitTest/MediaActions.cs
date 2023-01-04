using System;
using System.Collections.Generic;
using System.Text;
using JPN = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace ZLMediaKitTest
{

    /// <summary>
    /// 客户端和服务器网络信息
    /// </summary>
    public class MediaSock
    {
        ///<summary></summary>
        [JPN("identifier")]
        public string Identifier { get; set; }
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
    /// 音视频轨道
    /// </summary>
    public class MediaTrack
    {
        ///<summary> 音频通道数</summary>
        [JPN("channels")]
        public int Channels { get; set; }
        ///<summary> H264 = 0, H265 = 1, AAC = 2, G711A = 3, G711U = 4</summary>
        [JPN("codec_id")]
        public int Codec { get; set; }
        ///<summary> 编码类型名称 </summary>
        [JPN("codec_id_name")]
        public string CodecName { get; set; }
        ///<summary> Video = 0, Audio = 1</summary>
        [JPN("codec_type")]
        public int CodecType { get; set; }
        ///<summary> 轨道是否准备就绪</summary>
        [JPN("ready")]
        public bool Ready { get; set; }
        ///<summary> 音频采样位数</summary>
        [JPN("sample_bit")]
        public int? SampleBit { get; set; }
        ///<summary> 音频采样率</summary>
        [JPN("sample_rate")]
        public int? SampleRate { get; set; }

        ///<summary> 视频 Fps</summary>
        [JPN("fps")]
        public int? Fps { get; set; }
        ///<summary> 视频宽</summary>
        [JPN("width")]
        public int? Width { get; set; }
        ///<summary> 视频高</summary>
        [JPN("height")]
        public int? Height { get; set; }
    }

    /// <summary>
    /// 媒体在线状态返回值，参见<see cref="Api.IsMediaOnline(string, string, string, string)"
    /// </summary>
    public class MediaOnlineResponse : ApiResponse
    {
        /// <summary>
        /// 是否在线/>
        /// </summary>
        [JPN("online")]
        public bool Online { get; set; }

        ///<summary> 本协议观看人数</summary>
        [JPN("readerCount")]
        public int ReaderCount { get; set; }
        ///<summary> 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv</summary>
        [JPN("totalReaderCount")]
        public int TotalReaderCount { get; set; }
        ///<summary> 音视频轨道</summary>
        [JPN("tracks")]
        public MediaTrack[] Tracks { get; set; }
    }

    /// <summary>
    /// 媒体信息
    /// </summary>
    public class MediaInformation
    {
        ///<summary> 应用名</summary>
        [JPN("app")]
        public string App { get; set; }
        ///<summary> 是否在线</summary>
        [JPN("online")]
        public bool? Online { get; set; }

        ///<summary> 本协议观看人数</summary>
        [JPN("readerCount")]
        public int ReaderCount { get; set; }
        ///<summary> 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv</summary>
        [JPN("totalReaderCount")]
        public int TotalReaderCount { get; set; }
        ///<summary> 协议</summary>
        [JPN("schema")]
        public string Schema { get; set; }
        ///<summary> 流id, 如 test</summary>
        [JPN("stream")]
        public string Stream { get; set; }
        ///<summary> 客户端和服务器网络信息，可能为null类型</summary>
        [JPN("originSock")]
        public MediaSock[] OriginSock { get; set; }
        ///<summary>
        ///产生源类型，包括:
        ///unknown = 0,
        ///rtmp_push=1,
        ///rtsp_push=2,
        ///rtp_push=3,
        ///pull=4,
        ///ffmpeg_pull=5,
        ///mp4_vod=6,
        ///device_chn=7
        ///</summary>
        [JPN("originType")]
        public int OriginType { get; set; }
        ///<summary></summary>
        [JPN("originTypeStr")]
        public string OriginTypeText { get; set; }
        ///<summary>产生源的url</summary>
        [JPN("originUrl")]
        public string OriginUrl { get; set; }
        ///<summary>GMT unix系统时间戳，单位秒</summary>
        [JPN("createStamp")]
        public int CreateStamp { get; set; }
        ///<summary>存活时间，单位秒</summary>
        [JPN("aliveSecond")]
        public int AliveSecond { get; set; }
        ///<summary>数据产生速度，单位byte/s</summary>
        [JPN("bytesSpeed")]
        public int BytesSpeed { get; set; }
        ///<summary> 音视频轨道</summary>
        [JPN("tracks")]
        public MediaTrack[] Tracks { get; set; }
        ///<summary> 虚拟主机名</summary>
        [JPN("vhost")]
        public string Host { get; set; }
    }

    /// <summary>
    /// 搜索文件列表的数据回应
    /// </summary>
    /// <remarks>
    /// 示例如下: <br />
    /// # 搜索文件夹列表(按照前缀匹配规则)：period = 2020-01<br />
    /// {<br />
    ///   "paths" : [ "2020-01-25", "2020-01-24" ],<br />
    ///   "rootPath" : "/www/record/live/ss/"<br />
    /// }
    /// # 搜索mp4文件列表：period = 2020-01-24<br />
    /// {<br />
    ///   "paths" : [<br />
    ///      "22-20-30.mp4",<br />
    ///      "22-13-12.mp4",<br />
    ///      "21-57-07.mp4",<br />
    ///      "21-19-18.mp4",<br />
    ///      "21-24-21.mp4",<br />
    ///      "21-15-10.mp4",<br />
    ///      "22-14-14.mp4"<br />
    ///   ],<br />
    ///   "rootPath" : "/www/live/ss/2020-01-24/"<br />
    /// }
    /// </remarks>
    public class Mp4RecordFileData
    {
        public string[] Paths { get; set; }
        public string RootPath { get; set; }
    }
}
