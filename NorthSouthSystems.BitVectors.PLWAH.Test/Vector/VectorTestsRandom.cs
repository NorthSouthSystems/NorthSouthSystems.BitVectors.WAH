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
                // With Word.SIZE == 32 and maxBitPosition == 30 (the max on a single 32-bit WAH Word), the initial favorability
                // calculation results in count == 0 and count == maxBitPosition + 1 both having a favorability == 1 with
                // linear decreases from "both sides" when approaching count == 15 and 16 respectively. To further bias testing
                // with Vectors having Words with compression, we square the favorability. To avoid starvation, we set a floor
                // of 10% for all counts regardless of calculated favorability.
                double fillFactor = (double)count / (maxBitPosition + 1);

                double favorability = Math.Abs(fillFactor - .5) / .5;
                favorability = Math.Pow(favorability, 2);
                favorability = Math.Max(favorability, .1);

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