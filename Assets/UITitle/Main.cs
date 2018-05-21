using System.Threading.Tasks;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using UniRx.Diagnostics;
using System.Threading;

public class Main : MonoBehaviour {


	Func<IEnumerator> testAct;

	private void Start()
	{
		testAct += () => TestCoroutine("はじめ", 1);
		testAct += () => TestCoroutine("つぎ", 2);
		testAct += () => TestCoroutine("おわり", 3);
	}

	Coroutine c;
	IDisposable d;

	public void StartButton()
	{
		gameObject.SetActive(true);
		//this.enabled = true;
		//Observable.FromCoroutine(c => WhenAll2(testAct, c), true)
		//	.Debug("co")
		//	.TakeUntilDisable(this)
		//	.Subscribe();

		//Observable.WhenAll(
		//	Observable.FromCoroutine(() => TestCoroutine("はじめ", 1)),
		//	Observable.FromCoroutine(() => TestCoroutine("つぎ", 2)),
		//	Observable.FromCoroutine(() => TestCoroutine("おわり", 3))
		//	)
		//	.TakeUntilDisable(this)
		//	.Debug("co")
		//	.Subscribe();


		//Test("はじめ", 2).ToObservable()
		//	.TakeUntilDisable(this)
		//	.Debug("co")
		//	.Subscribe();

		//this.OnDisableAsObservable()
		//	.Debug("OnDisableAsObservable")
		//	.Subscribe(_=>d?.Dispose());

		//Observable.FromCoroutine(Hoge)
		//	.Subscribe();

		StartCoroutine(Hoge());
	}

	public void StopButton()
	{
		//d.Dispose();
		//d = null;

		//StopCoroutine(c);
		//c = null;
		//this.enabled = false;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	IEnumerator Hoge () {
		Debug.Log($"<color=red>★　演出開始準備中　★</color>");
		yield return new WaitForSeconds(2);

		Debug.Log($"<color=red>★　演出開始前　★</color>");

		//yield return Test("はじめ", 2)
		//	.ToObservable()
		//	.TakeUntilDisable(this)
		//	.StartAsCoroutine();


		//yield return TestCoroutine("はじめ", 2);

		//yield return Observable.Merge
		//	(
		//	Observable.FromCoroutine(c => TestCoroutine("moke", 1)).TakeUntilDisable(this),
		//	Observable.FromCoroutine(c => TestCoroutine("fuga", 2)).TakeUntilDisable(this)
		//	).Take(2)
		//	.TakeUntilDisable(this)
		//	.StartAsCoroutine();





		//yield return WhenAllParallel(testAct).StartAsCoroutine();

		yield return testAct.StartAsCoroutineParallel();
		yield return testAct.StartAsCoroutineSerial();

		Debug.Log($"<color=green>★　演出完了　★</color>");

		//yield return Test("www", 1);
	}



	// Update is called once per frame
	async Task Test (string label, float time) {
		Debug.Log($"{label} <color=red>演出開始前 {time}秒 (タスク版)</color>");

		await Task.Delay(TimeSpan.FromSeconds(time));

		Debug.Log($"{label} <color=green>演出完了 (タスク版)</color>");
	}

	IEnumerator TestCoroutine(string label, float time)
	{
		Debug.Log($"{label} <color=red>演出開始前 {time}秒 (コルーチン版)</color>");

		yield return new WaitForSeconds(time);

		Debug.Log($"{label} <color=green>演出完了 (コルーチン版)</color>");
	}

	IEnumerator TestCoroutine()
	{
		string label = "tesu";
		float time = 2;
		Debug.Log($"{label} <color=red>演出開始前 {time}秒 (コルーチン版)</color>");

		yield return new WaitForSeconds(time);

		Debug.Log($"{label} <color=green>演出完了 (コルーチン版)</color>");

		yield return TestCoroutine("hoge", 1);
	}
	/*
	class composite : IEnumerator
	{
		public IEnumerator me;
		public composite child;

		public object Current => me.Current;

		public composite(IEnumerator me)
		{
			this.me = me;
			if(me.Current is IEnumerator)
			{
				child = new composite(me.Current as IEnumerator);
			}
		}

		public bool MoveNext()
		{
			if(child != null)
			{
				if(!child.MoveNext())
				{
					child = null;
				}
			}
			else if(!me.MoveNext())
			{
				return false;
			}
			else if(me.Current is IEnumerator)
			{
				child = new composite(me.Current as IEnumerator);
			}
			return true;
		}

		public void Reset()
		{
		}
	}
	*/
	/*
	IObservable<Unit> WhenAllParallel(Func<IEnumerator> ev)
	{
		return Observable.WhenAll(
		ev.GetInvocationList()
			.Select(d => Observable.FromCoroutine((Func<IEnumerator>)d))
			);
	}

	IObservable<Unit> WhenAllSerial(Func<IEnumerator> ev)
	{
		var ret = Observable.Empty<Unit>();
		foreach(Func<IEnumerator> d in ev.GetInvocationList())
		{
			ret = ret.SelectMany(d);
		}
		return ret;
	}

	Stack<IEnumerator>[] GetAll(Func<IEnumerator> ev)
	{
		return ev.GetInvocationList()
			.Select(d => d.DynamicInvoke())
			.OfType<IEnumerator>()
			.Select(ie =>
			{
				var s = new Stack<IEnumerator>();
				s.Push(ie);
				return s;
			})
			.ToArray();
	}

	IEnumerator WhenAll2(Func<IEnumerator> ev, CancellationToken token)
	{
		var stacks = GetAll(ev);

		bool keepWaiting = true;
		while (keepWaiting)
		{
			keepWaiting = false;
			for (int i = 0; i < stacks.Length; i++)
			{
				if (stacks[i] != null)
				{
					if (StackDo(stacks[i]))
					{
						keepWaiting = true;
					}
					else
					{
						stacks[i] = null;
					}
				}
			}
			if (keepWaiting)
			{
				yield return null;
			}
			token.ThrowIfCancellationRequested();
		}
	}

	bool StackDo(Stack<IEnumerator> stack)
	{
		//while (0 < stack.Count)
		//{
			var now = stack.Peek();
			if (now.MoveNext())
			{
				//Debug.Log($"{stack.Count} コルーチン継続 {now}");
				while (now.Current is IEnumerator)
				{
					Debug.Log($"{stack.Count} コルーチンをプッシュ {now.Current}");
					stack.Push(now.Current as IEnumerator);
					now = now.Current as IEnumerator;
					//yield return null;
				}
			}
			else
			{
				stack.Pop();
			}
			//yield return null;
		//}
		return 0 < stack.Count;
	}
	*/

	IEnumerator WhenAll(Func<IEnumerator> ev)
	{

		Stack<IEnumerator> stack = new Stack<IEnumerator>();

		var now = ev();
		stack.Push(now);

			Debug.Log($"{stack.Count} コルーチン処理を開始!");
		while (0 < stack.Count)
		{
			now = stack.Peek();
			if (now.MoveNext())
			{
				//Debug.Log($"{stack.Count} コルーチン継続 {now}");
				while (now.Current is IEnumerator)
				{
					Debug.Log($"{stack.Count} コルーチンをプッシュ {now.Current}");
					stack.Push(now.Current as IEnumerator);
					now = now.Current as IEnumerator;
					yield return null;
				}
			}
			else
			{
				stack.Pop();
			}
			yield return null;
		}
		yield break;
		/*


			composite[] coroutines = ev.GetInvocationList()
			.Select(d => d.DynamicInvoke())
			.OfType<IEnumerator>()
			.Select(d => new composite() {me = d, })
			.ToArray();

		//int count = coroutines.Count;
		bool needWait = true;
		while (needWait)
		{
			needWait = false;
			for (int i = 0; i < coroutines.Length; i++)
			{
				if (coroutines[i] != null)
				{
					if (coroutines[i].MoveNext())
					{
						Debug.Log($"{i} : MoveNext = true, {coroutines[i].Current}");
						needWait = true;
					}
					else
					{
						Debug.Log($"{i} : MoveNext = false, {coroutines[i].Current}");
						coroutines[i] = null;
					}
				}
			}
			if(needWait)
				yield return null;
		}

		//foreach (var d in ev.GetInvocationList())
		//{
		//	var ie = (IEnumerator)d.DynamicInvoke();



		//	yield return ie;
		//}
		*/
	}
}


public class WaitForSeconds : CustomYieldInstruction
{
	float waitTime;

	public override bool keepWaiting
	{
		get
		{
			return Time.time < waitTime;
		}
	}

	public WaitForSeconds(float time)
	{
		waitTime = Time.time + time;
	}
}

	public static class Observable2
	{
		public static Coroutine StartAsCoroutineParallel(this Func<IEnumerator> self)
		{
			return Observable.WhenAll(
			self.GetInvocationList()
				.Select(d => Observable.FromCoroutine((Func<IEnumerator>)d))
				)
				.StartAsCoroutine();
		}

		public static Coroutine StartAsCoroutineSerial(this Func<IEnumerator> self)
		{
			IObservable<Unit> ret = null;
			foreach (Func<IEnumerator> d in self.GetInvocationList())
			{
				ret = ret == null
					? Observable.FromCoroutine(d)
					: ret.SelectMany(d);
			}
			return ret.StartAsCoroutine();
		}
	}
