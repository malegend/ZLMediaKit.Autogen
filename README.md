# 项目简介
 ZMLediaKit 服务端的 C# API 封装。

 感谢 [@夏楚](https://github.com/xia-chu) 提供了这么好的开源流媒体服务器软件[ZLMediaKit ](https://github.com/ZLMediaKit/ZLMediaKit)

 感谢 @Mono 提供了强大的 API 实现器[CppSharp](https://github.com/mono/CppSharp)

 本项目是对 ZLMediaKit 提供的 SDK (MK_API) 的 C# API 封装。采用 CppSharp 对 ZLMediaKit 进行解析，并进行微调和修改，基于 ZLMediaKit 项目的调用原始风格，各位网友可以参照 ZLMediaKit 原始项目文档编写应用程序。

# 项目组成
 本项目包含 ZLMediaKit.Autogen 和 ZLMediaKitTest 两个C#工程。基于 .Net Standard 2.1，可以用于大部分的 .NET 版本。
 ## ZLMediaKit.Autogen
 该工程为主类库。

 ## ZLMediaKitTest
 该工程为演示/测试程序，复刻了 ZLMediaKit 的 “\api\tests\server.c”。

# 更新日志
 ## 2022-06-22
 更新至2022年6月22日拉取版本。

 ## 2023-01-04
 更新至2023年1月3日拉取版本。

## 2025-11-26
更新至2025-11-25日发布版本。

mk_api.dll 版本：8.0.0.1

[ZLMediakit api 2025-11-25 commit, git hash:7b2bd22](https://github.com/ZLMediaKit/ZLMediaKit/commit/7b2bd221eac4086358d081f4f0bfeb2a95b050f2)

