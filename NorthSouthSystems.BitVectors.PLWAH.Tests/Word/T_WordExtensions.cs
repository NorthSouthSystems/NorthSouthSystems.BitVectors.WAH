#if WORDSIZE64
global using WordRawType = ulong;
#else
global using WordRawType = uint;
#endif

#if POSITIONLISTENABLED && WORDSIZE64
namespace NorthSouthSystems.BitVectors.PLWAH64;
#elif POSITIONLISTENABLED
namespace NorthSouthSystems.BitVectors.PLWAH;
#elif WORDSIZE64
namespace NorthSouthSystems.BitVectors.WAH64;
#else
namespace NorthSouthSystems.BitVectors.WAH;
#endif

internal static class WordExtensions
{
    internal const int LARGEPRIME = 9973;

    internal const WordRawType LARGEPRIME32ORFULLCOVERAGE64
#if WORDSIZE64
        = (WordRawType)LARGEPRIME * uint.MaxValue;
#else
        = LARGEPRIME;
#endif

    internal const int WORDCOUNTFORRANDOMTESTS
#if WORDSIZE64
        = 5;
#else
        = 10;
#endif
}