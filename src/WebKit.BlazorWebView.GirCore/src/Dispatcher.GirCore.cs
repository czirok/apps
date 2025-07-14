using System.Collections.Concurrent;

namespace WebKit.BlazorWebView.GirCore;

/// <summary>
/// This class is a wrapper around the IDispatcher interface to provide
/// compatibility with the Microsoft.AspNetCore.Components.Dispatcher.
/// It allows the Blazor WebView to use the GLib.Internal.MainLoopSynchronizationContext.
/// This is necessary because the Blazor WebView uses a different
/// synchronization context than the one used by the Blazor framework.
/// </summary>
/// <param name="dispatcher"></param>
internal sealed class GirCoreDispatcher(IDispatcher dispatcher) : Microsoft.AspNetCore.Components.Dispatcher
{
	public override bool CheckAccess() => !dispatcher.IsDispatchRequired;
	public override Task InvokeAsync(Action workItem) => dispatcher.DispatchAsync(workItem);
	public override Task InvokeAsync(Func<Task> workItem) => dispatcher.DispatchAsync(workItem);
	public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem) => dispatcher.DispatchAsync(workItem);
	public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem) => dispatcher.DispatchAsync(workItem);
}

internal interface IDispatcher
{
	bool Dispatch(Action action);
	bool IsDispatchRequired { get; }
}

internal sealed class Dispatcher(SynchronizationContext context) : IDispatcher
{
	private readonly ThreadLocal<bool> _isOnDispatcherThread = new(() =>
		SynchronizationContext.Current == context);

	public bool IsDispatchRequired => !_isOnDispatcherThread.Value;

	public bool Dispatch(Action action)
	{
		if (!IsDispatchRequired)
		{
			action();
			return true;
		}

		context.Post(static state => ((Action)state!)(), action);
		return true;
	}

	public void Dispose()
	{
		_isOnDispatcherThread.Dispose();
	}
}

internal static class TaskCompletionSourcePool<T>
{
	private static readonly ConcurrentQueue<TaskCompletionSource<T>> Pool = new();

	public static TaskCompletionSource<T> Rent() =>
		Pool.TryDequeue(out var tcs) ? tcs : new TaskCompletionSource<T>();

	public static void Return(TaskCompletionSource<T> tcs)
	{
		if (tcs.Task.IsCompleted)
			Pool.Enqueue(new TaskCompletionSource<T>());
	}
}

internal static class DispatcherExtensions
{
	public static Task DispatchAsync(this IDispatcher dispatcher, Action action)
	{
		if (!dispatcher.IsDispatchRequired)
		{
			action();
			return Task.CompletedTask;
		}

		var tcs = TaskCompletionSourcePool<bool>.Rent();
		dispatcher.Dispatch(() =>
		{
			try
			{
				action();
				tcs.SetResult(true);
			}
			catch (Exception e)
			{
				tcs.SetException(e);
			}
			finally
			{
				TaskCompletionSourcePool<bool>.Return(tcs);
			}
		});
		return tcs.Task;
	}

	public static Task<T> DispatchAsync<T>(this IDispatcher dispatcher, Func<T> func)
	{
		if (!dispatcher.IsDispatchRequired)
		{
			return Task.FromResult(func());
		}

		var tcs = TaskCompletionSourcePool<T>.Rent();
		dispatcher.Dispatch(() =>
		{
			try
			{
				var result = func();
				tcs.SetResult(result);
			}
			catch (Exception e)
			{
				tcs.SetException(e);
			}
			finally
			{
				TaskCompletionSourcePool<T>.Return(tcs);
			}
		});
		return tcs.Task;
	}

	public static Task<T> DispatchAsync<T>(this IDispatcher dispatcher, Func<Task<T>> funcTask)
	{
		if (!dispatcher.IsDispatchRequired)
		{
			return funcTask();
		}

		var tcs = TaskCompletionSourcePool<T>.Rent();
		dispatcher.Dispatch(async () =>
		{
			try
			{
				var result = await funcTask().ConfigureAwait(false);
				tcs.SetResult(result);
			}
			catch (Exception e)
			{
				tcs.SetException(e);
			}
			finally
			{
				TaskCompletionSourcePool<T>.Return(tcs);
			}
		});
		return tcs.Task;
	}

	public static Task DispatchAsync(this IDispatcher dispatcher, Func<Task> funcTask) =>
		dispatcher.DispatchAsync(async () => { await funcTask().ConfigureAwait(false); return true; });
}