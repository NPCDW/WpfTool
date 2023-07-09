using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = System.Windows.MessageBox;
using Paragraph = Spire.Doc.Documents.Paragraph;
using Path = System.IO.Path;
using Section = Spire.Doc.Section;

namespace WpfTool
{
    /// <summary>
    /// WordFileExtractWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WordFileExtractWindow : Wpf.Ui.Controls.UiWindow
    {
        public WordFileExtractWindow()
        {
            InitializeComponent();
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            ExtractButton.IsEnabled = false;
            DispatcherHelper.DoEvents();

            String filename = FilepathInput.Text;
            if (filename == string.Empty)
            {
                MessageBox.Show(this.FindResource("WordFileExtractWindow_WordNotFoundMessage") as String);
                return;
            }

            Task.Factory.StartNew(() =>
            {
                ConsoleOutput(this.FindResource("WordFileExtractWindow_WordFileSelectedOutput") + filename);
                String dir = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename);
                if (!Directory.Exists(dir))
                {
                    ConsoleOutput(this.FindResource("WordFileExtractWindow_MkdirOutput") as string);
                    Directory.CreateDirectory(dir);
                }

                Document document = new Document(filename);

                int imageIndex = 0;
                int fileIndex = 0;
                foreach (Section section in document.Sections)
                {
                    foreach (Paragraph paragraph in section.Paragraphs)
                    {
                        foreach (DocumentObject docObject in paragraph.ChildObjects)
                        {
                            if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                            {
                                DocPicture? picture = docObject as DocPicture;
                                String imageName = String.Format(@"Image-{0}.png", imageIndex);
                                ConsoleOutput(this.FindResource("WordFileExtractWindow_ExtractImageOutput") + imageName);
                                picture!.Image.Save(dir + "\\" + imageName, System.Drawing.Imaging.ImageFormat.Png);
                                imageIndex++;
                            }
                            else if (docObject.DocumentObjectType == DocumentObjectType.OleObject)
                            {
                                DocOleObject? Ole = docObject as DocOleObject;
                                string s = Ole!.ObjectType;
                                string oleName;
                                //"AcroExch.Document.11"是指PDF对象对应的ProgID
                                if (s == "AcroExch.Document.11")
                                {
                                    oleName = @"File-" + fileIndex + ".pdf";
                                }
                                //"Excel.Sheet.12"是指 Excel03之后的工作表对应的ProgID
                                else if (s == "Excel.Sheet.12")
                                {
                                    oleName = @"File-" + fileIndex + ".xlsx";
                                }
                                //"Word.Document.12"是指03之后的Word对应的ProgID
                                else if (s == "Word.Document.12")
                                {
                                    oleName = @"File-" + fileIndex + ".docx";
                                }
                                else
                                {
                                    oleName = @"File-" + fileIndex + "." + s;
                                }
                                ConsoleOutput(this.FindResource("WordFileExtractWindow_ExtractFileOutput") + oleName);
                                File.WriteAllBytes(dir + "\\" + oleName, Ole.NativeData);
                                fileIndex++;
                            }
                        }
                    }
                }
                ConsoleOutput(this.FindResource("WordFileExtractWindow_ExtractFinishOutput") + dir);
            }).ContinueWith(result =>
            {
                ExtractButton.Dispatcher.Invoke(new Action(delegate
                {
                    ExtractButton.IsEnabled = true;
                }));
            });

        }

        private void ConsoleOutput(string? text)
        {
            ConsoleOutputTextBox.Dispatcher.Invoke(new Action(delegate
            {
                ConsoleOutputTextBox.AppendText(text + Environment.NewLine);
                ConsoleOutputTextBox.ScrollToEnd();
            }));
            DispatcherHelper.DoEvents();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = this.FindResource("WordFileExtractWindow_SelectFileDoc") + " (*.doc;*.docx)|*.doc;*.docx|" + this.FindResource("WordFileExtractWindow_SelectFileAll") + " (*.*)|*.*",
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
}
