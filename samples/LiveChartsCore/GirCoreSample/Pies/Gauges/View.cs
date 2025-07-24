using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Pies.Gauges;

namespace GirCoreSample.Pies.Gauges;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly PieChart pieChart1;
	private readonly PieChart pieChart2;
	private readonly PieChart pieChart3;
	private readonly PieChart pieChart4;
	private readonly PieChart pieChart5;
	private readonly PieChart pieChart6;
	private readonly PieChart pieChart7;
	private readonly PieChart pieChart8;
	private readonly PieChart pieChart9;
	private readonly PieChart pieChart10;
	private readonly PieChart pieChart11;
	private readonly PieChart pieChart12;
	private readonly PieChart pieChart13;
	private readonly PieChart pieChart14;

	public View()
	{
		var viewModel = new ViewModel();
		pieChart1 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series1,
			MaxValue = viewModel.GaugeTotal1,
		};
		pieChart2 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series2,
			MaxValue = viewModel.GaugeTotal2,
			InitialRotation = viewModel.InitialRotation2
		};
		pieChart3 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series3,
			MaxValue = viewModel.GaugeTotal3,
			InitialRotation = viewModel.InitialRotation3
		};
		pieChart4 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series4,
			MaxValue = viewModel.GaugeTotal4,
			InitialRotation = viewModel.InitialRotation4
		};
		pieChart5 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series5,
			MaxValue = viewModel.GaugeTotal5,
			InitialRotation = viewModel.InitialRotation5
		};
		pieChart6 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series6,
			MaxValue = viewModel.GaugeTotal6,
			InitialRotation = viewModel.InitialRotation6
		};
		pieChart7 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series7,
			MaxValue = viewModel.GaugeTotal7,
			InitialRotation = viewModel.InitialRotation7
		};
		pieChart8 = new PieChart
		{
			HeightRequest = 200,
			WidthRequest = 200,
			Series = viewModel.Series8,
			MaxValue = viewModel.GaugeTotal8,
			InitialRotation = viewModel.InitialRotation8,
			MaxAngle = viewModel.MaxAngle8
		};

		pieChart9 = new PieChart
		{
			HeightRequest = 300,
			WidthRequest = 400,
			Series = viewModel.Series9,
			MaxValue = viewModel.GaugeTotal9,
			InitialRotation = viewModel.InitialRotation9,
			MaxAngle = viewModel.MaxAngle9
		};
		pieChart10 = new PieChart
		{
			HeightRequest = 300,
			WidthRequest = 400,
			Series = viewModel.Series10,
			MaxValue = viewModel.GaugeTotal10,
			InitialRotation = viewModel.InitialRotation10,
			MaxAngle = viewModel.MaxAngle10
		};
		pieChart11 = new PieChart
		{
			HeightRequest = 300,
			WidthRequest = 400,
			Series = viewModel.Series11,
			MaxValue = viewModel.GaugeTotal11,
			InitialRotation = viewModel.InitialRotation11,
			MaxAngle = viewModel.MaxAngle11
		};
		pieChart12 = new PieChart
		{
			HeightRequest = 300,
			WidthRequest = 400,
			Series = viewModel.Series12,
			MaxValue = viewModel.GaugeTotal12,
			InitialRotation = viewModel.InitialRotation12,
			MaxAngle = viewModel.MaxAngle12
		};
		pieChart13 = new PieChart
		{
			HeightRequest = 400,
			WidthRequest = 400,
			Series = viewModel.Series13,
			MaxValue = viewModel.GaugeTotal3,
			InitialRotation = viewModel.InitialRotation13,
			MaxAngle = viewModel.MaxAngle13
		};
		pieChart14 = new PieChart
		{
			HeightRequest = 400,
			WidthRequest = 400,
			Series = viewModel.Series14,
			MaxValue = viewModel.GaugeTotal14,
			InitialRotation = viewModel.InitialRotation14,
			MaxAngle = viewModel.MaxAngle14
		};


		var flowLayoutPanel1 = new Adw.WrapBox();

		var scroll = Gtk.ScrolledWindow.New();

		scroll.SetChild(flowLayoutPanel1);
		Child = scroll;

		flowLayoutPanel1.Append(pieChart1);
		flowLayoutPanel1.Append(pieChart2);
		flowLayoutPanel1.Append(pieChart3);
		flowLayoutPanel1.Append(pieChart4);
		flowLayoutPanel1.Append(pieChart5);
		flowLayoutPanel1.Append(pieChart6);
		flowLayoutPanel1.Append(pieChart7);
		flowLayoutPanel1.Append(pieChart8);
		flowLayoutPanel1.Append(pieChart9);
		flowLayoutPanel1.Append(pieChart10);
		flowLayoutPanel1.Append(pieChart11);
		flowLayoutPanel1.Append(pieChart12);
		flowLayoutPanel1.Append(pieChart13);
		flowLayoutPanel1.Append(pieChart14);
	}

	public override void Dispose()
	{
		pieChart1?.Dispose();
		pieChart2?.Dispose();
		pieChart3?.Dispose();
		pieChart4?.Dispose();
		pieChart5?.Dispose();
		pieChart6?.Dispose();
		pieChart7?.Dispose();
		pieChart8?.Dispose();
		pieChart9?.Dispose();
		pieChart10?.Dispose();
		pieChart11?.Dispose();
		pieChart12?.Dispose();
		pieChart13?.Dispose();
		pieChart14?.Dispose();

		base.Dispose();
	}
}
