using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.AsyncStateMachine;

public class TestState1 : AsyncState
{
	[SerializeField] bool pause;

	public override IEnumerator OnEnter()
	{
		Debug.LogFormat("<color=orange>{2}: {0}.{1} を開始</color>", GetType(), "OnEnter", Time.frameCount);
		yield break;
	}

	public override IEnumerator OnUpdate()
	{
		Debug.LogFormat("<color=orange>{2}: {0}.{1} を開始</color>", GetType(), "OnUpdate", Time.frameCount);
		yield return Co();
		Debug.LogFormat("<color=orange>{2}: 途中経過1</color>", GetType(), "OnUpdate", Time.frameCount);
		yield return tes.Co();
		Debug.LogFormat("<color=orange>{2}: 途中経過2</color>", GetType(), "OnUpdate", Time.frameCount);
		yield return StartCoroutine(tes.Co());
		Debug.LogFormat("<color=orange>{2}: 途中経過3</color>", GetType(), "OnUpdate", Time.frameCount);

		stateMachine.ChangeState(typeof(TestState2));
		yield break;

		yield return new WaitForSeconds(3);
	}

	public override IEnumerator OnExit()
	{
		Debug.LogFormat("<color=orange>{2}: {0}.{1} を開始</color>", GetType(), "OnExit", Time.frameCount);
		yield break;
//		yield return null;
//		yield return null;
//		yield return null;
//		yield return null;
//		yield return null;
//		yield return null;
//		yield return null;
//		yield return null;
	}

	IEnumerator Co()
	{
		Debug.Log("※※※演出スタート");

		yield return new WaitForSeconds(1);
		Debug.Log("※※※文字がバーン！");
		yield return new WaitForSeconds(1);
		Debug.Log("※※※アニメ完了待機");
		yield return new WaitUntil(() => pause);

		Debug.Log("※※※※演出おわり");
	}
}

public class tes
{
	public static IEnumerator Co()
	{
//		WaitForSeconds a;
		yield return new WaitForSeconds(3);
	}
}