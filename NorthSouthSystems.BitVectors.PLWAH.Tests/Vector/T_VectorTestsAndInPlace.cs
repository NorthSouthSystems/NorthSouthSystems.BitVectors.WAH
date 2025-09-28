#if POSITIONLISTENABLED && WORDSIZE64
namespace NorthSouthSystems.BitVectors.PLWAH64;
#elif POSITIONLISTENABLED
namespace NorthSouthSystems.BitVectors.PLWAH;
#elif WORDSIZE64
namespace NorthSouthSystems.BitVectors.WAH64;
#else
namespace NorthSouthSystems.BitVectors.WAH;
#endif

public class VectorTestsAndInPlace
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AndInPlaceRandom(bool isCompressed)
    {
        const int randomSeed = 22;

        VectorTestsRandom.LogicInPlaceBase(randomSeed, (Word.SIZE - 1) * WordExtensions.WORDCOUNTFORRANDOMTESTS + 1,
            isCompressed,
            (left, right) => left.AndInPlace(right),
            Enumerable.Intersect);
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () =>
        {
            var vector = new Vector(false);
            vector.AndInPlace(null);
        };
        act.Should().ThrowExactly<ArgumentNullException>(because: "AndInPlaceArgumentNull");

        act = () =>
        {
            var vector = new Vector(true);
            var input = new Vector(false);
            vector.AndInPlace(input);
        };
        act.Should().ThrowExactly<NotSupportedException>(because: "AndInPlaceNotSupported");
    }
}