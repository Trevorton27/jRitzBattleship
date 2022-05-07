using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace John_Console_App_Battleship.classes
{
  class StartGame
    {
        public void RunGame()
        {
           
           
                BattleShipGrid battleShipGrid = new(10, 10);

                BattleShipInput battleShipInput = new();

                BattleShipDisplay battleShipDisplay = new(battleShipGrid.GetNumberColumns(),
                                                                            battleShipGrid.GetNumberRows());

                UserBattleShipGrid userBattleShipGrid = new(battleShipGrid.GetNumberColumns(),
                                                                                battleShipGrid.GetNumberRows());

                battleShipDisplay.SetGridLocation(battleShipDisplay.GetHeaderLeft() + 20, battleShipDisplay.GetHeaderTop() + 11);

                InitialScreenLayout(battleShipDisplay);

                battleShipInput.ReadLineFromActor();

                userBattleShipGrid.ActorName = battleShipInput.GetLineFromActor();

                BattleShipDisplay.ResetScreen();

                const int numberOfTurnsMax = 8;
                userBattleShipGrid.CurrentNumberOfTurns = numberOfTurnsMax;

                int previousRow = -1;
                int previousColumn = -1;

                while (userBattleShipGrid.RunGame)
                {
                    BattleShipDisplay.ResetScreen();

                    ShowHeader(battleShipDisplay, userBattleShipGrid);

                    int numberOfStrikes = 0;
                    for (int row = 0; row < battleShipGrid.GetNumberRows(); row++)
                    {

                        char rowCharacter = Convert.ToChar(battleShipDisplay.GetGridTop() + 52 + row);
                        battleShipDisplay.WriteCharToGridPoint(rowCharacter, -3, row);

                        for (int column = 0; column < battleShipGrid.GetNumberColumns(); column++)
                        {
                            if (userBattleShipGrid.GetTargetLocation(column, row) == userBattleShipGrid.GetUserTargetChar()
                                && battleShipGrid.IsShipLocatedHere(column, row))
                            {

                                battleShipDisplay.WriteCharToGrid('X', column, row);
                                userBattleShipGrid.UpdateNumberOfHits(++numberOfStrikes);
                                // userBattleShipGrid.updatePlayerFires(false);

                            }
                            else if (userBattleShipGrid.GetTargetLocation(column, row) == userBattleShipGrid.GetUserTargetChar())
                            {
                                battleShipDisplay.WriteCharToGrid(userBattleShipGrid.GetUserTargetChar(), column, row);
                                // userBattleShipGrid.updatePlayerFires(false);
                            }
                            else
                            {
                                battleShipDisplay.WriteCharToGrid('.', column, row);
                            }

                            /* TESTING Easter Egg */
                            if (userBattleShipGrid.IsTesting)
                            {
                                if (battleShipGrid.IsShipLocatedHere(column, row))
                                {
                                    BattleShipDisplay.WriteCharToPoint('B',
                                                    battleShipDisplay.GetErrorLeft() + (column * 2) + 10, battleShipDisplay.GetErrorTop() + row);
                                }
                                else
                                {
                                    BattleShipDisplay.WriteCharToPoint('_',
                                                    battleShipDisplay.GetErrorLeft() + (column * 2) + 10, battleShipDisplay.GetErrorTop() + row);
                                }
                            }

                        }  // for column
                    }  // for row


                    MainContent(battleShipDisplay, userBattleShipGrid, battleShipGrid);

                    // This line HAS to be before ParseCharFromActor()
                    userBattleShipGrid.UpdateRestartGameStatus(false);

                    battleShipInput.ReadCharFromActor();

                    ParseCharFromActor(userBattleShipGrid, battleShipInput, battleShipInput.GetCharFromActor());

                    // When Actor presses ENTER
                    if (userBattleShipGrid.AreUserInputsValid() &&
                        userBattleShipGrid.ShipStrikes < battleShipGrid.GetShipLength() &&
                            userBattleShipGrid.PlayerFires &&
                                (previousRow != userBattleShipGrid.GetRowIndex() ||
                                    previousColumn != userBattleShipGrid.PlayerColumn - 1))
                    {

                        /*
                         * These previous### variables exist to prevent error
                         * after Actor types in Enter key
                         * but hasn't changed Row and Column keys.
                         * Preventing Actor from firing at the same target right
                         * after trying it on previous turn.
                         */
                        previousRow = userBattleShipGrid.GetRowIndex();
                        previousColumn = userBattleShipGrid.PlayerColumn - 1;

                        userBattleShipGrid.MarkUserTarget();

                        // If a Hit
                        if (userBattleShipGrid.DidUserFireHere(userBattleShipGrid.PlayerColumn - 1, userBattleShipGrid.GetRowIndex())
                                == battleShipGrid.IsShipLocatedHere(userBattleShipGrid.PlayerColumn - 1, userBattleShipGrid.GetRowIndex()))
                        {

                            if (userBattleShipGrid.ShipStrikes == 0)
                            {
                                userBattleShipGrid.CurrentNumberOfTurns = battleShipGrid.GetShipLength() - userBattleShipGrid.ShipStrikes + 2;
                            }

                            else
                            {
                                if (userBattleShipGrid.CurrentNumberOfTurns < battleShipGrid.GetShipLength())
                                {

                                    if (userBattleShipGrid.CurrentNumberOfTurns < battleShipGrid.GetShipLength() - userBattleShipGrid.ShipStrikes)
                                    {
                                        userBattleShipGrid.CurrentNumberOfTurns = battleShipGrid.GetShipLength() - userBattleShipGrid.ShipStrikes + 0;
                                    }
                                    else
                                    {
                                        userBattleShipGrid.CurrentNumberOfTurns--;
                                    }
                                }
                                else
                                {
                                    userBattleShipGrid.CurrentNumberOfTurns--;
                                }
                            }
                        }

                        // If not a hit
                        else
                        {
                            userBattleShipGrid.CurrentNumberOfTurns--;
                        }

                    } // If Actor fires

                    if (userBattleShipGrid.StartGameOver || userBattleShipGrid.CurrentNumberOfTurns <= 0)
                    {
                        battleShipGrid.StartGameOver();
                        userBattleShipGrid.ResetUserShipStatus();
                        userBattleShipGrid.CurrentNumberOfTurns = numberOfTurnsMax;

                        previousRow = -1;
                        previousColumn = -1;

                        if (!userBattleShipGrid.StartGameOver)
                        {
                            userBattleShipGrid.UserTriedAndFailed = true;
                        }
                    }

                } // while

            } // Main

            static void InitialScreenLayout(BattleShipDisplay battleShipDisplay)
            {
                battleShipDisplay.WriteStringLine(" ");
                battleShipDisplay.WriteStringLine("    TOP SECRET");
                battleShipDisplay.WriteStringLine("                     -  Our fate rests at your finger tips...");
                battleShipDisplay.WriteStringLine(" ");
                battleShipDisplay.WriteStringLine("The Battleship has been hidden behind an invisible cloak by Master CPU!");
                battleShipDisplay.WriteStringLine(" ");
                battleShipDisplay.WriteStringLine("     You must find it and sink it...");
                battleShipDisplay.WriteStringLine(" ");

                BattleShipDisplay.WriteString("       Type In Your Code Name Call Sign In : ");
            }

            static void ParseCharFromActor(UserBattleShipGrid userBattleShipGrid, BattleShipInput battleShipInput, char actorChar)
            {

                if (Char.IsNumber(actorChar) == true)
                {
                    userBattleShipGrid.UpdatePlayerFires(false);

                    if (actorChar >= 49)
                    {
                        userBattleShipGrid.UpdatePlayerColumn(actorChar - 48);
                    }
                    else
                    {
                        userBattleShipGrid.UpdatePlayerColumn(10);
                    }

                }
                else if (battleShipInput.GetKeyFromActor() == ConsoleKey.Enter)
                {
                    userBattleShipGrid.UpdatePlayerFires(true);
                }
                else
                {
                    userBattleShipGrid.UpdatePlayerFires(false);

                    // Could use a List to replace this Switch Case
                    switch (actorChar)
                    {
                        case 'A' or 'a':
                            userBattleShipGrid.UpdatePlayerRow('A');
                            break;

                        case 'B' or 'b':
                            userBattleShipGrid.UpdatePlayerRow('B');
                            break;

                        case 'C' or 'c':
                            userBattleShipGrid.UpdatePlayerRow('C');
                            break;

                        case 'D' or 'd':
                            userBattleShipGrid.UpdatePlayerRow('D');
                            break;

                        case 'E' or 'e':
                            userBattleShipGrid.UpdatePlayerRow('E');
                            break;

                        case 'F' or 'f':
                            userBattleShipGrid.UpdatePlayerRow('F');
                            break;

                        case 'G' or 'g':
                            userBattleShipGrid.UpdatePlayerRow('G');
                            break;

                        case 'H' or 'h':
                            userBattleShipGrid.UpdatePlayerRow('H');
                            break;

                        case 'I' or 'i':
                            userBattleShipGrid.UpdatePlayerRow('I');
                            break;

                        case 'J' or 'j':
                            userBattleShipGrid.UpdatePlayerRow('J');
                            break;

                        case 'Q' or 'q':
                            userBattleShipGrid.RunGame = false;
                            break;

                        case 'R' or 'r':
                            userBattleShipGrid.UpdatePlayerRow('_');
                            userBattleShipGrid.UpdateRestartGameStatus(true);
                            userBattleShipGrid.UserTriedAndFailed = false;
                            break;

                        case 'T' or 't':
                            userBattleShipGrid.ToggleTesting();
                            break;

                    } // switch

                } // else

            } // function ParseCharFromActor

            static void MainContent(BattleShipDisplay battleShipDisplay, UserBattleShipGrid userBattleShipGrid, BattleShipGrid battleShipGrid)
            {
                int testingPresentWidth = (userBattleShipGrid.IsTesting) ? 21 : 0;

                string extraS = (userBattleShipGrid.ShipStrikes == 1) ? "" : "s";

                if (userBattleShipGrid.ShipStrikes > 0)
                {

                    BattleShipDisplay.WriteStringToPoint($"You hit the ship {userBattleShipGrid.ShipStrikes} time{extraS}!",
                                         battleShipDisplay.GetErrorLeft() + testingPresentWidth + 11, battleShipDisplay.GetErrorTop() + 4);

                }
                else if (!userBattleShipGrid.StartGameOver && userBattleShipGrid.UserTriedAndFailed && !userBattleShipGrid.BattleShipSunk)
                {

                    userBattleShipGrid.UserTriedAndFailedCount++;

                    BattleShipDisplay.WriteStringToPoint("You Ran out of turns",
                                        battleShipDisplay.GetGridLeft() + testingPresentWidth + 1, battleShipDisplay.GetGridTop() + 2);
                    BattleShipDisplay.WriteStringToPoint("The Battleship smashed you.",
                                        battleShipDisplay.GetGridLeft() + testingPresentWidth + 2, battleShipDisplay.GetGridTop() + 4);
                    BattleShipDisplay.WriteStringToPoint("You reincarnated now try to find Battleship again to find and sink!",
                                        battleShipDisplay.GetGridLeft() + testingPresentWidth + 3, battleShipDisplay.GetGridTop() + 6);

                    if (userBattleShipGrid.UserTriedAndFailedCount >= 1)
                    {
                        userBattleShipGrid.BattleShipSunk = false;
                        userBattleShipGrid.UserTriedAndFailed = false;
                        userBattleShipGrid.UserTriedAndFailedCount = 0;
                    }
                }

                if (userBattleShipGrid.ShipStrikes > 4)
                {
                    userBattleShipGrid.BattleShipSunk = true;

                    BattleShipDisplay.ReverseColors();
                    Task.Delay(200);
                    BattleShipDisplay.WriteStringToPoint($"You sunk the Battleship!",
                                    battleShipDisplay.GetErrorLeft() + testingPresentWidth + 12, battleShipDisplay.GetErrorTop() + 7);
                    BattleShipDisplay.ForeColors();
                    Task.Delay(200);
                    BattleShipDisplay.ReverseColors();

                    BattleShipDisplay.WriteStringToPoint($"Press R or r to ReStart the Game.",
                                    battleShipDisplay.GetErrorLeft() + testingPresentWidth + 15, battleShipDisplay.GetErrorTop() + 10);

                }

                BattleShipDisplay.WriteStringToPoint("Press Row Letter on Keyboard to choose which row to target",
                                                        battleShipDisplay.GetErrorLeft(), battleShipDisplay.GetErrorTop() - 9);
                BattleShipDisplay.WriteStringToPoint("Press Column number on Keyboard to choose which column to target",
                                                        battleShipDisplay.GetErrorLeft(), battleShipDisplay.GetErrorTop() - 8);
                BattleShipDisplay.WriteStringToPoint("Where the Row and Column cross on the grid",
                                                        battleShipDisplay.GetErrorLeft() + 4, battleShipDisplay.GetErrorTop() - 6);
                BattleShipDisplay.WriteStringToPoint("is where you are targeting your Dove of death.",
                                                       battleShipDisplay.GetErrorLeft() + 4, battleShipDisplay.GetErrorTop() - 5);

                if (userBattleShipGrid.ShipStrikes < battleShipGrid.GetShipLength())
                {
                    BattleShipDisplay.WriteStringToPoint($"Battleship fires on you in {userBattleShipGrid.CurrentNumberOfTurns} turns.",
                                                            battleShipDisplay.GetErrorLeft() + 10, battleShipDisplay.GetErrorTop() - 2);
                }


                string actorRowString = (userBattleShipGrid.PlayerRow == '_') ? "Type in You Row Letter" : userBattleShipGrid.PlayerRow.ToString();
                string actorColumnString = (userBattleShipGrid.PlayerColumn == -99) ? "Type in Your Column number, for 10 type in a 0 (zero)" : userBattleShipGrid.PlayerColumn.ToString();

                BattleShipDisplay.WriteStringToPoint($"  You Pressed --> Row    {actorRowString}",
                                                        battleShipDisplay.GetInformationLeft(), battleShipDisplay.GetInformationTop() - 2);
                BattleShipDisplay.WriteStringToPoint($"              --> Column {actorColumnString}",
                                                        battleShipDisplay.GetInformationLeft(), battleShipDisplay.GetInformationTop() - 1);
                BattleShipDisplay.WriteStringToPoint("  ", battleShipDisplay.GetInformationLeft() + 1, battleShipDisplay.GetInformationTop() + 1);
                BattleShipDisplay.WriteStringToPoint("          Press the Enter / Return key to fire a shot at the MCU's Battleship.", battleShipDisplay.GetInformationLeft() - 5, battleShipDisplay.GetInformationTop() + 1);

                BattleShipDisplay.WriteStringToPoint("Grid Legend",
                        battleShipDisplay.GetGridLeft() - 16, battleShipDisplay.GetGridTop() + 1);
                BattleShipDisplay.WriteStringToPoint(".  Unknown",
                        battleShipDisplay.GetGridLeft() - 15, battleShipDisplay.GetGridTop() + 3);
                BattleShipDisplay.WriteStringToPoint("O  missed",
                        battleShipDisplay.GetGridLeft() - 15, battleShipDisplay.GetGridTop() + 5);
                BattleShipDisplay.WriteStringToPoint("X  Strike",
                        battleShipDisplay.GetGridLeft() - 15, battleShipDisplay.GetGridTop() + 7);
            }

            static void ShowHeader(BattleShipDisplay battleShipDisplay, UserBattleShipGrid userBattleShipGrid)
            {
                var currentDate = DateTime.Now;

                battleShipDisplay.WriteHeaderLine("........................", 0, 0);
                battleShipDisplay.WriteHeaderLine($"Hello, {userBattleShipGrid.ActorName}, on {currentDate:d} at {currentDate:t}!", 0, 1);
                battleShipDisplay.WriteHeaderLine("      Battleship        ", 0, 2);
                battleShipDisplay.WriteHeaderLine("........................", 0, 3);
                battleShipDisplay.WriteHeaderLine(".......The Ocean........", 0, 4);
                battleShipDisplay.WriteHeaderLine("........................", 0, 5);

                battleShipDisplay.WriteHeaderLine("Press 'q' or 'Q' to blow up the entire Grid...", 0, 7);

                BattleShipDisplay.WriteStringToPoint("1 2 3 4 5 6 7 8 9 10", battleShipDisplay.GetGridLeft(), battleShipDisplay.GetGridTop() - 2);
            }

        } 
    }


    