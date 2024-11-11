using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AATTechnical
{
    public class DataPersistenceService
    {
        private readonly AppDbContext _context;

        public DataPersistenceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveDataAsync(IEnumerable<int> data)
        {
            foreach (var value in data)
            {
                await _context.Numbers.AddAsync(new Number
                {
                    Value = value,
                    IsPrime = IsPrime(value) ? true : false
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> GetXmlDataAsync()
        {
            var numbers = await _context.Numbers.ToListAsync();
            var xmlSerializer = new XmlSerializer(typeof(List<Number>));
            using var memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, numbers);
            return memoryStream.ToArray();
        }

        public async Task<byte[]> GetBinaryDataAsync()
        {
            var numbers = await _context.Numbers.ToListAsync();
            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);

            foreach (var number in numbers)
            {
                binaryWriter.Write(number.Value);
                binaryWriter.Write(number.IsPrime);
            }

            return memoryStream.ToArray();
        }

        public bool IsPrime(int number)
        {
            if (number <= 1)
                return false; // Numbers less than 2 are not prime

            if (number == 2 || number == 3)
                return true; // 2 and 3 are prime numbers

            if (number % 2 == 0 || number % 3 == 0)
                return false; // Eliminate multiples of 2 and 3

            // Check divisibility starting from 5, only up to the square root of the number
            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true; 
        }

    }

}
