
//dev env fix https://stackoverflow.com/questions/51550265/t4-template-could-not-load-file-or-assembly-system-runtime-version-4-2-0-0
//<dependentAssembly>
//<assemblyIdentity name="System.Runtime" publicKeyToken="b03f5f7f11d50a3a" culture="neutral"/>
//<bindingRedirect oldVersion="0.0.0.0-5.0.0.0" newVersion="4.0.0.0"/>
//</dependentAssembly>

using Koko.RunTimeGui.Gui.Initable_Components;
using Koko.RunTimeGui;

namespace Koko.Generated {
	public class Generate : IInitable {

		private IParent component = GUI.Gui;
		private BaseComponent temp;

		public void Init() {
			component = new Nav() { Parent = component, };
			temp = new Button() { Parent = component, MarginalSpace = new Margin(5), BorderSpace = new Margin(1), };
			temp.Text = "Inventory";
			component.AddChild(temp);
			temp = new Button() { Parent = component, MarginalSpace = new Margin(5), BorderSpace = new Margin(1), };
			temp.Text = "Inventory2";
			component.AddChild(temp);
			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			component = new GridPanel() { Parent = component, MarginalSpace = new Margin(10), };
			component = new Panel() { Parent = component, MarginalSpace = new Margin(5), BorderSpace = new Margin(1), };
			temp = new Checkbox() { Parent = component, MarginalSpace = new Margin(5), };
			temp.Text = "Yoran likes boys? 冰淇淋";
			component.AddChild(temp);
			temp = new Button() { Parent = component, MarginalSpace = new Margin(5), BorderSpace = new Margin(1), };
			temp.Text = "Quit";
			component.AddChild(temp);
			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			component = new GridPanel() { Parent = component, MarginalSpace = new Margin(5), BorderSpace = new Margin(1), };
			temp = new Label() { Parent = component, MarginalSpace = new Margin(2), };
			temp.Text = "Name:";
			component.AddChild(temp);
			temp = new Label() { Parent = component, MarginalSpace = new Margin(2), };
			temp.Text = "Piet";
			component.AddChild(temp);
			temp = new Label() { Parent = component, MarginalSpace = new Margin(2), };
			temp.Text = "Age:";
			component.AddChild(temp);
			temp = new Label() { Parent = component, MarginalSpace = new Margin(2), };
			temp.Text = "18";
			component.AddChild(temp);
			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
			((BaseComponent)component).Parent.AddChild((BaseComponent)component);
			component = ((BaseComponent)component).Parent;
		}
	}
}

