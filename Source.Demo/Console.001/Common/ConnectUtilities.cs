using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Utilities.Common;

/// <summary>
/// 接続処理共通関数クラスです。
/// </summary>
internal sealed class ConnectUtilities {
	#region 内部メソッド定義
	/// <summary>
	/// 生成処理を取得します。
	/// </summary>
	/// <param name="source">処理名称</param>
	/// <param name="result">生成処理</param>
	/// <returns><paramref name="source" />に該当する生成処理が存在した場合、<c>True</c>を返却</returns>
	private static bool ChooseData(string source, [MaybeNullWhen(false)]out DbProviderFactory result) =>
		DbProviderFactories.TryGetFactory(source, out result);
	/// <summary>
	/// 接続処理を生成します。
	/// </summary>
	/// <param name="source">生成処理</param>
	/// <param name="result">接続処理</param>
	/// <returns>接続処理の生成に成功した場合、<c>True</c>を返却</returns>
	private static bool CreateData(DbProviderFactory source, [MaybeNullWhen(false)]out DbConnection result) {
		result = source.CreateConnection();
		return result != null;
	}
	/// <summary>
	/// 接続情報を適用します。
	/// </summary>
	/// <param name="source">接続処理</param>
	/// <param name="values">接続引数</param>
	/// <param name="errors">例外情報</param>
	/// <returns>接続引数の適用に成功した場合、<c>True</c>を返却</returns>
	private static bool UpdateData(DbConnection source, string values, [MaybeNullWhen(true)]out Exception errors) {
		try {
			source.ConnectionString = values;
			errors = default;
			return true;
		} catch (Exception error) {
			errors = error;
			return false;
		}
	}
	/// <summary>
	/// 接続処理を接続します。
	/// </summary>
	/// <param name="source">接続処理</param>
	/// <param name="errors">例外情報</param>
	/// <returns>接続処理の接続に成功した場合、<c>True</c>を返却</returns>
	private static bool InvokeData(DbConnection source, [MaybeNullWhen(true)]out Exception errors) {
		try {
			source.Open();
			errors = default;
			return true;
		} catch (Exception error) {
			errors = error;
			return false;
		}
	}
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// データベースへ接続します。
	/// </summary>
	/// <param name="connector">接続名称</param>
	/// <param name="parameter">接続引数</param>
	/// <returns>情報処理</returns>
	/// <exception cref="ConnectException">接続処理に失敗した場合</exception>
	public static DbConnection ConnectRDB(string connector, string parameter) {
		if (ChooseData(connector, out var factory) == false) {
			throw new ConnectException($"データベース処理の取得に失敗しました。{Environment.NewLine}接続名称={connector}");
		} else if (CreateData(factory, out var connection) == false) {
			throw new ConnectException($"データベース処理の生成に失敗しました。{Environment.NewLine}接続名称={connector}");
		} else if (UpdateData(connection, parameter, out var exception) == false) {
			throw new ConnectException($"データベース処理の設定に失敗しました。{Environment.NewLine}接続名称={connector}{Environment.NewLine}接続引数={parameter}", exception);
		} else if (InvokeData(connection, out exception) == false) {
			throw new ConnectException($"データベース処理の接続に失敗しました。{Environment.NewLine}接続名称={connector}{Environment.NewLine}接続引数={parameter}", exception);
		} else {
			return connection;
		}
	}
	#endregion 公開メソッド定義
}
