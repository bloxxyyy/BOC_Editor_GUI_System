using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using MonoGame.Extended;
using FontStashSharp;

namespace Koko.RunTimeGui;

public class GUI : BaseComponent, IParent {

	public static GUI Gui = new();

	public bool IsRendering { get; set; } = true;
	public List<BaseComponent> ChildComponents { get; set; } = new();
	public Color? BackgroundColor { get; set; } = null;
	public Game GameInstance { get; set; }
	public bool IsDraggable { get; set; } = false;
	public int DraggerHeight { get; set; } = 0;
	private bool ShowFpsCounter { get; set; } = false;
	private double frames = 0;
	private double updates = 0;
	private double elapsed = 0;
	private double last = 0;
	private double now = 0;
	public double msgFrequency = 1.0f;
	public string msg = "";

	public override void Update() {}
	public void Update(GameTime gameTime) {
		if (!IsRendering) return;
		InputHelper.UpdateSetup();

		if (Default.F12Press.Pressed(true))
        {
			string newState = !ShowFpsCounter ? "Enabled" : "Disabled";
			Debug.WriteLine($"{newState} FPS counter");
			ShowFpsCounter = !ShowFpsCounter;
		}

		for (int i = 0; i < ChildComponents.Count; i++) ChildComponents[i].Update();

		now = gameTime.TotalGameTime.TotalSeconds;
		elapsed = (double)(now - last);
		if (elapsed > msgFrequency)
		{
			msg = $" FPS: {frames / elapsed:0.##} \n Elapsed time: {elapsed:0.##}s \n Updates: {updates} \n Frames: {frames} ";
			//Console.WriteLine(msg);
			elapsed = 0;
			frames = 0;
			updates = 0;
			last = now;
		}
		updates++;

		InputHelper.UpdateCleanup();
	}

	public override void Draw(SpriteBatch sb) {
		for (int i = 0; i < ChildComponents.Count; i++)
		{
			ChildComponents[i].Draw(sb);
		}

		if (ShowFpsCounter)
		{
			var textSize = GuiHelper.MeasureString(msg, 16);
			var textPos = new Point((int)GuiHelper.UICamera.BoundingRectangle.Right - textSize.Width, 0);
			sb.FillRectangle(new Rectangle(textPos, textSize), Color.Black);
			sb.DrawString(FontHelper.FontSystem.GetFont(16), msg, textPos.ToVector2(), Color.White);
		}

		frames++;
	}

	public override void Init() {
		InputHelper.Setup(GameInstance);
		for (int i = 0; i < ChildComponents.Count; i++) {
			ChildComponents[i].Init();
		}

		UpdatePosition(Position);
	}

    public void UpdatePosition(Point newPosition) {
		int xpos = Position.X + PaddingLeft;
		int ypos = Position.Y + PaddingTop;

		for (int i = 0; i < ChildComponents.Count; i++)
		{
			var childPos = new Point(xpos, ypos);
			if (ChildComponents[i] is IParent child)
				child.UpdatePosition(childPos);
			else
				ChildComponents[i].Position = childPos;

			ypos += ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical;
		}
	}
}
