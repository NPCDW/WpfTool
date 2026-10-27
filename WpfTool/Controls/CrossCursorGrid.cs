using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;

namespace WpfTool.Controls;

/// <summary>
///     截图窗口的根容器：WinUI 3 里 UIElement.ProtectedCursor 只能由子类设置，
///     所以单独做一个能换成十字光标的 Grid
/// </summary>
public sealed class CrossCursorGrid : Grid
{
    public CrossCursorGrid()
    {
        ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Cross);
    }
}
