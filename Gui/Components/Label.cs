using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koko.RunTimeGui;

public class Label : BaseComponent {

	public string Text { get; set; } = "test string";
	public int TextFontSize { get; set; } = 16;
	public Color FontCol { get; set; } = Color.Black;

	public override void Draw(SpriteBatch sb) {
		SpriteFontBase font18 = FontHelper.FontSystem.GetFont(TextFontSize);
		sb.DrawString(font18, Text, new Vector2(Position.X, Position.Y), FontCol);
	}

	public override void Init() {
		DisplayedSize = GuiHelper.MeasureString(Text, TextFontSize);
	}
}
