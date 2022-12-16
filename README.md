# WpfTool

该项目由 [https://github.com/NPCDW/WindowsFormsOCR](https://github.com/NPCDW/WindowsFormsOCR) 重写而来

此项目调用 `TencentCloud` 和 `BaiduCloud` `BaiduAI` 实现的 `OCR` 以及翻译，腾讯云和百度云每月都有免费额度

# 功能

* 支持多屏幕截图文字识别，默认快捷键 `F4`
* 支持划词翻译，默认快捷键 `F2` ，先用鼠标选中一段文字，然后按下快捷键，会弹窗进行翻译
* 支持多屏幕截图翻译，默认快捷键 `Ctrl + F2`
* 支持对任意窗口置顶和取消置顶，默认快捷键 `F6` ，将鼠标移动到任意窗口的标题栏处（窗口最上方，有最大最小关闭按钮的地方），然后按下快捷键，即可实现窗口置顶和取消置顶，可置顶多个窗口，但无法置顶 UWP 应用，推荐项目 [PowerToys](https://github.com/microsoft/PowerToys)
* 支持提取 `Word` 中的图片和附件，无快捷键，可右键任务栏中的图标

# 说明

多屏幕截图文字识别或翻译：只会对鼠标所在的屏幕进行截图

如何取消截图？按 `ESC` 或 单击鼠标右键即可

划词（屏幕取词）是采用模拟发送 `Ctrl+C` ，然后再从剪切板取词

保持后台进程，内存占用大概在3-4M左右
