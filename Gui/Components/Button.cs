using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Button : BaseComponent, ISelectable {
	public bool IsSelectable { get; set; } = true;
	public int FontSize { get; set; } = 16;
	public Action<ISelectable> OnClick { get; set; }

	private bool IsHeld = false;

	private Color? _bgColor { get => IsHeld ? Color.Gray : BackgroundColor; }
	private Color _borderColor { get => IsHeld ? Color.SlateGray : Color.Black; }

	public override void Update() {
		if (Default.MouseInteraction.Released(false)) {
			IsHeld = false;
		}

		if (Rectangle.Contains(GuiHelper.Mouse) && IsSelectable && Default.MouseInteraction.Pressed(false)) {
			IsHeld = true;
			OnClick?.Invoke(this);
		}
	}

	/// <summary>
	/// To draw Text to the Window.
	/// </summary>
	public override void Draw(SpriteBatch sb) {
		// draw border
		var width = DisplayedSize.Width + BorderSpace.Width;
		var height = DisplayedSize.Height + BorderSpace.Height;
		var display = new Size(width, height);
		var borderPos = new Point(Position.X + MarginalSpace.Left, Position.Y + MarginalSpace.Top);

		sb.FillRectangle(new Rectangle(borderPos, display), _borderColor);

		// draw button background
		var backgroundPos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);

		if (BackgroundColor is null)
			BackgroundColor = Color.White;

		if (_bgColor is not null)
			sb.FillRectangle(new Rectangle(backgroundPos, DisplayedSize), _bgColor.Value);

		// draw text
		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(FontSize);
		var textPos = new Vector2(backgroundPos.X + MarginalSpace.Left, backgroundPos.Y + MarginalSpace.Top);
		sb.DrawString(font18, Text, textPos, Color.Black);
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

