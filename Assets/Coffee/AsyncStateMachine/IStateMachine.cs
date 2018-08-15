using System;

namespace Coffee.AsyncStateMachine
{
	/// <summary>
	/// ステートマシンインターフェース.
	/// </summary>
	public interface IAsyncStateMachine
	{
		/// <summary>
		/// 新しい状態を登録します.
		/// </summary>
		IAsyncStateMachine RegisterState(IAsyncState state);

		/// <summary>
		/// 指定したコールバックに新しい状態遷移を追加します.
		/// </summary>
		IAsyncStateMachine AddTransition(ref System.Action action, IAsyncState state);

		/// <summary>
		/// 状態を取得します.
		/// </summary>
		IAsyncState GetState(Type stateType);

		/// <summary>
		/// 状態変更をリクエストします.
		/// 実際に状態変更されるまでに別のリクエストがあった場合、リクエストは上書きされます.
		/// </summary>
		IAsyncStateMachine ChangeState(Type stateTyp);

		/// <summary>
		/// 状態変更をリクエストします.
		/// 実際に状態変更されるまでに別のリクエストがあった場合、リクエストは上書きされます.
		/// </summary>
		IAsyncStateMachine ChangeState(IAsyncState state);
	}
}