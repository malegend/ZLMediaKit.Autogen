using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.IO;
using JPN = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace ZLMediaKitTest
{
    internal class Api
    {
        public string ServerAddress { get; set; }
        public string Secret { get; set; }

        #region ApiCall
        #region FFMpeg
        /// <summary>
        /// 通过fork FFmpeg进程的方式拉流代理，支持任意协议
        /// </summary>
        /// <param name="srcUrl">FFmpeg拉流地址,支持任意协议或格式(只要FFmpeg支持即可)</param>
        /// <param name="dstUrl">FFmpeg rtmp推流地址，一般都是推给自己，例如rtmp://127.0.0.1/live/stream_form_ffmpeg</param>
        /// <param name="timeout">FFmpeg推流成功超时时间</param>
        /// <param name="enableHls">是否开启hls录制, 可选</param>
        /// <param name="enableMp4">是否开启mp4录制, 可选</param>
        /// <param name="ffmpegCmdKey">配置文件中FFmpeg命令参数模板key(非内容)，置空则采用默认模板:ffmpeg.cmd, 可选</param>
        public ApiResponse AddFFmpegSource(
            string srcUrl, string dstUrl, int timeout,
            bool enableHls, bool enableMp4,
            string ffmpegCmdKey = default)
        {
            return ApiQuery<ApiResponse<StreamKeyData>>(
                ApiCommands.AddFFmpegSource,
                ("src_url", srcUrl),
                ("dst_url", dstUrl),
                ("timeout_sec", timeout),
                ("enable_hls", enableHls),
                ("enable_mp4", enableMp4),
                ("ffmpeg_cmd_key", ffmpegCmdKey)
                );
        }

        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <remarks>
        /// 流注册成功后，也可以使用 <see cref="CloseStreams"/> 接口替代
        /// </remarks>
        public ApiResponse DelFFmpegSource(string streamKey)
        {
            return ApiQuery<ApiResponse<FlagData>>(ApiCommands.DelFFmpegSource, ("key", streamKey));
        }


        #endregion

        #region Stream、Proxy
        /// <summary>
        /// 动态添加rtsp/rtmp/hls拉流代理(只支持H264/H265/aac/G711负载)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp，必选</param>
        /// <param name="host">虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="url">拉流地址，例如rtmp://live.hkstv.hk.lxdns.com/live/hks2</param>
        /// <param name="retryCount">拉流重试次数，默认为-1无限重试</param>
        /// <param name="rtpType">rtsp拉流时，拉流方式，0：tcp，1：udp，2：组播	</param>
        /// <param name="timeout">拉流超时时间，单位秒，float类型</param>
        /// <param name="enableHls">是否转换成hls协议, 可选</param>
        /// <param name="enableMp4">是否允许mp4录制, 可选</param>
        /// <param name="enableRtsp">是否转rtsp协议, 可选</param>
        /// <param name="enableRtmp">是否转rtmp/flv协议, 可选</param>
        /// <param name="enableTs">是否转http-ts/ws-ts协议, 可选</param>
        /// <param name="enableFMp4">是否转http-fmp4/ws-fmp4协议, 可选</param>
        /// <param name="enableAudio">转协议时是否开启音频, 可选</param>
        /// <param name="addMuteAudio">转协议时，无音频是否添加静音aac音频, 可选</param>
        /// <param name="mp4SavePath">mp4录制文件保存根目录，置空使用默认, 可选</param>
        /// <param name="mp4SliceTime">mp4录制切片大小，单位秒, 可选</param>
        /// <param name="hlsSavePath">hls文件保存保存根目录，置空使用默认, 可选</param>
        public ApiResponse<StreamKeyData> AddStreamProxy(
            string schema, string host, string app, string stream, string url,
            int? retryCount = -1,
            int? rtpType = default,
            int? timeout = default,
            bool? enableHls = default,
            bool? enableMp4 = default,
            bool? enableRtsp = default,
            bool? enableRtmp = default,
            bool? enableTs = default,
            bool? enableFMp4 = default,
            bool? enableAudio = default,
            bool? addMuteAudio = default,
            string mp4SavePath = default,
            int? mp4SliceTime = default,
            string hlsSavePath = default
            )
        {
            return ApiQuery<ApiResponse<StreamKeyData>>(
                ApiCommands.AddStreamProxy,
                ("schema", schema),
                ("vhost", host),
                ("app", app),
                ("stream", stream),
                ("url", url),
                ("retry_count", retryCount),
                ("rtp_type", rtpType),
                ("timeout_sec", timeout),
                ("enable_hls", enableHls),
                ("enable_mp4", enableMp4),
                ("enable_rtsp", enableRtsp),
                ("enable_rtmp", enableRtmp),
                ("enable_ts", enableTs),
                ("enable_fmp4", enableFMp4),
                ("enable_audio", enableAudio),
                ("add_mute_audio", addMuteAudio),
                ("mp4_save_path", mp4SavePath),
                ("mp4_max_second", mp4SliceTime),
                ("hls_save_path", hlsSavePath)
                );
        }

        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <remarks>
        /// 流注册成功后，也可以使用 <see cref="CloseStreams"/> 接口替代
        /// </remarks>
        public ApiResponse<FlagData> DelStreamProxy(string streamKey)
        {
            return ApiQuery<ApiResponse<FlagData>>(ApiCommands.DelStreamProxy, ("key", streamKey));
        }

        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp，必选</param>
        /// <param name="host">虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)，可选</param>
        /// <returns>Result= 0:成功，-1:关闭失败，-2:该流不存在</returns>
        /// <remarks>
        /// 已过期，请使用 <see cref="CloseStream"/> 接口替换。
        /// </remarks>
        [Obsolete("已过期，请使用 CloseStream 接口替换")]
        public ResultResponse<int> CloseStream(string schema, string host, string app, string stream, bool? force = default)
        {
            return ApiQuery<ResultResponse<int>>(
                ApiCommands.CloseStream,
                ("schema", schema),
                ("vhost", host),
                ("app", app),
                ("stream", stream),
                ("force", force)
                );
        }

        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="host">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns>Result= 0:成功，-1:关闭失败，-2:该流不存在</returns>
        /// <remarks>各参数均为可选</remarks>
        public ApiResponse CloseStreams(string schema = default, string host = default, string app = default, string stream = default, bool? force = default)
        {
            return ApiQuery(
                ApiCommands.CloseStreams,
                ("schema", schema),
                ("vhost", host),
                ("app", app),
                ("stream", stream),
                ("force", force)
                );
        }

        /// <summary>
        /// 添加rtsp/rtmp主动推流(把本服务器的直播流推送到其他服务器去)
        /// </summary>
        /// <param name="host">添加的流的虚拟主机，例如__defaultVhost__</param>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="app">添加的流的应用名，例如live</param>
        /// <param name="stream">需要转推的流id</param>
        /// <param name="dstUrl">目标转推url，带参数需要自行url转义</param>
        /// <param name="retryCount">转推失败重试次数，默认无限重试</param>
        /// <param name="rtpType">rtsp拉流时，拉流方式，0：tcp，1：udp，2：组播</param>
        /// <param name="timeout">拉流超时时间，单位秒，float类型</param>
        public ApiResponse AddStreamPusherProxy(
            string host, string schema, string app,
            string stream, string dstUrl,
            int? retryCount = default,
            int? rtpType = default,
            int? timeout = default
            )
        {
            return ApiQuery<ApiResponse<StreamKeyData>>(
                ApiCommands.AddStreamPusherProxy,
                ("vhost", host),
                ("schema", schema),
                ("app", app),
                ("stream", stream),
                ("dst_url", dstUrl),
                ("retry_count", retryCount),
                ("rtp_type", rtpType),
                ("timeout_sec", timeout));
        }

        /// <summary>
        /// 关闭推流
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ApiResponse<FlagData> DelStreamPusherProxy(string key)
        {
            return ApiQuery<ApiResponse<FlagData>>(ApiCommands.DelStreamPusherProxy, ("key", key));
        }

        #endregion

        #region Sessions

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="sessionId">客户端唯一id，可以通过getAllSession接口获取</param>
        public ApiResponse KickSession(string sessionId)
        {
            return ApiQuery(ApiCommands.KickSession, ("id", sessionId));
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
        /// </summary>
        /// <param name="localPort">本机端口，例如筛选rtsp链接：554, 可选</param>
        /// <param name="peerIp">客户端ip, 可选</param>
        public ApiResponse KickSessions(string localPort = default, string peerIp = default)
        {
            return ApiQuery(ApiCommands.KickSessions, ("local_port", localPort), ("peer_ip", peerIp));
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="localPort">本机端口，例如筛选rtsp链接：554, 可选</param>
        /// <param name="peerIp">客户端ip, 可选</param>
        public ApiResponse GetAllSession(string localPort = default, string peerIp = default)
        {
            return ApiQuery<ApiResponse<Session[]>>(ApiCommands.GetAllSession, ("local_port", localPort), ("peer_ip", peerIp));
        }


        #endregion

        #region Server Control
        /// <summary>
        /// 获取API列表
        /// </summary>
        public ApiResponse<string[]> GetApiList()
        {
            return ApiQuery<ApiResponse<string[]>>(ApiCommands.GetApiList);
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        public ApiResponse<Dictionary<string, string>> GetServerConfig()
        {
            return ApiQuery<ApiResponse<Dictionary<string, string>>>(ApiCommands.GetServerConfig);
        }

        /// <summary>
        /// 设置服务器配置
        /// </summary>
        public ApiChangedResponse SetServerConfig(params (string Key, string Value)[] settings)
        {
            var mt = settings.Select(p => (p.Key, (object)p.Value)).ToArray();
            return ApiQuery<ApiChangedResponse>(ApiCommands.SetServerConfig, mt);
        }

        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        public ApiResponse GetThreadsLoad()
        {
            return ApiQuery(ApiCommands.GetThreadsLoad);
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        public ApiResponse GetWorkThreadsLoad()
        {
            return ApiQuery(ApiCommands.GetWorkThreadsLoad);
        }


        /// <summary>
        /// 重启服务器,只有Daemon方式才能重启，否则是直接关闭
        /// </summary>
        public ApiResponse RestartServer()
        {
            return ApiQuery(ApiCommands.RestartServer);
        }

        /// <summary>
        /// 获取主要对象个数统计，主要用于分析内存性能
        /// </summary>
        public ApiResponse<StatisticData> GetStatistic()
        {
            return ApiQuery<ApiResponse<StatisticData>>(ApiCommands.GetStatistic);
        }

        #endregion

        #region Media

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="host">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <param name="stream">筛选流id，例如 test</param>
        public ApiResponse<MediaInformation[]> GetMediaList(string schema = default, string host = default, string app = default, string stream = default)
        {
            return ApiQuery<ApiResponse<MediaInformation[]>>(ApiCommands.GetMediaList, ("schema", schema), ("vhost", host), ("app", app), ("stream", stream));
        }

        /// <summary>
        /// 判断直播流是否在线
        /// </summary>
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp，必选</param>
        /// <param name="host">虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <remarks>
        /// 已过期，请使用 <see cref="GetMediaList"/> 接口替代
        /// </remarks>
        [Obsolete("请使用 GetMediaList 接口替代")]
        public MediaOnlineResponse IsMediaOnline(string schema, string host, string app, string stream)
        {
            return ApiQuery<MediaOnlineResponse>(
                ApiCommands.IsMediaOnline,
                ("schema", schema),
                ("vhost", host),
                ("app", app),
                ("stream", stream)
                );
        }

        /// <summary>
        /// 获取流相关信息
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp，必选</param>
        /// <param name="host">虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// 已过期，请使用 <see cref="GetMediaList"/> 接口替代
        /// </remarks>
        [Obsolete("请使用 GetMediaList 接口替代")]
        public ApiResponse GetMediaInfo(string schema, string host, string app, string stream)
        {
            return ApiQuery(
                ApiCommands.GetMediaInfo,
                ("schema", schema),
                ("vhost", host),
                ("app", app),
                ("stream", stream)
                );
        }


        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="rtpSSRC">
        /// RTP的ssrc，16进制字符串或者是流的id(openRtpServer接口指定)
        /// </param>
        public ApiResponse GetRtpInfo(string rtpSSRC)
        {
            return ApiQuery(ApiCommands.GetRtpInfo, ("stream_id", rtpSSRC));
        }


        #endregion

        #region Record

        /// <summary>
        /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
        /// </summary>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="period">
        /// 流的录像日期，格式为2020-02-01,如果不是完整的日期，那么是
        /// 搜索录像文件夹列表，否则搜索对应日期下的mp4文件列表。
        /// </param>
        /// <param name="customizedPath">
        /// 自定义搜索路径，与startRecord方法中的customized_path一样，
        /// 默认为配置文件的路径, 可选。
        /// </param>
        public ApiResponse<Mp4RecordFileData> GetMp4RecordFile(
            string host, string app, string stream,
            string period, string customizedPath = default)
        {
            return ApiQuery<ApiResponse<Mp4RecordFileData>>(
                ApiCommands.GetMp4RecordFile,
                ("vhost", host),
                ("app", app),
                ("stream", stream),
                ("period", period),
                ("customized_path", customizedPath)
                );
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="mp4SliceTime">mp4录像切片时间大小,单位秒，置0则采用配置项</param>
        /// <param name="customizedPath">
        /// 自定义搜索路径，与startRecord方法中的customized_path一样，
        /// 默认为配置文件的路径, 可选。
        /// </param>
        public ResultResponse<bool> StartRecord(
            RecordFileType type, string host, string app, string stream,
            string customizedPath = default, int mp4SliceTime = 0)
        {
            return ApiQuery<ResultResponse<bool>>(
                ApiCommands.StartRecord,
                ("type", (int)type),
                ("vhost", host),
                ("app", app),
                ("stream", stream),
                ("max_second", mp4SliceTime),
                ("customized_path", customizedPath)
                );
        }

        /// <summary>
        /// 停止录制流
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        public ResultResponse<bool> StopRecord(RecordFileType type, string host, string app, string stream)
        {
            return ApiQuery<ResultResponse<bool>>(
                ApiCommands.StopRecord,
                ("type", (int)type),
                ("vhost", host),
                ("app", app),
                ("stream", stream)
                );
        }

        /// <summary>
        /// 获取流录制状态
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <remarks><seealso cref="GetRecordStatus"/></remarks>
        public StatusResponse IsRecording(RecordFileType type, string host, string app, string stream)
        {
            return ApiQuery<StatusResponse>(
                ApiCommands.IsRecording,
                ("type", (int)type),
                ("vhost", host),
                ("app", app),
                ("stream", stream)
                );
        }

        /// <summary>
        /// 获取流录制状态
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <remarks><seealso cref="IsRecording"/></remarks>
        public ApiResponse GetRecordStatus(RecordFileType type, string host, string app, string stream)
        {
            return ApiQuery<StatusResponse>(
                ApiCommands.GetRecordStatus,
                ("type", (int)type),
                ("vhost", host),
                ("app", app),
                ("stream", stream)
                );
        }

        /// <summary>
        /// 获取截图或生成实时截图并返回
        /// </summary>
        /// <param name="url">需要截图的url，可以是本机的，也可以是远程主机的</param>
        /// <param name="timeout">截图失败超时时间，防止FFmpeg一直等待截图, 单位秒</param>
        /// <param name="expire">截图的过期时间，该时间内产生的截图都会作为缓存返回, 单位秒</param>
        public ApiResponse GetSnap(string url, int timeout = 5, int expire = 5)
        {
            var ret = ApiQuery<ApiResponse>(
                ApiCommands.GetSnap,
                ("url", url),
                ("timeout_sec", timeout),
                ("expire_sec", expire)
                );
            if (ret.Code == ApiCode.JsonParseFailed && ret.Raw != null)
            {
                ret.Code = ApiCode.Success;
                ret.Message = default;
            }
            return ret;
        }

        #endregion

        #region Rtp、GB28181

        /// <summary>
        /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
        /// </summary>
        /// <param name="stream">该端口绑定的流ID，该端口只能创建这一个流(而不是根据ssrc创建多个)</param>
        /// <param name="enableTcp">启用UDP监听的同时是否监听TCP端口</param>
        /// <param name="port">接收端口，0则为随机端口</param>
        public OpenRtpResult OpenRtpServer(string stream, bool enableTcp, int port = 0)
        {
            return ApiQuery<OpenRtpResult>(
                    ApiCommands.OpenRtpServer,
                    ("port", port),
                    ("enable_tcp", enableTcp),
                    ("stream_id", stream)
                    );
        }

        /// <summary>
        /// 关闭GB28181 RTP接收端口
        /// </summary>
        public CloseRtpResponse CloseRtpServer(string stream)
        {
            return ApiQuery<CloseRtpResponse>(ApiCommands.CloseRtpServer, ("stream_id", stream));
        }

        /// <summary>
        /// 获取openRtpServer接口创建的所有RTP服务器
        /// </summary>
        public ApiResponse<ListRtpData[]> ListRtpServer()
        {
            return ApiQuery<ApiResponse<ListRtpData[]>>(ApiCommands.ListRtpServer);
        }

        /// <summary>
        /// 作为GB28181客户端，启动ps-rtp推流，支持rtp/udp方式；
        /// 该接口支持rtsp/rtmp等协议转ps-rtp推流。第一次推流失败
        /// 会直接返回错误，成功一次后，后续失败也将无限重试。
        /// </summary>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc">推流的rtp的ssrc,指定不同的ssrc可以同时推流到多个服务器</param>
        /// <param name="dstUrl">目标ip或域名</param>
        /// <param name="dstPort">目标端口</param>
        /// <param name="isUdp">是否为udp模式,否则为tcp模式</param>
        /// <param name="srcPort">使用的本机端口，为0或不传时默认为随机端口</param>
        /// <param name="pt">发送时，rtp的pt（uint8_t）,不传时默认为96</param>
        /// <param name="usePS">发送时，rtp的负载类型。为1时，负载为ps；为0时，为es；不传时默认为1</param>
        /// <param name="onlyAudio">当use_ps 为0时，有效。为1时，发送音频；为0时，发送视频；不传时默认为0</param>
        public SendRtpResponse StartSendRtp(
            string host, string app, string stream,
            string ssrc, string dstUrl, int dstPort, bool isUdp,
            int srcPort = 0,
            int? pt = default,
            bool? usePS = default,
            bool? onlyAudio = default
            )
        {
            return ApiQuery<SendRtpResponse>
                (
                ApiCommands.StartSendRtp,
                ("vhost", host), ("app", app),
                ("stream", stream), ("ssrc", ssrc),
                ("dst_url", dstUrl), ("dst_port", dstPort),
                ("is_udp", isUdp), ("src_port", srcPort), ("pt", pt),
                ("use_ps", usePS), ("only_audio", onlyAudio)
                );
        }

        /// <summary>
        /// 作为GB28181 Passive TCP服务器；该接口支持rtsp/rtmp等协议转ps-rtp被动推流。
        /// 调用该接口，zlm会启动tcp服务器等待连接请求，连接建立后，zlm会关闭tcp服务器，
        /// 然后源源不断的往客户端推流。第一次推流失败会直接返回错误，成功一次后，后续失败
        /// 也将无限重试(不停地建立tcp监听，超时后再关闭)。
        /// </summary>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc">推流的rtp的ssrc,指定不同的ssrc可以同时推流到多个服务器</param>
        /// <param name="srcPort">使用的本机端口，为0或不传时默认为随机端口</param>
        /// <param name="pt">发送时，rtp的pt（uint8_t）,不传时默认为96</param>
        /// <param name="usePS">发送时，rtp的负载类型。为1时，负载为ps；为0时，为es；不传时默认为1</param>
        /// <param name="onlyAudio">当use_ps 为0时，有效。为1时，发送音频；为0时，发送视频；不传时默认为0</param>
        public SendRtpResponse StartSendRtpPassive(
            string host, string app, string stream,
            string ssrc,
            int srcPort = 0,
            int? pt = default,
            bool? usePS = default,
            bool? onlyAudio = default
            )
        {
            return ApiQuery<SendRtpResponse>
                (
                ApiCommands.StartSendRtpPassive,
                ("vhost", host), ("app", app),
                ("stream", stream), ("ssrc", ssrc),
                ("src_port", srcPort), ("pt", pt),
                ("use_ps", usePS), ("only_audio", onlyAudio)
                );
        }

        /// <summary>
        /// 停止GB28181 ps-rtp推流
        /// </summary>
        /// <param name="host">流的虚拟主机，例如__defaultVhost__，必选</param>
        /// <param name="app">流的应用名，例如 live，必选</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="ssrc">根据ssrc关停某路rtp推流，置空时关闭所有流</param>
        public ApiResponse StopSendRtp(string host, string app, string stream, string ssrc = default)
        {
            return ApiQuery<ApiResponse>
                (
                ApiCommands.StopSendRtp,
                ("vhost", host), ("app", app),
                ("stream", stream), ("ssrc", ssrc)
                );
        }


        #endregion
        #endregion

        #region HttpQuery

        private ApiResponse ApiQuery((string Action, bool IsSecret) cmd, params (string Key, object Value)[] args)
            => ApiQuery<ApiResponse>(cmd, args);

        private T ApiQuery<T>((string Action, bool IsSecret) cmd, params (string Key, object Value)[] args) where T : ApiResponse, new()
        {
            string url = this.ServerAddress;
            if (url.EndsWith('/')) url = url[0..^1];
            url += cmd.Action;
            try
            {
                if (cmd.IsSecret)
                {
                    Array.Resize(ref args, args.Length + 1);
                    args[^1] = ("secret", this.Secret);
                }
                var (StatusCode, Message, Raw, Bytes) = GetHttpValue(url, args);
                if (StatusCode == 200)
                {
                    T ret;
                    try
                    {
                        ret = Raw.FromJson<T>();
                    }
                    catch (Exception)
                    {
                        ret = new T() { Code = ApiCode.JsonParseFailed, Message = "Json格式不正确或不符合预期实体对象。", Raw = Bytes };
                    }
                    return ret;
                }
                else
                    return new T() { Code = (ApiCode)StatusCode, Message = Message };
            }
            catch (WebException ex)
            {
                return new T() { Code = (ApiCode)ex.Status, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new T() { Code = ApiCode.ProgramError, Message = ex.Message };
            }
        }

        private ApiResponse<T> ApiQueryValue<T>((string Action, bool IsSecret) cmd, params (string Key, object Value)[] args) where T : class
        {
            string url = this.ServerAddress;
            if (url.EndsWith('/')) url = url[0..^1];
            url += cmd.Action;
            try
            {
                if (cmd.IsSecret)
                {
                    Array.Resize(ref args, args.Length + 1);
                    args[^1] = ("secret", this.Secret);
                }
                var (StatusCode, Message, Raw, Bytes) = GetHttpValue(url, args);
                if (StatusCode == 200)
                {
                    ApiResponse<T> ret;
                    try
                    {
                        ret = Raw.FromJson<ApiResponse<T>>();
                    }
                    catch (Exception)
                    {
                        ret = new ApiResponse<T>() { Code = ApiCode.JsonParseFailed, Message = "Json格式不正确或不符合预期实体对象。" };
                    }
                    return ret;
                }
                else
                    return new ApiResponse<T>() { Code = (ApiCode)StatusCode, Message = Message };
            }
            catch (WebException ex)
            {
                return new ApiResponse<T>() { Code = (ApiCode)ex.Status, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>() { Code = ApiCode.ProgramError, Message = ex.Message };
            }
        }

        private static (int StatusCode, string Message, string Raw, byte[] Bytes) GetHttpValue(string url, params (string Key, object Value)[] args) =>
            GetHttpValue(url, Encoding.UTF8, args);

        /// <summary>
        /// GET, 请求数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="args">
        /// Key: 参数名
        /// Value: 参数值
        /// </param>
        /// <returns></returns>
        private static (int StatusCode, string Message, string Raw, byte[] Bytes) GetHttpValue(string url, Encoding encoding, params (string Key, object Value)[] args)
        {
            if (args != null && args.Length > 0)
            {
                var param = string.Join(
                    "&",
                    args.Where(p => !string.IsNullOrWhiteSpace(p.Key) && p.Value != null)
                        .Select(p => $"{p.Key}={HttpUtility.UrlEncode(GetJsonValue(p.Value))}"));
                url += string.Concat((url.Contains('?') ? "&" : "?"), param);
            }

            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
            {

                using var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };
                handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;

                using var client = new HttpClient(handler);
                try
                {
                    var resp = client.GetAsync(uri).Result;
                    //var cont = resp.Headers.GetValues("Content-Type"];
                    if (resp.IsSuccessStatusCode)
                    {
                        var buff = resp.Content.ReadAsByteArrayAsync().Result;
                        var result = encoding.GetString(buff);
                        return (200, default, result, buff);
                    }
                    else
                    {
                        return ((int)resp.StatusCode, default, default, default);
                    }
                }
                catch (WebException ex)
                {
                    var cook = handler.CookieContainer.GetCookies(uri);
                    try
                    {
                        var rsp = (HttpWebResponse)ex.Response;
                        using var stmp = rsp.GetResponseStream();
                        using var reader = new StreamReader(stmp, encoding);
                        var result = reader.ReadToEnd(); // 懒惰处理法
                        var buff = encoding.GetBytes(result);
                        return ((int)rsp.StatusCode, default, result, buff);
                    }
                    catch (Exception wx)
                    {
                        return (9999, wx.Message, default, default);
                    }
                }
                catch (Exception ex)
                {
                    return (9999, ex.Message, default, default);
                }
            }
            else
            {
                return (9999, "Url格式不正确。", default, default);
            }
        }

        private static string GetJsonValue(object value)
        {
            if (value is string str)
                return str;
            else if (value is bool bval)
                return bval ? "1" : "0";
            else if (value is bool?)
            {
                var nbval = (bool?)value;
                return nbval.HasValue ? nbval.Value ? "1" : "0" : default;
            }
            else
                return value?.ToString();
        }

        private class QueryParam
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public bool IsCookie { get; set; }

            public QueryParam(string key, string value, bool isCookie = false)
            {
                this.Key = key;
                this.Value = value;
                this.IsCookie = isCookie;
            }
        }

        #endregion

        /// <summary>
        /// RestAPI 接口地址，全部支持 GET/POS 模式
        /// </summary>
        internal static class ApiCommands
        {
            #region FFMpeg
            /// <summary>
            /// 通过fork FFmpeg进程的方式拉流代理，支持任意协议
            /// </summary>
            public static readonly (string Action, bool IsSecret) AddFFmpegSource = ("/index/api/addFFmpegSource", true);
            /// <summary>
            /// 关闭ffmpeg拉流代理
            /// </summary>
            /// <remarks>
            /// 流注册成功后，也可以使用 <see cref="CloseStreams"/> 接口替代
            /// </remarks>
            public static readonly (string Action, bool IsSecret) DelFFmpegSource = ("/index/api/delFFmpegSource", true);
            #endregion

            #region Stream、Proxy
            /// <summary>
            /// 动态添加rtsp/rtmp/hls拉流代理(只支持H264/H265/aac/G711负载)
            /// </summary>
            public static readonly (string Action, bool IsSecret) AddStreamProxy = ("/index/api/addStreamProxy", true);
            /// <summary>
            /// 关闭拉流代理
            /// </summary>
            /// <remarks>
            /// 流注册成功后，也可以使用 <see cref="CloseStreams"/> 接口替代
            /// </remarks>
            public static readonly (string Action, bool IsSecret) DelStreamProxy = ("/index/api/delStreamProxy", true);

            /// <summary>
            /// 关闭流(目前所有类型的流都支持关闭)
            /// </summary>
            /// <remarks>
            /// 已过期，请使用 <see cref="CloseStream"/> 接口替换
            /// </remarks>
            [Obsolete("已过期，请使用 CloseStream 接口替换")]
            public static readonly (string Action, bool IsSecret) CloseStream = ("/index/api/close_stream", true);
            /// <summary>
            /// 关闭流(目前所有类型的流都支持关闭)
            /// </summary>
            public static readonly (string Action, bool IsSecret) CloseStreams = ("/index/api/close_streams", true);

            /// <summary>
            /// 添加rtsp/rtmp主动推流(把本服务器的直播流推送到其他服务器去)
            /// </summary>
            public static readonly (string Action, bool IsSecret) AddStreamPusherProxy = ("/index/api/addStreamPusherProxy", true);
            /// <summary>
            /// 关闭推流
            /// </summary>
            /// <remarks>
            /// </remarks>
            public static readonly (string Action, bool IsSecret) DelStreamPusherProxy = ("/index/api/delStreamPusherProxy", true);

            #endregion

            #region Sessions

            /// <summary>
            /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
            /// </summary>
            public static readonly (string Action, bool IsSecret) KickSession = ("/index/api/kick_session", true);
            /// <summary>
            /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等
            /// </summary>
            public static readonly (string Action, bool IsSecret) KickSessions = ("/index/api/kick_sessions", true);
            /// <summary>
            /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetAllSession = ("/index/api/getAllSession", true);

            #endregion

            #region Server Control
            /// <summary>
            /// 获取API列表
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetApiList = ("/index/api/getApiList", false);
            /// <summary>
            /// 获取服务器配置
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetServerConfig = ("/index/api/getServerConfig", true);
            /// <summary>
            /// 设置服务器配置
            /// </summary>
            public static readonly (string Action, bool IsSecret) SetServerConfig = ("/index/api/setServerConfig", true);
            /// <summary>
            /// 获取各epoll(或select)线程负载以及延时
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetThreadsLoad = ("/index/api/getThreadsLoad", false);
            /// <summary>
            /// 获取各后台epoll(或select)线程负载以及延时
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetWorkThreadsLoad = ("/index/api/getWorkThreadsLoad", false);

            /// <summary>
            /// 重启服务器,只有Daemon方式才能重启，否则是直接关闭
            /// </summary>
            public static readonly (string Action, bool IsSecret) RestartServer = ("/index/api/restartServer", true);
            /// <summary>
            /// 获取主要对象个数统计，主要用于分析内存性能
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetStatistic = ("/index/api/getStatistic", true);
            #endregion

            #region Media

            /// <summary>
            /// 获取流列表，可选筛选参数
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetMediaList = ("/index/api/getMediaList", true);
            /// <summary>
            /// 判断直播流是否在线
            /// </summary>
            /// <remarks>
            /// 已过期，请使用 <see cref="GetMediaList"/> 接口替代
            /// </remarks>
            [Obsolete("请使用 GetMediaList 接口替代")]
            public static readonly (string Action, bool IsSecret) IsMediaOnline = ("/index/api/isMediaOnline", true);
            /// <summary>
            /// 获取流相关信息
            /// </summary>
            /// 已过期，请使用 <see cref="GetMediaList"/> 接口替代
            /// </remarks>
            [Obsolete("请使用 GetMediaList 接口替代")]
            public static readonly (string Action, bool IsSecret) GetMediaInfo = ("/index/api/getMediaInfo", true);

            /// <summary>
            /// 获取rtp代理时的某路ssrc rtp信息
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetRtpInfo = ("/index/api/getRtpInfo", true);

            #endregion

            #region Record

            /// <summary>
            /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetMp4RecordFile = ("/index/api/getMp4RecordFile", true);

            /// <summary>
            /// 开始录制hls或MP4
            /// </summary>
            public static readonly (string Action, bool IsSecret) StartRecord = ("/index/api/startRecord", true);
            /// <summary>
            /// 停止录制流
            /// </summary>
            public static readonly (string Action, bool IsSecret) StopRecord = ("/index/api/stopRecord", true);
            /// <summary>
            /// 获取流录制状态
            /// </summary>
            /// <remarks><seealso cref="GetRecordStatus"/></remarks>
            public static readonly (string Action, bool IsSecret) IsRecording = ("/index/api/isRecording", true);
            /// <summary>
            /// 获取流录制状态
            /// </summary>
            /// <remarks><seealso cref="IsRecording"/></remarks>
            public static readonly (string Action, bool IsSecret) GetRecordStatus = ("/index/api/getRecordStatus", true);

            /// <summary>
            /// 获取截图或生成实时截图并返回
            /// </summary>
            public static readonly (string Action, bool IsSecret) GetSnap = ("/index/api/getSnap", true);

            #endregion

            #region Rtp、GB28181

            /// <summary>
            /// 创建GB28181 RTP接收端口，如果该端口接收数据超时，则会自动被回收(不用调用closeRtpServer接口)
            /// </summary>
            public static readonly (string Action, bool IsSecret) OpenRtpServer = ("/index/api/openRtpServer", true);
            /// <summary>
            /// 关闭GB28181 RTP接收端口
            /// </summary>
            public static readonly (string Action, bool IsSecret) CloseRtpServer = ("/index/api/closeRtpServer", true);
            /// <summary>
            /// 获取openRtpServer接口创建的所有RTP服务器
            /// </summary>
            public static readonly (string Action, bool IsSecret) ListRtpServer = ("/index/api/listRtpServer", true);
            /// <summary>
            /// 作为GB28181客户端，启动ps-rtp推流，支持rtp/udp方式；
            /// 该接口支持rtsp/rtmp等协议转ps-rtp推流。第一次推流失败
            /// 会直接返回错误，成功一次后，后续失败也将无限重试。
            /// </summary>
            public static readonly (string Action, bool IsSecret) StartSendRtp = ("/index/api/startSendRtp", true);
            /// <summary>
            /// 作为GB28181 Passive TCP服务器；该接口支持rtsp/rtmp等协议转ps-rtp被动推流。
            /// 调用该接口，zlm会启动tcp服务器等待连接请求，连接建立后，zlm会关闭tcp服务器，
            /// 然后源源不断的往客户端推流。第一次推流失败会直接返回错误，成功一次后，后续失败
            /// 也将无限重试(不停地建立tcp监听，超时后再关闭)。
            /// </summary>
            public static readonly (string Action, bool IsSecret) StartSendRtpPassive = ("/index/api/startSendRtpPassive", true);

            /// <summary>
            /// 停止GB28181 ps-rtp推流
            /// </summary>
            public static readonly (string Action, bool IsSecret) StopSendRtp = ("/index/api/stopSendRtp", true);

            #endregion
        }

    }

    /// <summary>
    /// Api 执行结果
    /// </summary>
    public enum ApiCode
    {
        /// <summary>
        /// 代码抛异常
        /// </summary>
        Exception = -400,
        /// <summary>
        /// 参数不合法
        /// </summary>
        InvalidArgs = -300,
        /// <summary>
        /// sql执行失败
        /// </summary>
        SqlFailed = -200,
        /// <summary>
        /// 鉴权失败
        /// </summary>
        AuthFailed = -100,
        /// <summary>
        /// 业务代码执行失败
        /// </summary>
        OtherFailed = -1,
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 错误请求
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// 拒绝访问
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// 指令不存在
        /// </summary>
        NotFound = 404,
        InternalServerError = 500,
        ServerError = 501,
        BedGateway = 502,
        ServerNotAvailable = 503,
        BedGatewayTimeout = 504,
        NotSupportedHttpVersion = 505,
        JsonParseFailed = 1202,
        ProgramError = 9999,
    }

    /// <summary>
    /// 录制文件类型
    /// </summary>
    public enum RecordFileType
    {
        Hls = 0,
        Mp4 = 1
    }

    public class ApiResponse
    {
        [JsonPropertyName("code")]
        public ApiCode Code { get; set; }
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        /// <summary>
        /// 若JSON解析失败，则返回原始值
        /// </summary>
        [JsonIgnore]
        public byte[] Raw { get; set; }
    }

    public class ResultResponse<T> :ApiResponse where T : struct
    {
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }

    /// <summary>
    /// 状态回应
    /// </summary>
    public class StatusResponse : ApiResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
    }

    /// <summary>
    /// 状态改变返回值
    /// </summary>
    public class ApiChangedResponse : ApiResponse
    {
        /// <summary>
        /// 是否改变，参见<see cref="SetServerConfig((string Key, string Value)[])"/>
        /// </summary>
        [JsonPropertyName("changed")]
        public int Changed { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class StatisticData
    {
        public int Buffer { get; set; }
        public int BufferLikeString { get; set; }
        public int BufferList { get; set; }
        public int BufferRaw { get; set; }
        public int Frame { get; set; }
        public int FrameImp { get; set; }
        public int MediaSource { get; set; }
        public int MultiMediaSourceMuxer { get; set; }
        public int Socket { get; set; }
        public int TcpClient { get; set; }
        public int TcpServer { get; set; }
        public int TcpSession { get; set; }
    }
}
