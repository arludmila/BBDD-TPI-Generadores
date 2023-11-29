using Bogus;
using Bogus.DataSets;
using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public static class PropCleaningServiceGen
    {
        public static void Generate(int num)
        {
            var faker = new Faker();
            var random = new Random();

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                var propertiesIds = connection.Query<int>("SELECT id FROM properties").ToArray();
                var staffIds = connection.Query<int>("SELECT id FROM cleaning_staff").ToArray();

                for (int i = 0; i < num; i++)
                {
                    // TODO: validacion de shift? le corresponde?
                    DateTime today = DateTime.Now;
                    DateTime twoYearsAgo = today.AddYears(-2);
                    TimeSpan range = today - twoYearsAgo;
                    TimeSpan randomRange = new TimeSpan((long)(random.NextDouble() * range.Ticks));
                    //
                    DateTime cleaningDate = twoYearsAgo + randomRange;
                    TimeOnly startingTime = TimeOnly.FromDateTime(cleaningDate);
                    TimeOnly finishingTime = startingTime.AddHours(1);
                    DateTime startTimeAsDateTime = DateTime.Today.Add(startingTime.ToTimeSpan());
                    DateTime endTimeAsDateTime = DateTime.Today.Add(finishingTime.ToTimeSpan());
                    var propertyId = propertiesIds[random.Next(propertiesIds.Length)];
                    var staffId = staffIds[random.Next(staffIds.Length)];


                    var insertQuery = "INSERT INTO properties_cleaning_services (cleaning_date, starting_time, finishing_time, staff_id, property_id) " +
                                           "VALUES (@cleaningDate,@startTimeAsDateTime,@endTimeAsDateTime,@staffId,@propertyId)";

                    int rowsAffected = connection.Execute(insertQuery, new
                    {
                        cleaningDate,
                        startTimeAsDateTime,
                        endTimeAsDateTime,
                        staffId,
                        propertyId,
                      
                    });

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
