using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Panel : BaseComponent, IParent {
	public bool IsRendering { get; set; } = true;
	public List<BaseComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;

	public override void Draw(SpriteBatch sb) {
		if (!IsRendering) return;

		if (BackgroundColor is not null) {

			var pos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
			sb.FillRectangle(new Rectangle(pos, DisplayedSize), BackgroundColor.Value);
		}

		for (int i = 0; i < ChildComponents.Count; i++)
			ChildComponents[i].Draw(sb);
	}

	public override void Init() {
		int xpos = Position.X + PaddingLeft;
		int ypos = Position.Y + PaddingTop;

		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Position = new Point(xpos, ypos);
			ChildComponents[i].Init();
			ypos += ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical;
		}

		var height = (ypos - Position.Y) - PaddingTop;
		DisplayedSize = new Size(GetBiggestPanelWidth(), height);
	}

	public void Update() {
		if (!IsRendering) return;
	}

	private int GetBiggestPanelWidth() {
		int width = 0;
		for (int i = 0; i < ChildComponents.Count; i++)
			width = Math.Max(ChildComponents[i].DisplayedSize.Width + ChildComponents[i].PaddingHorizontal, width);
		return width;
	}
}
