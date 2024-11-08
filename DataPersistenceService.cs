using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Serialization;
using System;

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
                    IsPrime = IsPrime(value) ? 1 : 0
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

//        private bool IsPrime(int number) => // Implement prime checking logic
}

}
