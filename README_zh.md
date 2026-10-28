# WpfTool

一个常驻托盘的 Windows 文字识别 / 翻译小工具，基于 **WinUI 3** 构建。

项目最早由 [NPCDW/WindowsFormsOCR](https://github.com/NPCDW/WindowsFormsOCR) 重写为 WPF 版本，现已迁移到 WinUI 3（Windows App SDK），并把与界面无关的逻辑抽成共享的 Core 类库。

此项目调用云平台开放接口实现文字识别以及翻译，目前仅支持下方所列的平台

免费额度：
- 文字识别
    - 腾讯云 [https://cloud.tencent.com/document/product/866/35945](https://cloud.tencent.com/document/product/866/35945)
    - 百度云 [https://cloud.baidu.com/doc/OCR/s/fk3h7xu7h](https://cloud.baidu.com/doc/OCR/s/fk3h7xu7h)
    - SpaceOCR [https://ocr.space/OCRAPI](https://ocr.space/OCRAPI)
- 翻译
    - 百度AI [https://fanyi-api.baidu.com/product/113](https://fanyi-api.baidu.com/product/113)
    - 谷歌翻译（免费不限量，但不是官方接入点）
    - Deeplx 使用 [linux.do](https://linux.do/) 或者 [自建](https://deeplx.owo.network/)
    - AI 翻译（任意 OpenAI 兼容接口，支持多个服务商，每个服务商下可配置多个模型）

# 功能

* 支持多屏幕截图文字识别，默认快捷键 `F4`
* 支持划词翻译，默认快捷键 `F2` ，先用鼠标选中一段文字，然后按下快捷键，会弹窗进行翻译

# 说明

多屏幕截图文字识别：只会对鼠标所在的屏幕进行截图

如何取消截图？按 `ESC` 或 单击鼠标右键即可

划词（屏幕取词）是采用模拟发送 `Ctrl+Insert` ，然后再从剪切板取词

保持后台进程，内存占用大概在 `3-4M` 左右

# 目录结构

```
WpfTool.sln
├─ WpfTool.Core   # net8.0-windows 类库：配置、云服务、P/Invoke、公共工具
└─ WpfTool        # WinUI 3 应用（Windows App SDK，非打包模式）
   ├─ Views       # 窗口：隐藏热键宿主 / 截图 / 结果 / 设置
   ├─ Pages       # 设置页，由 NavigationView 承载
   ├─ Services    # 托盘图标、主题、窗口登记
   ├─ Util        # 热键、截图与 DPI、多语言访问
   └─ Strings     # resx 资源（zh-CN、en-US）
```

# 构建

需要 .NET 8 SDK，Windows 10 1809 及以上（Windows App SDK 1.6）。

```
dotnet build WpfTool.sln -c Release -p:Platform=x64
```

# 升级

配置文件存储路径：
- 用户目录（默认），可直接将旧版本程序文件夹删除，然后解压更换新版本程序
- 程序目录，请先将旧版本程序文件夹中的 `WpfTool/Config/Setting.json` 取出，然后删除旧版本程序文件夹，再解压新的程序文件夹，并将 `Setting.json` 覆盖

配置结构保持兼容，仅新增 `Common.theme`（默认 `Default`，即跟随系统）。
