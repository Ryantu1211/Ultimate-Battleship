using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary2.Models
{
    // 0 = empty, 1 = ship, 2 = miss, 3 = hit, 4 = sunk (we place the ship in the shipLocation and the opponent hit that ship, and we're gonna mark that ship its sunk.
    public enum GridSpotStatus
    {
        // there are my enum values
        Empty,
        Ship,
        Miss,
        Hit,
        Sunk
    }
}
