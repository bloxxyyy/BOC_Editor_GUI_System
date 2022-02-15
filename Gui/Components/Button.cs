using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace Koko.RunTimeGui;

public class Button : BaseComponent, ISelectable {
	public bool IsSelectable { get; set; } = true;

	public int FontSize { get; set; } = 16;
	public Action OnClick { get; set; }

	bool _clicked = false;
	public override void Update() {
		if (Rectangle.Contains(GuiHelper.Mouse) && Default.MouseInteraction.Pressed()) {
			_clicked = true;
		}

		if (IsSelectable && _clicked) {
			OnClick?.Invoke();
			_clicked = false;
		}
	}

	/// <summary>
	/// To draw Text to the Window.
	/// </summary>
	public override void Draw(SpriteBatch sb) {
		var width = DisplayedSize.Width + BorderSpace.Width;
		var height = DisplayedSize.Height + BorderSpace.Height;
		var display = new Size(width, height);
		var pos3 = new Point(Position.X + MarginalSpace.Left, Position.Y + MarginalSpace.Top);
		sb.FillRectangle(new Rectangle(pos3, display), Color.Black);

		var pos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
		if (BackgroundColor is null) BackgroundColor = Color.White;
			sb.FillRectangle(new Rectangle(pos, DisplayedSize), BackgroundColor.Value);

		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(FontSize);
		var pos2 = new Vector2(pos.X + MarginalSpace.Left, pos.Y + MarginalSpace.Top);
		sb.DrawString(font18, Text, pos2, Color.Black);
	}

	/// <summary>
	/// To handle mouse input information and other nessesities.
	/// </summary>
	public override void Init() {
		var size = GuiHelper.MeasureString(Text, FontSize);

		// account for the padding of the text from the border in the displayed size
		int width = size.Width + MarginalSpace.Width;
		int height = size.Height + MarginalSpace.Height;
		DisplayedSize = new Size(width, height);
	}
}

