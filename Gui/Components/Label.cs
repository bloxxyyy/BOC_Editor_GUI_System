using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Label : BaseComponent {

	private int _TextFontSize { get; set; } = 16;
	private Color _FontCol { get; set; } = Color.Black;

	public override void Draw(SpriteBatch sb) {
		var width = DisplayedSize.Width + BorderSpace.Width;
		var height = DisplayedSize.Height + BorderSpace.Height;
		var display = new Size(width, height);

		if (BackgroundColor is not null) {
			var pos3 = new Point(Position.X + MarginalSpace.Left, Position.Y + MarginalSpace.Top);
			sb.FillRectangle(new Rectangle(pos3, display), Color.Black);
		}

		var pos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
		if (BackgroundColor is not null)
			sb.FillRectangle(new Rectangle(pos, DisplayedSize), BackgroundColor.Value);

		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(_TextFontSize);
		var pos2 = new Vector2(pos.X + MarginalSpace.Left, pos.Y + MarginalSpace.Top);
		sb.DrawString(font18, Text, pos2, _FontCol);
	}

	public override void Update() {
		
	}

	public override void Init() {
		var size = GuiHelper.MeasureString(Text, _TextFontSize);

		// account the inner marginal space (padding between border and text) in displayed size
		int width = size.Width + MarginalSpace.Width;
		int height = size.Height + MarginalSpace.Height;
		DisplayedSize = new Size(width, height);
	}
}
