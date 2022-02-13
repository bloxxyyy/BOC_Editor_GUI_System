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

	public void UpdatePosition(Point newPosition)
	{
		Position = newPosition;
		int xpos = Position.X + PaddingLeft;
		int ypos = Position.Y + PaddingTop;

		for (int i = 0; i < ChildComponents.Count; i++)
		{
			var childPos = new Point(xpos, ypos);
			if (ChildComponents[i] is IParent child)
				child.UpdatePosition(childPos);
			else
				ChildComponents[i].Position = childPos;

			ypos += ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical;
		}
	}

	public override void Init() {
		var height = 0;

		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Init();
			height += ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical;
		}

		DisplayedSize = new Size(GetBiggestPanelWidth(), height);
		UpdatePosition(Position);
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
