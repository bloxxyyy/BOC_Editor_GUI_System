
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
			
	//"//GUI"

	 component = new Nav() {Parent = component, }; 
	//"//GUI//Nav"

	 temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  component = new GridPanel() {Parent = component, MarginalSpace = new Margin(10),}; 
	//"//GUI//GridPanel"

	 component = new Panel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//GridPanel//Panel"

	 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  component = new GridPanel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//GridPanel//GridPanel"

	 temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 component = new Panel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//Panel"

	 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  component = new GridPanel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//GridPanel"

	 component = new Panel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//GridPanel//Panel"

	 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Checkbox() {Parent = component, MarginalSpace = new Margin(5),};  component.AddChild(temp);  temp = new Button() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),};  component.AddChild(temp);  component = new GridPanel() {Parent = component, MarginalSpace = new Margin(5),BorderSpace = new Margin(1),}; 
	//"//GUI//GridPanel//GridPanel"

	 temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp); 			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			 temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp);  temp = new Label() {Parent = component, MarginalSpace = new Margin(2),};  component.AddChild(temp); 		}
	}
}

