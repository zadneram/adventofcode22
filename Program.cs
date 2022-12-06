﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            string result = Day5_Part1();
            Console.WriteLine(result);
            Console.ReadKey();
        }

        public static string Day5_Part1()
        {
            List<string> stackLines = new List<string>();
            List<string> moves = new List<string>();
            Dictionary<int, Stack<char>> stacks = new Dictionary<int, Stack<char>>();

            foreach (var line in Utilities.GetInputForDay(5))
            {
                if (line.StartsWith("["))
                {
                    stackLines.Add(line);
                }
                else if (line.StartsWith("move"))
                {
                    moves.Add(line);
                }
            }

            stacks = ProcessStartingStacks(stackLines);
            ProcessMoves(stacks, moves);

            StringBuilder sb = new StringBuilder(9);
            for (int i = 1; i <= stacks.Count; i++)
            {
                sb.Append(stacks[i].First());
            }

            return sb.ToString();
        }

        private static void ProcessMoves(Dictionary<int, Stack<char>> stacks, List<string> moves)
        {
            foreach (var move in moves)
            {
                var words = move.Split(' ');
                int numberToMove = int.Parse(words[1]);
                int from = int.Parse(words[3]);
                int to = int.Parse(words[5]);

                List<char> cratesToMove = new List<char>();
                for (int i = 0; i < numberToMove; i++)
                {
                    cratesToMove.Add(stacks[from].Pop());
                }

                for (int j = cratesToMove.Count; j > 0; j--)
                {
                    stacks[to].Push(cratesToMove[j-1]);
                }
            }
        }

        private static Dictionary<int, Stack<char>> ProcessStartingStacks(List<string> stackLines)
        {
            Dictionary<int, Stack<char>> stacks = new Dictionary<int, Stack<char>>();

            for (int s = stackLines.Count - 1; s >= 0; s--)
            {
                string line = stackLines[s];

                // Items start at index 1 and are 4 apart
                for (int i = 1; i < line.Length; i += 4)
                {
                    if (line[i] != ' ')
                    {
                        int stackNo = (int)Math.Floor(i / 4m) + 1;
                        if (stacks.TryGetValue(stackNo, out Stack<char> stack) == false)
                        {
                            stack = new Stack<char>();
                            stacks.Add(stackNo, stack);
                        }

                        stack.Push(line[i]);
                    }
                }
            }

            return stacks;
        }

        public static string Day4_Part2()
        {
            int intersectionCount = 0;
            foreach (var line in Utilities.GetInputForDay(4))
            {
                List<int> elf1Sections, elf2Sections;
                GetDay4Sections(line, out elf1Sections, out elf2Sections);

                bool isIntersection = elf1Sections.Intersect(elf2Sections).Any();
                if (isIntersection)
                {
                    intersectionCount++;
                }
            }

            return intersectionCount.ToString();
        }

        public static string Day4_Part1()
        {
            int subsetCount = 0;
            foreach (var line in Utilities.GetInputForDay(4))
            {
                List<int> elf1Sections, elf2Sections;
                GetDay4Sections(line, out elf1Sections, out elf2Sections);

                bool isSubset = !elf1Sections.Except(elf2Sections).Any();
                isSubset |= !elf2Sections.Except(elf1Sections).Any();

                if (isSubset)
                {
                    subsetCount++;
                }
            }

            return subsetCount.ToString();
        }

        private static void GetDay4Sections(string line, out List<int> elf1Sections, out List<int> elf2Sections)
        {
            var elfs = line.Split(',');
            var elf1StartEnd = elfs[0].Split('-');
            int elf1Start = int.Parse(elf1StartEnd[0]);
            int elf1End = int.Parse(elf1StartEnd[1]);

            elf1Sections = new List<int>();
            for (int i = elf1Start; i <= elf1End; i++)
            {
                elf1Sections.Add(i);
            }

            var elf2StartEnd = elfs[1].Split('-');
            int elf2Start = int.Parse(elf2StartEnd[0]);
            int elf2End = int.Parse(elf2StartEnd[1]);

            elf2Sections = new List<int>();
            for (int i = elf2Start; i <= elf2End; i++)
            {
                elf2Sections.Add(i);
            }
        }

        public static void Day3_Part2()
        {
            string inputFile = @".\Day3.txt";
            int totalPriority = 0;

            using (FileStream fs = File.OpenRead(inputFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string elf1 = sr.ReadLine();
                        string elf2 = sr.ReadLine();
                        string elf3 = sr.ReadLine();

                        var badgeType = elf1.Intersect(elf2)
                                            .Intersect(elf3)
                                            .Single();

                        int priority;
                        if (Char.IsLower(badgeType))
                        {
                            // Lower 'a' is ASCII 97 so subtract 96 to get 1 as priority.
                            priority = (int)badgeType - 96;
                        }
                        else
                        {
                            // Upper 'A' is ASCII 65 so subtract 38 to get 27 as priority.
                            priority = (int)badgeType - 38;
                        }

                        totalPriority += priority;
                    }
                }
            }

            Console.WriteLine(totalPriority);

            Console.ReadKey();
        }

        public static void Day3_Part1()
        {
            string inputFile = @".\Day3.txt";
            int totalPriority = 0;

            using (FileStream fs = File.OpenRead(inputFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        int halfWay = line.Length / 2;
                        string comp1 = line.Substring(0, halfWay);
                        string comp2 = line.Substring(halfWay);

                        var commonItem = comp1.Intersect(comp2).Single();

                        int priority;
                        if (Char.IsLower(commonItem))
                        {
                            // Lower 'a' is ASCII 97 so subtract 96 to get 1 as priority.
                            priority = (int)commonItem - 96;
                        }
                        else
                        {
                            // Upper 'A' is ASCII 65 so subtract 38 to get 27 as priority.
                            priority = (int)commonItem - 38;
                        }

                        totalPriority += priority;
                    }
                }
            }

            Console.WriteLine(totalPriority);

            Console.ReadKey();
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