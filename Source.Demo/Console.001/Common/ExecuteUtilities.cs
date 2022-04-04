using System;
using System.IO;
using System.Reflection;

namespace Otchitta.Utilities.Common;

/// <summary>
/// 実行処理共通関数クラスです。
/// </summary>
internal static class ExecuteUtilities {
	#region メンバー変数定義
	/// <summary>
	/// 実行情報
	/// </summary>
	private static Assembly? executeData;
	/// <summary>
	/// 実行情報
	/// </summary>
	private static string? executeFile;
	/// <summary>
	/// 実行位置
	/// </summary>
	private static string? executePath;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 実行情報を取得します。
	/// </summary>
	/// <returns>実行情報</returns>
	private static Assembly ExecuteData => executeData ??= Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
	/// <summary>
	/// 実行名称を取得します。
	/// </summary>
	/// <returns>実行名称</returns>
	public static string ExecuteFile => executeFile ??= ExecuteData.Location;
	/// <summary>
	/// 実行位置を取得します。
	/// </summary>
	/// <returns>実行位置</returns>
	public static string ExecutePath => executePath ??= ChoosePath(ExecuteFile);
	#endregion プロパティー定義

	#region 内部メソッド定義
	/// <summary>
	/// 経由情報を取得します。
	/// </summary>
	/// <param name="source">実行名称</param>
	/// <returns>経由情報</returns>
	private static string ChoosePath(string source) =>
		Path.GetDirectoryName(source) ?? throw new SystemException($"don't convert directory name.(source={source})");
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 絶対パスを取得します。
	/// </summary>
	/// <param name="source">相対パス</param>
	/// <returns>絶対パス</returns>
	public static string GetAbsolutePath(string source) => Path.GetFullPath(Path.Combine(ExecutePath, source));
	#endregion 公開メソッド定義
}
