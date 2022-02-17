namespace Koko.RunTimeGui;

public interface ISelectable : IComponent {
	public bool IsSelectable { get; set; }
	public Action<ISelectable> OnClick { get; set; }
}
