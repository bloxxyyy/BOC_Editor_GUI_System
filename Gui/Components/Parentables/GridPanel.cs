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
		/// The amount of components that should be put next to each other.
		/// </summary>
		public int Columns { get; set; } = 2;

		/// <summary>
		/// Indexer for the next child in the _Children list.
		/// </summary>
		protected int _nextChildIndex = 0;

		public Color? BackgroundColor { get; set; } = null;
		public bool IsRendering { get; set; } = true;
		public List<BaseComponent> ChildComponents { get; set; } = new();

		#endregion

		#region Component updaters
		public void Update()
		{
			// TODO: Implement or remove function from abstract class parent
		}

		private int GetColumnWidth(int column)
        {
			if (column < 0 || column >= Columns)
				throw new ArgumentOutOfRangeException($"Column index {column} should be between 0 and {Columns}");

			var maxWidth = 0;

			for (int col = column; col < ChildComponents.Count; col += Columns)
            {
				if (ChildComponents[col] is not null)
					maxWidth = Math.Max(maxWidth, ChildComponents[col].DisplayedSize.Width + ChildComponents[col].PaddingHorizontal);
            }

			return maxWidth;
        }

		private int GetTotalRows() => (int) Math.Ceiling((double)ChildComponents.Count / Columns);

		private int GetRowHeight(int row)
        {
			int totalRows = GetTotalRows();

			if (row < 0 || row >= totalRows)
				throw new ArgumentOutOfRangeException($"Column index {row} should be between 0 and {totalRows}");

			var maxHeight = 0;

			for (int i = row * Columns; i < (Columns * (row + 1)) && i < ChildComponents.Count; i++)
			{
				if (ChildComponents[i] is not null)
					maxHeight = Math.Max(maxHeight, ChildComponents[i].DisplayedSize.Height + ChildComponents[i].PaddingVertical);
			}

			return maxHeight;
		}

		public void UpdatePosition(Point newPosition)
		{
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
			for (int i = 0; i < ChildComponents.Count; i++)
			{
				ChildComponents[i].Init();
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
