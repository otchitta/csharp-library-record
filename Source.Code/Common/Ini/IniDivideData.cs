using System.Collections.Generic;

namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用分類情報クラスです。
/// </summary>
public sealed class IniDivideData {
	#region メンバー変数定義
	/// <summary>
	/// 分類名称
	/// </summary>
	private readonly string divideName;
	/// <summary>
	/// 項目一覧
	/// </summary>
	private readonly IniDetailList detailList;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 分類名称を取得します。
	/// </summary>
	/// <value>分類名称</value>
	public string DivideName => this.divideName;
	/// <summary>
	/// 項目一覧を取得します。
	/// </summary>
	/// <value>項目一覧</value>
	public IniDetailList DetailList => this.detailList;
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// INI用分類情報を生成します。
	/// </summary>
	/// <param name="divideName">分類名称</param>
	/// <param name="detailList">項目一覧</param>
	public IniDivideData(string divideName, IniDetailList detailList) {
		this.divideName = divideName;
		this.detailList = detailList;
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 項目名称を取得します。
	/// </summary>
	/// <param name="search">検索番号</param>
	/// <returns>項目名称</returns>
	public string GetDetailName(int search) => this.detailList.GetDetailName(search);
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="search">検索番号</param>
	/// <returns>項目内容</returns>
	public string GetDetailText(int search) => this.detailList.GetDetailText(search);
	/// <summary>
	/// 項目情報を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">項目情報</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDetailData(string search, out IniDetailData result) => this.detailList.GetDetailData(search, out result);
	/// <summary>
	/// 項目番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <param name="result">項目番号</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetDetailCode(string search, out int result) => this.detailList.GetDetailCode(search, out result);
	/// <summary>
	/// 項目番号を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>項目番号</returns>
	public int GetDetailCode(string search) => this.detailList.GetDetailCode(search);
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <param name="search">検索名称</param>
	/// <returns>項目内容</returns>
	/// <exception cref="KeyNotFoundException"><paramref name="search" />が存在しない場合</exception>
	public string GetDetailText(string search) => this.detailList.GetDetailText(search);
	#endregion 公開メソッド定義
}
