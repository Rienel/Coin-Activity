using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coins_Activity
{
    internal class Library
    {
        public static void GetCoinPixels(
            Bitmap processed,
            ref int totalCount,
            ref float totalValue,
            ref int peso5Count,
            ref int peso1Count,
            ref int cent25Count,
            ref int cent10Count,
            ref int cent5Count)
        {
            List<Rectangle> detectedCoins = DetectCoins(processed);

            DebugPrintCoinSizes(detectedCoins);

            foreach (var coin in detectedCoins)
            {
                float coinSize = coin.Width * coin.Height;

                if (IsFivePeso(coinSize))
                {
                    Console.WriteLine($"5 Peso detected, size: {coinSize}");
                    peso5Count++;
                    totalValue += 5.0f;
                }
                else if (IsOnePeso(coinSize))
                {
                    Console.WriteLine($"1 Peso detected, size: {coinSize}");
                    peso1Count++;
                    totalValue += 1.0f;
                }
                else if (IsTwentyFiveCentavo(coinSize))
                {
                    Console.WriteLine($"25 Centavo detected, size: {coinSize}");
                    cent25Count++;
                    totalValue += 0.25f;
                }
                else if (IsTenCentavo(coinSize))
                {
                    Console.WriteLine($"10 Centavo detected, size: {coinSize}");
                    cent10Count++;
                    totalValue += 0.10f;
                }
                else if (IsFiveCentavo(coinSize))
                {
                    Console.WriteLine($"5 Centavo detected, size: {coinSize}");
                    cent5Count++;
                    totalValue += 0.05f;
                }
                else
                {
                    Console.WriteLine($"Unidentified coin, size: {coinSize}");
                }
            }

            totalCount = peso5Count + peso1Count + cent25Count + cent10Count + cent5Count;
        }

        private static List<Rectangle> DetectCoins(Bitmap img)
        {
            List<Rectangle> coinBoundingBoxes = new List<Rectangle>();

            Grayscale grayscaleFilter = new Grayscale(0.3, 0.59, 0.11);
            Bitmap grayImage = grayscaleFilter.Apply(img);

            GaussianBlur blurFilter = new GaussianBlur(2, 7);
            Bitmap blurredImage = blurFilter.Apply(grayImage);

            CannyEdgeDetector cannyFilter = new CannyEdgeDetector(100, 200);
            Bitmap edgeImage = cannyFilter.Apply(blurredImage);

            bool[,] visited = new bool[edgeImage.Width, edgeImage.Height];

            for (int y = 1; y < edgeImage.Height - 1; y++)
            {
                for (int x = 1; x < edgeImage.Width - 1; x++)
                {
                    if (edgeImage.GetPixel(x, y).R == 0 && !visited[x, y])
                    {
                        List<Point> blob = FloodFill(edgeImage, x, y, visited);

                        if (blob.Count > 100)
                        {
                            int minX = blob.Min(p => p.X);
                            int maxX = blob.Max(p => p.X);
                            int minY = blob.Min(p => p.Y);
                            int maxY = blob.Max(p => p.Y);

                            coinBoundingBoxes.Add(new Rectangle(minX, minY, maxX - minX, maxY - minY));
                        }
                    }
                }
            }

            return coinBoundingBoxes;
        }

        private static void DebugPrintCoinSizes(List<Rectangle> detectedCoins)
        {
            foreach (var coin in detectedCoins)
            {
                float coinSize = coin.Width * coin.Height;
                Console.WriteLine($"Detected coin size: {coinSize}");
            }
        }

        private static List<Point> FloodFill(Bitmap img, int startX, int startY, bool[,] visited)
        {
            List<Point> points = new List<Point>();
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(new Point(startX, startY));

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                if (p.X >= 0 && p.X < img.Width && p.Y >= 0 && p.Y < img.Height &&
                    img.GetPixel(p.X, p.Y).R == 0 && !visited[p.X, p.Y])
                {
                    visited[p.X, p.Y] = true;
                    points.Add(p);

                    queue.Enqueue(new Point(p.X + 1, p.Y));
                    queue.Enqueue(new Point(p.X - 1, p.Y));
                    queue.Enqueue(new Point(p.X, p.Y + 1));
                    queue.Enqueue(new Point(p.X, p.Y - 1));
                }
            }

            return points;
        }

        private static bool IsFivePeso(float size)
        {
            return size > 12000 && size <= 15000;
        }

        private static bool IsOnePeso(float size)
        {
            return size > 8000 && size <= 12000;
        }

        private static bool IsTwentyFiveCentavo(float size)
        {
            return size > 5000 && size <= 8000;
        }

        private static bool IsTenCentavo(float size)
        {
            return size > 3000 && size <= 5000;
        }

        private static bool IsFiveCentavo(float size)
        {
            return size > 1500 && size <= 3000;
        }
    }
}
