using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace Hangman_Motorola
{
    class Program
    {
        List<string> countries = new List<string>();
        List<string> capitals = new List<string>();
        List<string> lettersNotInWord = new List<string>();

        //Give the path of txt file on your local machine
        string[] locationReader()
        {
            return System.IO.File.ReadAllLines(@"C:\Users\marci\source\repos\Hangman_Motorola\Hangman_Motorola\countries_and_capitals.txt");
        }

        void countriesAndCapitalsCreator(string[] locations)
        {
            foreach (string location in locations)
            {
                countries.Add(location.Split('|')[0]);
                capitals.Add(location.Split('|')[1].Substring(1));
            }
        }

        int wordNumberGenerator(string[] locations)
        {
            Random random = new Random();
            int wordNumber = random.Next(0, locations.Length);
            return wordNumber; 
        }

        string readLetter()
        {
            Console.WriteLine("Choose your letter");
            return Console.ReadLine().ToUpper();
        }

        string readWord()
        {
            Console.WriteLine("Write your word");
            return Console.ReadLine().ToUpper();
        }

        static Stopwatch watch = new Stopwatch();
        int trialCounter = 0;
        void printWinMessage()
        {
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("You won!");
            Console.WriteLine("Your time: {0}m {1}s", watch.Elapsed.Minutes, watch.Elapsed.Seconds);
            Console.WriteLine("Number of trials: {0}", trialCounter);
        }

        void doWhileWin(string wordToGuess)
        {
            watch.Stop();
            printWinMessage();
            saveUserScoreInFile(wordToGuess);
            printTenBestHighscores();
            repeatGame();
        }
        void saveUserScoreInFile(string wordToGuess)
        {
            Console.WriteLine("What is your name?");
            string userName = Console.ReadLine();
            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string userEntry = String.Format("{0,20} | {1} |      {2:D3}      |       {3}        | {4}", userName, date, watch.ElapsedMilliseconds/1000, trialCounter, wordToGuess);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\marci\source\repos\Hangman_Motorola\Hangman_Motorola\Scoreboard.txt", true))
            {
                file.WriteLine(userEntry);
            }
        }

        //Give the path for txt file on your local machine
        void printTenBestHighscores()
        {
            string[] scoreboard = System.IO.File.ReadAllLines(@"C:\Users\marci\source\repos\Hangman_Motorola\Hangman_Motorola\Scoreboard.txt");

            IEnumerable<string> query = from score in scoreboard
                                        orderby score.Split('|')[2].Substring(6,3)
                                            select score;

            List<string> sortedScores = new List<string>();
            foreach (string score in query)
            {
                sortedScores.Add(score);
            }
            Console.WriteLine("                name |             date | guessing time | guessing tries | guessed word");
            
            if (sortedScores.Count >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(sortedScores[i]);
                }
            } else
            {
                for (int i = 0; i < sortedScores.Count; i++)
                {
                    Console.WriteLine(sortedScores[i]);
                }
            }
        }

        void repeatGame()
        {
            Console.WriteLine("Would you like to play again?");
            Console.WriteLine("Type: 1 - yes, 2 - no");
            int option = Int32.Parse(Console.ReadLine());

            if (option == 1)
            {
                play();
            } else if (option == 2)
            {
                Environment.Exit(0);
            } else
            {
                Console.WriteLine("Choose correct option!");
                repeatGame();
            }
        }

        void play()
        {
            Program program = new Program();

            string[] locations = program.locationReader();
            countriesAndCapitalsCreator(locations);

            int wordNumber = wordNumberGenerator(locations);
            string wordToGuess = capitals[wordNumber].ToUpper();
            StringBuilder dashWord = new StringBuilder(wordToGuess.Length);

            for (int i = 0; i < wordToGuess.Length; i++)
                if(wordToGuess[i].Equals(' '))
                {
                    dashWord.Append(" ");
                } else {
                    dashWord.Append("_");
                }

            Console.WriteLine("Welcome to the HANGMAN GAME!\n" +
                "Your task is to guess the capital of country.\n" +
                "You can guess the letter or whole word at once but be careful!\n" +
                "Wrong guess of letter will cost you live, word - 2 lives!\n" +
                "You have 5 lives. Enjoy!\n" +
                "---------------------------------------------------\n\n");

            watch.Start();
            int lives = 5;
            // To quicker check how the program works you can uncomment line below.
            // Console.WriteLine(wordToGuess);

            while (lives > 0)
            {
                Console.WriteLine("Lives: {0}", lives);
                if (lives == 1)
                {
                    Console.WriteLine("The Capital of {0}", countries[wordNumber]);
                }

                Console.WriteLine(dashWord);

                int guessOption = 0;
                while (!((guessOption.Equals(1)) || (guessOption.Equals(2))))
                {
                    Console.WriteLine("Would you like to guess a letter or the whole word?");
                    Console.WriteLine("1 - letter, 2 - word");
                    guessOption = Int32.Parse(Console.ReadLine());
                }

                if (guessOption == 1)
                {
                    string givenLetter = readLetter();
                    trialCounter++;

                    if (wordToGuess.Contains(givenLetter[0]))
                    {
                        int letterPosition = wordToGuess.IndexOf(givenLetter[0]);
                        dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);

                        while (wordToGuess.IndexOf(givenLetter, letterPosition + 1) != -1)
                        {
                            letterPosition = wordToGuess.IndexOf(givenLetter, letterPosition + 1);
                            dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);
                        }

                        if (dashWord.Equals(wordToGuess))
                        {
                            doWhileWin(wordToGuess);
                        }
                    }
                    else
                    {
                        lives--;

                        if (!lettersNotInWord.Contains(givenLetter))
                        {
                            lettersNotInWord.Add(givenLetter);
                        }
                        Console.WriteLine("---------------------------------------------------");
                        Console.WriteLine(string.Format("Missed letters: {0}", string.Join(", ", lettersNotInWord)));
                    }
                } else if (guessOption == 2)
                {
                    string givenWord = readWord();
                    trialCounter++;
                    if (givenWord.Equals(wordToGuess))
                    {
                        doWhileWin(wordToGuess);
                    } else
                    {
                        Console.WriteLine("---------------------------------------------------");
                        lives -=2;
                    }
                } else
                {
                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine("Type correct option!");
                }
            }
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("You lose! :(");
            Console.WriteLine("The word was: {0}", wordToGuess);
            repeatGame();
        }
     
        static void Main(string[] args)
        {
            Program game = new Program();
            game.play();
        }  
    }
}