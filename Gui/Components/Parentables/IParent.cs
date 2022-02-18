using Microsoft.Xna.Framework;

namespace Koko.RunTimeGui;

public interface IParent {
	public bool IsRendering { get; set; }
	public bool IsDraggable { get; set; }
	public int DraggerHeight { get; set; }
	public void UpdatePosition(Point newPosition);
	public void AddChild(IComponent newChild);
}
