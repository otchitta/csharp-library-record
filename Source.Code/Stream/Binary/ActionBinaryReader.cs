using System;

namespace Otchitta.Libraries.Stream.Binary;

/// <summary>
/// <see cref="Action" />経由バイナリ読込処理クラスです。
/// </summary>
public sealed class ActionBinaryReader : BinaryReader, IDisposable {
	/// <summary>
	/// 読込処理
	/// </summary>
	private BinaryReader? reader;
	/// <summary>
	/// 実行処理
	/// </summary>
	private Action<long>? action;
	/// <summary>
	/// 読込個数
	/// </summary>
	private int offset;

	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <returns>読込処理</returns>
	private BinaryReader Reader => this.reader ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 実行処理を取得します。
	/// </summary>
	/// <returns>実行処理</returns>
	private Action<long> Action => this.action ?? throw new ObjectDisposedException(GetType().FullName);

	/// <summary>
	/// <see cref="Action" />経由バイナリ読込処理を生成します。
	/// </summary>
	/// <param name="reader">読込処理</param>
	/// <param name="action">実行処理</param>
	public ActionBinaryReader(BinaryReader reader, Action<long> action) {
		this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
		this.action = action ?? throw new ArgumentNullException(nameof(action));
		this.offset = 0;
	}
	/// <summary>
	/// <see cref="Action" />経由バイナリ読込処理を生成します。
	/// </summary>
	/// <param name="reader">読込処理</param>
	/// <param name="action">実行処理</param>
	/// <param name="length"></param>
	public ActionBinaryReader(BinaryReader reader, Action<long, long> action, long length) {
		if (reader == null) {
			throw new ArgumentNullException(nameof(reader));
		} else if (action == null) {
			throw new ArgumentNullException(nameof(action));
		} else {
			this.reader = reader;
			this.action = offset => action(offset, length);
			this.offset = 0;
		}
	}

	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.reader = null;
		this.action = null;
	}

	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		var result = Reader.Read();
		if (0 <= result) Action(this.offset ++);
		return result;
	}
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	public int Read(byte[] buffer, int offset, int length) {
		var result = Reader.Read(buffer, offset, length);
		if (0 <= result) {
			this.offset += result;
			Action(this.offset);
		}
		return result;
	}
}
