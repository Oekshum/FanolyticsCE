
using Quintessential;

namespace Fanolytics.Utils
{
    internal class UtilMethods
    {
        public static bool CheckForHexOverlap(HexIndex[] HexList1, Part part1, HexIndex[] HexList2, Part part2)
        {
            
            foreach (HexIndex hex1 in HexList1) {
                foreach (HexIndex hex2 in HexList2)
                {
                    HexIndex worldHex1 = part1.InFrontBy(hex1);
                    HexIndex worldHex2 = part2.InFrontBy(hex2);

                    if (worldHex1 == worldHex2)
                    {
                        return true;
                    }
                }
            }
            return false;
            
        }
    }
}
