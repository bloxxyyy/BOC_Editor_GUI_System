using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public interface IComponent {
	public Point Position { get; set; }
	public Size DisplayedSize { get; set; }
	public IParent? Parent { get; set; }
	public Color? BackgroundColor { get; set; }
	public Margin MarginalSpace { get; set; }
	public Margin BorderSpace { get; set; }

	public int PaddingLeft { get; }
	public int PaddingTop { get; }
	public int PaddingRight { get; }
	public int PaddingBottom { get; }
	public int PaddingHorizontal { get; }
	public int PaddingVertical { get; }
	public Rectangle ContentRectangle { get; }

	public void Draw(SpriteBatch sb);
	public void Update();
	public void Init();
}
