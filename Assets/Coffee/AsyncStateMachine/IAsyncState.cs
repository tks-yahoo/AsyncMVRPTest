using System.Collections;

namespace Coffee.AsyncStateMachine
{
	/// <summary>
	/// 状態インターフェース.
	/// 状態の開始、更新、終了コールバックを宣言できます.
	/// </summary>
	public interface IAsyncState
	{
		/// <summary>
		/// 開始コールバック.
		/// </summary>
		IEnumerator OnEnter();

		/// <summary>
		/// 更新コールバック.
		/// </summary>
		IEnumerator OnUpdate();

		/// <summary>
		/// 更新完了コールバック.
		/// </summary>
		void OnUpdateComplete();

		/// <summary>
		/// 終了コールバック.
		/// </summary>
		IEnumerator OnExit();

		/// <summary>
		/// ステートマシン.
		/// </summary>
		IAsyncStateMachine stateMachine { get; set; }
	}
}