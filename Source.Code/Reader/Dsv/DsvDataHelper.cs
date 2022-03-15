using System;
using System.Collections.Generic;
using Otchitta.Libraries.Record;

namespace Otchitta.Libraries.Reader.Dsv;

/// <summary>
/// DSV用補助関数クラスです。
/// </summary>
public static class DsvDataHelper {
	/// <summary>
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復元情報</returns>
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
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復元情報</returns>
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
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <returns>復元情報</returns>
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
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <param name="header">変換処理</param>
	/// <returns>復元情報</returns>
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
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="search">区切文字</param>
	/// <param name="escape">迂回文字</param>
	/// <param name="header">変換処理</param>
	/// <returns>復元情報</returns>
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
