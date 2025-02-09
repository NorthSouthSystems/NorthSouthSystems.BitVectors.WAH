#if POSITIONLISTENABLED && WORDSIZE64
namespace NorthSouthSystems.BitVectors.PLWAH64;
#elif POSITIONLISTENABLED
namespace NorthSouthSystems.BitVectors.PLWAH;
#elif WORDSIZE64
namespace NorthSouthSystems.BitVectors.WAH64;
#else
namespace NorthSouthSystems.BitVectors.WAH;
#endif

internal static class VectorTestsRandom
{
    internal static void LogicInPlaceBase(int randomSeed, int maxBitPosition,
        bool isCompressed,
        Action<Vector, Vector> logic, Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>> expectedBitPositionCalculator)
    {
        var templates = GenerateRandomVectorsFavorCompression(randomSeed, maxBitPosition);

        foreach (var templateLeft in templates)
        {
            foreach (var templateRight in templates)
            {
                var resultLeft = new Vector(false, templateLeft.Item1);
                var resultRight = new Vector(isCompressed, templateRight.Item1);

                logic(resultLeft, resultRight);

                resultLeft.AssertBitPositions(expectedBitPositionCalculator(templateLeft.Item2, templateRight.Item2));
            }
        }
    }

    internal static void LogicOutOfPlaceBase(int randomSeed, int maxBitPosition,
        bool leftIsCompressed, bool rightIsCompressed,
        Func<Vector, Vector, Vector> logic, Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>> expectedBitPositionCalculator)
    {
        var templates = GenerateRandomVectorsFavorCompression(randomSeed, maxBitPosition);

        foreach (var templateLeft in templates)
        {
            foreach (var templateRight in templates)
            {
                var resultLeft = new Vector(leftIsCompressed, templateLeft.Item1);
                var resultRight = new Vector(rightIsCompressed, templateRight.Item1);

                var result = logic(resultLeft, resultRight);

                result.AssertBitPositions(expectedBitPositionCalculator(templateLeft.Item2, templateRight.Item2));
            }
        }
    }

    private static Tuple<Vector, int[]>[] GenerateRandomVectorsFavorCompression(int randomSeed, int maxBitPosition)
    {
        var random = new Random(randomSeed);

        // To set all bits to 1, count must == maxBitPosition + 1 because of position being 0-based.
        // In order to include count == 0 in the enumerable, we must have maxBitPosition + 2 counts.
        return Enumerable.Range(0, maxBitPosition + 2)
            .Where(count =>
            {
                double fillFactor = (double)count / maxBitPosition;
                double favorability = Math.Abs(fillFactor - .5) / .5;

                return random.NextDouble() <= favorability;
            })
            .Select(count =>
            {
                var vector = new Vector(false);
                int[] bitPositions = vector.SetBitsRandom(maxBitPosition, count, true);

                return Tuple.Create(vector, bitPositions);
            })
            .ToArray();
    }
}