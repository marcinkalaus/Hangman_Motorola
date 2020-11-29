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

        void play()
        {
            Program program = new Program();

            string[] locations = program.locationReader();
            countriesAndCapitalsCreator(locations);

            int wordNumber = wordNumberGenerator(locations);
            string wordToGuess = "Madrid"; //capitals[wordNumber];

            StringBuilder dashWord = new StringBuilder(wordToGuess.Length);
            for (int i = 0; i < wordToGuess.Length; i++)
                {
                    dashWord.Append("_");
                }

            int lives = 5;
            Console.WriteLine(wordToGuess);
            while (lives > 0)
            {
                Console.WriteLine("Choose your letter");
                Console.WriteLine(dashWord);
                string givenLetter = Console.ReadLine();
                if (wordToGuess.Contains(givenLetter))
                {
                    Console.WriteLine("Letter found");
                    int letterPosition = wordToGuess.IndexOf(givenLetter);
                    dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);

                    while(wordToGuess.IndexOf(givenLetter, letterPosition + 1) != -1)
                    {
                        letterPosition = wordToGuess.IndexOf(givenLetter, letterPosition + 1);
                        dashWord.Replace(dashWord[letterPosition], givenLetter[0], letterPosition, 1);
                    }

                    if (dashWord.Equals(wordToGuess))
                    {
                        Console.WriteLine("extra");
                        break;
                    }

                } else
                {
                    lives--;
                }

            }

        }
            
        
        static void Main(string[] args)
        {

            Program game = new Program();
            game.play();
            
            //Console.WriteLine(program.wordNumberGenerator(locations));

         /*   for (int i=0; i<10; i++)
            {
                Console.WriteLine(countries[i]);
                Console.WriteLine(capitals[i]);
            }*/

        }

       
    }
}