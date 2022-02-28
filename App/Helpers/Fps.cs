using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace Koko.RunTimeGui;

public static class Fps {

	private static bool ShowFpsCounter { get; set; } = false;
	private static double frames = 0;
	private static double updates = 0;
	private static double last = 0;
	private static readonly double msgFrequency = 1.0f;
	private static string msg = "";

	public static void CheckKeyPress() {
		if (Default.F12Press.Pressed(true)) {
			string newState = !ShowFpsCounter ? "Enabled" : "Disabled";
			Debug.WriteLine($"{newState} FPS counter");
			ShowFpsCounter = !ShowFpsCounter;
		}
	}

	public static void Update(GameTime gameTime) {
		var now = gameTime.TotalGameTime.TotalSeconds;
		var elapsed = now - last;
		if (elapsed > msgFrequency) {
			msg = $" FPS: {frames / elapsed:0.##} \n Elapsed time: {elapsed:0.##}s \n Updates: {updates} \n Frames: {frames} ";
			frames = 0;
			updates = 0;
			last = now;
		}
		updates++;
	}

	public static void Draw(SpriteBatch sb) {
		if (ShowFpsCounter) {
			var textSize = GuiHelper.MeasureString(msg, 16);
			var textPos = new Point((int)GuiHelper.GetUICamera().BoundingRectangle.Right - textSize.Width, 0);
			sb.FillRectangle(new Rectangle(textPos, textSize), Color.Black);
			sb.DrawString(FontHelper.FontSystem.GetFont(16), msg, textPos.ToVector2(), Color.White);
		}

		frames++;
	}
}
