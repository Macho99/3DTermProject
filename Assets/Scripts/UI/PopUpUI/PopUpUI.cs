using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class PopUpUI : BaseUI
{
    public override void CloseUI()
	{
		GameManager.UI.ClosePopUpUI();
	}
}