
//dev env fix https://stackoverflow.com/questions/51550265/t4-template-could-not-load-file-or-assembly-system-runtime-version-4-2-0-0
//<dependentAssembly>
//<assemblyIdentity name="System.Runtime" publicKeyToken="b03f5f7f11d50a3a" culture="neutral"/>
//<bindingRedirect oldVersion="0.0.0.0-5.0.0.0" newVersion="4.0.0.0"/>
//</dependentAssembly>

using Koko.RunTimeGui.Gui.Initable_Components;
using Koko.RunTimeGui;

namespace Koko.RuntimeGui { 
	public class Generate : IInitable {

		private IParent component = GUI.Gui;
		private BaseComponent temp;

		public void Init() {
			 component = new Nav() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  component = new GridPanel() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Checkbox() {Parent = component};  temp = new Button() {Parent = component};  component = new Panel() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Checkbox() {Parent = component};  temp = new Button() {Parent = component};  component = new GridPanel() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component};  component = new Panel() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Button() {Parent = component};  temp = new Checkbox() {Parent = component};  temp = new Button() {Parent = component};  component = new GridPanel() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component};  temp = new Label() {Parent = component}; 		}
	}
}

