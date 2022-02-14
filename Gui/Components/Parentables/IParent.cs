﻿using Microsoft.Xna.Framework;

namespace Koko.RunTimeGui;

public interface IParent {
	public bool IsRendering { get; set; }
	public List<BaseComponent> ChildComponents { get; set; }
	public void UpdatePosition(Point newPosition);
}
