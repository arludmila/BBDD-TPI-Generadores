using Bogus;
using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public static class ReservationGen
    {
        public static void Generate(int num)
        {
            var faker = new Faker();
            var random = new Random();
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();
                var propertiesIds = connection.Query<int>("SELECT id FROM properties").ToArray();
                var guestsIds = connection.Query<int>("SELECT id FROM guests").ToArray();


                for (int i = 0; i < num; i++)
                {
                    DateTime today = DateTime.Now;
                    DateTime twoYearsAgo = today.AddYears(-7);
                    TimeSpan range = today - twoYearsAgo;
                    TimeSpan randomRange = new TimeSpan((long)(random.NextDouble() * range.Ticks));
                    //
                    DateTime checkIn = twoYearsAgo + randomRange;


                    DateTime checkOut = checkIn.AddDays(random.Next(1, 14));
                    var propertyId = propertiesIds[random.Next(propertiesIds.Length)];
                    var guestId = guestsIds[random.Next(guestsIds.Length)];


                    var propertyCapacityQuery = "SELECT guest_capacity FROM properties WHERE id = @propertyId";

                    // Execute the query and retrieve the guest_capacity
                    var propertyCapacity = connection.Query<int>(propertyCapacityQuery, new { propertyId }).FirstOrDefault();

                    var propertyDailyPriceQuery = "SELECT daily_price FROM properties WHERE id = @propertyId";

                    // Execute the query and retrieve the guest_capacity
                    var propertyDailyPrice = connection.Query<int>(propertyDailyPriceQuery, new { propertyId }).FirstOrDefault();
                    TimeSpan difference = checkOut.Subtract(checkIn);
                    int daysDifference = difference.Days;

                    double totalAmount = propertyDailyPrice * daysDifference;
                    int guests = random.Next(1, propertyCapacity);
                    var insertQuery = "INSERT INTO reservations (check_in, check_out, guests, total_amount, property_id, guest_id) " +
                                      "VALUES (@checkIn, @checkOut, @guests, @totalAmount, @propertyId, @guestId)";

                    int rowsAffected = connection.Execute(insertQuery, new
                    {
                        checkIn,
                        checkOut,
                        guests,
                        totalAmount = propertyDailyPrice * daysDifference, // Use the calculated difference directly
                        propertyId,
                        guestId,
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
