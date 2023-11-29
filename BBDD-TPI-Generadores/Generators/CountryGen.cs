using Bogus;
using Dapper;
using MySqlConnector;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BBDD_TPI_Generadores.Generators
{
    public static class CountryGen
    {
        public static void Generate()
        {

            // Array of countries
            var countries = new string[] { "Argentina", "United States", "Brasil", "Uruguay", "Chile" };

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();

                foreach (var item in countries)
                {

                    var insertQuery = "INSERT INTO countries (name) VALUES (@name)";

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
