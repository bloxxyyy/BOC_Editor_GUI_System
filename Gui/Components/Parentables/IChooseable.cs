namespace Koko.RunTimeGui {
	public interface IChooseable<T> : IParent where T : ISelectable {
		public T? CurrentSelectedComponent { get; set; }
	}
}