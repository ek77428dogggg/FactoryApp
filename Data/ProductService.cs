using Microsoft.Data.Sqlite;
using FactoryApp.Models; // Подключаем наши модели
using System.Collections.Generic;

namespace FactoryApp.Data
{
    public class ProductService
    {
        // Строка подключения. Она говорит: "База лежит рядом с файлом запуска"
        private readonly string _connectionString = "Data Source=factory.db";

        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            // 1. Создаем соединение с базой
            // using гарантирует, что соединение закроется, даже если произойдет ошибка
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); // Открываем "трубу" к базе
                // 2. Создаем команду
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Price, Stock, Description FROM Products";

                // 3. Выполняем команду и получаем "читатель" (Reader)
                using (var reader = command.ExecuteReader())
                {
                    // 4. Читаем данные построчно, пока они есть
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            // Получаем значения по именам колонок и кладем в объект
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            // Description может быть пустым (null), поэтому проверяем
                            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                        ? ""
                                        : reader.GetString(reader.GetOrdinal("Description"))
                        };

                        products.Add(product);
                    }
                }
            }

            return products; // Возвращаем готовый список
        }
    }
}