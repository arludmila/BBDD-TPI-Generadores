using Bogus;
using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public static class AddressGen
    {
        // cantidad x ciudad
        public static void Generate(int num)
        {
            var random = new Random();
            var faker = new Faker();
            string[] addressTypes = ["house", "apartment", "room"];
            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();

                var query = "SELECT Id FROM cities";

                var cityIds = connection.Query<int>(query).ToArray();
                
                foreach (var cityId in cityIds)
                {
                    for (int i = 0; i < num; i++)
                    {
                        string street = "";
                        var cityIdSelected = random.Next(1,cityIds.Length+1);
                        var selectAddressType = random.Next(addressTypes.Length);
                        int number = 0;
                        string insertQuery = "";
                        int rowsAffected  = 0;
                        int floor = 0;
                        int apartmentNumber = 0;
                        switch (selectAddressType)
                        {

                            case 0:

                                street = faker.Address.StreetName();
                                number = random.Next(1, 9000);
                                insertQuery = $"INSERT INTO addresses (city_id, street, number) VALUES (@city_id, @street, @number)";


                                rowsAffected = connection.Execute(insertQuery, new { city_id = cityIdSelected, street = street, number = number });

                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Insertion failed or no rows were inserted.");
                                }
                                break;

                            case 1:
                                 street = faker.Address.StreetName();
                                 number = random.Next(1, 9000);
                                 floor = random.Next(1,10);
                                 apartmentNumber = random.Next(1,10);
                                 insertQuery = $"INSERT INTO addresses (city_id, street, number, floor, apartment_number) VALUES (@city_id, @street, @number, @floor, @apartment_number)";


                                 rowsAffected = connection.Execute(insertQuery, new { city_id = cityIdSelected, street = street, number = number, floor = floor, apartment_number = apartmentNumber });

                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Insertion failed or no rows were inserted.");
                                }
                                break;
                            case 2:
                                street = faker.Address.StreetName();
                                number = random.Next(1, 9000);
                                var roomNumber = random.Next(1, 100);
                                insertQuery = $"INSERT INTO addresses (city_id, street, number, room_number) VALUES (@city_id, @street, @number, @room_number)";


                                rowsAffected = connection.Execute(insertQuery, new { city_id = cityIdSelected, street = street, number = number, room_number = roomNumber });

                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Insertion failed or no rows were inserted.");
                                }
                                break;

                            default:
                                break;
                        }



                    }
                }
            }
        }
    }

}
