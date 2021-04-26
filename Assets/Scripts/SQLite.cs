using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace testDB
{

	public class SQLite : MonoBehaviour
	{

		private string dbPath;

		private void Start()
		{
			dbPath = "URI=file:" + Application.persistentDataPath + "/game_db.db";
			CreateSchema();
		}

		public void CreateSchema()
		{
			using (var conn = new SqliteConnection(dbPath))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'players' ( " +
									  "  'id' INTEGER NOT NULL, " +
									  "  'username' TEXT NOT NULL UNIQUE, " +
									  "  'password' TEXT NOT NULL, " +
									  "  'score' INTEGER NOT NULL DEFAULT 0, " +
									  "  PRIMARY KEY('id' AUTOINCREMENT)" +
									  ");";

					var result = cmd.ExecuteNonQuery();
					Debug.Log("create schema: " + result);
				}
			}
		}
	}
}