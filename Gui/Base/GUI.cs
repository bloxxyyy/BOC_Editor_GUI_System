using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koko.RunTimeGui;

public class GUI : BaseComponent, IParent {

	public static GUI Gui = new();

	public bool IsRendering { get; set; } = true;
	public List<BaseComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;

	public void Update() {
		if (!IsRendering) return;

		for (int i = 0; i < ChildComponents.Count; i++) {
			if (ChildComponents[i] is IParent child) child.Update();
		}
	}

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Draw(sb);
		}
	}

	public override void Init() {
		int xpos = Position.X + PaddingLeft;
		int ypos = Position.Y + PaddingTop;

		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Position = new Point(xpos, ypos);
			ChildComponents[i].Init();
			ypos += ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical;
		}
	}
}
