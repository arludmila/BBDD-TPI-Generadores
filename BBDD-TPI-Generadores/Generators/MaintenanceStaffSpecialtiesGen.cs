using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public static class MaintenanceStaffSpecialtiesGen
    {
        public static void Generate(int num)
        {

            var random = new Random();
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();
                var specialtiesIds = connection.Query<int>("SELECT id FROM specialties").ToArray();
                var staffIds = connection.Query<int>("SELECT id FROM maintenance_staff").ToArray();
                for (int i = 0; i < num; i++)
                {
                    try
                    {
                        // Your existing code for inserting into 'maintenance_staff_specialties'

                        var specialtyId = specialtiesIds[random.Next(specialtiesIds.Length)];
                        var staffId = staffIds[random.Next(staffIds.Length)];
                        var insertQuery = $"INSERT INTO maintenance_staff_specialties (staff_id, specialty_id) VALUES (@staff_id, @specialty_id)";

                        int rowsAffected = connection.Execute(insertQuery, new { staff_id = staffId, specialty_id = specialtyId });

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Insertion failed or no rows were inserted.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        // Check for MySQL error code 1062, which indicates a duplicate entry violation
                        if (ex.Number == 1062)
                        {
                            Console.WriteLine("Already inserted these values.");
                        }
                        else
                        {
                            // Handle other MySQL exceptions or rethrow the exception
                            Console.WriteLine($"MySQL Exception: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions
                        Console.WriteLine($"Exception: {ex.Message}");
                    }

                }
            }
        }
    }
}
