using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.AsyncStateMachine;

public class TestState3 : AsyncState
{
	public override IEnumerator OnEnter ()
	{
		Debug.LogFormat ("<color=orange>{2}: {0}.{1} を開始</color>", GetType (), "OnEnter", Time.frameCount);
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
	}

	public override IEnumerator OnUpdate ()
	{
		Debug.LogFormat ("<color=orange>{2}: {0}.{1} を開始</color>", GetType (), "OnUpdate", Time.frameCount);
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
//		yield return  base.OnUpdate ();
	}

	public override IEnumerator OnExit ()
	{
		Debug.LogFormat ("<color=orange>{2}: {0}.{1} を開始</color>", GetType (), "OnExit", Time.frameCount);
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
	}
}
