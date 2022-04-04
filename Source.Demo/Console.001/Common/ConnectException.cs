using System;

namespace Otchitta.Utilities.Common;

/// <summary>
/// 接続例外クラスです。
/// </summary>
public class ConnectException : Exception {
	/// <summary>
	/// 接続例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	public ConnectException(string reason) : base(reason) {
		// 処理なし
	}
	/// <summary>
	/// 接続例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	/// <param name="source">原因例外</param>
	public ConnectException(string reason, Exception source) : base(reason, source) {
		// 処理なし
	}
}
