using FontStashSharp;

namespace Koko.RunTimeGui;

public static class FontHelper {

	public static readonly FontSystem FontSystem = new();
	public static void Add(string rootDir) {
		FontSystem.AddFont(File.ReadAllBytes($"{rootDir}/font-file.ttf"));
	}

	/// <summary>
	/// From Apos.Gui
	/// Returns a font with a given size.
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	public static DynamicSpriteFont GetFont(int size) {
		return FontSystem.GetFont(size);
	}
}
