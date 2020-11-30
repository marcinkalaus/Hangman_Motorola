﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            int lives = 5;
           // Console.WriteLine(wordToGuess);
            while (lives > 0)
            {
                Console.WriteLine("Lives: {0}", lives);
                if (lives == 1)
                {
                    Console.WriteLine("The Capital of {0}", countries[wordNumber]);
                }

                Console.WriteLine(dashWord);

                Console.WriteLine("Would you like to guess a letter or the whole word?");
                Console.WriteLine("1 - letter, 2 - word");

                
                int guessOption = Int32.Parse(Console.ReadLine());


                if (guessOption == 1)
                {
                    string givenLetter = readLetter();

                    if (wordToGuess.Contains(givenLetter))
                    {

                        int letterPosition = wordToGuess.IndexOf(givenLetter);
                        dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);

                        while (wordToGuess.IndexOf(givenLetter, letterPosition + 1) != -1)
                        {
                            letterPosition = wordToGuess.IndexOf(givenLetter, letterPosition + 1);
                            dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);
                        }

                        if (dashWord.Equals(wordToGuess))
                        {
                            Console.WriteLine("---------------------------------------------------");
                            Console.WriteLine("You won!");
                            repeatGame();
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

                    if (givenWord.Equals(wordToGuess))
                    {
                        Console.WriteLine("You won!");
                        Console.WriteLine("---------------------------------------------------");
                        break;
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
            repeatGame();
        }
            
        
        static void Main(string[] args)
        {
            Program game = new Program();
            game.play();
        }  
    }
}