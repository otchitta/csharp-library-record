using System;
using System.Text;

namespace Otchitta.Libraries.Stream.String;

/// <summary>
/// <see cref="BinaryReader" />利用テキスト読込処理クラスです。
/// </summary>
public sealed class BinaryStringReader : StringReader, IDisposable {
	#region メンバー変数定義
	/// <summary>
	/// 読込処理
	/// </summary>
	private BinaryReader? sourceData;
	/// <summary>
	/// 変換情報
	/// </summary>
	private Encoding? encodeData;
	/// <summary>
	/// 変換処理
	/// </summary>
	private Decoder? decodeData;
	/// <summary>
	/// 一時情報
	/// </summary>
	private byte[]? binaryList;
	/// <summary>
	/// 一時個数
	/// </summary>
	private int binarySize;
	/// <summary>
	/// 先行情報
	/// </summary>
	private char[]? bufferList;
	/// <summary>
	/// 先行位置
	/// </summary>
	private int bufferCode;
	/// <summary>
	/// 先行個数
	/// </summary>
	private int bufferSize;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 読込処理を取得します。
	/// </summary>
	/// <returns>読込処理</returns>
	private BinaryReader SourceData => this.sourceData ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 変換処理を取得します。
	/// </summary>
	/// <returns>変換処理</returns>
	private Decoder DecodeData => this.decodeData ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 一時配列を取得します。
	/// </summary>
	/// <returns>一時配列</returns>
	private byte[] BinaryList => this.binaryList ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// <see cref="BinaryReader" />利用テキスト読込処理を生成します。
	/// </summary>
	/// <param name="sourceData">読込処理</param>
	/// <param name="encodeData">変換情報</param>
	/// <param name="bufferSize">一時個数</param>
	/// <exception cref="ArgumentNullException"><paramref name="sourceData" />が<c>Null</c>である場合</exception>
	/// <exception cref="ArgumentNullException"><paramref name="encodeData" />が<c>Null</c>である場合</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize" />が<c>0</c>以下である場合</exception>
	public BinaryStringReader(BinaryReader sourceData, Encoding encodeData, int bufferSize = 1024) {
		if (sourceData == null) {
			throw new ArgumentNullException(nameof(sourceData));
		} else if (encodeData == null) {
			throw new ArgumentNullException(nameof(encodeData));
		} else if (bufferSize <= 0) {
			throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, "must be positive.");
		} else {
			this.sourceData = sourceData;
			this.encodeData = encodeData;
			this.decodeData = encodeData.GetDecoder();
			this.binaryList = new byte[bufferSize];
			this.binarySize = 0;
			this.bufferList = new char[this.encodeData.GetMaxCharCount(this.binaryList.Length)];
			this.bufferCode = 0;
			this.bufferSize = 0;
		}
	}
	#endregion 生成メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.sourceData = null;
		this.encodeData = null;
		this.decodeData = null;
		this.binaryList = null;
		this.bufferList = null;
	}
	#endregion 破棄メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 読込処理を実行します。
	/// </summary>
	private void Reload() {
		if (this.binaryList == null || this.bufferList == null) {
			throw new ObjectDisposedException(GetType().FullName);
		} else {
			// バイナリ読込
			this.binarySize = SourceData.Read(this.binaryList, 0, this.binaryList.Length);
			if (this.binarySize <= 0) {
				// 読込なしの場合
				this.bufferCode = 0;
				this.bufferSize = 0;
			} else {
				// 読込ありの場合
				this.bufferCode = 0;
				this.bufferSize = DecodeData.GetChars(this.binaryList, 0, this.binarySize, this.bufferList, 0);
			}
		}
	}
	#endregion 内部メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	public int Read() {
		if (this.bufferList == null) {
			throw new ObjectDisposedException(GetType().FullName);
		} else {
			if (this.bufferSize - this.bufferCode <= 0) {
				// 一時情報なしの場合：読込処理実行
				Reload();
				if (this.bufferSize == 0) {
					// 読込なしの場合
					return -1;
				}
			}
			// 一時情報のデータを返却
			var resultData = this.bufferList[this.bufferCode];
			this.bufferCode++;
			return resultData;
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
		if (this.bufferList == null) {
			throw new ObjectDisposedException(GetType().FullName);
		} else if (buffer == null) {
			throw new ArgumentNullException(nameof(buffer));
		} else if (offset < 0) {
			throw new ArgumentOutOfRangeException(nameof(offset), offset, "must be non negative.");
		} else if (length < 0) {
			throw new ArgumentOutOfRangeException(nameof(length), length, "must be non negative.");
		} else if (length == 0) {
			return 0;
		} else if (buffer.Length < offset + length) {
			throw new ArgumentOutOfRangeException(nameof(buffer), "few capacity.");
		} else if (length <= this.bufferSize - this.bufferCode) {
			// 一時個数以下の場合
			for (var index = 0; index < length; index++) {
				buffer[offset + index] = this.bufferList[this.bufferCode + index];
			}
			this.bufferCode += length;
			return length;
		} else {
			// 一時個数超過の場合
			var amount = this.bufferSize - this.bufferCode;
			for (var index = 0; index < amount; index++) {
				buffer[offset + index] = this.bufferList[this.bufferCode + index];
			}
			while (true) {
				Reload();
				if (this.binarySize <= 0) {
					return amount;
				} else if (this.bufferSize < length - amount) {
					for (var index = 0; index < this.bufferSize; index++) {
						buffer[offset + amount + index] = this.bufferList[index];
					}
					amount += this.bufferSize;
				} else {
					for (var index = 0; index < length - amount; index++) {
						buffer[offset + amount + index] = this.bufferList[index];
					}
					this.bufferCode = length - amount;
					return amount + this.bufferCode;
				}
			}
		}
	}
	#endregion 実装メソッド定義
}
