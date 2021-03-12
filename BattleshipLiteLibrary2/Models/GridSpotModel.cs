using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary2.Models
{
    public class GridSpotModel
    {
        // so GridSpot like A5 or D3
        public string SpotLetter { get; set; } //D
        public int SpotNumber { get; set; } //3

        // i use "GridSpotStatus" like type instead of using
        public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty; // every spot will be empty

    }
}
