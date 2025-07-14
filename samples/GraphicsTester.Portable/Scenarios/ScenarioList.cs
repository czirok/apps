namespace GraphicsTester.Scenarios
{
	public static class ScenarioList
	{
		private static List<AbstractScenario> _scenarios = default!;

		public static List<AbstractScenario> Scenarios
		{
			get
			{
				_scenarios ??=
					[
						new DrawLines(),
						new DrawLinesScaled(),
						new DrawRectangles(),
						new DrawEllipses(),
						new DrawRoundedRectangles(),
						new DrawArcs(),
						new DrawArcs(true),
						new ArcScenario1(true),
						new ArcScenario2(),
						new DrawPaths(),
						new DrawPathsScaled(),
						new DrawFlattenedPaths(),
						// new DrawImages(), // Not supported in GirCore
						new DrawTextAtPoint(),
						new DrawTextRotatedAtPoint(),
						new DrawLongTextInRect(),
						new DrawLongTextInRectWithoutOverflow(),
						new DrawLongTextInRectWithOverflow(),
						new DrawShortTextInRect(),
						new DrawShortTextInRect2(),
						new DrawVerticallyCenteredText(),
						new DrawVerticallyCenteredText2(),
						new DrawHorizontallyCenteredTextWithSimpleApi(),
						new DrawMarkdigAttributedText(),
						new FillRectangles(),
						new FillEllipses(),
						new FillRoundedRectangles(),
						new FillArcs(),
						new FillArcs(true),
						new FillPaths(),
						new TestPattern1(),
						new TestPattern2(),
						new TransformRotateFromOrigin(),
						new LineJoins(),
						new StrokeLocations(),
						// new ImageFills(), Not supported in GirCore.
						new PatternFills(),
						new RadialGradientInCircle(),
						new RectangleWithZeroStroke(),
						new SimpleShadowTest(),
						new MultipleShadowTest(),
						new ArcDirection(),
						new ArcDirection(true),
						new FilledArcDirection(),
						new ClipRect(),
						new SubtractFromClip(),
						new DimensionTest(),
						new ScaleCanvas(),
					];

				return _scenarios;
			}
		}
	}
}
