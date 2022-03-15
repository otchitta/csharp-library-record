using System.Collections.Generic;

namespace Otchitta.Libraries.Record;

/// <summary>
/// 要素情報インターフェースです。
/// </summary>
public interface DataRecord : IReadOnlyCollection<DataPacket> {
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="code">要素番号</param>
	/// <value>要素情報</value>
	object? this[int code] {
		get;
	}
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="name">要素名称</param>
	/// <value>要素情報</value>
	object? this[string name] {
		get;
	}
}
