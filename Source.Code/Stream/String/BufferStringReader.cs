using System;
using System.Text;

namespace Otchitta.Libraries.Stream.String;

/// <summary>
/// 一時テキスト読込処理クラスです。
/// </summary>
public sealed class BufferStringReader : StringReader, IDisposable {
	#region メンバー変数定義
	/// <summary>
	/// 要素情報
	/// </summary>
	private StringBuilder? source;
	/// <summary>
	/// 現在位置
	/// </summary>
	private int offset;
	/// <summary>
	/// 要素残数
	/// </summary>
	private int length;
	#endregion メンバー変数定義

	#region 生成メソッド定義
	/// <summary>
	/// 一時テキスト読込処理を生成します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">読込個数</param>
	public BufferStringReader(StringBuilder source, int offset, int length) {
		this.source = source;
		this.offset = offset;
		this.length = length;
	}
	/// <summary>
	/// 一時文字読込処理を生成します。
	/// </summary>
	/// <param name="source">要素情報</param>
	public BufferStringReader(StringBuilder source) : this(source, 0, source.Length) {
		// 処理なし
	}
	#endregion 生成メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.source = null;
		this.offset = default;
		this.length = default;
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		var source = this.source;
		if (source == null) {
			throw new ObjectDisposedException(GetType().FullName);
		} else if (this.length <= 0) {
			return -1;
		} else {
			var result = source[this.offset];
			this.offset ++;
			this.length --;
			return result;
		}
	}
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存個数</returns>
	public int Read(char[] buffer, int offset, int length) {
		var source = this.source;
		if (source == null) {
			throw new ObjectDisposedException(GetType().FullName);
		} else {
			var result = Math.Min(this.length, length);
			source.CopyTo(this.offset, buffer, offset, result);
			this.offset += result;
			this.length -= result;
			return result;
		}
	}
	#endregion 実装メソッド定義
}
