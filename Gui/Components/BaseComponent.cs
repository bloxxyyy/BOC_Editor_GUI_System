using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui;

public abstract class BaseComponent : IComponent {

	public string Tag { get; set; } = "";
	public string Text { get; set; } = "";

	public IParent? Parent { get; set; } = null;

	public Point Position { get; set; }
	public Margin MarginalSpace { get; set; } = new Margin(0);
	public Margin BorderSpace { get; set; } = new Margin(0);

	public int PaddingLeft {
		get => MarginalSpace.Left + BorderSpace.Left;
		private set { }
	}

	public int PaddingTop {
		get => MarginalSpace.Top + BorderSpace.Top;
		private set { }
	}

	public int PaddingRight {
		get => MarginalSpace.Right + BorderSpace.Right;
		private set { }
	}

	public int PaddingBottom {
		get => MarginalSpace.Bottom + BorderSpace.Bottom;
		private set { }
	}

	public int PaddingHorizontal {
		get => PaddingLeft + PaddingRight;
		private set { }
	}

	public int PaddingVertical {
		get => PaddingTop + PaddingBottom;
		private set { }
	}

	public Size DisplayedSize { get; set; }

	public abstract void Draw(SpriteBatch sb);
	public abstract void Init();
}