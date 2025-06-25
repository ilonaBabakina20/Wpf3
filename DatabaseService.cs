using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace WpfApp3
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Client> GetClients(string searchText = null)
        {
            var clients = new List<Client>();
            
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    command.CommandText = @"
                        SELECT 
                            COALESCE(FullName, '') as FullName, 
                            COALESCE(Phone, '') as Phone, 
                            COALESCE(Email, '') as Email, 
                            (SELECT COUNT(*) FROM Cars WHERE ClientId = Clients.Id) as car_count 
                        FROM Clients";
                }
                else
                {
                    command.CommandText = @"
                        SELECT 
                            COALESCE(FullName, '') as FullName, 
                            COALESCE(Phone, '') as Phone, 
                            COALESCE(Email, '') as Email, 
                            (SELECT COUNT(*) FROM Cars WHERE ClientId = Clients.Id) as car_count 
                        FROM Clients
                        WHERE FullName LIKE @search OR Phone LIKE @search OR Email LIKE @search";
                    command.Parameters.AddWithValue("@search", $"%{searchText}%");
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            FullName = reader.IsDBNull(0) ? "Не указано" : reader.GetString(0),
                            Phone = reader.IsDBNull(1) ? "Не указан" : reader.GetString(1),
                            Email = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2),
                            CarCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        });
                    }
                }
            }
            
            return clients;
        }

        public List<Order> GetOrders()
        {
            var orders = new List<Order>();
            
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        so.Id, 
                        COALESCE(c.FullName, '') as client_name,
                        COALESCE(car.Brand || ' ' || car.Model || ' (' || car.Year || ')', '') as car, 
                        COALESCE(so.Status, '') as status, 
                        COALESCE(so.TotalCost, 0) as total_amount, 
                        COALESCE(so.CreatedAt, '') as created_at
                    FROM ServiceOrders so
                    LEFT JOIN Clients c ON so.ClientId = c.Id
                    LEFT JOIN Cars car ON so.CarVin = car.Vin";
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            ClientName = reader.IsDBNull(1) ? "Не указан" : reader.GetString(1),
                            Car = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2),
                            Status = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3),
                            Total = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                            Date = reader.IsDBNull(5) ? "Не указана" : reader.GetDateTime(5).ToString("dd.MM.yyyy")
                        });
                    }
                }
            }
            
            return orders;
        }

        public List<InventoryItem> GetInventory()
        {
            var inventory = new List<InventoryItem>();
            
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        COALESCE(Articul, '') as Articul, 
                        COALESCE(Name, '') as Name, 
                        COALESCE(Quantity, 0) as Quantity, 
                        COALESCE(Price, 0) as Price 
                    FROM SpareParts";
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventory.Add(new InventoryItem
                        {
                            Article = reader.IsDBNull(0) ? "Не указан" : reader.GetString(0),
                            Name = reader.IsDBNull(1) ? "Не указано" : reader.GetString(1),
                            Quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            Price = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3)
                        });
                    }
                }
            }
            
            return inventory;
        }
    }
}
