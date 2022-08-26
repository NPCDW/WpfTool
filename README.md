该项目由 [https://github.com/NPCDW/WindowsFormsOCR](https://github.com/NPCDW/WindowsFormsOCR) 重写而来

此项目调用 `TencentCloud` 和 `BaiduCloud` `BaiduAI` 实现的 `OCR` 以及翻译，腾讯百度每月都有免费额度

* 支持截图文字识别
* 支持划词翻译，先用鼠标选中一段文字，然后按 F2 或自定义的快捷键，会弹窗进行翻译
* 支持截图翻译
* 支持对任意窗口置顶/取消置顶，将鼠标移动到任意窗口的标题栏处（窗口最上方，有最大最小关闭按钮的地方），按 F6 或你自己设置的快捷键，即可实现窗口置顶/取消置顶，可置顶多个窗口
* 支持提取 `Word` 中的图片和附件

划词（屏幕取词）是采用模拟发送 `Ctrl+C` ，然后再从剪切板取词

保持后台进程，内存占用大概在3-4M左右
