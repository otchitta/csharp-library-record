using System;
using System.Data;
using System.IO;
using System.Text;
using Otchitta.Libraries.Common.Dsv;
using Otchitta.Utilities.Common;

namespace Otchitta.Utilities.Record;

/// <summary>
/// メインルーチンクラスです。
/// </summary>
internal static class MainRoutine {

	private const string ConnectName = "System.Data.SQLite";

	#region プロパティー定義
	/// <summary>
	/// 変換処理
	/// </summary>
	private static Encoding? decoder;
	/// <summary>
	/// 変換処理を取得します。
	/// </summary>
	/// <returns>変換処理</returns>
	private static Encoding Decoder => decoder ??= Encoding.GetEncoding("Shift-JIS");
	#endregion プロパティー定義

	#region 内部メソッド定義(OutputText)
	/// <summary>
	/// 引数情報を出力します。
	/// </summary>
	/// <param name="source">出力情報</param>
	private static void OutputText(string source) =>
		Console.WriteLine(String.Format("{0:yyyy-MM-dd HH\\:mm\\:ss.fff} {1}", DateTime.Now, source));
	#endregion 内部メソッド定義(OutputText)

	#region 内部メソッド定義(CreateText)
	/// <summary>
	/// 生成構文を生成します。
	/// </summary>
	/// <returns>生成構文</returns>
	private static string CreateText() {
		var result = new StringBuilder();
		result.Append("CREATE TABLE t_import_data(");
		result.Append(  "f_data_name NVARCHAR(100) NOT NULL");
		result.Append(", f_file_name NVARCHAR(200) NOT NULL");
		result.Append(", f_line_no   BIGINT        NOT NULL");
		for (var index = 1; index <= 150; index ++) {
			result.Append(", f_item_");
			result.Append(index.ToString("0000"));
			result.Append(" NVARCHAR(8000)     NULL");
		}
		result.Append(", CONSTRAINT pk_import_data PRIMARY KEY(f_data_name, f_file_name, f_line_no))");
		return result.ToString();
	}
	/// <summary>
	/// 挿入構文を生成します。
	/// </summary>
	/// <param name="length">要素個数</param>
	/// <returns>挿入構文</returns>
	private static string CreateText(int length) {
		var result = new StringBuilder();
		result.Append("INSERT INTO t_import_data(f_data_name, f_file_name, f_line_no");
		for (var index = 1; index <= length; index ++) {
			result.Append(", f_item_");
			result.Append(index.ToString("0000"));
		}
		result.Append(") VALUES(@DataName, @FileName, @LineNo");
		for (var index = 1; index <= length; index ++) {
			result.Append(", @Item");
			result.Append(index.ToString("0000"));
		}
		result.Append(")");
		return result.ToString();
	}
	#endregion 内部メソッド定義(CreateText)

	#region 内部メソッド定義(ImportFile)
	/// <summary>
	/// 要素情報を取込みます。
	/// </summary>
	/// <param name="divideName">分類名称</param>
	/// <param name="sourceFile">取込情報</param>
	/// <param name="invokeData">実行処理</param>
	private static void ImportFile(string divideName, string sourceFile, IDbCommand invokeData) {
		OutputText($"[開始]{divideName}:{sourceFile}");
		using (var stream = File.Open(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
		using (var reader = new StreamReader(stream, Decoder)) {
			invokeData.CommandText = "DELETE FROM t_import_data WHERE f_data_name = @DataName AND f_file_name = @FileName";
			invokeData.Parameters.Clear();
			invokeData.AddSettingText("DataName", divideName);
			invokeData.AddSettingText("FileName", Path.GetFileName(sourceFile));
			invokeData.ExecuteNonQuery();
			foreach (var choose in DsvDataHelper.DecodeData(reader.Read, ',', '"')) {
				invokeData.CommandText = CreateText(choose.Source.Count);
				invokeData.Parameters.Clear();
				invokeData.AddSettingText("DataName", divideName);
				invokeData.AddSettingText("FileName", Path.GetFileName(sourceFile));
				invokeData.AddSettingInt8("LineNo",   choose.Offset);
				for (var index = 0; index < choose.Source.Count; index ++) {
					invokeData.AddSettingText(String.Format("Item{0:0000}", index + 1), choose.Source[index]);
				}
				invokeData.ExecuteNonQuery();
			}
		}
		OutputText($"[終了]{divideName}:{sourceFile}");
	}
	/// <summary>
	/// 要素情報を取込みます。
	/// </summary>
	/// <param name="divideName">分類名称</param>
	/// <param name="sourceFile">取込情報</param>
	private static void ImportFile(string divideName, string sourceFile) {
		var importFile = ExecuteUtilities.GetAbsolutePath("import.db");
		var existsFlag = File.Exists(importFile);
		using (var connection  = ConnectUtilities.ConnectRDB(ConnectName, $"Data Source={importFile}"))
		using (var transaction = connection.BeginTransaction())
		using (var command     = connection.CreateCommand()) {
			command.Transaction = transaction;
			if (existsFlag == false) {
				command.CommandText = CreateText();
				command.ExecuteNonQuery();
			}
			try {
				ImportFile(divideName, sourceFile, command);
				transaction.Commit();
			} catch {
				transaction.Rollback();
				throw;
			}
		}
	}
	#endregion 内部メソッド定義(ImportFile)

	/// <summary>
	/// 引数情報を実行します。
	/// </summary>
	/// <param name="divideName">分類名称</param>
	/// <param name="searchPath"></param>
	private static void InvokeData(string divideName, string searchPath) {
		if (File.Exists(searchPath)) {
			ImportFile(divideName, searchPath);
		} else if (Directory.Exists(searchPath)) {
			foreach (var chooseFile in Directory.GetFiles(searchPath, "*.csv")) {
				ImportFile(divideName, chooseFile);
			}
		} else {
			OutputText("[情報]第二引数がファイルでもフォルダでもありません。");
		}
	}

	public static void Main(string[] commands) {
		if (commands.Length == 2) {
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			System.Data.Common.DbProviderFactories.RegisterFactory("System.Data.SQLite", System.Data.SQLite.SQLiteFactory.Instance);
			try {
				InvokeData(commands[0], commands[1]);
			} catch (Exception error) {
				Console.WriteLine("[失敗]取込処理に失敗しました。");
				Console.WriteLine(error);
			}
		} else {
			Console.WriteLine("Otchitta.Utilities.Record 情報名称 取込パス");
		}
	}
}
