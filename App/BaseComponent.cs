using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public abstract class BaseComponent : IComponent {

	public string Tag { get; set; } = "";
	public string Text { get; set; } = "";
	public Point Position { get; set; } = new Point(0, 0);
	public Size DisplayedSize { get; set; } = new Size(0, 0);
	public IParent? Parent { get; set; } = null;
	public Color? BackgroundColor { get; set; }
	public Margin MarginalSpace { get; set; } = new Margin(0);
	public Margin BorderSpace { get; set; } = new Margin(0);

	public int PaddingLeft => MarginalSpace.Left + BorderSpace.Left;
	public int PaddingTop => MarginalSpace.Top + BorderSpace.Top;
	public int PaddingRight => MarginalSpace.Right + BorderSpace.Right;
	public int PaddingBottom => MarginalSpace.Bottom + BorderSpace.Bottom;
	public int PaddingHorizontal => PaddingLeft + PaddingRight;
	public int PaddingVertical => PaddingTop + PaddingBottom;

	public Rectangle ContentRectangle => new(Position.X + PaddingLeft, Position.Y + PaddingTop, DisplayedSize.Width, DisplayedSize.Height);
	public abstract void Draw(SpriteBatch sb);
	public abstract void Init();
	public abstract void Update();
}