using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Koko.RunTimeGui;

public class GUI : BaseComponent, IParent {

	public static readonly GUI Gui = new();
	public static Game? GAME;

	public bool IsRendering { get; set; } = true;
	private List<IComponent> ChildComponents { get; set; } = new();
	public new Color? BackgroundColor { get; set; } = null;

	public bool IsDraggable { get; set; } = false;
	public int DraggerHeight { get; set; } = 0;

	public override void Update() {}
	public void Update(GameTime gameTime) {
		if (!IsRendering) return;
		InputHelper.UpdateSetup();

		Fps.CheckKeyPress();

		for (int i = 0; i < ChildComponents.Count; i++) ChildComponents[i].Update();

		Fps.Update(gameTime);

		InputHelper.UpdateCleanup();
	}

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++)
		{
			ChildComponents[i].Draw(sb);
		}

		Fps.Draw(sb);
	}

	public override void Init() {
		InputHelper.Setup(GAME);
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

	public void AddChild(IComponent newChild) => ChildComponents.Add(newChild);
	public void RemoveChild(IComponent child) => ChildComponents.Remove(child);
	public List<IComponent> GetChildren() => ChildComponents;
}
