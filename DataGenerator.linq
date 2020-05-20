<Query Kind="Statements" />

var colors = new string[]{"Red", "Green", "Blue", "Pink", "Black", "White"};
var carMakes = new string[]{"Acura", "Alfa Romeo", "Aston Martin", "Audi", "Bentley", "BMW", "Bugatti", "Buick", "Cadillac", "Chevrolet", "Chrysler", "Citroen", "Dodge", "Ferrari", "Fiat", "Ford", "Geely", "General Motors", "GMC", "Honda", "Hyundai", "Infiniti", "Jaguar", "Jeep", "Kia", "Koenigsegg", "Lamborghini", "Land Rover", "Lexus", "Maserati", "Mazda", "McLaren", "Mercedes-Benz", "Mini", "Mitsubishi", "Nissan", "Pagani", "Peugeot", "Porsche", "Renault Logo", "Rolls Royce", "Saab", "Subaru", "Suzuki", "Tata Motors", "Tesla", "Toyota", "Volkswagen", "Volvo"};
Console.WriteLine("LicensePlate;Color;ColorEnum;ActivationDate;CarMake");
for (int i = 0; i < 100; i++) {
var r = new Random(i);
var colorInt = r.Next(colors.Length);
//Console.WriteLine($"                new Car{{\n                    LicensePlate = \"{(char)(r.Next(27)+65)}{(char)(r.Next(27)+65)}{(char)(r.Next(27)+65)} {r.Next(10)}{r.Next(10)}{r.Next(10)}\",\n                    Color = \"{colors[colorInt]}\",\n                    ColorEnum = Color.{colors[colorInt]},\n                    ActivationDate = DateTime.Parse(\"{DateTime.Now.AddDays(-r.Next(3650)).Date}\"),\n                }},");
Console.WriteLine($"{(char)(r.Next(26)+65)}{(char)(r.Next(26)+65)}{(char)(r.Next(26)+65)} {r.Next(10)}{r.Next(10)}{r.Next(10)};{colors[colorInt]};{colorInt};{DateTime.Now.AddDays(-r.Next(3650)).Date};{carMakes[r.Next(carMakes.Length)]}");
}