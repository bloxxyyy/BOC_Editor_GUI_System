using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koko.RunTimeGui;

public class GUI : BaseComponent, IParent {
	public static GUI Gui = new();

	public bool IsRendering { get; set; } = true;
	public List<IComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;

	public void Update() {
		if (ChildComponents.Count <= 0) return;

		var yPos = ChildComponents[0].Position.Y; // get first pos.
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Position = new Point(ChildComponents[i].Position.X, yPos);
			yPos += ChildComponents[i].DisplayedSize.Height;

			if (ChildComponents[i] is IParent child) {
				child.Update();
			}
		}
	}

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Draw(sb);
		}
	}

	public override void Init() {
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Init();
		}
	}
}
