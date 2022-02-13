using Microsoft.Xna.Framework;

namespace Koko.RunTimeGui;

public interface IParent {
	public bool IsRendering { get; set; }
	public List<BaseComponent> ChildComponents { get; set; }
	public void Update();
	public void UpdatePosition(Point newPosition);
	public void Init();
}
