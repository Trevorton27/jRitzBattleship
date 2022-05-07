using System;
using static System.Console;
namespace John_Console_App_Battleship.classes
{
	public class BattleShipInput
	{
		private string _ActorInputString = "";
		private char _ActorInputChar = ' ';
		private ConsoleKey _ActorKeyInfo;
        //readonly List<int> intList = new List<int>();

		public BattleShipInput()
		{
		}

		public char GetCharFromActor()
		{
			return _ActorInputChar;
		}

		public ConsoleKey GetKeyFromActor()
		{
			return _ActorKeyInfo;
		}


		public void ReadCharFromActor()
		{
			ConsoleKeyInfo cKeyInfo;

			cKeyInfo = ReadKey(true);

			_ActorInputChar = cKeyInfo.KeyChar;
			_ActorKeyInfo = cKeyInfo.Key;

		}

		public string GetLineFromActor()
		{
			return _ActorInputString;
		}

		public void ReadLineFromActor()
		{
			string? inputLine = ReadLine();
			if (inputLine != null && inputLine != "")
			{

				string printThis = $"ReadLineFromActor if (inputLine != null) Actor typed <{inputLine}>";

				SetCursorPosition(26, 16);
				Write(printThis);

				_ActorInputString = inputLine;
			}
			else
			{
			SetCursorPosition(26, 16);
				Write("ReadLineFromActor is null");

				_ActorInputString = "Player 1";
			}
		}
	}
}