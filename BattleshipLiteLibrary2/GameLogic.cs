using BattleshipLiteLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary2
{
    public static class GameLogic // put "static" because it has no need to store data
    {
        // This signature line , return and take
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5

            };
            // now start adding these grid items
            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model, letter, number); // This will handle these lines of code below
                }

            }

        }  
        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            // initialized this spot
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }

        // at least one ship that not sunk
        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false; 

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk) 
                {
                    isActive = true; // if none of them not sunk, and  then never set for this true
                }
            }
            return isActive;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty) // They've either taken a shot and miss or hit
                {
                    shotCount += 1;
                }
            }
            return shotCount;
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            bool output = false;
            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValiLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (isValiLocation && isSpotOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                });
                output = true;
            }
            return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = false;
                } 
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = false;

            foreach (var ship in model.ShotGrid)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = true;
                }
            }
            return isValidLocation;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = "";
            int column = 0;

            if (shot.Length != 2)
            {
                throw new ArgumentException("This was invalid shot type.", "shot");

            }


            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            //return tuple
            return (row, column);
        }
          
        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValidShot = false;

            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if (gridSpot.Status == GridSpotStatus.Empty) // go ahead and shoot there
                    {
                        isValidShot = true;
                    }
                }
            }
            return isValidShot;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isAHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }
            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
          

            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridSpot.Status = GridSpotStatus.Miss;
                    }
                }
            }
            
        }
    }
}