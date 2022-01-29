using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public class Panel : BaseComponent, IParent {
	public bool IsRendering { get; set; } = true;
	public List<IComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;

	public override void Draw(SpriteBatch sb) {
		if (!IsRendering) return;

		if (BackgroundColor is not null) {
			sb.FillRectangle(new Rectangle(Position, DisplayedSize), BackgroundColor.Value);
		}

		foreach (var childComponent in ChildComponents) {
			childComponent.Draw(sb);
		}
	}

	public override void Init() {
		foreach (var childComponent in ChildComponents) {
			childComponent.Init();
		}
	}

	public void Update() {
		if (!IsRendering) return;

		var yChildPos = Position.Y;

		for (int i = 0; i < ChildComponents.Count; i++) {
			var x = ChildComponents[i].Position.X + Position.X;
			var y = yChildPos;
			ChildComponents[i].Position = new Point(x,y);

			if (ChildComponents[i] is IParent child) {
				child.Update();
			}

			yChildPos += ChildComponents[i].DisplayedSize.Height;
		}

		DisplayedSize = new Size(DisplayedSize.Width, yChildPos);
	}
}
