using System.Collections.Generic;

namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用項目一覧クラスです。
/// </summary>
public sealed class IniDetailList : IReadOnlyList<IniDetailData> {
	#region メンバー変数定義
	/// <summary>
	/// 項目配列
	/// </summary>
	private readonly IniDetailData[] values;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 項目個数を取得します。
	/// </summary>
	/// <value>項目個数</value>
	public int Count => this.values.Length;
	/// <summary>
	/// 項目情報を取得します。
	/// </summary>
	/// <param name="index">項目番号</param>
	/// <returns>項目情報</returns>
	public IniDetailData this[int index] => this.values[index];
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="name">項目名称</param>
	/// <returns>項目内容</returns>
	/// <exception cref="KeyNotFoundException"><paramref name="name" />が存在しない場合</exception>
	public string this[string name] => GetDetailText(name);
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// INI用項目一覧を生成します。
	/// </summary>
	/// <param name="values">項目集合</param>
	public IniDetailList(IEnumerable<IniDetailData> values) {
		this.values = (new List<IniDetailData>(values)).ToArray();
	}
	/// <summary>
	/// INI用項目一覧を生成します。
	/// </summary>
	/// <param name="values">項目集合</param>
	public IniDetailList(params IniDetailData[] values) {
		this.values = (IniDetailData[])values.Clone();
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 項目情報を取得します。
	/// </summary>
	/// <param name="searchName">検索名称</param>
	/// <param name="resultCode">項目番号</param>
	/// <param name="resultData">項目情報</param>
	/// <returns><paramref name="searchName" />が存在した場合、<c>True</c>を返却</returns>
	private bool GetDetailData(string searchName, out int resultCode, out IniDetailData resultData) {
		for (var index = 0; index < this.values.Length; index ++) {
			var choose = this.values[index];
			if (choose.DetailName == searchName) {
				resultCode = index;
				resultData = choose;
				return true;
			}
		}
		resultCode = default;
		resultData = default;
		return false;
	}
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 項目名称を取得します。
	/// </summary>
	/// <param name="search">検索番号</param>
	/// <returns>項目名称</returns>
	public string GetDetailName(int search) => this.values[search].DetailName;
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="search">検索番号</param>
	/// <returns>項目内容</returns>
	public string GetDetailText(int search) => this.values[search].DetailText;
	/// <summary>
	/// 項目情報を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">項目情報</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDetailData(string search, out IniDetailData result) => GetDetailData(search, out var caches, out result);
	/// <summary>
	/// 項目番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">項目番号</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDetailCode(string search, out int result) => GetDetailData(search, out result, out var caches);
	/// <summary>
	/// 項目番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>項目番号</returns>
	public int GetDetailCode(string search) => GetDetailData(search, out var result, out var caches)? result: -1;
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>項目内容</returns>
	/// <exception cref="KeyNotFoundException"><paramref name="search" />が存在しない場合</exception>
	public string GetDetailText(string search) => GetDetailData(search, out var result)? result.DetailText: throw new KeyNotFoundException($"{nameof(search)} is not found.({nameof(search)}={search})");
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
	IEnumerator<IniDetailData> IEnumerable<IniDetailData>.GetEnumerator() {
		foreach (var choose in this.values) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義
}
