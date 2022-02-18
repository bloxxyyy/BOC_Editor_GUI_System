using Apos.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public static class GuiHelper {
	private static OrthographicCamera? _uiCamera;

	public static Vector2 ScreenToWorldSpace(in Point point) {
		Matrix invertedMatrix = Matrix.Invert(GetUICamera().GetViewMatrix());
		return Vector2.Transform(point.ToVector2(), invertedMatrix);
	}

	public static Point Mouse => ScreenToWorldSpace(InputHelper.NewMouse.Position).ToPoint();

	/// <summary>
	/// Taken from Apos.Gui.
	/// Measures text using a font with a given size. The line height is the same no matter the text content.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	public static Size MeasureString(string text, int size) {
		var font = FontHelper.GetFont(size);
		return new Size((int)font.MeasureString(text).X, font.FontSize * CountLines(text));
	}

	/// <summary>
	/// Grab the amount of lines in a piece of text.
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	private static int CountLines(string text) {
		// https://stackoverflow.com/a/40928366/1710293
		// A piece of text is always atleast 1 line long.
		if (text == null || text == string.Empty)
			return 1;
		int index = -1;
		int count = 0;
		while (-1 != (index = text.IndexOf('\n', index + 1)))
			count++;

		return count + 1;
	}

	public static OrthographicCamera GetUICamera() {
		if (_uiCamera is not null)
			return _uiCamera;
		throw new ArgumentNullException($"Instance: {_uiCamera} in GuiHelper not allowed to be null", nameof(_uiCamera));
	}

	public static void SetUICamera(OrthographicCamera value) => _uiCamera = value;
}
