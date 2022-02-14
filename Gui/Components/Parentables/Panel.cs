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

		var contentPos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
		var borderPos = new Point(contentPos.X - BorderSpace.Left, contentPos.Y - BorderSpace.Top);

		if ((BorderSpace.Width | BorderSpace.Height) != 0)
		{
			if (BackgroundColor is not null && BackgroundColor.Value.A == 255)
			{
				var borderRectSize = new Size(DisplayedSize.Width + BorderSpace.Width, DisplayedSize.Height + BorderSpace.Height);
				sb.FillRectangle(new Rectangle(borderPos, borderRectSize), Color.Black);
			}
			else
			{
				// TODO: draw color when component background has alpha or component does not have background
			}
		}

		if (BackgroundColor is not null) {
			sb.FillRectangle(new Rectangle(contentPos, DisplayedSize), BackgroundColor.Value);
		}

		for (int i = 0; i < ChildComponents.Count; i++)
			ChildComponents[i].Draw(sb);
	}

	public void UpdatePosition(Point newPosition)
	{
		Position = newPosition;
		int xpos = Position.X + PaddingLeft;
		int ypos = Position.Y + PaddingTop;

		for (int i = 0; i < ChildComponents.Count; i++) {
			var childPos = new Point(xpos, ypos);
			if (ChildComponents[i] is IParent child) child.UpdatePosition(childPos);
			else ChildComponents[i].Position = childPos;

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

	public override void Update() {
		if (!IsRendering) return;
		for (int i = 0; i < ChildComponents.Count; i++)
			ChildComponents[i].Update();
	}

	private int GetBiggestPanelWidth() {
		int width = 0;
		for (int i = 0; i < ChildComponents.Count; i++)
			width = Math.Max(ChildComponents[i].DisplayedSize.Width + ChildComponents[i].PaddingHorizontal, width);
		return width;
	}
}
