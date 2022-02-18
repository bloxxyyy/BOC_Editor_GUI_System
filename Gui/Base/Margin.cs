namespace Koko.RunTimeGui;

public class Margin {
	public ushort Top { get; set; } = 0;
	public ushort Bottom { get; set; } = 0;
	public ushort Left { get; set; } = 0;
	public ushort Right { get; set; } = 0;

	public int Width { get => Left + Right; }
	public int Height { get => Top + Bottom; }

	public  Margin(ushort top, ushort bottom, ushort left, ushort right) {
		Top = top;
		Bottom = bottom;
		Left = left;
		Right = right;
	}

	public Margin(ushort yAxis, ushort xAxis) {
		Top = yAxis;
		Bottom = yAxis;
		Left = xAxis;
		Right = xAxis;
	}

	public Margin(ushort size) {
		Top = size;
		Bottom = size;
		Left = size;
		Right = size;
	}
}
