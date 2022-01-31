namespace Koko.RunTimeGui;

public interface ISelectable {
	public bool IsSelectable { get; set; }
	public void OnClick();
}
