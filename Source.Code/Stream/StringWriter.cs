namespace Otchitta.Libraries.Stream;

/// <summary>
/// テキスト書込処理インターフェースです。
/// </summary>
public interface StringWriter {
	/// <summary>
	/// 引数情報を書込みます。
	/// </summary>
	/// <param name="buffer">書込情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">書込個数</param>
	void Write(char[] buffer, int offset, int length);
	/// <summary>
	/// 引数情報を書込みます。
	/// </summary>
	/// <param name="buffer">書込情報</param>
	void Write(params char[] buffer) => Write(buffer, 0, buffer.Length);
	/// <summary>
	/// 引数情報を書込みます。
	/// </summary>
	/// <param name="source">書込情報</param>
	void Write(string source) => Write(source.ToCharArray());

	/// <summary>
	/// 保持情報を書込みます。
	/// </summary>
	void Flush();
}
