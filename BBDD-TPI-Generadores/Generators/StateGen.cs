using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDD_TPI_Generadores.Generators
{
    public static class StateGen
    {
        public static void Generate()
        {
            // Arrays of states for each country
            var argentinaStates = new string[] { "Buenos Aires", "Cordoba", "Santa Fe" };
            var usStates = new string[] { "California", "Texas", "New York" };
            var brasilStates = new string[] { "Sao Paulo", "Rio de Janeiro", "Minas Gerais" };
            var uruguayStates = new string[] { "Montevideo", "Canelones", "Maldonado" };
            var chileStates = new string[] { "Santiago", "Valparaiso", "Biobio" };

            InsertStates("Argentina", argentinaStates);
            InsertStates("United States", usStates);
            InsertStates("Brasil", brasilStates);
            InsertStates("Uruguay", uruguayStates);
            InsertStates("Chile", chileStates);
        }
        public static void InsertStates(string countryName, string[] states)
        {
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();

                foreach (var item in states)
                {

                    var insertQuery = $"INSERT INTO states (name, country_id) VALUES (@name, (SELECT id FROM countries WHERE name = '{countryName}'))";

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
