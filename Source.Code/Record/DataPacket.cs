namespace Otchitta.Libraries.Record;

/// <summary>
/// 要素情報構造体です。
/// </summary>
public readonly struct DataPacket {
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

	/// <summary>
	/// 要素情報を変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>変換情報</returns>
	public static implicit operator System.Collections.Generic.KeyValuePair<string, object?>(DataPacket source) => new System.Collections.Generic.KeyValuePair<string, object?>(source.Name, source.Data);
	/// <summary>
	/// 要素情報を変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>変換情報</returns>
	public static explicit operator DataPacket(System.Collections.Generic.KeyValuePair<string, object?> source) => new DataPacket(source.Key, source.Value);
}
