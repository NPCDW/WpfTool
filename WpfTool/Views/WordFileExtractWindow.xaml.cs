using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Windows.Storage.Pickers;
using WpfTool.Util;
using WinRT.Interop;

namespace WpfTool.Views;

/// <summary>
///     Word 图片 / 附件提取
/// </summary>
public sealed partial class WordFileExtractWindow : Window
{
    public WordFileExtractWindow()
    {
        InitializeComponent();
        Title = Strings.WordFileExtractWindow_Title;
        AppTitleTextBlock.Text = Strings.WordFileExtractWindow_Title;
        FilePathHeader.Text = Strings.WordFileExtractWindow_WordFilePath;
        SelectFileButton.Content = Strings.WordFileExtractWindow_SelectedFile;
        ExtractButton.Content = Strings.WordFileExtractWindow_Extract;

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        var handle = WindowNative.GetWindowHandle(this);
        var scale = ScreenshotHelper.GetScaleFactor(handle);
        AppWindow.Resize(new Windows.Graphics.SizeInt32((int)(800 * scale), (int)(650 * scale)));
    }

    private async void SelectFileButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        picker.FileTypeFilter.Add(".doc");
        picker.FileTypeFilter.Add(".docx");
        picker.FileTypeFilter.Add("*");

        var file = await picker.PickSingleFileAsync();
        if (file != null) FilepathInput.Text = file.Path;
    }

    private void ExtractButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = FilepathInput.Text;
        if (string.IsNullOrEmpty(filename))
        {
            DialogService.Show(Strings.WordFileExtractWindow_WordNotFoundMessage);
            return;
        }

        ExtractButton.IsEnabled = false;
        ConsoleOutputTextBox.Text = "";

        Task.Run(() =>
        {
            ConsoleOutput(Strings.WordFileExtractWindow_WordFileSelectedOutput + filename);
            var dir = Path.Combine(Path.GetDirectoryName(filename)!, Path.GetFileNameWithoutExtension(filename));
            if (!Directory.Exists(dir))
            {
                ConsoleOutput(Strings.WordFileExtractWindow_MkdirOutput);
                Directory.CreateDirectory(dir);
            }

            var document = new Document(filename);

            var imageIndex = 0;
            var fileIndex = 0;
            foreach (Section section in document.Sections)
            foreach (Paragraph paragraph in section.Paragraphs)
            foreach (DocumentObject docObject in paragraph.ChildObjects)
                if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                {
                    var picture = docObject as DocPicture;
                    var imageName = string.Format(@"Image-{0}.png", imageIndex);
                    ConsoleOutput(Strings.WordFileExtractWindow_ExtractImageOutput + imageName);
                    picture!.Image.Save(Path.Combine(dir, imageName), ImageFormat.Png);
                    imageIndex++;
                }
                else if (docObject.DocumentObjectType == DocumentObjectType.OleObject)
                {
                    var ole = docObject as DocOleObject;
                    var objectType = ole!.ObjectType;
                    string oleName;
                    //"AcroExch.Document.11"是指PDF对象对应的ProgID
                    if (objectType == "AcroExch.Document.11")
                        oleName = @"File-" + fileIndex + ".pdf";
                    //"Excel.Sheet.12"是指 Excel03之后的工作表对应的ProgID
                    else if (objectType == "Excel.Sheet.12")
                        oleName = @"File-" + fileIndex + ".xlsx";
                    //"Word.Document.12"是指03之后的Word对应的ProgID
                    else if (objectType == "Word.Document.12")
                        oleName = @"File-" + fileIndex + ".docx";
                    else
                        oleName = @"File-" + fileIndex + "." + objectType;
                    ConsoleOutput(Strings.WordFileExtractWindow_ExtractFileOutput + oleName);
                    File.WriteAllBytes(Path.Combine(dir, oleName), ole.NativeData);
                    fileIndex++;
                }

            ConsoleOutput(Strings.WordFileExtractWindow_ExtractFinishOutput + dir);
        }).ContinueWith(_ => DispatcherHelper.Invoke(() =>
        {
            ExtractButton.IsEnabled = true;
            Utils.FlushMemory();
        }));
    }

    private void ConsoleOutput(string? text)
    {
        DispatcherHelper.Invoke(() =>
        {
            ConsoleOutputTextBox.Text += text + Environment.NewLine;
            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
        });
    }
}
