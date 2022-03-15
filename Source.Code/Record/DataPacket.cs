namespace Otchitta.Libraries.Record;

/// <summary>
/// 要素情報構造体です。
/// </summary>
public struct DataPacket {
	/// <summary>
	/// 要素名称を取得します。
	/// </summary>
	/// <value>要素名称</value>
	public string Name {
		get;
	}
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <value>要素情報</value>
	public object? Data {
		get;
	}

	/// <summary>
	/// 要素情報を生成します。
	/// </summary>
	/// <param name="name">要素情報</param>
	/// <param name="data">要素情報</param>
	public DataPacket(string name, object? data) {
		Name = name;
		Data = data;
	}

	/// <summary>
	/// 保持情報を出力します。
	/// </summary>
	/// <param name="name">要素情報</param>
	/// <param name="data">要素情報</param>
	public void Deconstruct(out string name, out object? data) {
		name = Name;
		data = Data;
	}
}
