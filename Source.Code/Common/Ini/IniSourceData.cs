namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用全体情報クラスです。
/// </summary>
public sealed class IniSourceData {
	#region メンバー変数定義
	/// <summary>
	/// 項目一覧
	/// </summary>
	private readonly IniDetailList detailList;
	/// <summary>
	/// 分類一覧
	/// </summary>
	private readonly IniDivideList divideList;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 項目一覧を取得します。
	/// </summary>
	/// <value>項目一覧</value>
	public IniDetailList DetailList => this.detailList;
	/// <summary>
	/// 分類一覧を取得します。
	/// </summary>
	/// <value>分類一覧</value>
	public IniDivideList DivideList => this.divideList;
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// INI用全体情報を生成します。
	/// </summary>
	/// <param name="detailList">項目一覧</param>
	/// <param name="divideList">分類一覧</param>
	public IniSourceData(IniDetailList detailList, IniDivideList divideList) {
		this.detailList = detailList;
		this.divideList = divideList;
	}
	#endregion 生成メソッド定義
}
