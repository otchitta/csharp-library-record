using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Otchitta.Libraries.Stream;

namespace Otchitta.Libraries.Common.Ini;

/// <summary>
/// INI用補助関数クラスです。
/// </summary>
public static class IniDataHelper {
	/// <summary>
	/// 復元情報へ変換します。
	/// </summary>
	/// <param name="reader">読込処理</param>
	/// <returns>復元情報</returns>
	public static IniSourceData DecodeData(Func<int> reader) {
		var caches = new System.Text.StringBuilder();
		var buffer = new Builder();
		var before = 0;
		while (true) {
			var choose = reader();
			if (choose < 0) {
				break;
			} else if (before == '\r' && choose == '\n') {
				buffer.RegistData(caches.ToString(0, caches.Length - 1));
				caches.Clear();
			} else {
				caches.Append((char)choose);
			}
			before = choose;
		}
		return buffer.CreateData();
	}

	#region 非公開クラス定義
	/// <summary>
	/// 構築処理クラスです。
	/// </summary>
	private sealed class Builder {
		#region メンバー変数定義
		/// <summary>
		/// 分類名称
		/// </summary>
		private string? divideName;
		/// <summary>
		/// 設定情報
		/// </summary>
		private IniDetailList? headerData;
		/// <summary>
		/// 分類一覧
		/// </summary>
		private readonly List<IniDivideData> divideList;
		/// <summary>
		/// 項目一覧
		/// </summary>
		private readonly List<IniDetailData> detailList;
		#endregion メンバー変数定義

		#region 生成メソッド定義
		/// <summary>
		/// 構築処理を生成します。
		/// </summary>
		public Builder() {
			this.divideName = null;
			this.headerData = null;
			this.divideList = new List<IniDivideData>();
			this.detailList = new List<IniDetailData>();
		}
		#endregion 生成メソッド定義

		#region 内部メソッド定義
		/// <summary>
		/// 項目情報を解析します。
		/// </summary>
		/// <param name="sourceText">解析情報</param>
		/// <param name="resultName">結果名称</param>
		/// <param name="resultText">結果内容</param>
		/// <returns>解析に成功した場合、<c>True</c>を返却</returns>
		private static bool DecodeData(string sourceText, [MaybeNullWhen(false)]out string resultName, [MaybeNullWhen(false)]out string resultText) {
			var choose = sourceText.IndexOf('=');
			if (choose < 0) {
				resultName = default;
				resultText = default;
				return false;
			} else {
				resultName = sourceText.Substring(0, choose);
				resultText = sourceText.Substring(choose + 1);
				return true;
			}
		}
		#endregion 内部メソッド定義

		#region 公開メソッド定義
		/// <summary>
		/// 要素情報を登録します。
		/// </summary>
		/// <param name="registText">要素情報</param>
		public void RegistData(string registText) {
			if (String.IsNullOrWhiteSpace(registText) || registText.StartsWith(';') || registText.StartsWith('#')) {
				// 処理なし
			} else if (registText.StartsWith('[') && registText.EndsWith(']')) {
				var detailList = new IniDetailList(this.detailList);
				if (this.divideName == null) {
					this.headerData = detailList;
				} else {
					this.divideList.Add(new IniDivideData(this.divideName, detailList));
				}
				this.divideName = registText.Substring(1, registText.Length - 2);
				this.detailList.Clear();
			} else if (DecodeData(registText, out var detailName, out var detailText)) {
				this.detailList.Add(new IniDetailData(detailName, detailText));
			} else {
				throw new Exception($"invalid ini format.({registText})");
			}
		}
		/// <summary>
		/// 全体情報を生成します。
		/// </summary>
		/// <returns>全体情報</returns>
		public IniSourceData CreateData() {
			var detailList = this.headerData ?? new IniDetailList();
			var divideList = new IniDivideList(this.divideList);
			this.divideName = null;
			this.headerData = null;
			this.divideList.Clear();
			this.detailList.Clear();
			return new IniSourceData(detailList, divideList);
		}
		#endregion 公開メソッド定義
	}
	#endregion 非公開クラス定義
}
