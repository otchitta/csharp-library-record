using System;
using System.Text;

namespace Otchitta.Libraries.Stream.String;

/// <summary>
/// <see cref="StringBuilder" />利用書込処理クラスです。
/// </summary>
public sealed class BufferStringWriter : StringWriter, IDisposable {
	/// <summary>
	/// 書込情報
	/// </summary>
	private readonly StringBuilder buffer;

	/// <summary>
	/// <see cref="StringBuilder" />利用書込処理を生成します。
	/// </summary>
	public BufferStringWriter() {
		this.buffer = new StringBuilder();
	}

	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.buffer.Clear();
	}

	/// <summary>
	/// 引数情報を書込みます。
	/// </summary>
	/// <param name="buffer">書込情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">書込個数</param>
	public void Write(char[] buffer, int offset, int length) {
		this.buffer.Append(buffer, offset, length);
	}
	/// <summary>
	/// 保持情報を書込みます。
	/// </summary>
	public void Flush() {
		// 処理なし
	}

	/// <summary>
	/// 保持情報を表現文字列へ変換します。
	/// </summary>
	/// <returns>保持情報</returns>
	public override string ToString() {
		return this.buffer.ToString();
	}
}
