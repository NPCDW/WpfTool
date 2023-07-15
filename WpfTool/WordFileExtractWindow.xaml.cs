using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Wpf.Ui.Controls;
using WpfTool.Util;
using MessageBox = System.Windows.MessageBox;

namespace WpfTool;

/// <summary>
///     WordFileExtractWindow.xaml 的交互逻辑
/// </summary>
public partial class WordFileExtractWindow
{
    public WordFileExtractWindow()
    {
        InitializeComponent();
    }

    private void ExtractButton_Click(object sender, RoutedEventArgs e)
    {
        ExtractButton.IsEnabled = false;
        DispatcherHelper.DoEvents();

        var filename = FilepathInput.Text;
        if (filename == string.Empty)
        {
            MessageBox.Show(FindResource("WordFileExtractWindow_WordNotFoundMessage") as string);
            return;
        }

        Task.Factory.StartNew(() =>
        {
            ConsoleOutput(FindResource("WordFileExtractWindow_WordFileSelectedOutput") + filename);
            var dir = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename);
            if (!Directory.Exists(dir))
            {
                ConsoleOutput(FindResource("WordFileExtractWindow_MkdirOutput") as string);
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
                    ConsoleOutput(FindResource("WordFileExtractWindow_ExtractImageOutput") + imageName);
                    picture!.Image.Save(dir + "\\" + imageName, ImageFormat.Png);
                    imageIndex++;
                }
                else if (docObject.DocumentObjectType == DocumentObjectType.OleObject)
                {
                    var ole = docObject as DocOleObject;
                    var s = ole!.ObjectType;
                    string oleName;
                    //"AcroExch.Document.11"是指PDF对象对应的ProgID
                    if (s == "AcroExch.Document.11")
                        oleName = @"File-" + fileIndex + ".pdf";
                    //"Excel.Sheet.12"是指 Excel03之后的工作表对应的ProgID
                    else if (s == "Excel.Sheet.12")
                        oleName = @"File-" + fileIndex + ".xlsx";
                    //"Word.Document.12"是指03之后的Word对应的ProgID
                    else if (s == "Word.Document.12")
                        oleName = @"File-" + fileIndex + ".docx";
                    else
                        oleName = @"File-" + fileIndex + "." + s;
                    ConsoleOutput(FindResource("WordFileExtractWindow_ExtractFileOutput") + oleName);
                    File.WriteAllBytes(dir + "\\" + oleName, ole.NativeData);
                    fileIndex++;
                }

            ConsoleOutput(FindResource("WordFileExtractWindow_ExtractFinishOutput") + dir);
        }).ContinueWith(_ => { ExtractButton.Dispatcher.Invoke(delegate { ExtractButton.IsEnabled = true; }); });
    }

    private void ConsoleOutput(string? text)
    {
        ConsoleOutputTextBox.Dispatcher.Invoke(delegate
        {
            ConsoleOutputTextBox.AppendText(text + Environment.NewLine);
            ConsoleOutputTextBox.ScrollToEnd();
        });
        DispatcherHelper.DoEvents();
    }

    private void SelectFile_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = FindResource("WordFileExtractWindow_SelectFileDoc") + " (*.doc;*.docx)|*.doc;*.docx|" +
                     FindResource("WordFileExtractWindow_SelectFileAll") + " (*.*)|*.*",
            Multiselect = false
        };
        var result = openFileDialog.ShowDialog();
        if (result == true)
        {
            string[] fileNames = openFileDialog.FileNames;
            FilepathInput.Text = fileNames[0];
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Utils.FlushMemory();
    }
}