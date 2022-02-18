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

	public int PaddingLeft {
		get => MarginalSpace.Left + BorderSpace.Left;
	}

	public int PaddingTop {
		get => MarginalSpace.Top + BorderSpace.Top;
	}

	public int PaddingRight	{
		get => MarginalSpace.Right + BorderSpace.Right;
	}

	public int PaddingBottom {
		get => MarginalSpace.Bottom + BorderSpace.Bottom;
	}

	public int PaddingHorizontal {
		get => PaddingLeft + PaddingRight;
	}

	public int PaddingVertical {
		get => PaddingTop + PaddingBottom;
	}

	public Rectangle ContentRectangle {
		get => new(Position.X + PaddingLeft, Position.Y + PaddingTop, DisplayedSize.Width, DisplayedSize.Height);
	}

	public abstract void Draw(SpriteBatch sb);
	public abstract void Init();
	public abstract void Update();
}