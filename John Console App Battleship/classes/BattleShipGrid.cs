using System;

namespace John_Console_App_Battleship.classes
{
    public class BattleShipGrid
    {
        private readonly int _numberRows;
        private readonly int _numberCols;

        private readonly char[,] _shipLocations;


        private int[,] _shipPositions = { { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 },
                                    { 0, 0 } };

        private int _battleShipRowStart;
        private int _battleShipColStart;
        private readonly int _battleShipLength;

        public BattleShipGrid(int numberColums, int numberRows)
        {
            _numberCols = numberColums;
            _numberRows = numberRows;
            _battleShipLength = 5;
            TypeBattleShip = 1;

            _shipLocations = new char[GetNumberRows(), GetNumberColumns()];

            ResetShipLocation();
        }

        public int TypeBattleShip
        {
            private set; get;
        }

        public void StartGameOver()
        {
            ResetShipLocation();
        }

        private void ResetShipLocation()
        {

            for (int rowL = 0; rowL < GetNumberRows(); rowL++)
            {
                for (int colL = 0; colL < GetNumberColumns(); colL++)
                {
                    _shipLocations[rowL, colL] = '_';
                }
            }

            Random rand = new Random();
            _battleShipRowStart = rand.Next(0, GetNumberRows());
            _battleShipColStart = rand.Next(0, GetNumberColumns());

            _shipLocations[GetBattleShipRowStart, GetBattleShipColStart] = 'B';

            // Figure out how many directions the ship can go
            _shipPositions = WhichDirectionsCanShipGo(GetBattleShipRowStart, GetBattleShipColStart);

            // Then pick one of them

            int directionsCanUseCount = 0;

            for (int row = 0; row < 8; row++)
            {
                if ((_shipPositions[row, 0] >= 0 && _shipPositions[row, 0] < GetNumberRows())
                    && (_shipPositions[row, 1] >= 0 && _shipPositions[row, 1] < GetNumberColumns()))
                {
                    directionsCanUseCount++;
                }
            }

            int directionRow = -1;
            int directionCol = -1;
            int DirectionOn = 1;

            // 1 to 8
            int matrixDirection = rand.Next(1, directionsCanUseCount + 1);

            if (directionsCanUseCount > 0)
            {


                for (int row = 0; row < GetNumberRows(); row++)
                {

                    if ((_shipPositions[row, 0] >= 0 && _shipPositions[row, 0] < GetNumberRows())
                        && (_shipPositions[row, 1] >= 0 && _shipPositions[row, 1] < GetNumberColumns()))
                    {
                        if (DirectionOn == matrixDirection)
                        {
                            directionRow = _shipPositions[row, 0];
                            directionCol = _shipPositions[row, 1];
                            break;
                        }
                        else
                        {
                            DirectionOn++;
                        }
                    }

                } // for

            }
            else
            {
                Console.SetCursorPosition(26, 26);
                Console.Write(" Could not find a random direction to initialize the Ship Position on the board.");
            }

            // Then fill in ship grid

            int rowChangeNumber;
            if (directionRow < GetBattleShipRowStart)
            {
                rowChangeNumber = -1;
            }
            else if (directionRow > GetBattleShipRowStart)
            {
                rowChangeNumber = 1;
            }
            else
            {
                rowChangeNumber = 0;
            }

            int columnChangeNumber;
            if (directionCol < GetBattleShipColStart)
            {
                columnChangeNumber = -1;
            }
            else if (directionCol > GetBattleShipColStart)
            {
                columnChangeNumber = 1;
            }
            else
            {
                columnChangeNumber = 0;
            }

            // 5 + 0 = 5 -> 5 + 0 = 5
            // 5 + 1 = 6 -> 6 + 1 = 7
            // 5 - 1 = 4 -> 4 - 1 = 3
            int rowLocation = GetBattleShipRowStart + rowChangeNumber;
            int colLocation = GetBattleShipColStart + columnChangeNumber;
            _shipLocations[rowLocation, colLocation] = 'B';

            /* rowLocation ;
            colLocation ; */
            _shipLocations[rowLocation += rowChangeNumber, colLocation += columnChangeNumber] = 'B';

            _shipLocations[rowLocation += rowChangeNumber, colLocation += columnChangeNumber] = 'B';

            _shipLocations[rowLocation += rowChangeNumber, colLocation += columnChangeNumber] = 'B';

        } // resetShipLocation()

        private int[,] WhichDirectionsCanShipGo(int shipRowStart, int shipColStart)
        {

            int[,] tdShipLocations = new int[8, 2];


            int positionUpEnd = GetBattleShipRowStart - (_battleShipLength - 1);


            int positionRightEnd = GetBattleShipColStart + (_battleShipLength - 1);

            // 0 + 4 =  4
            // 9 + 4 = 13
            int positionDownEnd = GetBattleShipRowStart + (_battleShipLength - 1);

            // 0 - 4 = -4
            // 9 - 4 = 5
            int positionLeftEnd = GetBattleShipColStart - (_battleShipLength - 1);


            // Range allowed -4 to 5
            int positionUpRightRowEnd = positionUpEnd;

            // Range allowed 4 to 13
            int positionUpRightColEnd = positionRightEnd;

            // Range allowed 4 to 13
            int positionDownRightRowEnd = positionDownEnd;

            // Range allowed 4 to 13
            int positionDownRightColEnd = positionRightEnd;

            // Range allowed -4 to 5
            int positionUpLeftRowEnd = positionUpEnd;

            // Range allowed -4 to 5
            int positionUpLeftColEnd = positionLeftEnd;

            // Range allowed 4 to 13
            int positionDownLeftRowEnd = positionDownEnd;

            // Range allowed -4 to 5
            int positionDownLeftColEnd = positionLeftEnd;

            // if getBattleShipRowStart = 0 && getBattleShipColStart = 0
            // then [-4, 0]
            // Ship going Up            = Row then Colmn
            tdShipLocations[0, 0] = positionUpEnd;
            tdShipLocations[0, 1] = GetBattleShipColStart;

            // if getBattleShipRowStart = 0 && getBattleShipColStart = 9
            // then [-4, 13]
            // Ship going Up   && Right
            tdShipLocations[1, 0] = positionUpRightRowEnd;
            tdShipLocations[1, 1] = positionUpRightColEnd;

            // Ship going         Right = Row then Colmn
            tdShipLocations[2, 0] = GetBattleShipRowStart;
            tdShipLocations[2, 1] = positionRightEnd;

            // Ship going Down && Right
            tdShipLocations[3, 0] = positionDownRightRowEnd;
            tdShipLocations[3, 1] = positionDownRightColEnd;

            // Ship going Down         = Row then Colmn
            tdShipLocations[4, 0] = positionDownEnd;
            tdShipLocations[4, 1] = GetBattleShipColStart;

            // Ship going Down && Left
            tdShipLocations[5, 0] = positionDownLeftRowEnd;
            tdShipLocations[5, 1] = positionDownLeftColEnd;

            // Ship going         Left = Row then Colmn
            tdShipLocations[6, 0] = GetBattleShipRowStart;
            tdShipLocations[6, 1] = positionLeftEnd;

            // Ship going Up   && Left
            tdShipLocations[7, 0] = positionUpLeftRowEnd;
            tdShipLocations[7, 1] = positionUpLeftColEnd;

            return tdShipLocations;
        }

        public int GetNumberColumns()
        {
            return _numberCols;
        }

        public int GetNumberRows()
        {
            return _numberRows;
        }

        public int GetShipLength()
        {
            return _battleShipLength;
        }

        public int GetBattleShipRowStart
        {
            get
            {
                return _battleShipRowStart;
            }
        }
        public int GetBattleShipColStart
        {
            get
            {
                return _battleShipColStart;
            }
        }

        public bool IsShipLocatedHere(int col, int row)
        {
            if (_shipLocations[row, col] == 'B')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}