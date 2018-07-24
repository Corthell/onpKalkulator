using System;
using System.Collections;

namespace onp
{	
	class Program
	{
        // funkcja zwraca range ważnosci symbolu matematycznego.
        public static int checkRankSymbol(string sign)
		{
			switch(sign)
			{
				case "+": return 1;
				break;
				case "-": return 1;
				break;
				case "*": return 2;
				break;
				case "/": return 2;
				break;
				case "^": return 3;
				break;
				case "(": return 0;
				break;
				case ")": return 0;
				break;
				default: return -1;
			}
		}
		// funkcja sprawdza czy danyc znak jest liczba czy znakiem.
		public static bool checkValue(string sign)
		{
			int result;
			
			if(Int32.TryParse(sign, out result))
			{
				return true;
			}
			else
			{
				return false;
			}			
		}
		// funkcja obliczajaca wartosc wyrazenia z formy postfix.
		public static double Calculate(string postExp)
		{
			Stack stackExp = new Stack();
			double resultParse = 0;
			// zmienne pomocnicze do obliczen.
			double a, b;
			double c = 1;
			
			for (int i = 0; i < postExp.Length; i++) {
			
				if(double.TryParse(postExp[i].ToString(), out resultParse))
				{
					stackExp.Push(resultParse);
				}
				else
				{
					switch(postExp[i])
					{
						case '+':
							a = (double)stackExp.Pop();
							b = (double)stackExp.Pop();
							stackExp.Push(b+a);
						break;
						case '-':
							a = (double)stackExp.Pop();
							b = (double)stackExp.Pop();
							stackExp.Push(b-a);
						break;
						case '*':
							a = (double)stackExp.Pop();
							b = (double)stackExp.Pop();
							stackExp.Push(b*a);
						break;
						case '/':
							a = (double)stackExp.Pop();
							b = (double)stackExp.Pop();
							stackExp.Push(b/a);
						break;
						case '^':
							a = (double)stackExp.Pop();
							b = (double)stackExp.Pop();
							while(a!=0)
							{
								c *= b;
								a--;
							}
							stackExp.Push(c);
							c = 1;
						break;
							
					}
				}
			}
			
			return (double)stackExp.Pop(); 
		}

        // ##########################################################################################################################################

        public static void Main(string[] args)
		{
            Exception ex;
            ex = new Exception();

            try
			{
				Stack signStack = new Stack();
				string outString = "";
				string tempString = "";

                Console.WriteLine("WSZIB (1K331 Rulez:)	                           2017");
                Console.WriteLine("-------------------------------------------------------");
				Console.WriteLine("Obliczanie i zamiana wyrazen z infiksowej na postfixowa");
				Console.WriteLine("Dostepne dzialania: +, -, *, /, ^, ( )");
				Console.WriteLine("Dostepne liczby: 0 - 9");
				Console.WriteLine("-------------------------------------------------------");
				Console.WriteLine("Autor: Michal Dudek");
				Console.WriteLine("-------------------------------------------------------\n");
					
				Console.Write("\nInfiks:   ");
                //pobieranie danych wejsciowych i zapis do zmiennej.
				string stringData = Console.ReadLine();
				
				// zamiana nieprawidlowego znaku - lub + na poczatku wejscia np. -2+.... na (0-2)... - konieczne do obliczania wyrazenia.
				if(stringData[0] == '-')
				{
					tempString = stringData;
					
					stringData = "(0" + stringData[0] + stringData[1]+ ")" + tempString.Remove(0,2);
				}
				else if (stringData[0] == '+')
				{
					tempString = stringData;
					stringData = "(0" + stringData[0] + stringData[1]+ ")" + tempString.Remove(0,2);
				}

                // ZAMIANA NA POSTFIX ---------------------------------------------------------------------------------------------------------------

                for (int i = 0; i < stringData.Length; i++) {

                    //jesli liczba to zapisz na wyjscie.
                    if (checkValue(stringData[i].ToString()))
                    {
                        outString += stringData[i];
                    }
                    // jesli odczyta niedozwoly znak to rzuca wyjatek i konczy dzialanie programu.
                    else if (checkRankSymbol(stringData[i].ToString()) == -1) throw ex;
                    else
					{	
	                    // jeśli stos pusty lub znak bedacy ( ) lub znak wiekszy od znaku na stosie.
						if(signStack.Count == 0  || stringData[i] == '(' || stringData[i]!= ')' && checkRankSymbol(stringData[i].ToString()) > checkRankSymbol(signStack.Peek().ToString()))
						{
							signStack.Push(stringData[i]);
						}
						else
						{   // operacja kasowania nazwiasu i zapis na wyjscie znakow bedacych w nawiasach.
	                        if(stringData[i] == ')')
	                        {
	                            while (signStack.Count != 0 && checkRankSymbol(signStack.Peek().ToString()) != 0)
	                            {
	                                outString += signStack.Pop();
	                            }
	                            signStack.Pop();
	                        }
	                        else
	                        {
	                            while (signStack.Count != 0 && checkRankSymbol(signStack.Peek().ToString()) != 0 && checkRankSymbol(stringData[i].ToString()) <= checkRankSymbol(signStack.Peek().ToString()))
	                            {
	                                outString += signStack.Pop();
	                            }
	                            signStack.Push(stringData[i]);
	                        }
						}
					}
				}
				// pobranie reszy wartosci ze stosu po odczycie ostatniego znaku wyrazenia.
				while(signStack.Count != 0)
				{
					outString += signStack.Pop();
				}
				
				Console.WriteLine("Postfiks: " + outString + "\n");

				signStack.Clear();
				
                // OBLICZANIE WYRAŻENIA -------------------------------------------------------------------------------------------------------------

				Console.WriteLine("Wynik: " + Calculate(outString).ToString());
                Console.WriteLine("\n\n-------\nKONIEC.");
				
				Console.ReadKey(true);
			}
			catch
			{
				Console.Clear();
				Console.WriteLine("\n Niedozwolona operacja!\n\n KONIEC");

                Console.ReadKey(true);
			}
		}
	}
}