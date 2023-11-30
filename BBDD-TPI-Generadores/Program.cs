// See https://aka.ms/new-console-template for more information
using BBDD_TPI_Generadores.Generators;

Console.WriteLine("Enter:");
Console.ReadLine();
//CountryGen.Generate();
//StateGen.Generate();
//CityGen.Generate();
//SpecialtyGen.Generate();
AddressGen.Generate(1000);
MaintenanceStaffGen.Generate(50);
CleaningStaffGen.Generate(50);
MaintenanceStaffSpecialtiesGen.Generate(75);
OwnerGen.Generate(50);
GuestGen.Generate(50);
PropertyGen.Generate(1000);
PropCleaningServiceGen.Generate(1300);
PropMaintenanceServiceGen.Generate(1300);
ReservationGen.Generate(2500);
PropCleaningServiceALLGen.Generate();
OpinionGen.Generate();
PaymentGen.Generate();