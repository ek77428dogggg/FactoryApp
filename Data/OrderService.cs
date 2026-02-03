using FactoryApp.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace FactoryApp.Data
{
    public class OrderService
    {
        private string connectionString = "Data Source=factory.db";

        public OrderService()
        {
            // Создаем таблицу при инициализации сервиса
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                
                // Создаем таблицу, если она не существует
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerName TEXT NOT NULL,
                        OrderDate TEXT NOT NULL,
                        TotalAmount REAL NOT NULL
                    )";
                command.ExecuteNonQuery();

                // Проверяем, есть ли данные в таблице
                command.CommandText = "SELECT COUNT(*) FROM Orders";
                var count = (long)command.ExecuteScalar();
                
                // Если таблица пустая, добавляем тестовые данные
                if (count == 0)
                {
                    AddSampleData(connection);
                }
            }
        }

        private void AddSampleData(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Orders (CustomerName, OrderDate, TotalAmount) VALUES
                    ('ООО Ромашка', '2024-01-15', 12500.50),
                    ('ИП Иванов', '2024-01-16', 8450.75),
                    ('ЗАО Лютик', '2024-01-17', 32000.00),
                    ('АО Светлячок', '2024-01-18', 15670.30)";
            command.ExecuteNonQuery();
        }

        public List<Order> GetOrders()
        {
            var orders = new List<Order>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Orders ORDER BY Id";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            Id = reader.GetInt32(0),
                            CustomerName = reader.GetString(1),
                            OrderDate = reader.GetString(2),
                            TotalAmount = reader.GetDecimal(3)
                        };
                        orders.Add(order);
                    }
                }
            }

            return orders;
        }
    }
}