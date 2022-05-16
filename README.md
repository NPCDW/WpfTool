该项目由 [https://github.com/NPCDW/WindowsFormsOCR](https://github.com/NPCDW/WindowsFormsOCR) 重写而来

此项目调用 `TencentCloud` 和 `BaiduCloud` 实现的 `OCR` 以及翻译

* 支持截图文字识别
* 支持划词翻译
* 支持截图翻译

划词（屏幕取词）是采用模拟发送 `Ctrl+C` ，然后再从剪切板取词

保持后台进程，内存占用大概在3-4M左右
