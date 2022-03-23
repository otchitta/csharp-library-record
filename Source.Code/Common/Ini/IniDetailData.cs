namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用項目情報構造体です。
/// </summary>
public readonly struct IniDetailData {
	/// <summary>
	/// 項目名称を取得します。
	/// </summary>
	/// <value>項目名称</value>
	public string DetailName {
		get;
	}
	/// <summary>
	/// 項目内容を取得します。
	/// </summary>
	/// <value>項目内容</value>
	public string DetailText {
		get;
	}

	/// <summary>
	/// INI用項目情報を生成します。
	/// </summary>
	/// <param name="detailName">項目名称</param>
	/// <param name="detailText">項目内容</param>
	public IniDetailData(string detailName, string detailText) {
		DetailName = detailName;
		DetailText = detailText;
	}
}
