using MyFilesServer.Database;
using System;
using System.Collections.Generic;

namespace MyFilesServer
{
    public static class Program
    {
        private static void Main(string[] args) {

            DatabaseManager.Initialize();

            while (true) {
                string input = Console.ReadLine();
                
                if (input == "close") {
                    DatabaseManager.Destroy();
                    break;
                }

                if (input == "q") {
                    string query = Console.ReadLine();
                    if (DatabaseManager.Database.Query(query)) {
                        string key = Console.ReadLine();

                        while (DatabaseManager.Database.NextEntry()) {
                            Console.WriteLine(DatabaseManager.Database.GetValue(key));
                        }
                    }
                    continue;
                }

                if (input == "populate") {

                    
                }

                if (input == "nq") {
                    string command = Console.ReadLine();
                    DatabaseManager.Database.NonQuery(command);
                    continue;
                }

                if (input == "help") {
                    Console.WriteLine("CREATE TABLE Table_Name(Column1 Type1, Column2 Type2);");
                    Console.WriteLine("SELECT Column1,Column2 FROM Table_Name WHERE Condition;");
                    Console.WriteLine("INSERT INTO Table_Name(Column1, Column2) VALUES(Value1, Value2);");
                    Console.WriteLine("ALTER TABLE Table_Name ADD Column_Name Type;");
                    Console.WriteLine("DELETE FROM Table_Name WHERE Condition;");
                    Console.WriteLine("UPDATE Table_Name SET Column1=Value1, Column2=Value2 WHERE condition;");
                    continue;
                }

                if (input == "cls" || input == "clear") {
                    Console.Clear();
                    continue;
                }

                Console.WriteLine("Unknown command: " + input);
            }
        }
    }
}
