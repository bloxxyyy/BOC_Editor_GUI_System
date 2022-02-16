using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koko.RunTimeGui;

public class GUI : BaseComponent, IParent {

	public static GUI Gui = new();

	public bool IsRendering { get; set; } = true;
	public List<BaseComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;
	public Game GameInstance { get; set; }
	public bool IsDragable { get; set; } = false;
	public int DraggerHeight { get; set; } = 0;

	public override void Update() {
		if (!IsRendering) return;
		InputHelper.UpdateSetup();
		for (int i = 0; i < ChildComponents.Count; i++) ChildComponents[i].Update();
		InputHelper.UpdateCleanup();
	}

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Draw(sb);
		}
	}

	public override void Init() {
		InputHelper.Setup(GameInstance);
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Init();
		}

		UpdatePosition(Position);
	}

    public void UpdatePosition(Point newPosition) {
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
}
