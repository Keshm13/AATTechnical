namespace AATTechnical
{
    public class DataComputationService
    {
        private readonly List<int> globalList = new();
        private readonly object lockObj = new();
        private const int maxEntries = 10000000;
        private bool evenThreadStarted = false;

        public void StartComputation()
        {
            Task.Run(() => GenerateOddNumbers());
            Task.Run(() => GenerateNegatedPrimes());
        }

        private void GenerateOddNumbers()
        {
            while (globalList.Count < maxEntries)
            {
                int number = GenerateRandomOdd(0, 1000);
                AddToGlobalList(number);
                CheckStartEvenThread();
            }
        }

        private void GenerateNegatedPrimes()
        {
            int primeCandidate = 2;
            while (globalList.Count < maxEntries)
            {
                if (IsPrime(primeCandidate))
                {
                    AddToGlobalList(-primeCandidate);
                }
                primeCandidate++;
            }
        }

        private void CheckStartEvenThread()
        {
            if (globalList.Count >= 2500000 && !evenThreadStarted)
            {
                Task.Run(() => GenerateEvenNumbers());
                evenThreadStarted = true;
            }
        }

        private void GenerateEvenNumbers()
        {
            while (globalList.Count < maxEntries)
            {
                int number = GenerateRandomEven(0, 1000);
                AddToGlobalList(number);
            }
        }

        private void AddToGlobalList(int number)
        {
            lock (lockObj)
            {
                if (globalList.Count < maxEntries)
                {
                    globalList.Add(number);
                }
            }
        }

        private bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            for (int i = 3; i <= Math.Sqrt(number); i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        public int GenerateRandomOdd(int min, int max)
        {
            Random rand = new Random();
            int number;
            do
            {
                number = rand.Next(min, max);
            } while (number % 2 == 0);
            return number;
        }

        public int GenerateRandomEven(int min, int max)
        {
            Random rand = new Random();
            int number;
            do
            {
                number = rand.Next(min, max);
            } while (number % 2 != 0);
            return number;
        }
    }

}
