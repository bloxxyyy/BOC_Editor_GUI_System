using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace Koko.RunTimeGui;

public class Panel : BaseComponent, IParent {
	public bool IsRendering { get; set; } = true;
	private List<IComponent> ChildComponents { get; set; } = new();

	public new Color? BackgroundColor { get; set; } = null;
	public bool IsDraggable { get; set; } = false;

	public int DraggerHeight { get; set; } = 20;

	public override void Draw(SpriteBatch sb) {
		if (!IsRendering) return;

		var contentPos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
		var borderPos = new Point(contentPos.X - BorderSpace.Left, contentPos.Y - BorderSpace.Top);

		if ((BorderSpace.Width | BorderSpace.Height) != 0) {
			if (BackgroundColor is not null && BackgroundColor.Value.A == 255) {
				var borderRectSize = new Size(DisplayedSize.Width + BorderSpace.Width, DisplayedSize.Height + BorderSpace.Height);
				sb.FillRectangle(new Rectangle(borderPos, borderRectSize), Color.Black);
			} else {
				// TODO: draw color when component background has alpha or component does not have background
			}
		}

		if (BackgroundColor is not null)
			sb.FillRectangle(new Rectangle(contentPos, DisplayedSize), BackgroundColor.Value);

		if (IsDraggable) {
			var posY = contentPos.Y + DisplayedSize.Height - DraggerHeight;
			var point = new Point(contentPos.X + 5, posY + 5);
			var size = new Size(DisplayedSize.Width - 10, DraggerHeight - 10);
			sb.FillRectangle(new Rectangle(point, size), Color.Orange);
		}

		for (int i = 0; i < ChildComponents.Count; i++)
			ChildComponents[i].Draw(sb);
	}

	public void UpdatePosition(Point newPosition) {
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

		if (IsDraggable)
			height += DraggerHeight;

		DisplayedSize = new Size(GetBiggestPanelWidth(), height);
		UpdatePosition(Position);
	}

	private IParent? DoesContain(List<IComponent> components) {
		IParent? IParent = null;
		components.ForEach(c => {
			if (c != this && c is IParent parent && c.ContentRectangle.Contains(ContentRectangle.Location)) {
				var item = DoesContain(parent.GetChildren());
				if (item is not null) {
					IParent = item;
				} else {
					IParent = parent;
				}
			}
		});

		return IParent;
	}

	Point offset;
	bool isHeld = false;
	public override void Update() {
		if (!IsRendering) return;

		if (Default.MouseInteraction.Released(false) && isHeld) {
			isHeld = false;
			Debug.WriteLine(this + " : " + Parent);
			Parent.RemoveChild(this);
			GUI.Gui.GetChildren().ForEach(c => {
				if (c != this && c is IParent parent && c.ContentRectangle.Contains(ContentRectangle.Location)) {
					var item = DoesContain(parent.GetChildren());
					if (item is not null) {
						Parent = item;
					} else {
						Parent = parent;
					}
				}
			});
			Parent.AddChild(this);
			((IComponent)Parent).Init();
		}

		for (int i = 0; i < ChildComponents.Count; i++)
			ChildComponents[i].Update();

		if (IsDraggable && Default.MouseInteraction.Pressed(false)) {

			var handleRect = new Rectangle(
				new Point(ContentRectangle.Left, ContentRectangle.Bottom - DraggerHeight),
				new Size(ContentRectangle.Width, DraggerHeight)
			);

			if (handleRect.Contains(GuiHelper.Mouse)) {
				Debug.WriteLine("Pressed: " + this);

				Parent.RemoveChild(this);
				Parent = GUI.Gui;
				Parent.AddChild(this);

				offset = GuiHelper.Mouse - Position;
				isHeld = true;
			}
		}

		if (Default.MouseInteraction.Held(false) && IsDraggable && isHeld)
			UpdatePosition(GuiHelper.Mouse - offset);
	}

	private int GetBiggestPanelWidth() {
		int width = 0;
		for (int i = 0; i < ChildComponents.Count; i++)
			width = Math.Max(ChildComponents[i].DisplayedSize.Width + ChildComponents[i].PaddingHorizontal, width);
		return width;
	}

	public void AddChild(IComponent newChild) => ChildComponents.Add(newChild);
	public void RemoveChild(IComponent child) {

		if (!ChildComponents.Contains(child))
			throw new ArgumentException("No child found!");

		ChildComponents.Remove(child);
		Init();
	}
	public List<IComponent> GetChildren() => ChildComponents;
}
