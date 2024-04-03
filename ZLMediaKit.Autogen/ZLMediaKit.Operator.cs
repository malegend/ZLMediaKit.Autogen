using System;

namespace ZLMediaKit
{
    public unsafe partial class MkBufferT
    {

        public static implicit operator void*(MkBufferT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkBufferT value) => value.__Instance;

        public static explicit operator MkBufferT(void* value) => new MkBufferT(value);

        public static explicit operator MkBufferT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkSockInfoT
    {

        public static implicit operator void*(MkSockInfoT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkSockInfoT value) => value.__Instance;

        public static explicit operator MkSockInfoT(void* value) => new MkSockInfoT(value);

        public static explicit operator MkSockInfoT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkTcpSessionT
    {

        public static implicit operator void*(MkTcpSessionT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkTcpSessionT value) => value.__Instance;

        public static explicit operator MkTcpSessionT(void* value) => new MkTcpSessionT(value);

        public static explicit operator MkTcpSessionT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkTcpSessionRefT
    {

        public static implicit operator void*(MkTcpSessionRefT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkTcpSessionRefT value) => value.__Instance;

        public static explicit operator MkTcpSessionRefT(void* value) => new MkTcpSessionRefT(value);

        public static explicit operator MkTcpSessionRefT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkTcpClientT
    {

        public static implicit operator void*(MkTcpClientT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkTcpClientT value) => value.__Instance;

        public static explicit operator MkTcpClientT(void* value) => new MkTcpClientT(value);

        public static explicit operator MkTcpClientT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkFrameT
    {

        public static implicit operator void*(MkFrameT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkFrameT value) => value.__Instance;

        public static explicit operator MkFrameT(void* value) => new MkFrameT(value);

        public static explicit operator MkFrameT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkFrameMergerT
    {

        public static implicit operator void*(MkFrameMergerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkFrameMergerT value) => value.__Instance;

        public static explicit operator MkFrameMergerT(void* value) => new MkFrameMergerT(value);

        public static explicit operator MkFrameMergerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkMpegMuxerT
    {

        public static implicit operator void*(MkMpegMuxerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkMpegMuxerT value) => value.__Instance;

        public static explicit operator MkMpegMuxerT(void* value) => new MkMpegMuxerT(value);

        public static explicit operator MkMpegMuxerT(IntPtr value) => __CreateInstance(value);


    }

    public unsafe partial class MkTrackT
    {

        public static implicit operator void*(MkTrackT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkTrackT value) => value.__Instance;

        public static explicit operator MkTrackT(void* value) => new MkTrackT(value);

        public static explicit operator MkTrackT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkIniT
    {

        public static implicit operator void*(MkIniT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkIniT value) => value.__Instance;

        public static explicit operator MkIniT(void* value) => new MkIniT(value);

        public static explicit operator MkIniT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkMp4InfoT
    {

        public static implicit operator void*(MkMp4InfoT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkMp4InfoT value) => value.__Instance;

        public static explicit operator MkMp4InfoT(void* value) => new MkMp4InfoT(value);

        public static explicit operator MkMp4InfoT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkParserT
    {

        public static implicit operator void*(MkParserT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkParserT value) => value.__Instance;

        public static explicit operator MkParserT(void* value) => new MkParserT(value);

        public static explicit operator MkParserT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkMediaInfoT
    {

        public static implicit operator void*(MkMediaInfoT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkMediaInfoT value) => value.__Instance;

        public static explicit operator MkMediaInfoT(void* value) => new MkMediaInfoT(value);

        public static explicit operator MkMediaInfoT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkMediaSourceT
    {

        public static implicit operator void*(MkMediaSourceT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkMediaSourceT value) => value.__Instance;

        public static explicit operator MkMediaSourceT(void* value) => new MkMediaSourceT(value);

        public static explicit operator MkMediaSourceT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkHttpBodyT
    {

        public static implicit operator void*(MkHttpBodyT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkHttpBodyT value) => value.__Instance;

        public static explicit operator MkHttpBodyT(void* value) => new MkHttpBodyT(value);

        public static explicit operator MkHttpBodyT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkHttpResponseInvokerT
    {

        public static implicit operator void*(MkHttpResponseInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkHttpResponseInvokerT value) => value.__Instance;

        public static explicit operator MkHttpResponseInvokerT(void* value) => new MkHttpResponseInvokerT(value);

        public static explicit operator MkHttpResponseInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkHttpAccessPathInvokerT
    {

        public static implicit operator void*(MkHttpAccessPathInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkHttpAccessPathInvokerT value) => value.__Instance;

        public static explicit operator MkHttpAccessPathInvokerT(void* value) => new MkHttpAccessPathInvokerT(value);

        public static explicit operator MkHttpAccessPathInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkRtspGetRealmInvokerT
    {

        public static implicit operator void*(MkRtspGetRealmInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkRtspGetRealmInvokerT value) => value.__Instance;

        public static explicit operator MkRtspGetRealmInvokerT(void* value) => new MkRtspGetRealmInvokerT(value);

        public static explicit operator MkRtspGetRealmInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkRtspAuthInvokerT
    {

        public static implicit operator void*(MkRtspAuthInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkRtspAuthInvokerT value) => value.__Instance;

        public static explicit operator MkRtspAuthInvokerT(void* value) => new MkRtspAuthInvokerT(value);

        public static explicit operator MkRtspAuthInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkPublishAuthInvokerT
    {

        public static implicit operator void*(MkPublishAuthInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkPublishAuthInvokerT value) => value.__Instance;

        public static explicit operator MkPublishAuthInvokerT(void* value) => new MkPublishAuthInvokerT(value);

        public static explicit operator MkPublishAuthInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkAuthInvokerT
    {

        public static implicit operator void*(MkAuthInvokerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkAuthInvokerT value) => value.__Instance;

        public static explicit operator MkAuthInvokerT(void* value) => new MkAuthInvokerT(value);

        public static explicit operator MkAuthInvokerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkRtcTransportT
    {

        public static implicit operator void*(MkRtcTransportT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkRtcTransportT value) => value.__Instance;

        public static explicit operator MkRtcTransportT(void* value) => new MkRtcTransportT(value);

        public static explicit operator MkRtcTransportT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkH264SplitterT
    {

        public static implicit operator void*(MkH264SplitterT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkH264SplitterT value) => value.__Instance;

        public static explicit operator MkH264SplitterT(void* value) => new MkH264SplitterT(value);

        public static explicit operator MkH264SplitterT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkHttpDownloaderT
    {

        public static implicit operator void*(MkHttpDownloaderT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkHttpDownloaderT value) => value.__Instance;

        public static explicit operator MkHttpDownloaderT(void* value) => new MkHttpDownloaderT(value);

        public static explicit operator MkHttpDownloaderT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkHttpRequesterT
    {

        public static implicit operator void*(MkHttpRequesterT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkHttpRequesterT value) => value.__Instance;

        public static explicit operator MkHttpRequesterT(void* value) => new MkHttpRequesterT(value);

        public static explicit operator MkHttpRequesterT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkMediaT
    {

        public static implicit operator void*(MkMediaT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkMediaT value) => value.__Instance;

        public static explicit operator MkMediaT(void* value) => new MkMediaT(value);

        public static explicit operator MkMediaT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkThreadT
    {

        public static implicit operator void*(MkThreadT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkThreadT value) => value.__Instance;

        public static explicit operator MkThreadT(void* value) => new MkThreadT(value);

        public static explicit operator MkThreadT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkThreadPoolT
    {

        public static implicit operator void*(MkThreadPoolT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkThreadPoolT value) => value.__Instance;

        public static explicit operator MkThreadPoolT(void* value) => new MkThreadPoolT(value);

        public static explicit operator MkThreadPoolT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkTimerT
    {

        public static implicit operator void*(MkTimerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkTimerT value) => value.__Instance;

        public static explicit operator MkTimerT(void* value) => new MkTimerT(value);

        public static explicit operator MkTimerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkSemT
    {

        public static implicit operator void*(MkSemT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkSemT value) => value.__Instance;

        public static explicit operator MkSemT(void* value) => new MkSemT(value);

        public static explicit operator MkSemT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkProxyPlayerT
    {

        public static implicit operator void*(MkProxyPlayerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkProxyPlayerT value) => value.__Instance;

        public static explicit operator MkProxyPlayerT(void* value) => new MkProxyPlayerT(value);

        public static explicit operator MkProxyPlayerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkFlvRecorderT
    {

        public static implicit operator void*(MkFlvRecorderT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkFlvRecorderT value) => value.__Instance;

        public static explicit operator MkFlvRecorderT(void* value) => new MkFlvRecorderT(value);

        public static explicit operator MkFlvRecorderT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkPlayerT
    {

        public static implicit operator void*(MkPlayerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkPlayerT value) => value.__Instance;

        public static explicit operator MkPlayerT(void* value) => new MkPlayerT(value);

        public static explicit operator MkPlayerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkPusherT
    {

        public static implicit operator void*(MkPusherT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkPusherT value) => value.__Instance;

        public static explicit operator MkPusherT(void* value) => new MkPusherT(value);

        public static explicit operator MkPusherT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkRtpServerT
    {

        public static implicit operator void*(MkRtpServerT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkRtpServerT value) => value.__Instance;

        public static explicit operator MkRtpServerT(void* value) => new MkRtpServerT(value);

        public static explicit operator MkRtpServerT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkDecoderT
    {

        public static implicit operator void*(MkDecoderT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkDecoderT value) => value.__Instance;

        public static explicit operator MkDecoderT(void* value) => new MkDecoderT(value);

        public static explicit operator MkDecoderT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkFramePixT
    {

        public static implicit operator void*(MkFramePixT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkFramePixT value) => value.__Instance;

        public static explicit operator MkFramePixT(void* value) => new MkFramePixT(value);

        public static explicit operator MkFramePixT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class MkSwscaleT
    {

        public static implicit operator void*(MkSwscaleT value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(MkSwscaleT value) => value.__Instance;

        public static explicit operator MkSwscaleT(void* value) => new MkSwscaleT(value);

        public static explicit operator MkSwscaleT(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class AVFrame
    {

        public static implicit operator void*(AVFrame value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(AVFrame value) => value.__Instance;

        public static explicit operator AVFrame(void* value) => new AVFrame(value);

        public static explicit operator AVFrame(IntPtr value) => __CreateInstance(value);


    }
    public unsafe partial class AVCodecContext
    {

        public static implicit operator void*(AVCodecContext value) => value.__Instance.ToPointer();

        public static implicit operator IntPtr(AVCodecContext value) => value.__Instance;

        public static explicit operator AVCodecContext(void* value) => new AVCodecContext(value);

        public static explicit operator AVCodecContext(IntPtr value) => __CreateInstance(value);
    }
}
