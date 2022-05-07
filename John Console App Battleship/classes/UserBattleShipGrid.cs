using System;
using System.Collections.Generic;
namespace John_Console_App_Battleship.classes
{
    public class UserBattleShipGrid
    {
        private readonly int _numberRows;
        private readonly int _numberCols;
        private readonly char _userTargetChar;

        private readonly char[,] _targetLocations;

        private readonly List<char> _rowNumbers = new();

        public UserBattleShipGrid(int numberColums, int numberRows)
        {
            _numberCols = numberColums;
            _numberRows = numberRows;

            ActorName = "Default Actor Name";

            RunGame = true;

            UserTriedAndFailed = false;
            UserTriedAndFailedCount = 0;

            BattleShipSunk = false;

            _userTargetChar = 'O';

            _rowNumbers.Add('A');
            _rowNumbers.Add('B');
            _rowNumbers.Add('C');
            _rowNumbers.Add('D');
            _rowNumbers.Add('E');
            _rowNumbers.Add('F');
            _rowNumbers.Add('G');
            _rowNumbers.Add('H');
            _rowNumbers.Add('I');
            _rowNumbers.Add('J');

            _targetLocations = new char[GetNumberRows(), GetNumberColumns()];

            ResetUserShipStatus();

        }

        public char PlayerRow { get; private set; }

        public int PlayerColumn { get; private set; }

        public bool PlayerFires { get; private set; }

        public int ShipStrikes { get; private set; }

        public bool RunGame { get; set; }

        public bool UserTriedAndFailed { get; set; }

        public string ActorName { get; set; }

        public int UserTriedAndFailedCount { get; set; }

        public bool BattleShipSunk { get; set; }

        public int CurrentNumberOfTurns { get; set; }

        public void ResetUserShipStatus()
        {


            PlayerRow = '_';
            PlayerColumn = -99;

            UpdatePlayerFires(false);

            UpdateNumberOfHits(0);

            IsTesting = false;

            for (int row = 0; row < GetNumberRows(); row++)
            {
                for (int col = 0; col < GetNumberColumns(); col++)
                {
                    _targetLocations[row, col] = '~';
                }
            }
        }


        public char GetUserTargetChar()
        {
            return _userTargetChar;
        }

        public void UpdatePlayerColumn(int playerColumn)
        {
            if (playerColumn > 0 && playerColumn <= GetNumberColumns())
            {
                PlayerColumn = playerColumn;
            }
            else
            {
                PlayerColumn = -99;
            }
        }

        public void UpdatePlayerRow(char playerRow)
        {
            PlayerRow = playerRow;
        }

        private int GetNumberRows()
        {
            return _numberRows;
        }

        private int GetNumberColumns()
        {
            return _numberCols;
        }

        public void UpdatePlayerFires(bool playerFires)
        {
            PlayerFires = playerFires;
        }

        public char GetTargetLocation(int column, int row)
        {
            return _targetLocations[row, column];
        }

        public bool DidUserFireHere(int column, int row)
        {
            bool didFireHere = false;
            if (_targetLocations[row, column] == GetUserTargetChar())
            {
                didFireHere = true;
            }
            return didFireHere;
        }

        public bool AreUserInputsValid()
        {
            bool returnBoolean = false;
            if (GetRowIndex() >= 0 && GetRowIndex() < _rowNumbers.Count)
            {
                if (PlayerColumn > 0 && PlayerColumn <= _numberCols)
                {
                    returnBoolean = true;
                }
            }
            return returnBoolean;
        }

        public int GetRowIndex()
        {
            // why did creating this rowIndex variable fix this?
            int rowIndex = _rowNumbers.FindIndex(e => e == PlayerRow);

            // when I did a direct return of the _RowNumbers.FindIndex it broke the code.
            return rowIndex;
        }

        public char GetRowChar(int index)
        {

            char charAtIndex = _rowNumbers.ElementAt(index);

            return charAtIndex;
        }

        public void MarkUserTarget()
        {

            if (PlayerRow != '_' && PlayerColumn != -99)
            {
                _targetLocations[GetRowIndex(), PlayerColumn - 1] = GetUserTargetChar();
            }
            else
            {
                UpdatePlayerFires(false);
            }

        

        }

        public void UpdateNumberOfHits(int numberOfShipStrikes)
        {
            ShipStrikes = numberOfShipStrikes;
        }

        public void UpdateRestartGameStatus(bool gameStatus)
        {
            StartGameOver = gameStatus;
        }

        public bool StartGameOver
        {
            get; private set;
        }

        public bool IsTesting
        {
            get; private set;
        }

        public void ToggleTesting()
        {
            IsTesting = !IsTesting;
        }
    }
}