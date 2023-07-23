using System;
using System.Windows.Threading;

namespace WpfTool.Util;

internal static class DispatcherHelper
{
    public static void DoEvents()
    {
        var frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            new DispatcherOperationCallback(ExitFrames), frame);
        try
        {
            Dispatcher.PushFrame(frame);
        }
        catch (InvalidOperationException)
        {
        }
    }

    private static object? ExitFrames(object frame)
    {
        ((DispatcherFrame)frame).Continue = false;
        return null;
    }
}