namespace Koko.RunTimeGui {
	public interface IChooseable<ISelectable> {
		public IComponent CurrentSelectedComponent { get; set; }
		public List<ISelectable> SelectableComponents { get; set; }
	}
}