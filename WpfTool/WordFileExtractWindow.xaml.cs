using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Section = Spire.Doc.Section;
using Paragraph = Spire.Doc.Documents.Paragraph;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using TencentCloud.Cwp.V20180228.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WpfTool
{
    /// <summary>
    /// WordFileExtractWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WordFileExtractWindow : Window
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
                MessageBox.Show("请先选择Word文件");
                return;
            }

            Task.Factory.StartNew(() =>
            {
                ConsoleOutput("Word文件选择：" + filename);
                String dir = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename);
                if (!Directory.Exists(dir))
                {
                    ConsoleOutput("Word文件同名文件夹不存在，创建同名文件夹");
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
                                DocPicture picture = docObject as DocPicture;
                                String imageName = String.Format(@"Image-{0}.png", imageIndex);
                                ConsoleOutput("提取图片：" + imageName);
                                picture.Image.Save(dir + "\\" + imageName, System.Drawing.Imaging.ImageFormat.Png);
                                imageIndex++;
                            }
                            else if (docObject.DocumentObjectType == DocumentObjectType.OleObject)
                            {
                                DocOleObject Ole = docObject as DocOleObject;
                                string s = Ole.ObjectType;
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
                                ConsoleOutput("提取附件：" + oleName);
                                File.WriteAllBytes(dir + "\\" + oleName, Ole.NativeData);
                                fileIndex++;
                            }
                        }
                    }
                }
                ConsoleOutput("提取完成，提取位置：" + dir);
            }).ContinueWith(result =>
            {
                ExtractButton.Dispatcher.Invoke(new Action(delegate
                {
                    ExtractButton.IsEnabled = true;
                }));
            });

        }

        private void ConsoleOutput(string text)
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
                Filter = "Word文档 (*.doc;*.docx)|*.doc;*.docx|所有文件 (*.*)|*.*",
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
