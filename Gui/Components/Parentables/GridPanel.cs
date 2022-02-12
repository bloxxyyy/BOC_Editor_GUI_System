using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui {
	/// <summary>
	/// Panel Object that puts Components next to each other.
	/// </summary>
	public class GridPanel : BaseComponent, IParent {

		#region Properties

		/// <summary>
		/// The amount of components that should be put next to each other.
		/// </summary>
		private int _ComponentsInWidth = 2;

		/// <summary>
		/// Indexer for the next child in the _Children list.
		/// </summary>
		protected int _nextChildIndex = 0;

		public Color? BackgroundColor { get; set; } = null;
		public bool IsRendering { get; set; } = true;
		public List<BaseComponent> ChildComponents { get; set; } = new();

		#endregion

		#region Component updaters

		public void Update() {
			for (int i = 0; i < ChildComponents.Count; i++) {
				if (ChildComponents[i] is IParent) ((IParent)ChildComponents[i]).Update();
			}

			var currentXPos = Position.X;
			var currentYPos = Position.Y;
			var rowStartIndex = 0;
			var biggestHeightOfCurrentRow = GetBiggestHeightOfCurrentRow(rowStartIndex);

			for (int i = 0; i < ChildComponents.Count; i++) {
				if (CheckIndexOnANewLine(i)) {
					rowStartIndex += _ComponentsInWidth;
					if (rowStartIndex + _ComponentsInWidth < ChildComponents.Count) { // only do this if we have more index to process after this row.
						biggestHeightOfCurrentRow = GetBiggestHeightOfCurrentRow(rowStartIndex);
					}
				} else {
					if (i != 0) currentXPos += ChildComponents[i - 1].DisplayedSize.Width;
				}

				ChildComponents[i].Position = new Point(currentXPos, currentYPos);

				if (IsLastIndexInRow(i)) {
					currentYPos += biggestHeightOfCurrentRow;
					currentXPos = Position.X;
				}
			}

			DisplayedSize = new Size(GetGridWidth(), currentYPos);
		}

		private int GetGridWidth() {
			int maxWidth = 0;
			var width = 0;
			for (int i = 1; i <= ChildComponents.Count; i++) {
				width += ChildComponents[i - 1].DisplayedSize.Width;

				if (i % _ComponentsInWidth == 0) {
					maxWidth = Math.Max(width, maxWidth);
					width = 0;
				}
			}

			return maxWidth;
		}

		private bool IsLastIndexInRow(int index) {
			return ((index % _ComponentsInWidth) == (_ComponentsInWidth - 1));
		}

		private int GetBiggestHeightOfCurrentRow(int rowIndex) {
			var biggestHeight = 0;

			for (int i = rowIndex; i < rowIndex + _ComponentsInWidth; i++) {
				biggestHeight = Math.Max(ChildComponents[i].DisplayedSize.Height, biggestHeight);
			}

			return biggestHeight;
		}

		private bool CheckIndexOnANewLine(int index) {
			if (index == 0) return false;
			return ((index % _ComponentsInWidth) == 0);
		}

		public override void Init() {
			foreach (var childComponent in ChildComponents) {
				childComponent.Init();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(SpriteBatch sb) {
			if (BackgroundColor is not null) {
				sb.FillRectangle(new Rectangle(Position, DisplayedSize), BackgroundColor.Value);
			}

			foreach (var c in ChildComponents)
				c.Draw(sb);
		}
		#endregion
	}
}
