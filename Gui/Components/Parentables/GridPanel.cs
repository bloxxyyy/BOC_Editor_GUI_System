using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Koko.RunTimeGui
{
	/// <summary>
	/// Panel Object that puts Components next to each other.
	/// </summary>
	public class GridPanel : BaseComponent, IParent {

		#region Properties

		/// <summary>
		/// Information on if this panel can be dragged.
		/// </summary>
		public bool IsDraggable { get; set; }

		/// <summary>
		/// The height of the dragPanel
		/// </summary>
		public int DraggerHeight { get; set; } = 20;

		/// <summary>
		/// The amount of components that should be put next to each other.
		/// </summary>
		public int Columns { get; set; } = 2;

		/// <summary>
		/// Indexer for the next child in the _Children list.
		/// </summary>
		protected int _nextChildIndex = 0;

		public new Color? BackgroundColor { get; set; } = null;
		public bool IsRendering { get; set; } = true;
		public List<BaseComponent> ChildComponents { get; set; } = new();

		#endregion

		#region Component updaters
		public override void Update() {
			for (int i = 0; i < ChildComponents.Count; i++)
				ChildComponents[i].Update();
		}

		private int GetColumnWidth(int column) {
			if (column < 0 || column >= Columns)
				throw new ArgumentOutOfRangeException($"Column index {column} should be between 0 and {Columns}");

			var maxWidth = 0;

			for (int col = column; col < ChildComponents.Count; col += Columns) {
				if (ChildComponents[col] is not null)
					maxWidth = Math.Max(maxWidth, ChildComponents[col].DisplayedSize.Width + ChildComponents[col].PaddingHorizontal);
            }

			return maxWidth;
        }

		private int GetTotalRows() => (int) Math.Ceiling((double)ChildComponents.Count / Columns);

		private int GetRowHeight(int row) {
			int totalRows = GetTotalRows();

			if (row < 0 || row >= totalRows)
				throw new ArgumentOutOfRangeException($"Column index {row} should be between 0 and {totalRows}");

			var maxHeight = 0;

			for (int i = row * Columns; i < (Columns * (row + 1)) && i < ChildComponents.Count; i++) {
				if (ChildComponents[i] is not null)
					maxHeight = Math.Max(maxHeight, ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical);
			}

			return maxHeight;
		}

		public void UpdatePosition(Point newPosition) {
			Position = newPosition;

			int[] columnWidths = new int[Columns];
			for (int i = 0; i < Columns; i++)
            {
				columnWidths[i] = GetColumnWidth(i);
            }

			int totalRows = GetTotalRows();
			int[] rowHeights = new int[totalRows];
			for (int i = 0; i < totalRows; i++)
            {
				rowHeights[i] = GetRowHeight(i);
            }

			var xPos = newPosition.X + PaddingLeft;
			var yPos = newPosition.Y + PaddingTop;

			for (int i = 0; i < ChildComponents.Count; i++)
            {
				if (i % Columns == 0 && i != 0)
				{
					xPos = newPosition.X + PaddingLeft;
					yPos += rowHeights[(i - 1) / Columns]; // add row height of previous column to y
				}

				var childPos = new Point(xPos, yPos);

				if (ChildComponents[i] is IParent child)
					child.UpdatePosition(childPos);
				else
					ChildComponents[i].Position = childPos;

				xPos += columnWidths[i % Columns];
            }
		}

		public override void Init() {
			// initialize each child component so their sizes are calculated
			for (int i = 0; i < ChildComponents.Count; i++) {
				ChildComponents[i].Init();
			}

			int width = 0;
			for (int i = 0; i < Columns; i++) {
				width += GetColumnWidth(i);
			}

			int totalRows = GetTotalRows();
			int height = 0;
			for (int i = 0; i < totalRows; i++) {
				height += GetRowHeight(i);
			}

			DisplayedSize = new Size(width, height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(SpriteBatch sb) {
			var contentPos = new Point(Position.X + PaddingLeft, Position.Y + PaddingTop);
			var borderPos = new Point(contentPos.X - BorderSpace.Left, contentPos.Y - BorderSpace.Top);

			if ((BorderSpace.Width | BorderSpace.Height) != 0) {
				if (BackgroundColor is not null && BackgroundColor.Value.A == 255) {
					var borderRectSize = new Size(DisplayedSize.Width + BorderSpace.Width, DisplayedSize.Height + BorderSpace.Height);
					sb.FillRectangle(new Rectangle(borderPos, borderRectSize), Color.Black);
				} else {
					// TODO: draw color when component background has alpha or component does not have background
				}
			}

			if (BackgroundColor is not null) {
				sb.FillRectangle(new Rectangle(contentPos, DisplayedSize), BackgroundColor.Value);
			}

			foreach (var c in ChildComponents)
				c.Draw(sb);
		}


		#endregion
	}
}
