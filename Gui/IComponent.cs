using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public interface IComponent {
	public Point Position { get; set; }
	public Size DisplayedSize { get; set; }
	public void Draw(SpriteBatch sb);
	public void Init();
}
