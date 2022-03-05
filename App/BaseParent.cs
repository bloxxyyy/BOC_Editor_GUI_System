using Microsoft.Xna.Framework;
namespace Koko.RunTimeGui;

public abstract class BaseParent : BaseComponent, IParent {
	protected List<IComponent> ChildComponents { get; set; } = new();

	public bool IsRendering { get; set; } = true;
	public bool IsDraggable { get; set; } = true;
	public int DraggerHeight { get; set; } = 20;

	public void AddChild(IComponent newChild) {
		ChildComponents.Add(newChild);
	}

	public List<IComponent> GetChildren() {
		return (from IComponent selectableIComponent in ChildComponents select selectableIComponent).ToList();
	}

	public void RemoveChild(IComponent component) {
		if (!ChildComponents.Contains(component))
			throw new ArgumentException("No child found!");

		ChildComponents.Remove(component);
		Init();
	}

	public abstract void UpdatePosition(Point newPosition);
}