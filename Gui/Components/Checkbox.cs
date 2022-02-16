using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Checkbox : BaseComponent, ISelectable
{
	private int TextFontSize { get; set; } = 16;
	private Size CheckboxSize { get => new(TextFontSize, TextFontSize); }
	private Color FontCol { get; set; } = Color.Black;
	public bool IsSelectable { get; set; } = true;
	public bool IsChecked { get; private set; }
	public Action OnClick { get; set; }

	public override void Draw(SpriteBatch sb)
	{
		// check if background color is set and opaque and draw border
		if (BackgroundColor is not null &&
			BackgroundColor.Value.A == 0xff &&
			BorderSpace.Width + BorderSpace.Height != 0)
		{
			var borderRectWidth = DisplayedSize.Width + BorderSpace.Width;
			var borderRectHeight = DisplayedSize.Height + BorderSpace.Height;
			var borderRectSize = new Size(borderRectWidth, borderRectHeight);
			var borderRectPos = new Point(Position.X + MarginalSpace.Left, Position.Y + MarginalSpace.Top);

			sb.FillRectangle(new Rectangle(borderRectPos, borderRectSize), Color.Black);
		}

		// draw background if color is set
		var backgroundPos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
		if (BackgroundColor is not null)
			sb.FillRectangle(new Rectangle(backgroundPos, DisplayedSize), BackgroundColor.Value);

		// draw rectangle for checkbox
		var checkboxBorderRect = new RectangleF(
			backgroundPos.X + MarginalSpace.Left,
			backgroundPos.Y + MarginalSpace.Top,
			CheckboxSize.Width,
			CheckboxSize.Height
		);
		var checkboxFillerRect = new RectangleF(
			checkboxBorderRect.X + 1,
			checkboxBorderRect.Y + 1,
			checkboxBorderRect.Width - 2,
			checkboxBorderRect.Height - 2
		);
		sb.FillRectangle(checkboxBorderRect, Color.Black);
		sb.FillRectangle(checkboxFillerRect, IsSelectable ? Color.White : Color.LightGray);

		if (IsChecked)
		{
			var checkboxCheckRect = new RectangleF(
                checkboxFillerRect.Position + new Point(2, 2),
                checkboxFillerRect.Size + new Point(-4, -4)
            );
            sb.FillRectangle(checkboxCheckRect, Color.Black);
        }

		// draw label
		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(TextFontSize);
		var textPos = new Vector2(checkboxBorderRect.X + checkboxBorderRect.Width + MarginalSpace.Left * 2, checkboxBorderRect.Y);
		sb.DrawString(font18, Text, textPos, FontCol);
	}

	public override void Update()
	{
		if (Rectangle.Contains(GuiHelper.Mouse) && Default.MouseInteraction.Pressed(false) && IsSelectable)
        {
			// update state
			IsChecked = !IsChecked;

			// invoke listener
            OnClick?.Invoke();
        }
    }

	public override void Init()
	{
		var textCalculatedSize = GuiHelper.MeasureString(Text, TextFontSize);

		// account the inner marginal space (padding between border, checkbox, and label) in displayed size
		int width = textCalculatedSize.Width + MarginalSpace.Left + MarginalSpace.Width + CheckboxSize.Width;
		int height = textCalculatedSize.Height + MarginalSpace.Height;

		DisplayedSize = new Size(width, height);
	}
}
