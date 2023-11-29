using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BBDD_TPI_Generadores.Generators
{
    public static class SpecialtyGen
    {
        public static void Generate()
        {
            string[] maintenanceStaffSpecialties = ["Electrician", "Plumber", "HVAC Technician", "Carpenter", "Painter", "Locksmith", "Landscaper", "Janitor", "Roofing Specialist", "Flooring Specialist"];
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();
                foreach (var item in maintenanceStaffSpecialties)
                {
                    var insertQuery = $"INSERT INTO specialties (name) VALUES (@name)";

                    int rowsAffected = connection.Execute(insertQuery, new { name = item });

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Insertion failed or no rows were inserted.");
                    }
                }
            }
        }
    }
}
