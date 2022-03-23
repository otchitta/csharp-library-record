using System;

namespace Otchitta.Libraries.Stream.Binary;

/// <summary>
/// <see cref="Stream" />利用バイナリ読込処理クラスです。
/// </summary>
public sealed class StreamBinaryReader : BinaryReader, IDisposable {
	/// <summary>
	/// 読込処理
	/// </summary>
	private System.IO.Stream? stream;

	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <returns>読込処理</returns>
	private System.IO.Stream Stream => this.stream ?? throw new ObjectDisposedException(GetType().FullName);

	/// <summary>
	/// <see cref="Stream" />利用バイナリ読込処理を生成します。
	/// </summary>
	/// <param name="stream">読込処理</param>
	/// <exception cref="ArgumentNullException"><paramref name="stream" />が<c>Null</c>である場合</exception>
	public StreamBinaryReader(System.IO.Stream stream) {
		this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
	}

	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.stream = null;
	}

	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() => Stream.ReadByte();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(byte[] buffer, int offset, int length) => Stream.Read(buffer, offset, length);
}
