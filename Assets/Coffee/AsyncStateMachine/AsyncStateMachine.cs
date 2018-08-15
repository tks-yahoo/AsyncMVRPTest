using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Coffee.AsyncStateMachine
{
	/// <summary>
	/// 非同期ステートマシン.
	/// </summary>
	public class AsyncStateMachine : MonoBehaviour, IAsyncStateMachine
	{
		readonly List<IAsyncState> states = new List<IAsyncState>();
		readonly List<System.Func<IEnumerator>> queuedCoroutines = new List<System.Func<IEnumerator>>();
		List<Func<IEnumerator>> removableCoroutines = new List<Func<IEnumerator>>();

		public IAsyncState currentState { get; private set; }

		public IAsyncState previousState { get; private set; }

		/// <summary>
		/// 新しい状態を登録します.
		/// </summary>
		public IAsyncStateMachine RegisterState(IAsyncState state)
		{
			states.Add(state);
			state.stateMachine = this;
			return this;
		}

		/// <summary>
		/// 指定したコールバックに新しい状態遷移を追加します.
		/// </summary>
		public IAsyncStateMachine AddTransition(ref Action action, IAsyncState state)
		{
			action += () => ChangeState(state);
			return this;
		}

		/// <summary>
		/// 状態を取得します.
		/// </summary>
		public IAsyncState GetState(Type stateType)
		{
			return states.Find(x => x.GetType() == stateType);
		}

		/// <summary>
		/// 状態変更をリクエストします.
		/// 実際に状態変更されるまでに別のリクエストがあった場合、リクエストは上書きされます.
		/// </summary>
		public IAsyncStateMachine ChangeState(Type stateType)
		{
			ChangeState(GetState(stateType));
			return this;
		}

		/// <summary>
		/// 状態変更をリクエストします.
		/// 実際に状態変更されるまでに別のリクエストがあった場合、リクエストは上書きされます.
		/// </summary>
		public IAsyncStateMachine ChangeState(IAsyncState state)
		{
			Debug.LogFormat("<color=red>[{0}][ステート変更リクエスト] {1} -> {2}</color>", Time.frameCount, currentState, state.GetType().Name);

			// ステート変更に伴い、削除リストにあるコルーチンを削除する.
			removableCoroutines.ForEach(x => queuedCoroutines.Remove(x));
			removableCoroutines.Clear();

			Func<IEnumerator> onUpdate = () => OnUpdate(state);
			Func<IEnumerator> onExit = () => OnExit(state);
			Func<IEnumerator> onEnter = () => OnEnter(state, onExit);

			// コルーチンキューに追加
			queuedCoroutines.Add(onEnter);
			queuedCoroutines.Add(onUpdate);
			queuedCoroutines.Add(onExit);

			// ステート変更時の削除コルーチンを追加.
			removableCoroutines.Add(onEnter);
			removableCoroutines.Add(onUpdate);
			removableCoroutines.Add(onExit);

			return this;
		}

		IEnumerator OnEnter(IAsyncState state, params Func<IEnumerator>[] removes)
		{
			Debug.LogFormat("<color=orange>[{0}][ステート開始] {1} -> {2}</color>", Time.frameCount, currentState, state.GetType().Name);
			previousState = currentState;
			currentState = state;

			foreach (var cb in removes)
			{
				removableCoroutines.Remove(cb);
			}

			yield return state.OnEnter();
		}

		IEnumerator OnUpdate(IAsyncState state)
		{
			yield return state.OnUpdate();
			state.OnUpdateComplete();
		}

		IEnumerator OnExit(IAsyncState state)
		{
			yield return state.OnExit();
			Debug.LogFormat("<color=green>[{0}][ステート完了] {1}</color>", Time.frameCount, state.GetType().Name);
		}

		protected virtual IEnumerator Start()
		{
			while (true)
			{
				if (queuedCoroutines.Count == 0)
				{
					yield return null;
					continue;
				}

				var next = queuedCoroutines[0];
				queuedCoroutines.RemoveAt(0);
				yield return next();
			}
		}
	}
}