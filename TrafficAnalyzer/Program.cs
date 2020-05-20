using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TrafficAnalyzer.Domain;
using TrafficAnalyzer.Enumerators;

namespace TrafficAnalyzer
{
    class Program
    {
        static string GetSelection(int index) => Selection[index-1];
        static string[] Selection = new string[]
        {
            "Hämta senaste aktiveringsdatum för varje bil",
            "Räkna bilar efter färg",
            "Räkna bilar efter färg (enum vs sträng)",
            "Räkna bilar efter bilmärke"
        };
        static void Main(string[] args)
        {
            Console.WriteLine("\n\n  ######################################################");
            Console.Write("  ### ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Välkommen till Andreas mikroptimeringsverktyg!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ###");
            Console.WriteLine("  ######################################################\n\n");

            var cont = true;
            while (cont)
            {
                for (int i = 0; i < Selection.Length; i++)
                {
                    Console.WriteLine($"({i + 1}) {Selection[i]}");
                };
                Console.Write("(q): Avsluta\n\nDitt val: ");
                var choise = Console.ReadLine();
                if (choise.Equals("q", StringComparison.OrdinalIgnoreCase))
                    break;
                if (!int.TryParse(choise, out int selection))
                {
                    Console.WriteLine("Det måste vara ett heltal");
                    continue;
                }
                Console.Write("Liten (1), medel (2), stor (3) eller extra stor (4) bilpool?: ");
                var carPoolSizeInput = Console.ReadLine();
                if (!int.TryParse(carPoolSizeInput, out int carPoolSize))
                {
                    Console.WriteLine("Det måste vara ett heltal!");
                    continue;
                }
                List<Car> cars;
                switch (carPoolSize)
                {
                    case 1:
                        cars = GetSmallPool();
                        break;
                    case 2:
                        cars = GetMediumPool();
                        break;
                    case 3:
                        cars = GetLargePool();
                        break;
                    case 4:
                        cars = GetExtraLargePool();
                        break;
                    default:
                        Console.WriteLine("Det finns inte!");
                        continue;
                }
                Console.WriteLine("Optimera resultat? (y/[n]): ");
                var optimizeSearch = Console.ReadLine().Trim().Equals("y", StringComparison.OrdinalIgnoreCase);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < 100; i++)
                {
                    switch (selection)
                    {
                        case 1:
                            var lastActivationResponse = GetLastActivationDate(cars, optimizeSearch);
                            break;
                        case 2:
                            var countResponse = CountCarsByColor(cars, optimizeSearch);
                            break;
                        case 3:
                            var countResponse2 = CountCarsByColor2(cars, optimizeSearch);
                            break;
                        case 4:
                            var countByMakeResponse = CountCarsByMake(cars, optimizeSearch);
                            break;
                        default:
                            Console.WriteLine("Det finns inte!");
                            continue;
                    }
                }
                //var smallPool = CsvHelper.CsvDataReader
                stopwatch.Stop();
                Console.WriteLine("\n\n##########################");
                Console.Write("######   ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Resultat");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("   ######");
                Console.WriteLine("##########################\n\n");

                Console.WriteLine($"Testkörning: {GetSelection(selection)}");
                Console.WriteLine($"Bilpoolsstorlek: {(carPoolSize == 1 ? "liten" : carPoolSize == 2 ? "Medel" : "Stor")}");
                Console.WriteLine($"Optimering: {(optimizeSearch ? "ja" : "nej")}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nTidsåtgång: {stopwatch.ElapsedMilliseconds} ms\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("...");
                var dummyInput = Console.ReadLine();
            }
        }

        private static Dictionary<string, int> CountCarsByMake(List<Car> cars, bool optimizeSearch)
        {
            var dictionary = new Dictionary<string, int>();
            if (optimizeSearch)
            {
                dictionary = cars.Select(c => c.CarMake).Distinct().ToDictionary(k => k, v => 0);
                foreach (var car in cars)
                {
                    dictionary[car.CarMake]++;
                }
            }
            else
            {
                dictionary = cars.GroupBy(c => c.CarMake).ToDictionary(k => k.Key, v => v.Count());
                //var carMakes = cars.Select(c => c.CarMake).Distinct();
                //foreach (var carMake in carMakes)
                //{
                //    var count = cars.Count(c => c.CarMake.Equals(carMake));
                //    dictionary.Add(carMake, count);
                //}
            }
            return dictionary;
        }

        private static Dictionary<string, int> CountCarsByColor(List<Car> cars, bool optimizeSearch)
        {
            var dictionary = new Dictionary<string, int>();
            if (optimizeSearch)
            {
                dictionary = cars.Select(c => c.Color).Distinct().ToDictionary(k => k, v => 0);
                foreach (var car in cars)
                {
                    dictionary[car.Color]++;
                }
            }
            else
            {
                dictionary = cars.GroupBy(c => c.Color).ToDictionary(k => k.Key, v => v.Count());



                //dictionary = cars.Select(c => c.Color).Distinct().ToDictionary(k => k, v => cars.Count(x => x.Color == v));

                //var colors = cars.Select(c => c.Color).Distinct();
                //foreach (var color in colors)
                //{
                //    var count = cars.Count(c => c.Color.Equals(color));
                //    dictionary.Add(color, count);
                //}
            }
            return dictionary;
        }

        private static Dictionary<string, int> CountCarsByColor2(List<Car> cars, bool optimizeSearch)
        {
            var dictionary = new Dictionary<string, int>();
            if (optimizeSearch)
            {
                var colors = Enum.GetValues(typeof(Color)).Cast<Color>();
                foreach (var color in colors)
                {
                    var count = cars.Count(c => c.ColorEnum == color);
                    dictionary.Add(color.ToString(), count);
                }
            }
            else
            {
                var colors = cars.Select(c => c.Color).Distinct();
                foreach (var color in colors)
                {
                    var count = cars.Count(c => c.Color == color);
                    dictionary.Add(color, count);
                }
            }
            return dictionary;
        }

        static Dictionary<string, DateTime> GetLastActivationDate(List<Car> cars, bool optimized = false)
        {
            var output = new Dictionary<string, DateTime>();
            if (optimized)
            {
                var sortedList = cars.OrderBy(x => x.ActivationDate).OrderBy(c => c.LicensePlate).ToArray();
                for (int i = 0; i < sortedList.Length; i++)
                {
                    if (i != sortedList.Length - 1 && sortedList[i + 1].LicensePlate.Equals(sortedList[i].LicensePlate))
                        continue;
                    output.Add(sortedList[i].LicensePlate, sortedList[i].ActivationDate);
                }
            }
            else
            {
                output = cars.OrderByDescending(c => c.ActivationDate).Distinct()
                    .ToDictionary(b => b.LicensePlate, v => v.ActivationDate);
            }
            return output;
        }

        static List<Car> GetSmallPool()
        {
            return GetCars("SmallRegionCarPool.csv");
        }

        static List<Car> GetLargePool()
        {
            return GetCars("LargeRegionCarPool.csv");
        }

        static List<Car> GetMediumPool()
        {
            return GetCars("MediumRegionCarPool.csv");
        }

        static List<Car> GetExtraLargePool()
        {
            return GetCars("ExtraLargeRegionCarPool.csv");
        }

        static List<Car> GetCars(string fileName)
        {
            List<Car> cars;
            using (var reader = new StreamReader($"{ System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{fileName}"))
            using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            }))
            {
                cars = csvReader.GetRecords<Car>().ToList();
            }
            foreach (var car in cars)
            {
                car.ColorEnum = (Color)Enum.Parse(typeof(Color), car.Color);
            }
            return cars;
        }
    }
}
