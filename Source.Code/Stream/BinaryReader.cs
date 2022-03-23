using System;

namespace Otchitta.Libraries.Stream;

/// <summary>
/// バイナリ読込処理インターフェースです。
/// </summary>
public interface BinaryReader : IDisposable {
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	int Read();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="result">後続情報</param>
	/// <returns>後続情報が存在した場合、<c>True</c>を返却</returns>
	bool Read(out byte result) {
		var caches = Read();
		result = (byte)caches;
		return 0 <= caches;
	}
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	int Read(byte[] buffer, int offset, int length);
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <returns>保存した個数を返却</returns>
	int Read(byte[] buffer) => Read(buffer, 0, buffer.Length);
}
