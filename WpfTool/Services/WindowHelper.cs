using System;
using System.Collections.Generic;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace WpfTool;

/// <summary>
///     窗口复用与跨窗口调用的小工具：原 WPF 版本靠遍历 Application.Current.Windows，
///     WinUI 3 里需要自己记住已打开的窗口
/// </summary>
internal static class WindowHelper
{
    private static readonly Dictionary<Type, Window> OpenWindows = new();

    public static DispatcherQueue Dispatcher => App.Current.MainWindow.DispatcherQueue;

    /// <summary>
    ///     单例打开：已存在则激活，否则创建
    /// </summary>
    public static T Activate<T>(params object[] args) where T : Window
    {
        T window;
        if (OpenWindows.TryGetValue(typeof(T), out var existing) && existing is T typed)
        {
            window = typed;
        }
        else
        {
            window = (T)Activator.CreateInstance(typeof(T), args)!;
            Track(window);
        }

        window.Activate();
        Restore(window);
        return window;
    }

    public static IEnumerable<Window> GetOpenWindows()
    {
        return OpenWindows.Values;
    }

    public static T? Find<T>() where T : Window
    {
        return OpenWindows.TryGetValue(typeof(T), out var existing) ? existing as T : null;
    }

    public static void Track(Window window)
    {
        OpenWindows[window.GetType()] = window;
        window.Closed += (_, _) =>
        {
            if (OpenWindows.TryGetValue(window.GetType(), out var current) && ReferenceEquals(current, window))
                OpenWindows.Remove(window.GetType());
        };
    }

    public static void Restore(Window window)
    {
        if (window.AppWindow.Presenter is OverlappedPresenter presenter &&
            presenter.State == OverlappedPresenterState.Minimized)
            presenter.Restore();
        window.AppWindow.MoveInZOrderAtTop();
    }
}
