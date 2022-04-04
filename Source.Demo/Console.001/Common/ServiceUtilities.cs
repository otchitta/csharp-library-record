using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Utilities.Common;

/// <summary>
/// 情報処理共通関数クラスです。
/// </summary>
internal static class ServiceUtilities {
	#region 内部メソッド定義(AddSettingData)
	/// <summary>
	/// 設定情報を設定します。
	/// </summary>
	/// <param name="executeData">実行処理</param>
	/// <param name="settingName">設定名称</param>
	/// <param name="settingType">設定種別</param>
	/// <param name="settingData">設定情報</param>
	private static void AddSettingData(IDbCommand executeData, string settingName, DbType settingType, object? settingData) {
		var parameter = executeData.CreateParameter();
		parameter.ParameterName = settingName;
		parameter.DbType        = settingType;
		parameter.Value         = settingData ?? DBNull.Value;
		executeData.Parameters.Add(parameter);
	}
	#endregion 内部メソッド定義(AddSettingData)

	#region 公開メソッド定義(AddSetting～)
	/// <summary>
	/// 設定情報を設定します。
	/// </summary>
	/// <param name="executeData">実行処理</param>
	/// <param name="settingName">設定名称</param>
	/// <param name="settingData">設定情報</param>
	public static void AddSettingText(this IDbCommand executeData, string settingName, string? settingData) =>
		AddSettingData(executeData, settingName, DbType.String, settingData);
	/// <summary>
	/// 設定情報を設定します。
	/// </summary>
	/// <param name="executeData">実行処理</param>
	/// <param name="settingName">設定名称</param>
	/// <param name="settingData">設定情報</param>
	public static void AddSettingInt4(this IDbCommand executeData, string settingName, int? settingData) =>
		AddSettingData(executeData, settingName, DbType.Int32, settingData);
	/// <summary>
	/// 設定情報を設定します。
	/// </summary>
	/// <param name="executeData">実行処理</param>
	/// <param name="settingName">設定名称</param>
	/// <param name="settingData">設定情報</param>
	public static void AddSettingInt8(this IDbCommand executeData, string settingName, long? settingData) =>
		AddSettingData(executeData, settingName, DbType.Int64, settingData);
	#endregion 公開メソッド定義(AddSetting～)
}
