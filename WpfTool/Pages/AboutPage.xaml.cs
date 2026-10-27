using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WpfTool.Util;
using Windows.ApplicationModel.DataTransfer;

namespace WpfTool.Pages;

public sealed partial class AboutPage : Page
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        VersionHeader.Text = Strings.Setting_Version;
        LicenseHeader.Text = Strings.Setting_license;
        CopyrightHeader.Text = Strings.Setting_AllRightsReserved;
        SourceCodeHeader.Text = Strings.Setting_SourceCode;
        FeedbackHeader.Text = Strings.Setting_feedback;
        LinksHeader.Text = Strings.Setting_Links;
    }

    private void EmailButton_Click(object sender, RoutedEventArgs e)
    {
        var package = new DataPackage();
        package.SetText("chenyongli0520@gmail.com");
        Clipboard.SetContent(package);
        DialogService.Show(Strings.Setting_CopyEmailMessage, Strings.Setting_Title);
    }
}
