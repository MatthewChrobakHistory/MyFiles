using MySql.Data.MySqlClient;
using System;

namespace MyFilesServer.Database.MySQL
{
    public class MySQL : IDatabase
    {
        private const string _serverHost = "localhost";
        private const string _userID = "root";
        private const string _userPassword = "admin";

        private MySqlConnection _connection;
        private MySqlCommand _command;
        private MySqlDataReader _reader;

        public MySQL() {
            Console.Write("Please select a database to connect to: ");
            string _database = Console.ReadLine();
            string _connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}",
                _serverHost,
                _database,
                _userID,
                _userPassword
            );
            this._connection = new MySqlConnection(_connectionString);
            this._connection.StateChange += _connection_StateChange;

            try {
                _connection.Open();
            } catch (MySqlException e) {
                Console.WriteLine(e.Message);
                _connection.Close();
            }
        }

        private void _connection_StateChange(object sender, System.Data.StateChangeEventArgs e) {
            Console.WriteLine(e.OriginalState + " -> " + e.CurrentState);
        }

        public bool Query(string query) {
            this._command?.Dispose();
            this._reader?.Close();
            this._command = new MySqlCommand(query, this._connection);
            this._command.Prepare();
            try {
                this._reader = _command.ExecuteReader();
                return true;
            } catch (MySqlException e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool NonQuery(string query) {
            this._command?.Dispose();
            this._command = new MySqlCommand(query, this._connection);
            this._command.Prepare();
            try {
                this._command.ExecuteNonQuery();
                return true;
            } catch (MySqlException e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public object GetValue(string key) {
            for (int i = 0; i < this._reader.FieldCount; i++) {
                if (this._reader.GetName(i).ToLower() == key.ToLower()) {
                    return this._reader.GetValue(i);
                }
            }

            Console.WriteLine("Key not found: " + key);
            return default(object);
        }

        public bool NextEntry() {
            return this._reader.Read();
        }

        public void Close() {
            this._connection.Close();
        }
    }
}
