using System.Collections;
using System.Collections.Generic;
using Coffee.AsyncStateMachine;

public class TestStateMachine : AsyncStateMachine
{
	// Use this for initialization
	protected override IEnumerator Start ()
	{
		var s1 = GetComponent<TestState1> ();
		var s2 = GetComponent<TestState2> ();
		var s3 = GetComponent<TestState3> ();

		RegisterState(s1)
		.AddTransition(ref s1.onUpdateComplete, s2);

		
		RegisterState (s2)
		.AddTransition (ref s2.onUpdateComplete, s3);

		RegisterState (s3);


		// 初期ステートとしてｓ１を指定
		ChangeState(s1);

		yield return base.Start ();
	}
}
