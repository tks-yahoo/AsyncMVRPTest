using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class UIModelState
{
	
}

public interface IUIModel
{
	Task BeforeShow();
	Task Show();
	Task AfterShow();
}

/// <summary>
/// ドメインロジックはすべてこちらに記述
/// </summary>
public class UITitleModel : IUIModel
{
	public async Task BeforeShow()
	{

	}

	public async Task Show()
	{

	}

	public async Task AfterShow()
	{

	}
}
