using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Otchitta.Libraries.Common.Dsv;

/// <summary>
/// DSV用要素情報クラスです。
/// </summary>
public sealed class DsvDataSource {
	/// <summary>
	/// 要素番号を取得します。
	/// </summary>
	/// <value>要素番号</value>
	public long Offset {
		get;
	}
	/// <summary>
	/// 項目一覧を取得します。
	/// </summary>
	/// <value>項目一覧</value>
	public ReadOnlyCollection<string> Source {
		get;
	}

	/// <summary>
	/// DSV用要素情報を生成します。
	/// </summary>
	/// <param name="lineNo">要素番号</param>
	/// <param name="source">項目集合</param>
	public DsvDataSource(long lineNo, IEnumerable<string> source) {
		Offset = lineNo;
		Source = new ReadOnlyCollection<string>(new List<string>(source));
	}
}
