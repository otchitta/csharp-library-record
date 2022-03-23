using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用分類一覧クラスです。
/// </summary>
public sealed class IniDivideList : IReadOnlyList<IniDivideData> {
	#region メンバー変数定義
	/// <summary>
	/// 分類配列
	/// </summary>
	private readonly IniDivideData[] values;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 分類個数を取得します。
	/// </summary>
	/// <value>分類個数</value>
	public int Count => this.values.Length;
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <param name="index">分類番号</param>
	/// <returns>分類情報</returns>
	public IniDivideData this[int index] => this.values[index];
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <param name="name">分類名称</param>
	/// <returns>分類情報</returns>
	/// <exception cref="KeyNotFoundException"><paramref name="name" />が存在しない場合</exception>
	public IniDivideData this[string name] => GetDivideData(name);
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// INI用分類一覧を生成します。
	/// </summary>
	/// <param name="values">分類集合</param>
	public IniDivideList(IEnumerable<IniDivideData> values) {
		this.values = (new List<IniDivideData>(values)).ToArray();
	}
	/// <summary>
	/// INI用分類一覧を生成します。
	/// </summary>
	/// <param name="values">分類集合</param>
	public IniDivideList(params IniDivideData[] values) {
		this.values = (IniDivideData[])values.Clone();
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="offset">分類番号</param>
	/// <param name="result">分類情報</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	private bool GetDivideData(string search, out int offset, [MaybeNullWhen(false)]out IniDivideData result) {
		for (var index = 0; index < this.values.Length; index ++) {
			var choose = this.values[index];
			if (choose.DivideName == search) {
				offset = index;
				result = choose;
				return true;
			}
		}
		offset = default;
		result = default;
		return false;
	}
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 分類名称を取得します。
	/// </summary>
	/// <param name="search">検索番号</param>
	/// <returns>分類名称</returns>
	public string GetDivideName(int search) => this.values[search].DivideName;
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">分類情報</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDivideData(string search, [MaybeNullWhen(false)]out IniDivideData result) => GetDivideData(search, out var caches, out result);
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>分類情報</returns>
	/// <exception cref="KeyNotFoundException"><paramref name="search" />が存在しない場合</exception>
	public IniDivideData GetDivideData(string search) => GetDivideData(search, out var result)? result: throw new KeyNotFoundException($"{nameof(search)} is not found.({nameof(search)}={search})");
	/// <summary>
	/// 分類番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">分類番号</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDivideCode(string search, out int result) => GetDivideData(search, out result, out var caches);
	/// <summary>
	/// 分類番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>分類番号</returns>
	public int GetDivideCode(string search) => GetDivideData(search, out var result, out var caches)? result: -1;
	#endregion 公開メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		foreach (var choose in this.values) {
			yield return choose;
		}
	}
	/// <summary>
	/// 反復処理を取得します。
	/// </summary>
	/// <returns>反復処理</returns>
	IEnumerator<IniDivideData> IEnumerable<IniDivideData>.GetEnumerator() {
		foreach (var choose in this.values) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義
}
