﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            Day1();
        }

        public static void Day1()
        {
            string inputFile = @".\Day1.txt";
            List<int> elfCalCounts = new List<int>();
            int currentElfCalCount = 0;
            int lineCounter = 0;

            using (FileStream fs = File.OpenRead(inputFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        lineCounter++;

                        if (String.IsNullOrWhiteSpace(line))
                        {
                            elfCalCounts.Add(currentElfCalCount);
                            currentElfCalCount = 0;
                            continue;
                        }

                        if (int.TryParse(line, out int calForItem))
                        {
                            currentElfCalCount += calForItem;
                        }
                        else
                        {
                            Console.WriteLine($"Unable to parse cal count for line {lineCounter} with value: {line}");
                        }
                    }
                }
            }

            int mostCaloriesForOneElf = elfCalCounts.Max();
            Console.WriteLine($"PART 1- Most calories for single elf is: {mostCaloriesForOneElf}");

            var top3ElfCalCounts = elfCalCounts.OrderByDescending(c => c)
                                               .Take(3);

            int top3ElfTotalCalCount = top3ElfCalCounts.Sum();
            Console.WriteLine($"PART 2 - Top 3 Elves are carrying total calories of: {top3ElfTotalCalCount}");

            Console.ReadKey();
        }

        public static void Day2()
        {
            string inputFile = @".\Day2.txt";
            var pointsForPlaying = new Dictionary<string, int>()
            {
                { "A", 1 },
                { "B", 2 },
                { "C", 3 }
            };

            var pointsForResultPart1 = new Dictionary<(string, string), int>()
            {
                { ("A", "X"), 3 },
                { ("A", "Y"), 6 },
                { ("A", "Z"), 0 },
                { ("B", "X"), 0 },
                { ("B", "Y"), 3 },
                { ("B", "Z"), 6 },
                { ("C", "X"), 6 },
                { ("C", "Y"), 0 },
                { ("C", "Z"), 3 },
            };

            var pointsForResultPart2 = new Dictionary<(string, string), int>()
            {
                { ("A", "A"), 3 },
                { ("A", "B"), 6 },
                { ("A", "C"), 0 },
                { ("B", "A"), 0 },
                { ("B", "B"), 3 },
                { ("B", "C"), 6 },
                { ("C", "A"), 6 },
                { ("C", "B"), 0 },
                { ("C", "C"), 3 },
            };

            var wins = new Dictionary<string, string>()
            {
                { "A", "B" },
                { "B", "C" },
                { "C", "A" }
            };

            var losses = new Dictionary<string, string>()
            {
                { "A", "C" },
                { "B", "A" },
                { "C", "B" }
            };

            int totalScore = 0;
            using (FileStream fs = File.OpenRead(inputFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        var strategy = line.Split(' ');
                        var oppositionPlay = strategy[0];
                        var yourPlay = strategy[1];

                        string shapeToPlay;
                        if (yourPlay == "X")
                        {
                            // Lose
                            shapeToPlay = losses[oppositionPlay];
                        }
                        else if (yourPlay == "Y")
                        {
                            // Draw
                            shapeToPlay = oppositionPlay;
                        }
                        else
                        {
                            // Win
                            shapeToPlay = wins[oppositionPlay];
                        }

                        totalScore += pointsForPlaying[shapeToPlay];
                        totalScore += pointsForResultPart2[(oppositionPlay, shapeToPlay)];
                    }
                }
            }

            Console.WriteLine($"Total score = {totalScore}");
            Console.ReadKey();
        }
    }
}