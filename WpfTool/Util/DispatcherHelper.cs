using System;
using System.Threading.Tasks;

namespace WpfTool.Util;

internal static class DispatcherHelper
{
    /// <summary>
    ///     让出当前线程，把排队的界面刷新跑完（对应 WPF 版本的 DoEvents）
    /// </summary>
    public static void DoEvents()
    {
        var dispatcher = WindowHelper.Dispatcher;
        if (dispatcher.HasThreadAccess)
        {
            var tcs = new TaskCompletionSource();
            if (!dispatcher.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () => tcs.TrySetResult()))
                return;
            tcs.Task.Wait(TimeSpan.FromMilliseconds(200));
        }
    }

    public static void Invoke(Action action)
    {
        var dispatcher = WindowHelper.Dispatcher;
        if (dispatcher.HasThreadAccess)
            action();
        else
            dispatcher.TryEnqueue(() => action());
    }
}
