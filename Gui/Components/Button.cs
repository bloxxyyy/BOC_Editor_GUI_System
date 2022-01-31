
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Button : BaseComponent, ISelectable {

	private int margin = 4;
	private int borderSize = 2;

	public bool IsSelectable { get; set; }

	public string Text { get; set; } = "Button";
	public int FontSize { get; set; } = 16;

	/// <summary>
	/// To draw Text to the Window.
	/// </summary>
	public override void Draw(SpriteBatch sb) {

		sb.FillRectangle(new Rectangle(Position, DisplayedSize), Color.Black);

		var contentPos = new Point(Position.X + borderSize, Position.Y + borderSize);
		var contentSize = new Point(DisplayedSize.Width - (borderSize * 2), DisplayedSize.Height - (borderSize * 2));
		sb.FillRectangle(new Rectangle(contentPos, contentSize), Color.White);

		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(FontSize);
		var textPos = new Point2(Position.X + borderSize + margin, Position.Y + borderSize + margin);
		sb.DrawString(font18, Text, textPos, Color.Black);
	}

	/// <summary>
	/// To handle mouse input information and other nessesities.
	/// </summary>
	public override void Init() {
		DisplayedSize = GuiHelper.MeasureString(Text, FontSize);

		var width = DisplayedSize.Width + (margin * 2) + (borderSize * 2);
		var height = DisplayedSize.Height + (margin * 2) + (borderSize * 2);

		DisplayedSize = new Size(width, height);
	}

	public void OnClick() {
		throw new NotImplementedException();
	}
}
