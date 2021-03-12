using BattleshipLiteLibrary2;
using BattleshipLiteLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattleshipLite
{
    class Program
    {
        //every method has one single reponsibility
        static void Main(string[] args)
        {
            WelcomeMessage();

            // How do you know after the first player , it will be second player
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {

                // Display grid from activePlayer on where they fired
                DisplayShotGrid(activePlayer);

                // Ask activePlayer for a shot
                // Determine if it is a valid shot ( And if it's not right, keep looping to give a valid shot
                // Determine shot results
                RecordPlayerShot(activePlayer, opponent);


                // Determine if the game continue or not,  when no more ship ( check  PlayerinfoModel for our opponent, nothing to do  with UI) . That should be a GameLogic call
                // if It's True, the game continue and if it's false, the game does not continue.
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // if Game is over , set activeplayer as the winnner 
                // else, swap positions (activePlayer to opponent)
                if (doesGameContinue == true) // we will swap positions and continue to go
                {
                 

                    // Use Tuple (Swap positions)
                    (activePlayer, opponent) = (opponent, activePlayer);

                }
                else
                {
                    winner = activePlayer;
                }
                
            } while (winner == null); // we will loop through until we fill in that winner spot

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratuation to { winner.UserName } for winning!" );
            Console.WriteLine($"{ winner.UserName } took { GameLogic.GetShotCount(winner) } shots.");
        }

        // we have to do this method in console UI, talk to UI, where you want to shoot
        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            // Ask for a shot , where do you want to shoot ( do we ask for "B2" )
            // Determine what row and column that is - split it apart
            // Determine if that is a valid shot
            // Go back to the bignning if not a valid shot and ask for a new shot

            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot(activePlayer);

                //split apart with tuple
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {

                    isValidShot = false;
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid Shot Location. Please try again.");
                }

            } while (isValidShot == false);


            // Determine shot results
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            // Record results
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
            DisplayShotResults(row, column, isAHit);

        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{ row }{ column } is a Hit.");
            }
            else
            {
                Console.WriteLine($"{ row }{ column } is a Miss.");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{ player.UserName }, Please enter your shot selection: ");
            string output = Console.ReadLine();

            return output;
        }

        // This going to display the console
        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            // it's going to be whatever the letter is 
            string currentRow = activePlayer.ShotGrid[0].SpotLetter; // This right here wil throw exception  when ShotGrid is empty

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    // This is for UI design and specific to just this console
                    Console.Write($" { gridSpot.SpotLetter }{ gridSpot.SpotNumber } ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            // It's User Interface Code
            Console.WriteLine("Welcome to BattleShip Lite");
            Console.WriteLine("created by Ryan Tu");
            Console.WriteLine();
        }

        // create player's not actually going to do the real work, it's going to call out the different methods
        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.WriteLine($"Player information for { playerTitle }"); // For Player 1 & 2

            // Ask the user for their name
            output.UserName = AskForUsersName();

            // Load up the shot grid (ex: A1 to A5, B1 to B5)
            GameLogic.InitializeGrid(output);

            // Ask the user for their 5 ship placements
            PlaceShips(output);

            // clear
            Console.Clear();

            return output;
        }
        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place your ship number {model.ShipLocations.Count + 1 }: ");
                // We're going to place ships and ask them where do you want to place a ship and then we're going to capture the response
                string location = Console.ReadLine();
                bool isValidLocation = false;

                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
