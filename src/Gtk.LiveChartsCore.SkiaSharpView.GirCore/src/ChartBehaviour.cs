// Copyright (c) Ferenc Czirok
// Licensed under the MIT License.
// See the LICENSE file in the root for more information.

using LiveChartsCore;
using LiveChartsCore.Behaviours.Events;
using LiveChartsCore.Drawing;
using System.Reflection;
using EventArgs = LiveChartsCore.Behaviours.Events.EventArgs;

namespace Gtk.LiveChartsCore.SkiaSharpView.GirCore;

public partial class ChartBehaviour
{
	double lastX = 0;
	double lastY = 0;

	public event PressedHandler? Pressed;
	public event ScreenHandler? Update;
	public event PressedHandler? Release;
	public event ScreenHandler? Motion;
	public event Handler? Leave;
	public event ScrollHandler? Scroll;
	public event PinchHandler? ScaleChanged;

	public void On(Widget widget)
	{
		var click = GestureClick.New();
		var motion = EventControllerMotion.New();
		var scroll = EventControllerScroll.New(EventControllerScrollFlags.Vertical);
		var zoom = GestureZoom.New();

		click.OnPressed += OnPressed;
		click.OnUpdate += OnUpdate;
		click.OnReleased += OnRelease;
		motion.OnMotion += OnMotion;
		motion.OnLeave += OnLeave;
		scroll.OnScroll += OnScroll;
		zoom.OnScaleChanged += OnScaleChanged;

		widget.AddController(click);
		widget.AddController(motion);
		widget.AddController(scroll);
		widget.AddController(zoom);
	}

	protected void OnPressed(GestureClick gesture, GestureClick.PressedSignalArgs args)
	{
		gesture.GetPoint(null, out lastX, out lastY);
		var location = new LvcPoint(lastX, lastY);
		var isSecondaryPress = args.NPress > 1;
		Pressed?.Invoke(gesture, new PressedEventArgs(location, isSecondaryPress, args));
	}

	private void OnUpdate(Gesture gesture, Gesture.UpdateSignalArgs args)
	{
		gesture.GetPoint(null, out lastX, out lastY);
		var location = new LvcPoint(lastX, lastY);
		Update?.Invoke(gesture, new ScreenEventArgs(location, args));
	}

	private void OnRelease(GestureClick gesture, GestureClick.ReleasedSignalArgs args)
	{
		gesture.GetPoint(null, out lastX, out lastY);
		var location = new LvcPoint(lastX, lastY);
		var isSecondaryPress = args.NPress > 1;
		Release?.Invoke(gesture, new PressedEventArgs(location, isSecondaryPress, args));
	}

	private void OnMotion(EventControllerMotion controller, EventControllerMotion.MotionSignalArgs args)
	{
		lastX = args.X;
		lastY = args.Y;
		var location = new LvcPoint(lastX, lastY);
		Motion?.Invoke(controller, new ScreenEventArgs(location, args));
	}

	private void OnLeave(EventControllerMotion controller, System.EventArgs args)
	{
		Leave?.Invoke(controller, new EventArgs(args));
	}

	private bool OnScroll(EventControllerScroll controller, EventControllerScroll.ScrollSignalArgs args)
	{
		var wheelStart = new LvcPoint(lastX, lastY);
		Scroll?.Invoke(controller, new ScrollEventArgs(wheelStart, args.Dy, args));
		return false;
	}

	private void OnScaleChanged(GestureZoom gesture, GestureZoom.ScaleChangedSignalArgs args)
	{
		gesture.GetPoint(null, out lastX, out lastY);
		var location = new LvcPoint(lastX, lastY);
		ScaleChanged?.Invoke(gesture, new PinchEventArgs((float)args.Scale, location, args));
	}
}

public static class ChartExtensions
{
	private static Action<Chart, LvcPoint, bool>? _cachedPressedDelegate;
	private static Action<Chart, LvcPoint, bool>? _cachedReleasedDelegate;
	private static Action<Chart, LvcPoint>? _cachedMotionDelegate;
	private static Action<Chart>? _cachedLeaveDelegate;

	public static void InvokePressed(this Chart chart, LvcPoint point, bool isSecondary)
	{
		if (_cachedPressedDelegate == null)
		{
			var method = typeof(Chart).GetMethod("InvokePointerDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedPressedDelegate = (Action<Chart, LvcPoint, bool>)
				Delegate.CreateDelegate(typeof(Action<Chart, LvcPoint, bool>), method);
		}
		_cachedPressedDelegate(chart, point, isSecondary);
	}

	public static void InvokeReleased(this Chart chart, LvcPoint point, bool isSecondary)
	{
		if (_cachedReleasedDelegate == null)
		{
			var method = typeof(Chart).GetMethod("InvokePointerUp", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedReleasedDelegate = (Action<Chart, LvcPoint, bool>)
				Delegate.CreateDelegate(typeof(Action<Chart, LvcPoint, bool>), method);
		}
		_cachedReleasedDelegate(chart, point, isSecondary);
	}

	public static void InvokeMotion(this Chart chart, LvcPoint point)
	{
		if (_cachedMotionDelegate == null)
		{
			var method = typeof(Chart).GetMethod("InvokePointerMove", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedMotionDelegate = (Action<Chart, LvcPoint>)
				Delegate.CreateDelegate(typeof(Action<Chart, LvcPoint>), method);
		}
		_cachedMotionDelegate(chart, point);
	}

	public static void InvokeLeave(this Chart chart)
	{
		if (_cachedLeaveDelegate == null)
		{
			var method = typeof(Chart).GetMethod("InvokePointerLeft", BindingFlags.NonPublic | BindingFlags.Instance)!;
			_cachedLeaveDelegate = (Action<Chart>)
				Delegate.CreateDelegate(typeof(Action<Chart>), method);
		}
		_cachedLeaveDelegate(chart);
	}
}