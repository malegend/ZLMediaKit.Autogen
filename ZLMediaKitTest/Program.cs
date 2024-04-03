using System;
using ZLMediaKit;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace ZLMediaKitTest
{
    unsafe class Program
    {
        private const int LOG_LEV = 1 << 2;
        /**
         * 注册或反注册MediaSource事件广播
         * @param regist 注册为1，注销为0
         * @param sender 该MediaSource对象
         */
        static void On_mk_media_changed(int regist, IntPtr senderPtr)
        {
            var sender = (MkMediaSourceT)senderPtr;
            Log_printf(
                LOG_LEV,
                "{0} {1}/{2}/{3}/{4}",
                //"%d %s/%s/%s/%s",
                (int)regist,
                mk_events_objects.MkMediaSourceGetSchema(sender),
                mk_events_objects.MkMediaSourceGetVhost(sender),
                mk_events_objects.MkMediaSourceGetApp(sender),
                mk_events_objects.MkMediaSourceGetStream(sender));

        }

        /**
         * 收到rtsp/rtmp推流事件广播，通过该事件控制推流鉴权
         * @see mk_publish_auth_invoker_do
         * @param url_info 推流url相关信息
         * @param invoker 执行invoker返回鉴权结果
         * @param sender 该tcp客户端相关信息
         */
        static void On_mk_media_publish(IntPtr url,
                                  IntPtr invoker,
                                  IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                           "client info, local: {0}:{1}, peer: {2}:{3}\n{4}/{5}/{6}/{7}, url params: {8}",
                           //"client info, local: %s:%d, peer: %s:%d\n%s/%s/%s/%s, url params: %s",
                           mk_tcp.MkSockInfoLocalIp(sender, ip),
                           mk_tcp.MkSockInfoLocalPort(sender),
                           mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkMediaInfoGetSchema(url_info),
                       mk_events_objects.MkMediaInfoGetVhost(url_info),
                       mk_events_objects.MkMediaInfoGetApp(url_info),
                       mk_events_objects.MkMediaInfoGetStream(url_info),
                       mk_events_objects.MkMediaInfoGetParams(url_info));

            //允许推流，并且允许转hls/mp4
            mk_events_objects.MkPublishAuthInvokerDo((MkPublishAuthInvokerT)invoker, null, 1, 1);
        }

        /**
         * 播放rtsp/rtmp/http-flv/hls事件广播，通过该事件控制播放鉴权
         * @see mk_auth_invoker_do
         * @param url_info 播放url相关信息
         * @param invoker 执行invoker返回鉴权结果
         * @param sender 播放客户端相关信息
         */
        static void On_mk_media_play(IntPtr url,
                                       IntPtr invoker,
                                       IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}\n{4}/{5}/{6}/{7}, url params: {8}",
                       //"client info, local: %s:%d, peer: %s:%d\n%s/%s/%s/%s, url params: %s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkMediaInfoGetSchema(url_info),
                       mk_events_objects.MkMediaInfoGetVhost(url_info),
                       mk_events_objects.MkMediaInfoGetApp(url_info),
                       mk_events_objects.MkMediaInfoGetStream(url_info),
                       mk_events_objects.MkMediaInfoGetParams(url_info));

            //允许播放
            mk_events_objects.MkAuthInvokerDo((MkAuthInvokerT)invoker, null);
        }

        /**
         * 未找到流后会广播该事件，请在监听该事件后去拉流或其他方式产生流，这样就能按需拉流了
         * @param url_info 播放url相关信息
         * @param sender 播放客户端相关信息
         */
        static int On_mk_media_not_found(IntPtr url,
                                        IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}\n{4}/{5}/{6}/{7}, url params: {8}",
                       //"client info, local: %s:%d, peer: %s:%d\n%s/%s/%s/%s, url params: %s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkMediaInfoGetSchema(url_info),
                       mk_events_objects.MkMediaInfoGetVhost(url_info),
                       mk_events_objects.MkMediaInfoGetApp(url_info),
                       mk_events_objects.MkMediaInfoGetStream(url_info),
                       mk_events_objects.MkMediaInfoGetParams(url_info));
            return 0;
        }

        /**
         * 某个流无人消费时触发，目的为了实现无人观看时主动断开拉流等业务逻辑
         * @param sender 该MediaSource对象
         */
        static void On_mk_media_no_reader(IntPtr senderPtr)
        {
            var sender = (MkMediaSourceT)senderPtr;
            Log_printf(LOG_LEV,
                       "{0}/{1}/{2}/{3}",
                       //"%s/%s/%s/%s",
                       mk_events_objects.MkMediaSourceGetSchema(sender),
                       mk_events_objects.MkMediaSourceGetVhost(sender),
                       mk_events_objects.MkMediaSourceGetApp(sender),
                       mk_events_objects.MkMediaSourceGetStream(sender));
        }

        /**
         * 收到http api请求广播(包括GET/POST)
         * @param parser http请求内容对象
         * @param invoker 执行该invoker返回http回复
         * @param consumed 置1则说明我们要处理该事件
         * @param sender http客户端相关信息
         */
        //测试url : http://127.0.0.1/api/test
        private static void On_mk_http_request(IntPtr parserPtr,
                                         IntPtr invoker,
                                         int* consumed,
                                         IntPtr sock)
        {
            var parser = (MkParserT)parserPtr;
            var sender = (MkSockInfoT)sock;
            ulong tmp = 0;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}\n{4} {5}?{6} {7}\nUser-Agent: {8}\n{9}",
                       //"client info, local: %s:%d, peer: %s:%d\n%s %s?%s %s\nUser-Agent: %s\n%s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkParserGetMethod(parser),
                       mk_events_objects.MkParserGetUrl(parser),
                       mk_events_objects.MkParserGetUrlParams(parser),
                       mk_events_objects.MkParserGetTail(parser),
                       mk_events_objects.MkParserGetHeader(parser, "User-Agent"),
                       mk_events_objects.MkParserGetContent(parser, ref tmp));

            var url = mk_events_objects.MkParserGetUrl(parser);
            *consumed = 1;

            //拦截api: /api/test
            if (string.Compare(url, "/api/test") == 0)
            {
                var buff = new byte[3][];
                buff[0] = Encoding.UTF8.GetBytes("Content-Type");
                buff[1] = Encoding.UTF8.GetBytes("text/html");
                buff[2] = new byte[0];
                //IntPtr* response_header[] = { "Content-Type", "text/html", null };
                fixed (byte* bresponse_header = &buff[0][0])
                {
                    var response_header = (sbyte*)bresponse_header;
                    var content = "<html>\n" +
                                      "<head>\n" +
                                      "<title>hello world</title>\n" +
                                      "</head>\n" +
                                      "<body bgcolor=\"white\">\n" +
                                      "<center><h1>hello world</h1></center><hr>\n" +
                                      "<center>\n" +
                                      "ZLMediaKit-4.0</center>\n" +
                                      "</body>\n" +
                                      "</html>";
                    var body = mk_events_objects.MkHttpBodyFromString(content, 0);
                    mk_events_objects.MkHttpResponseInvokerDo((MkHttpResponseInvokerT)invoker, 200, &response_header, body);
                    mk_events_objects.MkHttpBodyRelease(body);
                }
            }
            //拦截api: /index/api/webrtc
            else if (string.Compare(url, "/index/api/webrtc") == 0)
            {
                //mk_events_objects.MkWebrtcHttpResponseInvokerDo(invoker, parser, sender);
            }
            else
            {
                *consumed = 0;
                return;
            }
        }

        /**
         * 在http文件服务器中,收到http访问文件或目录的广播,通过该事件控制访问http目录的权限
         * @param parser http请求内容对象
         * @param path 文件绝对路径
         * @param is_dir path是否为文件夹
         * @param invoker 执行invoker返回本次访问文件的结果
         * @param sender http客户端相关信息
         */
        static void On_mk_http_access(IntPtr parserPtr,
                                        string path,
                                        int is_dir,
                                        IntPtr invoker,
                                        IntPtr sock)
        {
            var parser = (MkParserT)parserPtr;
            var sender = (MkSockInfoT)sock;
            ulong tmp = 0;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}, path: {4} ,is_dir: {5}\n{6} {7}?{8} {9}\nUser-Agent: {10}\n{11}",
                       //"client info, local: %s:%d, peer: %s:%d, path: %s ,is_dir: %d\n%s %s?%s %s\nUser-Agent: %s\n%s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       path, (int)is_dir,
                       mk_events_objects.MkParserGetMethod(parser),
                       mk_events_objects.MkParserGetUrl(parser),
                       mk_events_objects.MkParserGetUrlParams(parser),
                       mk_events_objects.MkParserGetTail(parser),
                       mk_events_objects.MkParserGetHeader(parser, "User-Agent"),
                       mk_events_objects.MkParserGetContent(parser, ref tmp));

            //有访问权限,每次访问文件都需要鉴权
            mk_events_objects.MkHttpAccessPathInvokerDo((MkHttpAccessPathInvokerT)invoker, null, null, 0);
        }

        /**
         * 在http文件服务器中,收到http访问文件或目录前的广播,通过该事件可以控制http url到文件路径的映射
         * 在该事件中通过自行覆盖path参数，可以做到譬如根据虚拟主机或者app选择不同http根目录的目的
         * @param parser http请求内容对象
         * @param path 文件绝对路径,覆盖之可以重定向到其他文件
         * @param sender http客户端相关信息
         */
        static void On_mk_http_before_access(IntPtr parserPtr,
                                               sbyte* path,
                                               IntPtr sock)
        {
            var parser = (MkParserT)parserPtr;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            ulong tmp = 0;
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}, path: {4}\n{5} {6}?{7} {8}\nUser-Agent: {9}\n{10}",
                       //"client info, local: %s:%d, peer: %s:%d, path: %s\n%s %s?%s %s\nUser-Agent: %s\n%s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       new string(path),
                       mk_events_objects.MkParserGetMethod(parser),
                       mk_events_objects.MkParserGetUrl(parser),
                       mk_events_objects.MkParserGetUrlParams(parser),
                       mk_events_objects.MkParserGetTail(parser),
                       mk_events_objects.MkParserGetHeader(parser, "User-Agent"),
                       mk_events_objects.MkParserGetContent(parser, ref tmp));
            //覆盖path的值可以重定向文件
        }

        /**
         * 该rtsp流是否需要认证？是的话调用invoker并传入realm,否则传入空的realm
         * @param url_info 请求rtsp url相关信息
         * @param invoker 执行invoker返回是否需要rtsp专属认证
         * @param sender rtsp客户端相关信息
         */
        static void On_mk_rtsp_get_realm(IntPtr url,
                                           IntPtr invoker,
                                           IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}\n{4}/{5}/{6}/{7}, url params: {8}",
                       //"client info, local: %s:%d, peer: %s:%d\n%s/%s/%s/%s, url params: %s",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkMediaInfoGetSchema(url_info),
                       mk_events_objects.MkMediaInfoGetVhost(url_info),
                       mk_events_objects.MkMediaInfoGetApp(url_info),
                       mk_events_objects.MkMediaInfoGetStream(url_info),
                       mk_events_objects.MkMediaInfoGetParams(url_info));

            //rtsp播放默认鉴权
            mk_events_objects.MkRtspGetRealmInvokerDo((MkRtspGetRealmInvokerT)invoker, "zlmediakit");
        }

        /**
         * 请求认证用户密码事件，user_name为用户名，must_no_encrypt如果为1，则必须提供明文密码(因为此时是base64认证方式),否则会导致认证失败
         * 获取到密码后请调用invoker并输入对应类型的密码和密码类型，invoker执行时会匹配密码
         * @param url_info 请求rtsp url相关信息
         * @param realm rtsp认证realm
         * @param user_name rtsp认证用户名
         * @param must_no_encrypt 如果为1，则必须提供明文密码(因为此时是base64认证方式),否则会导致认证失败
         * @param invoker  执行invoker返回rtsp专属认证的密码
         * @param sender rtsp客户端信息
         */
        static void On_mk_rtsp_auth(IntPtr url,
                                      string realm,
                                      string user_name,
                                      int must_no_encrypt,
                                      IntPtr invoker,
                                      IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;

            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                       "client info, local: {0}:{1}, peer: {2}:{3}\n{4}/{5}/{6}/{7}, url params: {8}\nrealm: {9}, user_name: {10}, must_no_encrypt: {11}",
                       //"client info, local: %s:%d, peer: %s:%d\n%s/%s/%s/%s, url params: %s\nrealm: %s, user_name: %s, must_no_encrypt: %d",
                       mk_tcp.MkSockInfoLocalIp(sender, ip),
                       mk_tcp.MkSockInfoLocalPort(sender),
                       mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                       mk_tcp.MkSockInfoPeerPort(sender),
                       mk_events_objects.MkMediaInfoGetSchema(url_info),
                       mk_events_objects.MkMediaInfoGetVhost(url_info),
                       mk_events_objects.MkMediaInfoGetApp(url_info),
                       mk_events_objects.MkMediaInfoGetStream(url_info),
                       mk_events_objects.MkMediaInfoGetParams(url_info),
                       realm, user_name, (int)must_no_encrypt);

            //rtsp播放用户名跟密码一致
            mk_events_objects.MkRtspAuthInvokerDo((MkRtspAuthInvokerT)invoker, 0, user_name);
        }

        /**
         * 录制mp4分片文件成功后广播
         */
        static void On_mk_record_mp4(IntPtr mp4Ptr)
        {
            var mp4 = (MkMp4InfoT)mp4Ptr;
            Log_printf(LOG_LEV,
                       "start_time: {0}\ntime_len: {1}\nfile_size: {2}\nfile_path: {3}\nfile_name: {4}\nfolder: {5}\nurl: {6}\nvhost: {7}\napp: {8}\nstream: {9}\n",
                       //"\nstart_time: %d\ntime_len: %d\nfile_size: %d\nfile_path: %s\nfile_name: %s\nfolder: %s\nurl: %s\nvhost: %s\napp: %s\nstream: %s\n",
                       mk_events_objects.MkMp4InfoGetStartTime(mp4),
                       mk_events_objects.MkMp4InfoGetTimeLen(mp4),
                       mk_events_objects.MkMp4InfoGetFileSize(mp4),
                       mk_events_objects.MkMp4InfoGetFilePath(mp4),
                       mk_events_objects.MkMp4InfoGetFileName(mp4),
                       mk_events_objects.MkMp4InfoGetFolder(mp4),
                       mk_events_objects.MkMp4InfoGetUrl(mp4),
                       mk_events_objects.MkMp4InfoGetVhost(mp4),
                       mk_events_objects.MkMp4InfoGetApp(mp4),
                       mk_events_objects.MkMp4InfoGetStream(mp4));
        }

        /**
         * shell登录鉴权
         */
        static void On_mk_shell_login(string user_name,
                                        string passwd,
                                        IntPtr invoker,
                                        IntPtr sock)
        {
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                      "client info, local: {0}:{1}, peer: {2}:{3}\nuser_name: {4}, passwd: {5}",
                      //"client info, local: %s:%d, peer: %s:%d\nuser_name: %s, passwd: %s",
                      mk_tcp.MkSockInfoLocalIp(sender, ip),
                      mk_tcp.MkSockInfoLocalPort(sender),
                      mk_tcp.MkSockInfoPeerIp(sender, ip + 32),
                      mk_tcp.MkSockInfoPeerPort(sender),
                      user_name, passwd);
            //允许登录shell
            mk_events_objects.MkAuthInvokerDo((MkAuthInvokerT)invoker, null);
        }

        /**
         * 停止rtsp/rtmp/http-flv会话后流量汇报事件广播
         * @param url_info 播放url相关信息
         * @param total_bytes 耗费上下行总流量，单位字节数
         * @param total_seconds 本次tcp会话时长，单位秒
         * @param is_player 客户端是否为播放器
         * @param peer_ip 客户端ip
         * @param peer_port 客户端端口号
         */
        static void On_mk_flow_report(IntPtr url,
                                        ulong total_bytes,
                                        ulong total_seconds,
                                        int is_player,
                                        IntPtr sock)
        {
            var url_info = (MkMediaInfoT)url;
            var sender = (MkSockInfoT)sock;
            sbyte[] vs = new sbyte[64];
            fixed (sbyte* ip = &vs[0])
                Log_printf(LOG_LEV,
                    "{0}/{1}/{2}/{3}, url params: {4}, total_bytes: {5}, total_seconds: {6}, is_player: {7}, peer_ip:{8}, peer_port:{9}",
                    //"%s/%s/%s/%s, url params: %s, total_bytes: %d, total_seconds: %d, is_player: %d, peer_ip:%s, peer_port:%d",
                    mk_events_objects.MkMediaInfoGetSchema(url_info),
                    mk_events_objects.MkMediaInfoGetVhost(url_info),
                    mk_events_objects.MkMediaInfoGetApp(url_info),
                    mk_events_objects.MkMediaInfoGetStream(url_info),
                    mk_events_objects.MkMediaInfoGetParams(url_info),
                    (int)total_bytes,
                    (int)total_seconds,
                    (int)is_player,
                    mk_tcp.MkSockInfoPeerIp(sender, ip),
                    (int)mk_tcp.MkSockInfoPeerPort(sender));
        }

        private static void Log_printf(int level, string fmt, params object[] args)
        {
            /*
            __FILE__:正在编译文件的文件名
            __LINE__: 正在编译文件的行号
            __DATE_: 编译时刻的日期字符串 如“Sep 22 2020”
            __TIME_: 编译时刻的时间字符串 如“10:00:00”
            __STDC_: 判断该文件是不是标准C程序
             */
            var trace = new StackTrace();
            var frame = trace.GetFrame(1);
            var file = frame.GetFileName();
            var method = frame.GetMethod();
            var line = frame.GetFileLineNumber();
            var className = method.ReflectedType.Name;
            var methodName = method.Name;
            var func = $"{className}.{methodName}";
            if (string.IsNullOrEmpty(file))
            {
                file = System.IO.Path.GetFileName(method.DeclaringType.Assembly.CodeBase);
            }

            if (line == 0)
                line = frame.GetILOffset();

            var proc = System.Diagnostics.Process.GetCurrentProcess().Id;

            //// 用 .net 的 console 打印日志
            Console.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {file}[{proc}-{line}] {func} | ");
            Console.Write(fmt, args);

            //// 用 MK_API 打印日志
            //mk_util.MkLogPrintf(level, file, func, line, fmt, args);
        }

        static void On_h264_frame(void* user_data, mk_h264_splitter splitter, IntPtr frame, int size)
        {
            Thread.Sleep(40);
            mk_media.MkMediaInputH264((MkMediaT)user_data, frame, size, 0, 0);
        }

        static void TheMain()
        {
            var init_path = mk_util.MkUtilGetExeDir("c_api.ini");
            var ssl_path = mk_util.MkUtilGetExeDir("ssl.pl2");
            MkConfig config = new MkConfig()
            {
                Ini = new string(init_path),
                IniIsPath = 1,
                LogLevel = 0,
                LogMask = (int)LogMask.Console,
                LogFilePath = null,
                LogFileDays = 0,
                Ssl = new string(ssl_path),
                SslIsPath = 1,
                SslPwd = null,
                ThreadNum = 0
            };
            mk_common.MkEnvInit(config);
            mk_common.MkHttpServerStart(7880, 0);
            mk_common.MkRtspServerStart(554, 0);
            mk_common.MkRtmpServerStart(1935, 0);
            mk_common.MkShellServerStart(9000);
            mk_common.MkRtpServerStart(10000);
            mk_common.MkRtcServerStart(8000);

            var events = new MkEvents()
            {
                OnMkMediaChanged = On_mk_media_changed,
                OnMkMediaPublish = On_mk_media_publish,
                OnMkMediaPlay = On_mk_media_play,
                OnMkMediaNotFound = On_mk_media_not_found,
                OnMkMediaNoReader = On_mk_media_no_reader,
                OnMkHttpRequest = On_mk_http_request,
                OnMkHttpAccess = On_mk_http_access,
                OnMkHttpBeforeAccess = On_mk_http_before_access,
                OnMkRtspGetRealm = On_mk_rtsp_get_realm,
                OnMkRtspAuth = On_mk_rtsp_auth,
                OnMkRecordMp4 = On_mk_record_mp4,
                OnMkShellLogin = On_mk_shell_login,
                OnMkFlowReport = On_mk_flow_report
            };
            MkEvents.MkEventsListen(events);

            var rtsp = "rtsp://10.10.10.222/user=remote&password=miaodun2022&channel=1&stream=0.sdp?real_stream";

            //rtsp://10.10.10.222/user=remote&password=miaodun2022&channel=1&stream=0.sdp?real_stream
            //var mediaSource = mk_media.MkMediaCreate("_defaultVhost_", "live", "0", 2, 0, 0);
            //using (var video = new CodecArgs.Video())
            //using (var audio = new CodecArgs.Audio())
            //{
            //    var codec = new CodecArgs() { audio = audio, video = video };
            //    var mediaTrack = mk_track.MkTrackCreate(mk_frame.MKCodecH264 | mk_frame.MKCodecAAC, codec);

            //    mk_media.MkMediaInitTrack(mediaSource, mediaTrack);
            //    //mk_track.MkTrackAddDelegate(mediaTrack, )
            //    mk_media.MkMediaRelease(mediaTrack);
            //    mk_media.MkMediaRelease(mediaSource);
            //}

            var apc = new Api();
            apc.ServerAddress = "http://127.0.0.1:7880";
            var smk = apc.AddStreamProxy("rtsp", "defaultHost", "live", "test", rtsp, enableRtmp: true);
            if (smk.Code == ApiCode.Success)
            {
                Log_printf(2, $"拉流代理创建成功。");

            }
            else
            {
                Log_printf(2, $"无法创建拉流代理:{smk.Message}");
            }


            Log_printf(2, "media server started!\r\nPress Enter to exit.\r\n");
            Console.ReadLine();
            if (smk.Code == ApiCode.Success)
            {
                apc.DelStreamProxy(smk.Data.Key);
            }
            mk_common.MkStopAllServer();

        }


        static void Main()
        {

            var rtsp = "rtsp://10.10.10.222/user=remote&password=miaodun2022&channel=1&stream=1.sdp?real_stream";


            var apc = new Api();
            apc.ServerAddress = "http://127.0.0.1:7880";
            var smk = apc.AddStreamProxy("rtsp", "defaultHost", "live", "test", rtsp,
                enableHls: false,
                enableRtmp: true,
                enableAudio: true);
            if (smk.Code == ApiCode.Success)
            {
                Log_printf(2, $"拉流代理创建成功。");
                while (true)
                {
                    var ms = Console.ReadLine();
                    if (ms == "exit")
                        break;
                    else
                    {
                        var capt = apc.GetSnap("rtmp://127.0.0.1:1935/live/test");
                        if (capt.Code == ApiCode.Success)
                        {
                            if (System.IO.File.Exists(@"D:\ttt.jpg"))
                                System.IO.File.Delete(@"D:\ttt.jpg");
                            System.IO.File.WriteAllBytes(@"D:\ttt.jpg", capt.Raw);
                            Console.WriteLine(capt);
                        }
                    }
                }
                apc.DelStreamProxy(smk.Data.Key);
            }
            else
            {
                Log_printf(2, $"无法创建拉流代理:{smk.Message}");
            }

        }
    }
}
