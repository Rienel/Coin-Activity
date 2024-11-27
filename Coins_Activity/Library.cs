using System.Collections.Generic;
using System.Drawing;

namespace Coins_Activity
{
    internal class Library
    {
        public Bitmap Helper(Bitmap original)
        {
            Bitmap processed = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(processed))
            {
                g.DrawImage(original, new Rectangle(0, 0, processed.Width, processed.Height));
            }

            Binary(ref original, ref processed, 200);
            return processed;
        }

        private void Binary(ref Bitmap original, ref Bitmap processed, int threshold)
        {
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int gray = (pixel.R + pixel.G + pixel.B) / 3;
                    Color newColor = gray < threshold ? Color.Black : Color.White;
                    processed.SetPixel(x, y, newColor);
                }
            }
        }

        public Dictionary<string, double> CoinCounter(Bitmap image)
        {
            var objects = DetectCoins(image);
            var coins = new Dictionary<string, double>
            {
                { "5 Peso", 0 },
                { "1 Peso", 0 },
                { "25 Centavo", 0 },
                { "10 Centavo", 0 },
                { "5 Centavo", 0 },
                { "Value", 0 }
            };

            foreach (var coinPoints in objects)
            {
                int size = coinPoints.Count;

                if (size > 18001)
                {
                    coins["5 Peso"]++;
                    coins["Value"] += 5;
                }
                else if (size > 15001)
                {
                    coins["1 Peso"]++;
                    coins["Value"] += 1;
                }
                else if (size > 11001)
                {
                    coins["25 Centavo"]++;
                    coins["Value"] += 0.25;
                }
                else if (size > 8001)
                {
                    coins["10 Centavo"]++;
                    coins["Value"] += 0.10;
                }
                else if (size > 6500)
                {
                    coins["5 Centavo"]++;
                    coins["Value"] += 0.05;
                }
            }

            return coins;
        }

        private List<List<Point>> DetectCoins(Bitmap image)
        {
            List<List<Point>> objects = new List<List<Point>>();
            bool[,] visited = new bool[image.Width, image.Height];
            int blackThreshold = 20;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (image.GetPixel(x, y).R == 0 && !visited[x, y])
                    {
                        List<Point> coinPoints = new List<Point>();
                        BLOBS(image, x, y, visited, blackThreshold, coinPoints);
                        if (coinPoints.Count > 0)
                        {
                            objects.Add(coinPoints);
                        }
                    }
                }
            }

            return objects;
        }

        private void BLOBS(Bitmap image, int startX, int startY, bool[,] visited, int threshold, List<Point> contour)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(startX, startY));
            visited[startX, startY] = true;

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                contour.Add(p);

                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int nx = p.X + dx;
                        int ny = p.Y + dy;

                        if (nx >= 0 && ny >= 0 && nx < image.Width && ny < image.Height && !visited[nx, ny])
                        {
                            Color neighborColor = image.GetPixel(nx, ny);
                            if (neighborColor.R < threshold)
                            {
                                visited[nx, ny] = true;
                                stack.Push(new Point(nx, ny));
                            }
                        }
                    }
                }
            }
        }
    }
}