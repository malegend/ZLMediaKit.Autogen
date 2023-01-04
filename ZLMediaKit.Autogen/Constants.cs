using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit
{
    [Flags]
    public enum LogMask : int
    {
        /// <summary>
        /// 输出日志到shell
        /// </summary>
        Console = 1 << 0,
        /// <summary>
        /// 输出日志到文件
        /// </summary>
        File = 1 << 1,
        /// <summary>
        /// 输出日志到回调函数(mk_events::on_mk_log)
        /// </summary>
        Callback = 1 << 2
    }

    internal static class Constants
    {
        public const string ApiDll = "mk_api";
    }
}
