using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary2.Models
{
    public class PlayerInfoModel
    {
        public string UserName { get; set; }

        // five entree here which are the five places that a ship will be
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();

        // A1-5, B1-5, C1-4,D1-5, and E1-5 ( They all live in the shot grid).
        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
    }
}
