using System.Collections.Generic;

namespace Otchitta.Libraries.Record;

/// <summary>
/// 可変要素情報クラスです。
/// </summary>
public sealed class UnfixDataRecord : DataRecord {
	#region メンバー変数定義
	/// <summary>
	/// 要素一覧
	/// </summary>
	private readonly List<DataPacket> values;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素個数を取得します。
	/// </summary>
	/// <value>要素個数</value>
	public int Count => this.values.Count;
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="code">要素番号</param>
	/// <value>要素情報</value>
	public object? this[int code] {
		get => GetData(code);
		set => SetData(code, value);
	}
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="name">要素名称</param>
	/// <value>要素情報</value>
	public object? this[string name] {
		get => GetData(name);
		set => SetData(name, value);
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 可変要素情報を生成します。
	/// </summary>
	public UnfixDataRecord() {
		this.values = new List<DataPacket>();
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 要素番号を取得します。
	/// </summary>
	/// <param name="search">要素名称</param>
	/// <param name="result">要素番号</param>
	/// <returns><paramref name="search" />が存在した場合、<c>True</c>を返却</returns>
	public bool GetCode(string search, out int result) {
		for (var index = 0; index < this.values.Count; index ++) {
			var choose = this.values[index].Name;
			if (choose == search) {
				result = index;
				return true;
			}
		}
		result = default;
		return false;
	}
	/// <summary>
	/// 要素名称を取得します。
	/// </summary>
	/// <param name="code">要素番号</param>
	/// <returns>要素名称</returns>
	public string GetName(int code) => this.values[code].Name;
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="code">要素番号</param>
	/// <returns>要素情報</returns>
	public object? GetData(int code) => this.values[code].Data;
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="name">要素名称</param>
	/// <returns>要素情報</returns>
	public object? GetData(string name) => GetCode(name, out var code)? GetData(code): throw new KeyNotFoundException($"name is not found.(name={name})");
	/// <summary>
	/// 要素情報を設定します。
	/// </summary>
	/// <param name="code">要素番号</param>
	/// <param name="data">要素情報</param>
	public void SetData(int code, object? data) => this.values[code] = new DataPacket(GetName(code), data);
	/// <summary>
	/// 要素情報を設定します。
	/// </summary>
	/// <param name="name">要素名称</param>
	/// <param name="data">要素情報</param>
	public void SetData(string name, object? data) {
		if (GetCode(name, out var code)) {
			this.values[code] = new DataPacket(name, data);
		} else {
			this.values.Add(new DataPacket(name, data));
		}
	}
	/// <summary>
	/// 要素情報を削除します。
	/// </summary>
	/// <param name="code">要素番号</param>
	public void Remove(int code) => this.values.RemoveAt(code);
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
	IEnumerator<DataPacket> IEnumerable<DataPacket>.GetEnumerator() {
		foreach (var choose in this.values) {
			yield return choose;
		}
	}
	#endregion 実装メソッド定義
}
