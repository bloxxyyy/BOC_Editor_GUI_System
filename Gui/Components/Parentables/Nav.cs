using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

/// <summary>
/// Container for navigatable elements, Does not choose children size or position.
/// </summary>
public class Nav : BaseComponent, IChooseable<ISelectable> {
	public List<ISelectable> SelectableComponents { get; set; } = new();
	public IComponent CurrentSelectedComponent { get; set; }

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < SelectableComponents.Count; i++) {
			var obj = SelectableComponents[i];
			if (obj != CurrentSelectedComponent) return;

			if (obj == CurrentSelectedComponent) {
				var targetPos = new Point(obj.Position.X + obj.DisplayedSize.Width, obj.Position.Y + obj.DisplayedSize.Height);
				sb.DrawLine(obj.Position.X, obj.Position.Y, targetPos.X, targetPos.Y, Color.Orange);
				break;
			}
		}
	}

	public override void Init() => SelectableComponents.ForEach(c => c.OnClick += SetActive);
	private void SetActive(ISelectable selectedComponent) => CurrentSelectedComponent = selectedComponent;

	public override void Update() { }

	public void UpdatePosition(Point newPosition) {
		
	}
}

