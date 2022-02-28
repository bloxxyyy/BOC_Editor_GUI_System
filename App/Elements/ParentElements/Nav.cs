using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

/// <summary>
/// Container for navigatable elements, Does not choose children size or position.
/// </summary>
public class Nav : BaseComponent, IChooseable<ISelectable> {
	public ISelectable? CurrentSelectedComponent { get; set; } = null;
	public bool IsRendering { get; set; } = true;
	public bool IsDraggable { get; set; } = false;
	public int DraggerHeight { get; set; } = 0;
	private List<ISelectable> ChildComponents { get; set; } = new();

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++) {
			var obj = ChildComponents[i];
			obj.Draw(sb);

			if (obj == CurrentSelectedComponent) {
				var targetPos = new Point(obj.Position.X + obj.DisplayedSize.Width + obj.PaddingHorizontal, obj.Position.Y + obj.DisplayedSize.Height + obj.PaddingVertical);
				sb.DrawLine(obj.Position.X, targetPos.Y, targetPos.X, targetPos.Y, Color.Orange);
			}
		}
	}

	public override void Init() {
		var width = 0;
		var height = 0;

		for (var i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Init();
			ChildComponents[i].OnClick += SetActive;
			width += ChildComponents[i].DisplayedSize.Width + ChildComponents[i].PaddingHorizontal;
			height = Math.Max(ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical, height);
		}

		DisplayedSize = new Size(width, height);
	}

	private void SetActive(ISelectable selectedComponent) => CurrentSelectedComponent = selectedComponent;

	public override void Update() {
		if (!IsRendering)
			return;

		for (var i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Update();
		}
	}

	public void UpdatePosition(Point newPosition) {
		Position = newPosition;

		var x = newPosition.X;
		var y = newPosition.Y;

		for (var i = 0; i < ChildComponents.Count; i++) {
			if (ChildComponents[i] is IParent container) {
				container.UpdatePosition(new Point(x, y));
			} else {
				ChildComponents[i].Position = new Point(x, y);
			}

			x += ChildComponents[i].DisplayedSize.Width + ChildComponents[i].PaddingHorizontal;
		}
	}

	public void AddChild(ISelectable newChild) {
		ChildComponents.Add(newChild);
	}

	public void AddChild(IComponent newChild) {
		if (newChild is not ISelectable)
			throw new ArgumentException("New child is not an ISelectable");

		AddChild((ISelectable)newChild);
	}

	public void RemoveChild(IComponent child) {
		if (child is ISelectable selectable) ChildComponents.Remove(selectable);
		throw new ArgumentException("Cant remove this object from the list!");
	}

	public List<IComponent> GetChildren() {
		return (from IComponent selectableIComponent in ChildComponents select selectableIComponent).ToList();
	}
}
