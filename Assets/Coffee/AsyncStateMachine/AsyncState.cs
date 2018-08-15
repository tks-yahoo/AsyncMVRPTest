using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.AsyncStateMachine
{
	/// <summary>
	/// 非同期状態.
	/// </summary>
	public abstract class AsyncState : MonoBehaviour, IAsyncState
	{
		/// <summary>
		/// この状態を管理するためのステートマシン.
		/// </summary>
		public IAsyncStateMachine stateMachine { get; set; }

		/// <summary>
		/// 更新完了コールバック.
		/// </summary>
		public System.Action onUpdateComplete;

		/// <summary>
		/// 開始コールバック.
		/// </summary>
		public abstract IEnumerator OnEnter();

		/// <summary>
		/// 更新コールバック.
		/// </summary>
		public abstract IEnumerator OnUpdate();

		/// <summary>
		/// 更新コールバック.
		/// </summary>
		public void OnUpdateComplete()
		{
			if (onUpdateComplete != null)
				onUpdateComplete();
		}

		/// <summary>
		/// 終了コールバック.
		/// </summary>
		public abstract IEnumerator OnExit();

		public class WaitForSeconds : CustomYieldInstruction
		{
			float waitTime;

			public override bool keepWaiting { get { return Time.time < waitTime; } }

			public WaitForSeconds(float time)
			{
				waitTime = Time.time + time;
			}
		}
	}
}