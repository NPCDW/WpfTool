# WpfTool

[简体中文](README_zh.md)

This project is rewritten from [NPCDW/WindowsFormsOCR](https://github.com/NPCDW/WindowsFormsOCR) .

This project calls the open interface of the cloud platform to realize text recognition and translation, and currently only supports the platforms listed below.

Free Quota:
- OCR
    - Tencent Cloud [https://cloud.tencent.com/document/product/866/35945](https://cloud.tencent.com/document/product/866/35945)
    - Baidu Cloud [https://cloud.baidu.com/doc/OCR/s/fk3h7xu7h](https://cloud.baidu.com/doc/OCR/s/fk3h7xu7h)
    - SpaceOCR [https://ocr.space/OCRAPI](https://ocr.space/OCRAPI)
- Translate
    - Tencent Cloud [https://cloud.tencent.com/document/product/551/35017](https://cloud.tencent.com/document/product/551/35017)
    - Baidu AI [https://fanyi-api.baidu.com/product/113](https://fanyi-api.baidu.com/product/113)
    - Google Translate (free and unlimited, but unofficial endpoint)

# Features

* Support multi-screenshot text recognition, the default shortcut key is `F4`
* Support word translation, the default shortcut key is `F2`, first select a piece of text with the mouse, and then press the shortcut key, a window will pop up for translation
* Support multi-screenshot translation, the default shortcut key is `Ctrl + F2`
* Supports setting and unpinning any window. The default shortcut key is `F6`. How to use it: move the mouse to the title bar of any window (at the top of the window, where there are the largest and smallest close buttons), and then press the shortcut key , the window can be pinned and unpinned, and multiple windows can be pinned, but UWP applications cannot be pinned. Recommended project [microsoft/PowerToys](https://github.com/microsoft/PowerToys)
* Support extracting pictures and attachments in `Word`, there is no shortcut key available, you can right-click the icon in the taskbar

# IMPORTANT

Multi-screen capture text recognition or translation: only the screen where the mouse is placed will be captured

How to cancel screenshot? Press `ESC` or right-click

Marking words (taking words from the screen) is to send `Ctrl+C` by analog, and then get words from the clipboard

Keep the background process, the memory usage is about `3-4M`

# Upgrade

Configuration file storage path:
- User directory (default), you can directly delete the old version of the program folder, and then decompress and replace it with the new version of the program
- Program directory, please take out `WpfTool/Resources/Setting.json` in the old version program folder first, then delete the old version program folder, then decompress the new program folder, and overwrite `Setting.json`