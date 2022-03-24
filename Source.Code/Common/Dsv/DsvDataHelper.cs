using System;
using System.Collections.Generic;
using System.Text;
using Otchitta.Libraries.Record;

namespace Otchitta.Libraries.Common.Dsv;

/// <summary>
/// DSV用補助関数クラスです。
/// </summary>
public static class DsvDataHelper {
	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">対象個数</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	internal static string DecodeText(StringBuilder source, int offset, int length, char escape) {
		var result = new StringBuilder(length);
		var ignore = false;
		for (var index = 0; index < length; index ++) {
			var choose = source[offset + index];
			if (choose == escape) {
				if (ignore == false) result.Append(choose);
				ignore = !ignore;
			} else if (ignore == true) {
				// 連続した迂回文字ではない場合：例外発行
				throw new SystemException($"escape must be duplicated.(escape={escape}, source={source})");
			} else {
				result.Append(choose);
			}
		}
		if (ignore == true) {
			// 連続した迂回文字ではない場合：例外発行
			throw new SystemException($"escape must be duplicated.(escape={escape}, source={source})");
		}
		return result.ToString();
	}
	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">対象個数</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	internal static string DecodeItem(StringBuilder source, int offset, int length, char escape) {
		if (source.Length <= 0) {
			return String.Empty;
		} else if (source.Length == 1) {
			return DecodeText(source, offset, length, escape);
		} else if (source[offset] == escape && source[offset + length - 1] == escape) {
			return DecodeText(source, offset + 1, length - 2, escape);
		} else {
			return DecodeText(source, offset, length, escape);
		}
	}
	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	internal static IEnumerable<string> DecodeLine(StringBuilder source, char search, char escape) {
		var offset = 0;
		var ignore = false;
		for (var index = 0; index < source.Length; index ++) {
			var choose = source[index];
			if (choose == escape) {
				ignore = !ignore;
			} else if (ignore) {
				// 処理なし
			} else if (choose == search) {
				yield return DecodeItem(source, offset, index - offset, escape);
				offset = index + 1;
			}
		}
		if (offset < source.Length) {
			yield return DecodeItem(source, offset, source.Length - offset, escape);
		}
	}
	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="reader">読込処理</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	public static IEnumerable<DsvDataSource> DecodeData(Func<int> reader, char search, char escape) {
		var buffer = new StringBuilder();
		var ignore = false;
		var before = (char)0;
		var offset = 1;
		while (true) {
			var choose = reader();
			if (choose < 0) {
				break;
			} else if (choose == escape) {
				ignore = !ignore;
				buffer.Append(choose);
			} else if (ignore) {
				buffer.Append(choose);
			} else if (before == '\r' && choose == '\n') {
				buffer.Length --;
				yield return new DsvDataSource(offset, DecodeLine(buffer, search, escape));
				buffer.Length = 0;
			} else {
				buffer.Append(choose);
			}
			if (choose == '\n') offset ++;
		}
		if (buffer.Length != 0) {
			yield return new DsvDataSource(offset, DecodeLine(buffer, search, escape));
		}
	}

	#region 暗号メソッド定義(EncodeText/EncodeData)
	/// <summary>
	/// 暗号情報へ変換します。
	/// </summary>
	/// <param name="source">復号情報</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>暗号情報</returns>
	internal static string EncodeText(string source, char escape) {
		var change = source.Replace($"{escape}", $"{escape}{escape}");
		return $"{escape}{change}{escape}";
	}
	/// <summary>
	/// 復号情報を書込みます。
	/// </summary>
	/// <param name="writer">書込処理</param>
	/// <param name="source">書込情報</param>
	/// <param name="middle">区切文字</param>
	/// <param name="escape">迂回文字</param>
	internal static void EncodeData(Action<string> writer, DsvDataSource source, string middle, char escape) {
		var prefix = String.Empty;
		foreach (var choose in source.Source) {
			writer(prefix);
			writer(EncodeText(choose, escape));
			prefix = middle;
		}
	}
	/// <summary>
	/// 復号情報を書込みます。
	/// </summary>
	/// <param name="writer">書込処理</param>
	/// <param name="source">書込集合</param>
	/// <param name="middle">区切文字</param>
	/// <param name="escape">迂回文字</param>
	public static void EncodeData(Action<string> writer, IEnumerable<DsvDataSource> source, char middle, char escape) {
		var change = middle.ToString();
		var prefix = String.Empty;
		foreach (var choose in source) {
			if (String.IsNullOrEmpty(prefix) == false) writer(prefix);
			EncodeData(writer, choose, change, escape);
			prefix = "\r\n";
		}
	}
	#endregion 暗号メソッド定義(EncodeText/EncodeData)

	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	[Obsolete]
	internal static string DecodeText(ReadOnlySpan<char> source, char escape) {
		var result = new System.Text.StringBuilder(source.Length);
		var ignore = false;
		foreach (var choose in source) {
			if (choose == escape) {
				if (ignore == false) result.Append(choose);
				ignore = !ignore;
			} else if (ignore == true) {
				throw new SystemException($"escape must be duplicated.(escape={escape}, source={source})");
			} else {
				result.Append(choose);
			}
		}
		if (ignore == true) {
			throw new SystemException($"escape must be duplicated.(escape={escape}, source={source})");
		}
		return result.ToString();
	}

	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	[Obsolete]
	internal static string DecodeItem(ReadOnlySpan<char> source, char escape) {
		if (source.Length <= 0) {
			return String.Empty;
		} else if (source.Length == 1) {
			return DecodeText(source, escape);
		} else if (source[0] == escape && source[source.Length - 1] == escape) {
			return DecodeText(source.Slice(1, source.Length - 2), escape);
		} else {
			return DecodeText(source, escape);
		}
	}

	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復号情報</returns>
	[Obsolete]
	internal static List<string> DecodeLine(ReadOnlySpan<char> source, char search, char escape) {
		var result = new List<string>();
		var offset = 0;
		var ignore = false;
		for (var index = 0; index < source.Length; index ++) {
			var choose = source[index];
			if (choose == escape) {
				ignore = !ignore;
			} else if (ignore) {
				// 処理なし
			} else if (choose == search) {
				result.Add(DecodeItem(source.Slice(offset, index - offset), escape));
				offset = index + 1;
			}
		}
		if (offset < source.Length) {
			result.Add(DecodeItem(source.Slice(offset), escape));
		}
		return result;
	}

	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <param name="header">変換処理</param>
	/// <returns>復号情報</returns>
	[Obsolete]
	internal static DataRecord DecodeData(ReadOnlySpan<char> source, char search, char escape, Func<int, string> header) {
		var result = new UnfixDataRecord();
		var offset = 0;
		foreach (var cache2 in DecodeLine(source, search, escape)) {
			result[header(offset)] = cache2;
			offset ++;
		}
		return result;
	}

	/// <summary>
	/// 復号情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <param name="header">変換処理</param>
	/// <returns>復号情報</returns>
	[Obsolete]
	public static List<DataRecord> DecodeText(ReadOnlySpan<char> source, char search, char escape, Func<int, string> header) {
		var result = new List<DataRecord>();
		var offset = 0;
		var ignore = false;
		var before = (char)0;
		for (var index = 0; index < source.Length; index ++) {
			var choose = source[index];
			if (choose == escape) {
				ignore = !ignore;
			} else if (ignore) {
				// 処理なし
			} else if (before == '\r' && choose == '\n') {
				result.Add(DecodeData(source.Slice(offset, index - offset - 1), search, escape, header));
				offset = index + 1;
			}
			before = choose;
		}
		if (offset < source.Length) {
			result.Add(DecodeData(source.Slice(offset), search, escape, header));
		}
		return result;
	}
}
