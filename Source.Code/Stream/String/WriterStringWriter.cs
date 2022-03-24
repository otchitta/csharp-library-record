using System;
using System.IO;

namespace Otchitta.Libraries.Stream.String;

/// <summary>
/// <see cref="TextWriter" />利用書込処理クラスです。
/// </summary>
public sealed class WriterStringWriter : StringWriter, IDisposable {
	/// <summary>
	/// 書込処理
	/// </summary>
	private TextWriter? writer;

	/// <summary>
	/// 書込処理を取得します。
	/// </summary>
	/// <returns>書込処理</returns>
	private TextWriter Writer => this.writer ?? throw new ObjectDisposedException(GetType().FullName);

	/// <summary>
	/// <see cref="TextWriter" />利用書込処理を生成します。
	/// </summary>
	/// <param name="writer">書込処理</param>
	public WriterStringWriter(TextWriter writer) {
		this.writer = writer;
	}

	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.writer = null;
	}

	/// <summary>
	/// 引数情報を書込みます。
	/// </summary>
	/// <param name="buffer">書込情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">書込個数</param>
	public void Write(char[] buffer, int offset, int length) {
		Writer.Write(buffer, offset, length);
	}
	/// <summary>
	/// 保持情報を書込みます。
	/// </summary>
	public void Flush() {
		Writer.Flush();
	}
}
