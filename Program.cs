using Microsoft.Win32;
using System;
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
            string result = Day10_Part2();
            Console.WriteLine(result);
            Console.ReadKey();
        }

        public static string Day10_Part2()
        {
            int register = 1;
            int cycles = 0;

            void PrintPixel()
            {
                int printPixelAt = (cycles - 1) % 40;
                int pixelMatchSprite = Math.Abs(register - printPixelAt);
                if (pixelMatchSprite == 0 || pixelMatchSprite == 1)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write(".");
                }

                if (cycles % 40 == 0)
                {
                    Console.WriteLine();
                }
            }

            int lineNo = 0;
            //foreach (var line in Utilities.GetInputFromFile(day: 10, example: true))
            foreach (var line in Utilities.GetInputForDay(10))
            {
                lineNo++;

                if (line == "noop")
                {
                    cycles++;
                    PrintPixel();
                    continue;
                }
                else if (line.StartsWith("addx"))
                {
                    int value = int.Parse(line.Split(' ')[1]);

                    // First cycle
                    cycles++;
                    PrintPixel();

                    // Second cycle
                    cycles++;
                    PrintPixel();
                    register += value;
                }
            }

            return String.Empty;
        }

        public static string Day10_Part1()
        {
            int register = 1;
            int cycles = 0;
            int signalStrength = 0;

            void CheckForKeyCycle()
            {
                if ((cycles - 20) % 40 == 0)
                {
                    signalStrength += (cycles * register);
                }
            }

            int lineNo = 0;
            //foreach (var line in Utilities.GetInputFromFile(day: 10, example: true))
            foreach (var line in Utilities.GetInputForDay(10))
            {
                lineNo++;

                if (line == "noop")
                {
                    cycles++;
                    CheckForKeyCycle();
                    continue;
                }
                else if (line.StartsWith("addx"))
                {
                    int value = int.Parse(line.Split(' ')[1]);
                    
                    // First cycle
                    cycles++;
                    CheckForKeyCycle();

                    // Second cycle
                    cycles++;
                    CheckForKeyCycle();
                    register += value;
                }
            }

            return signalStrength.ToString();
        }

        public static void MoveKnotsUp(Pair<int, int> lead, Pair<int, int> trail)
        {
            // Only move tail if it's y coord is two spaces different to the head
            int yDelta = lead.y - trail.y;
            if (yDelta >= 2)
            {
                // Move tail towards head
                trail.y += yDelta - 1;

                // If the x coord is different, move it one space nearer
                // Math.Sign return -1 (left), 0 (no move), 1 (right) regardless of how many spaces between x coords.
                int xDelta = Math.Sign(lead.x - trail.x);
                trail.x += xDelta;
            }
        }

        public static void MoveKnotsDown(Pair<int, int> lead, Pair<int, int> trail)
        {
            // Only move tail if it's y coord is two spaces different to the head
            int yDelta = lead.y - trail.y;
            if (yDelta <= -2)
            {
                // Move tail towards head
                trail.y += yDelta + 1;

                // If the x coord is different, move it one space nearer
                // Math.Sign return -1 (left), 0 (no move), 1 (right) regardless of how many spaces between x coords.
                int xDelta = Math.Sign(lead.x - trail.x);
                trail.x += xDelta;
            }
        }

        public static void MoveKnotsLeft(Pair<int, int> lead, Pair<int, int> trail)
        {
            // Only move tail if it's x coord is two spaces different to the head
            int xDelta = lead.x - trail.x;
            if (xDelta <= -2)
            {
                // Move tail towards head
                trail.x += xDelta + 1;

                // If the y coord is different, move it one space nearer
                // Math.Sign return -1 (down), 0 (no move), 1 (up) regardless of how many spaces between y coords.
                int yDelta = Math.Sign(lead.y - trail.y);
                trail.y += yDelta;
            }
        }

        public static void MoveKnotsRight(Pair<int, int> lead, Pair<int, int> trail)
        {
            // Only move tail if it's x coord is two spaces different to the head
            int xDelta = lead.x - trail.x;
            if (xDelta >= 2)
            {
                // Move tail towards head
                trail.x += xDelta - 1;

                // If the y coord is different, move it one space nearer
                // Math.Sign return -1 (down), 0 (no move), 1 (up) regardless of how many spaces between y coords.
                int yDelta = Math.Sign(lead.y - trail.y);
                trail.y += yDelta;
            }
        }

        public static string Day9_Part2()
        {
            List<Pair<int, int>> knots = new List<Pair<int, int>>(10);
            for (int i = 0; i < 10; i++)
            {
                knots.Add(new Pair<int, int>(0, 0));
            }

            var head = knots[0];
            var tail = knots[9];

            List<Pair<int, int>> tailPositions = new List<Pair<int, int>>() { new Pair<int, int>(0, 0) };

            void PrintRope()
            {
                for (int y = 4; y >= 0; y--)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        int knotAtCoord = knots.FindIndex(p => p.Equals(new Pair<int, int>(x, y)));
                        if (knotAtCoord != -1)
                        {
                            Console.Write($"{(knotAtCoord == 0 ? "H" : knotAtCoord.ToString())}");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            string lastLine;
            foreach (string line in Utilities.GetInputForDay(9))
            //foreach (string line in Utilities.GetInputFromFile(day: 9, example: true, part: 2))
            {
                lastLine = line;
                var move = line.Split(' ');
                string direction = move[0];
                int noOfSpaces = int.Parse(move[1]);

                //Console.WriteLine();
                //Console.WriteLine();

                if (direction == "U")
                {
                    int destinationY = head.y + noOfSpaces;
                    for (int j = head.y + 1; j <= destinationY; j++)
                    {
                        head.y = j;

                        for (int l = 0; l < 9; l++)
                        {
                            MoveKnotsUp(knots[l], knots[l + 1]);
                            MoveKnotsDown(knots[l], knots[l + 1]);
                            MoveKnotsLeft(knots[l], knots[l + 1]);
                            MoveKnotsRight(knots[l], knots[l + 1]);
                        }

                        //PrintRope();
                        tailPositions.Add(new Pair<int, int>(knots[9].x, knots[9].y));
                    }
                }
                else if (direction == "D")
                {
                    int destinationY = head.y - noOfSpaces;
                    for (int j = head.y - 1; j >= destinationY; j--)
                    {
                        head.y = j;

                        for (int l = 0; l < 9; l++)
                        {
                            MoveKnotsDown(knots[l], knots[l + 1]);
                            MoveKnotsUp(knots[l], knots[l + 1]);
                            MoveKnotsLeft(knots[l], knots[l + 1]);
                            MoveKnotsRight(knots[l], knots[l + 1]);
                        }

                        //PrintRope();
                        tailPositions.Add(new Pair<int, int>(knots[9].x, knots[9].y));
                    }
                }
                else if (direction == "L")
                {
                    int destinationX = head.x - noOfSpaces;
                    for (int i = head.x - 1; i >= destinationX; i--)
                    {
                        head.x = i;

                        for (int k = 0; k < 9; k++)
                        {
                            MoveKnotsLeft(knots[k], knots[k + 1]);
                            MoveKnotsRight(knots[k], knots[k + 1]);
                            MoveKnotsUp(knots[k], knots[k + 1]);
                            MoveKnotsDown(knots[k], knots[k + 1]);
                        }

                        //PrintRope();
                        tailPositions.Add(new Pair<int, int>(knots[9].x, knots[9].y));
                    }
                }
                else if (direction == "R")
                {
                    int destinationX = head.x + noOfSpaces;
                    for (int i = head.x + 1; i <= destinationX; i++)
                    {
                        head.x = i;

                        for (int k = 0; k < 9; k++)
                        {
                            MoveKnotsRight(knots[k], knots[k + 1]);
                            MoveKnotsLeft(knots[k], knots[k + 1]);
                            MoveKnotsUp(knots[k], knots[k + 1]);
                            MoveKnotsDown(knots[k], knots[k + 1]);
                        }

                        //PrintRope();
                        tailPositions.Add(new Pair<int, int>(knots[9].x, knots[9].y));
                    }
                }
            }

            return tailPositions.Distinct().Count().ToString();
        }

        public static string Day9_Part1()
        {
            var currentHeadPos = (x: 0, y: 0);
            var currentTailPos = (x: 0, y: 0);
            List<(int, int)> tailPositions = new List<(int, int)>() { currentTailPos };
            //foreach (string line in Utilities.GetInputFromFile(day: 9, example: true))
            foreach (string line in Utilities.GetInputForDay(9))
            {
                var move = line.Split(' ');
                string direction = move[0];
                int noOfSpaces = int.Parse(move[1]);

                if (direction == "L")
                {
                    int destinationX = currentHeadPos.x - noOfSpaces;
                    for (int i = currentHeadPos.x - 1; i >= destinationX; i--)
                    {
                        currentHeadPos = (x: i, y: currentHeadPos.y);

                        // Only move tail if it's x coord is two spaces different to the head
                        if (Math.Abs(currentHeadPos.x - currentTailPos.x) == 2)
                        {
                            // Move tail towards head
                            int newTailx = currentTailPos.x - 1;

                            // Move diagonally if needed by moving to the same y coord as the head
                            int newTaily = currentHeadPos.y == currentTailPos.y ? currentTailPos.y : currentHeadPos.y;

                            currentTailPos = (x: newTailx, y: newTaily);
                            tailPositions.Add(currentTailPos);
                        }
                    }
                }
                else if (direction == "R")
                {
                    int destinationX = currentHeadPos.x + noOfSpaces;
                    for (int i = currentHeadPos.x + 1; i <= destinationX; i++)
                    {
                        currentHeadPos = (x: i, y: currentHeadPos.y);

                        // Only move tail if it's x coord is two spaces different to the head
                        if (Math.Abs(currentHeadPos.x - currentTailPos.x) == 2)
                        {
                            // Move tail towards head
                            int newTailx = currentTailPos.x + 1;

                            // Move diagonally if needed by moving to the same y coord as the head
                            int newTaily = currentHeadPos.y == currentTailPos.y ? currentTailPos.y : currentHeadPos.y;

                            currentTailPos = (x: newTailx, y: newTaily);
                            tailPositions.Add(currentTailPos);
                        }
                    }
                }
                else if (direction == "U")
                {
                    int destinationY = currentHeadPos.y + noOfSpaces;
                    for (int j = currentHeadPos.y + 1; j <= destinationY; j++)
                    {
                        currentHeadPos = (x: currentHeadPos.x, y: j);

                        // Only move tail if it's y coord is two spaces different to the head
                        if (Math.Abs(currentHeadPos.y - currentTailPos.y) == 2)
                        {
                            // Move tail towards head
                            int newTaily = currentTailPos.y + 1;

                            // Move diagonally if needed by moving to the same y coord as the head
                            int newTailx = currentHeadPos.x == currentTailPos.x ? currentTailPos.x : currentHeadPos.x;

                            currentTailPos = (x: newTailx, y: newTaily);
                            tailPositions.Add(currentTailPos);
                        }
                    }
                }
                else if (direction == "D")
                {
                    int destinationY = currentHeadPos.y - noOfSpaces;
                    for (int j = currentHeadPos.y - 1; j >= destinationY; j--)
                    {
                        currentHeadPos = (x: currentHeadPos.x, y: j);

                        // Only move tail if it's y coord is two spaces different to the head
                        if (Math.Abs(currentHeadPos.y - currentTailPos.y) == 2)
                        {
                            // Move tail towards head
                            int newTaily = currentTailPos.y - 1;

                            // Move diagonally if needed by moving to the same y coord as the head
                            int newTailx = currentHeadPos.x == currentTailPos.x ? currentTailPos.x : currentHeadPos.x;

                            currentTailPos = (x: newTailx, y: newTaily);
                            tailPositions.Add(currentTailPos);
                        }
                    }
                }
            }

            return tailPositions.Distinct().Count().ToString();
        }

        public static string Day8_Part2()
        {
            int[,] grid = Day8_GenerateGrid(out int squareSize);

            int CalcTreeScenicScore(int x, int y, int treeHeight)
            {
                int scenicScore = 1;

                // Look up
                int upScore = 0;
                for (int up = y - 1; up >= 0; up--)
                {
                    upScore++;

                    if (grid[x, up] >= treeHeight)
                    {
                        break;
                    }
                }

                scenicScore *= upScore;

                // Look down
                int downScore = 0;
                for (int down = y + 1; down < squareSize; down++)
                {
                    downScore++;

                    if (grid[x, down] >= treeHeight)
                    {
                        break;
                    }
                }

                scenicScore *= downScore;

                // Look left
                int leftScore = 0;
                for (int left = x - 1; left >= 0; left--)
                {
                    leftScore++;

                    if (grid[left, y] >= treeHeight)
                    {
                        break;
                    }
                }

                scenicScore *= leftScore;

                // Look right
                int rightScore = 0;
                for (int right = x + 1; right < squareSize; right++)
                {
                    rightScore++;

                    if (grid[right, y] >= treeHeight)
                    {
                        break;
                    }
                }

                scenicScore *= rightScore;

                return scenicScore;
            }

            int maxScenicScore = 0;
            for (int x = 0; x < squareSize; x++)
            {
                for (int y = 0; y < squareSize; y++)
                {
                    // Is it an edge tree?
                    if (x == 0 || y == 0 || x == squareSize - 1 || y == squareSize - 1)
                    {
                        // Will have scenic score = 0 so ignore.
                        continue;
                    }
                    else
                    {
                        // Not an edge tree.
                        int treeHeight = grid[x, y];
                        int treeScenicScore = CalcTreeScenicScore(x, y, treeHeight);
                        if (treeScenicScore > maxScenicScore)
                        {
                            maxScenicScore = treeScenicScore;
                        }
                    }
                }
            }

            return maxScenicScore.ToString();
        }

        public static int[,] Day8_GenerateGrid(out int squareSize)
        {
            squareSize = 0;
            int[,] grid = null;
            int lineNo = 0;
            //foreach (string line in Utilities.GetInputFromFile(day: 8, example: true))
            foreach (string line in Utilities.GetInputForDay(8))
            {
                if (lineNo == 0)
                {
                    squareSize = line.Length;
                    grid = new int[squareSize, squareSize];
                }

                for (int i = 0; i < squareSize; i++)
                {
                    grid[i, lineNo] = int.Parse(line[i].ToString());
                }

                lineNo++;
            }

            return grid;
        }

        public static string Day8_Part1()
        {
            int[,] grid = Day8_GenerateGrid(out int squareSize);

            bool IsTreeVisibleFromAnyEdge(int x, int y, int treeHeight)
            {
                for (int left = 0; left < squareSize; left++)
                {
                    if (left == x)
                    {
                        return true;
                    }

                    if (grid[left, y] < treeHeight)
                    {
                        continue;
                    }
                    else
                    {
                        // There is a tree with the same or greater height hiding this tree from the left.
                        break;
                    }
                }

                for (int right = squareSize - 1; right >= 0; right--)
                {
                    if (right == x)
                    {
                        return true;
                    }

                    if (grid[right, y] < treeHeight)
                    {
                        continue;
                    }
                    else
                    {
                        // There is a tree with the same or greater height hiding this tree from the right.
                        break;
                    }
                }

                for (int top = 0; top < squareSize; top++)
                {
                    if (top == y)
                    {
                        return true;
                    }

                    if (grid[x, top] < treeHeight)
                    {
                        continue;
                    }
                    else
                    {
                        // There is a tree with the same or greater height hiding this tree from the top.
                        break;
                    }
                }

                for (int bottom = squareSize - 1; bottom >= 0; bottom--)
                {
                    if (bottom == y)
                    {
                        return true;
                    }

                    if (grid[x, bottom] < treeHeight)
                    {
                        continue;
                    }
                    else
                    {
                        // There is a tree with the same or greater height hiding this tree from the bottom.
                        break;
                    }
                }

                return false;
            }

            int visibleTreeCount = 0;
            for (int x = 0; x < squareSize; x++)
            {
                for (int y = 0; y < squareSize; y++)
                {
                    // Is it an edge tree?
                    if (x == 0 || y == 0 || x == squareSize - 1 || y == squareSize - 1)
                    {
                        visibleTreeCount++;
                    }
                    else
                    {
                        // Not an edge tree.
                        int treeHeight = grid[x, y];
                        if (IsTreeVisibleFromAnyEdge(x, y, treeHeight))
                        {
                            visibleTreeCount++;
                        }
                    }
                }
            }

            return visibleTreeCount.ToString();
        }

        public static string Day7_Part2()
        {
            List<int> dirSizes = new List<int>();
            Stack<Pair<string, int>> dirs = new Stack<Pair<string, int>>();
            string currentDir = null;
            int currentDirSize = 0;

            foreach (string line in Utilities.GetInputForDay(7))
            //foreach (string line in Utilities.GetInputFromFile(day: 7, example: true))
            {
                if (line.StartsWith("$ cd "))
                {
                    if (dirs.Count > 0)
                    {
                        // When changing dir (up or down) set the size of the current dir first to whatever the sum of the file sizes is.
                        dirs.Peek().Item2 = currentDirSize;
                    }

                    if (line.StartsWith("$ cd .."))
                    {
                        ChangeDirUp();

                        currentDirSize = dirs.Peek().Item2;
                    }
                    else
                    {
                        currentDir = line.Substring(5);
                        currentDirSize = 0;
                        dirs.Push(new Pair<string, int>(currentDir, currentDirSize));
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    continue;
                }
                else if (line.StartsWith("dir "))
                {
                    continue;
                }
                else
                {
                    // It's a file with a size.
                    currentDirSize += int.Parse(line.Split(' ')[0]);
                }
            }

            // Pop any remaining dirs off the stack, i.e. go back up to root.
            while (dirs.Any())
            {
                if (dirs.Count > 0)
                {
                    // When changing dir (up or down) set the size of the current dir first to whatever the sum of the file sizes is.
                    dirs.Peek().Item2 = currentDirSize;
                }

                ChangeDirUp();

                if (dirs.Count > 0)
                {
                    currentDirSize = dirs.Peek().Item2;
                }
            }

            int totalSpace = 70000000;
            int requiredSpaceForUpdate = 30000000;
            int totalUsedSpace = dirSizes.Last();
            int minSpaceToFree = requiredSpaceForUpdate - (totalSpace - totalUsedSpace);
            int sizeOfSmallestDirToDelete = dirSizes.OrderBy(x => x)
                                                    .First(x => x >= minSpaceToFree);

            return sizeOfSmallestDirToDelete.ToString();

            void ChangeDirUp()
            {
                // Pop the current directory off the stack.
                dirs.Pop();

                // Add currrent dir size to it's parent directory when going back up to it
                if (dirs.Any())
                {
                    dirs.Peek().Item2 += currentDirSize;
                }

                // Track the size of each directory.
                dirSizes.Add(currentDirSize);
            }
        }

        public static string Day7_Part1()
        {
            int totalSizeOfDirsAtMost100k = 0;
            Stack<Pair<string, int>> dirs = new Stack<Pair<string, int>>();
            string currentDir = null;
            int currentDirSize = 0;

            //foreach (string line in Utilities.GetInputForDay(7))
            foreach (string line in Utilities.GetInputFromFile(day: 7, example: true))
            {
                if (line.StartsWith("$ cd "))
                {
                    if (dirs.Count > 0)
                    {
                        // When changing dir (up or down) set the size of the current dir first to whatever the sum of the file sizes is.
                        dirs.Peek().Item2 = currentDirSize;
                    }

                    if (line.StartsWith("$ cd .."))
                    {
                        ChangeDirUp();

                        currentDirSize = dirs.Peek().Item2;
                    }
                    else
                    {
                        currentDir = line.Substring(5);
                        currentDirSize = 0;
                        dirs.Push(new Pair<string, int>(currentDir, currentDirSize));
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    continue;
                }
                else if (line.StartsWith("dir "))
                {
                    continue;
                }
                else
                {
                    // It's a file with a size.
                    currentDirSize += int.Parse(line.Split(' ')[0]);
                }
            }

            // Pop any remaining dirs off the stack, i.e. go back up to root.
            while (dirs.Any())
            {
                if (dirs.Count > 0)
                {
                    // When changing dir (up or down) set the size of the current dir first to whatever the sum of the file sizes is.
                    dirs.Peek().Item2 = currentDirSize;
                }

                ChangeDirUp();

                if (dirs.Count > 0)
                {
                    currentDirSize = dirs.Peek().Item2;
                }
            }

            return totalSizeOfDirsAtMost100k.ToString();

            void ChangeDirUp()
            {
                // Pop the current directory off the stack.
                dirs.Pop();

                // Add currrent dir size to it's parent directory when going back up to it
                if (dirs.Any())
                {
                    dirs.Peek().Item2 += currentDirSize;
                }

                // If the current dir size (which we just popped off the stack) is small enough, add it to the total count.
                if (currentDirSize <= 100000)
                {
                    totalSizeOfDirsAtMost100k += currentDirSize;
                }
            }
        }

        public static string Day6(int uniqueCharsRequired)
        {
            List<char> marker = new List<char>(uniqueCharsRequired);
            foreach (var line in Utilities.GetInputForDay(6))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char nextChar = line[i];
                    if (marker.Any(m => m == nextChar))
                    {
                        // Duplicate detected
                        int firstDupe = marker.IndexOf(nextChar);
                        marker = marker.GetRange(firstDupe + 1, marker.Count - firstDupe - 1);
                        marker.Add(nextChar);
                        continue;
                    }
                    else
                    {
                        marker.Add(nextChar);
                        if (marker.Count == uniqueCharsRequired)
                        {
                            return (i + 1).ToString();
                        }
                    }
                }
            }

            return "No solution found";
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
                    stacks[to].Push(cratesToMove[j - 1]);
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