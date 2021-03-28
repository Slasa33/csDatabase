using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace csDatabase
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Customer> customers = new List<Customer>()
            {
                new Customer()
                {
                    Name = "Jakub",
                    Address = "Karvina"
                },
                new Customer()
                {
                    Name = "Petr",
                    Address = null
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    CustomerId = 1,
                    Price = 100,
                    Product = "Dildo"
                },
                new Order()
                {
                    CustomerId = 2,
                    Price = 111,
                    Product = "Kondomy"
                }
            };


            string sql = File.ReadAllText("create.sql");

            string connectionstring = "Data Source=mydb.db";


            //foreach (var customer in customers)
            
                using (SqliteConnection connection = new SqliteConnection(connectionstring))
                {
                    connection.Open();

                //string sqlinsert = "INSERT INTO Customer (Name, Address) VALUES (@Name, @Address)";

                //using (SqliteCommand cmd = new SqliteCommand(sqlinsert, connection))
                //{
                //    cmd.Parameters.Add(new SqliteParameter()
                //    {
                //        DbType = DbType.String, 
                //        ParameterName = "@Name", 
                //        Value = customer.Name
                //    });

                //    cmd.Parameters.Add(new SqliteParameter()
                //    {
                //        DbType = DbType.String, 
                //        ParameterName = "@Address", 
                //        Value = customer.Address == null ? (object)DBNull.Value : customer.Address
                //    });

                //    cmd.ExecuteNonQuery();

                //}

                //string sqlselect = "SELECT * FROM Customer";


                //using (SqliteCommand cmd = new SqliteCommand(sqlselect, connection))
                //{
                //    using (SqliteDataReader reader = cmd.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            Customer c = new Customer()
                //            {
                //                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                //                Name = reader.GetString(reader.GetOrdinal("Name")),
                //                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                //            };

                //            Console.WriteLine(c.Id + " " + c.Name + " " + c.Address);
                //        }
                //    }
                //}

                //foreach (var order in orders)
                //{

                //    string insert = "INSERT INTO \"Order\" (CustomerId, Product, Price) VALUES (@CustomerId, @Product, @Price)";

                //    using (SqliteCommand cmd = new SqliteCommand(insert, connection))
                //    {
                //        cmd.Parameters.Add(new SqliteParameter()
                //        {
                //            DbType = DbType.Int32,
                //            ParameterName = "@CustomerId",
                //            Value = order.CustomerId
                //        });
                //        cmd.Parameters.Add(new SqliteParameter()
                //        {
                //            DbType = DbType.Int32,
                //            ParameterName = "@Product",
                //            Value = order.Product
                //        });
                //        cmd.Parameters.Add(new SqliteParameter()
                //        {
                //            DbType = DbType.Int32,
                //            ParameterName = "@Price",
                //            Value = order.Price
                //        });

                //        cmd.ExecuteNonQuery();
                //    }
                //}


                //    string selectSQL =
                //        "SELECT * FROM Customer LEFT JOIN \"Order\" ON Customer.Id = \"Order\".CustomerId";

                //using (SqliteCommand cmd = new SqliteCommand(selectSQL, connection))
                //{
                //    using (SqliteDataReader reader = cmd.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            Customer c = new Customer()
                //            {
                //                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                //                Name = reader.GetString(reader.GetOrdinal("Name")),
                //                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                //            };

                //            Order r = reader.IsDBNull("CustomerId") ? null : new Order()
                //            {
                //                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                //                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                //                Product = reader.GetString(reader.GetOrdinal("Product")),
                //                Price = reader.GetInt32(reader.GetOrdinal("Price"))
                //            };

                //            Console.WriteLine(c.Id + " " + c.Name + " " + c.Address + " " + r?.Product + " " + r?.Price);
                //        }
                //    }
                //}

                //string select = "SELECT * FROM Customer WHERE Id = @Id";

                //using (SqliteCommand cmd = new SqliteCommand(select, connection))
                //{
                //    cmd.Parameters.AddWithValue("@Id", 1);

                //    Customer c = null;
                //    using (SqliteDataReader reader = cmd.ExecuteReader())
                //    {

                //        while (reader.Read())
                //        {
                //            c = new Customer()
                //            {
                //                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                //                Name = reader.GetString(reader.GetOrdinal("Name")),
                //                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                //            };

                //        }
                //    }

                //    c.Name = c.Name.ToLower();

                //    string updateSql = "UPDATE Customer SET Name=@Name WHERE Id=@Id";

                //    using (SqliteCommand cmd2 = new SqliteCommand(updateSql, connection))
                //    {
                //        cmd2.Parameters.AddWithValue("@Id", c.Id);
                //        cmd2.Parameters.AddWithValue("@Name", c.Name);
                //        cmd2.ExecuteNonQuery();
                //    }


                //    Console.WriteLine(c.Id + " " + c.Name + " " + c.Address);
                //}

                string select = "SELECT * FROM Customer WHERE Id = @Id";

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {

                        using (SqliteCommand cmd = new SqliteCommand(select, connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Id", 1);

                            Customer c = null;
                            using (SqliteDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    c = new Customer()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Address = reader.IsDBNull(reader.GetOrdinal("Address"))
                                            ? null
                                            : reader.GetString(reader.GetOrdinal("Address"))
                                    };

                                }
                            }

                            c.Name = c.Name.ToLower();

                            string updateSql = "UPDATE Customer SET Name=@Name WHERE Id=@Id";

                            using (SqliteCommand cmd2 = new SqliteCommand(updateSql, connection))
                            {
                                cmd2.Transaction = transaction;
                                cmd2.Parameters.AddWithValue("@Id", c.Id);
                                cmd2.Parameters.AddWithValue("@Name", c.Name);
                                cmd2.ExecuteNonQuery();
                            }


                            Console.WriteLine(c.Id + " " + c.Name + " " + c.Address);
                        }

                        transaction.Commit();
                    }

                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }



                }
            //Console.WriteLine("Hello World!");
        }
    }
}
