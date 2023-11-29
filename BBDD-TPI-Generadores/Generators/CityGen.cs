using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDD_TPI_Generadores.Generators
{
    public static class CityGen
    {
        public static void Generate()
        {
            // Arrays of cities for each state
            var buenosAiresCities = new string[] { "Buenos Aires City", "La Plata" };
            var cordobaCities = new string[] { "Cordoba City", "Villa Maria" };
            var santaFeCities = new string[] { "Santa Fe City", "Rosario" };

            var californiaCities = new string[] { "Los Angeles", "San Francisco" };
            var texasCities = new string[] { "Houston", "Dallas" };
            var newYorkCities = new string[] { "New York City", "Buffalo" };

            var saoPauloCities = new string[] { "Sao Paulo City", "Campinas" };
            var rioDeJaneiroCities = new string[] { "Rio de Janeiro City", "Niteroi" };
            var minasGeraisCities = new string[] { "Belo Horizonte", "Uberlandia" };

            var montevideoCities = new string[] { "Montevideo City", "Ciudad de la Costa" };
            var canelonesCities = new string[] { "Canelones City", "Las Piedras" };
            var maldonadoCities = new string[] { "Maldonado City", "Punta del Este" };

            var santiagoCities = new string[] { "Santiago City", "Puente Alto" };
            var valparaisoCities = new string[] { "Valparaiso City", "Vina del Mar" };
            var biobioCities = new string[] { "Concepcion", "Talcahuano" };

            InsertCities("Buenos Aires",buenosAiresCities);
            InsertCities("Cordoba", cordobaCities);
            InsertCities("Santa Fe", santaFeCities);

            InsertCities("California", californiaCities);
            InsertCities("Texas", texasCities);
            InsertCities("New York", newYorkCities);

            InsertCities("Sao Paulo", saoPauloCities);
            InsertCities("Rio de Janeiro", rioDeJaneiroCities);
            InsertCities("Minas Gerais", minasGeraisCities);

            InsertCities("Montevideo", montevideoCities);
            InsertCities("Canelones", canelonesCities);
            InsertCities("Maldonado", maldonadoCities);

            InsertCities("Santiago", santiagoCities);
            InsertCities("Valparaiso", valparaisoCities);
            InsertCities("Biobio", biobioCities);
        }
        public static void InsertCities(string stateName, string[] cities)
        {
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();

                foreach (var item in cities)
                {

                    var insertQuery = $"INSERT INTO cities (name, state_id) VALUES (@name, (SELECT id FROM states WHERE name = '{stateName}'))";

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
