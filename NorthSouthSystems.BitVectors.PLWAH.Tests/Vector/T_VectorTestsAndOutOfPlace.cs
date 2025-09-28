#if POSITIONLISTENABLED && WORDSIZE64
namespace NorthSouthSystems.BitVectors.PLWAH64;
#elif POSITIONLISTENABLED
namespace NorthSouthSystems.BitVectors.PLWAH;
#elif WORDSIZE64
namespace NorthSouthSystems.BitVectors.WAH64;
#else
namespace NorthSouthSystems.BitVectors.WAH;
#endif

public class VectorTestsAndOutOfPlace
{
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, true, true)]
    [InlineData(true, false, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    public void AndOutOfPlaceRandom(bool leftIsCompressed, bool rightIsCompressed, bool resultIsCompressed)
    {
        const int randomSeed = 22;

        VectorTestsRandom.LogicOutOfPlaceBase(randomSeed, (Word.SIZE - 1) * WordExtensions.WORDCOUNTFORRANDOMTESTS + 1,
            leftIsCompressed, rightIsCompressed,
            (left, right) => left.AndOutOfPlace(right, resultIsCompressed),
            Enumerable.Intersect);
    }
}