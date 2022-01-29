using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public abstract class BaseComponent : IComponent {
	public Point Position { get; set; }
	public Size DisplayedSize { get; set; }

	public abstract void Draw(SpriteBatch sb);
	public abstract void Init();
}