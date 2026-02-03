using Microsoft.Data.Sqlite;
using FactoryApp.Models; // Подключаем наши модели
using System.Collections.Generic;

namespace FactoryApp.Data
{
    public class OrderService
    {
        private readonly string _connectionString = "Data Source=orders.db";

        public List<Order> GetOrders()
        {
            var orders = new List<Order>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); // Открываем "трубу" к базе
                // 2. Создаем команду
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Orders";

                // 3. Выполняем команду и получаем "читатель" (Reader)
                using (var reader = command.ExecuteReader())
                {
                    // 4. Читаем данные построчно, пока они есть
                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            // Получаем значения по именам колонок и кладем в объект
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                            OrderDate = reader.GetDecimal(reader.GetOrdinal("OrderDate")),
                            TotalAmount = reader.GetInt32(reader.GetOrdinal("TotalAmount")),
                        };

                        orders.Add(order);
                    }
                }
            }

            return orders; // Возвращаем готовый список
        }
    }
}