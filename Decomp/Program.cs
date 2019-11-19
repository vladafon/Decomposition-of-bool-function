using System;
using System.Collections.Generic;
using System.Linq;

namespace Decomp
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Console.WriteLine("Аргументы функции при 0");
            string zeros = Console.ReadLine();
            Console.WriteLine("Аргументы функции при 1");
            string ones = Console.ReadLine();

            string[] s0 = zeros.Split(' '); //decimal format
            string[] s1 = ones.Split(' ');

            string [] b0 = new string[s0.Length]; //binary format
            string[] b1 = new string[s1.Length];

            Console.WriteLine(new string('_',10));
            Console.WriteLine("В 0:");

            for (int i = 0; i < s0.Length; i++)
            {
                b0[i] = BinaryConverter(Convert.ToInt32(s0[i]));

                Console.WriteLine(s0[i]+"="+ b0[i]);
            }

            Console.WriteLine(new string('_', 10));
            Console.WriteLine("В 1:");

            for (int i = 0; i < s1.Length; i++)
            {
                b1[i] = BinaryConverter(Convert.ToInt32(s1[i]));
                Console.WriteLine(s1[i] + "=" + b1[i]);
            }

            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Матрица Q");
            //forming matrix with hemming 2 & 3

            List<string> Q = new List<string>();

            foreach (var zero in b0)
            {
                foreach (var one in b1)
                {
                    int h = 0;
                    string element = String.Empty;

                    for (int i = 0; i < 5; i++)
                    {
                        if (zero[i] != one[i])
                        {
                            element += "1";
                            h++;
                        }
                        else
                        {
                            element += "0";
                        }
                    }

                    if (h == 2 || h == 3)
                    {
                        Console.WriteLine(element+"  __  "+Convert.ToInt32(zero,2)+","+Convert.ToInt32(one,2));

                        Q.Add(element);
                    }

                }
            }


            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Матрица F = Q^T * Q");
            //forming matrix F

            int [,] F =
            {
                {Transponing(Q,0,0),Transponing(Q,0,1), Transponing(Q,0,2), Transponing(Q,0,3), Transponing(Q,0,4)},
                {Transponing(Q,1,0),Transponing(Q,1,1), Transponing(Q,1,2), Transponing(Q,1,3), Transponing(Q,1,4)},
                {Transponing(Q,2,0),Transponing(Q,2,1), Transponing(Q,2,2), Transponing(Q,2,3), Transponing(Q,2,4)},
                {Transponing(Q,3,0),Transponing(Q,3,1), Transponing(Q,3,2), Transponing(Q,3,3), Transponing(Q,3,4)},
                {Transponing(Q,4,0),Transponing(Q,4,1), Transponing(Q,4,2), Transponing(Q,4,3), Transponing(Q,4,4)}
            };

            for (int i = 0; i < F.GetLength(0); i++)
            {
                for (int j = 0; j < F.GetLength(1);j++)
                {
                    Console.Write(F[i,j]+" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Матрица производных");
            //forming matrix diff

            double[,] D =
            {
                {0,Differentator(F,0,1), Differentator(F,0,2), Differentator(F,0,3), Differentator(F,0,4)},
                {Differentator(F,1,0),0, Differentator(F,1,2), Differentator(F,1,3), Differentator(F,1,4)},
                {Differentator(F,2,0),Differentator(F,2,1), 0, Differentator(F,2,3), Differentator(F,2,4)},
                {Differentator(F,3,0),Differentator(F,3,1), Differentator(F,3,2), 0, Differentator(F,3,4)},
                {Differentator(F,4,0),Differentator(F,4,1), Differentator(F,4,2), Differentator(F,4,3), 0}

            };

            for (int i = 0; i < D.GetLength(0); i++)
            {
                for (int j = 0; j < D.GetLength(1); j++)
                {
                    Console.Write(D[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Разбиения:");

            string rasb = String.Empty;
            string userRasb = String.Empty;
            double min = double.MaxValue;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (j <= i) continue;

                    int a = Others(i, j)[0];
                    int b = Others(i, j)[1];
                    int c = Others(i, j)[2];
                    double result = D[i, j] + D[a, b] + D[b, c] + D[a, c];
                    Console.WriteLine($"({i+1},{j+1})&({a+1},{b+1},{c+1}) = {result}");

                    if (result < min)
                    {
                        min = result;
                        rasb = $"{i},{j};{a},{b},{c}";
                        userRasb = $"({i + 1},{j + 1})&({a + 1},{b + 1},{c + 1}) = {result}";
                    }
                }
            }

            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Самое лучшее разбиение: "+ userRasb);

            //graph building
            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Граф:");

            int x0 = Convert.ToInt32(rasb.Split(';')[0].Split(',')[0]);
            int x1 = Convert.ToInt32(rasb.Split(';')[0].Split(',')[1]);
            int y0 = Convert.ToInt32(rasb.Split(';')[1].Split(',')[0]);
            int y1 = Convert.ToInt32(rasb.Split(';')[1].Split(',')[1]);
            int y2 = Convert.ToInt32(rasb.Split(';')[1].Split(',')[2]);

            //creating graph array

   
            List<string> graph = new List<string>();

            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 7; y++)
                {
                    string xb = BinaryConverter(x, 2);
                    string yb = BinaryConverter(y, 3);

                    char[] numberArray = new char[5];

                    numberArray[x0] = xb[0];
                    numberArray[x1] = xb[1];
                    numberArray[y0] = yb[0];
                    numberArray[y1] = yb[1];
                    numberArray[y2] = yb[2];

                    string number = new string(numberArray);

                    if (b0.Contains(number))
                    {
                        graph.Add($"{xb} + {yb}");
                        Console.WriteLine($"{xb} -- {yb} : линия с черточкой");
                    }
                    else if (b1.Contains(number))
                    {
                        graph.Add($"{xb} - {yb}");
                        Console.WriteLine($"{xb} -- {yb} : линия");
                    }

                }
            }


            //graph controversial 3
            Console.WriteLine(new string('_', 10));
            Console.WriteLine("Граф противоречивости:");

            for (int i = 0; i < graph.Count; i++)
            {
                for (int j = 0; j < graph.Count; j++)
                {
                    if (j <= i) continue;

                    string[] elements1 = graph[i].Split(' ');
                    string[] elements2 = graph[j].Split(' ');

                    if (elements1[0] != elements2[0]) continue;

                    if ((elements1[1] == "+" && elements2[1] == "-") || (elements1[1] == "-" && elements2[1] == "+"))
                        Console.WriteLine($"{elements1[2]} -- {elements2[2]} - линия");
                }
            }

            Console.ReadKey();
        }

        private static string BinaryConverter(int a, int bas = 5)
        {

            string result = Convert.ToString(a, 2);

            if (result.Length == bas)
                return result;

            return new string('0',bas-result.Length)+result;

        }

        private static int Transponing(List<string> Q, int index1, int index2)
        {
            int count = 0;
            foreach (var item in Q)
            {
                if (item[index1] == '1' && item[index2] == '1')
                    count++;
            }

            return count;
        }

        private static double Differentator(int[,] F, int i, int j)
        {
            double result = (F[i, j]*1.0) / (F[i, i] - 2 * F[i, j] + F[j, j]);
            return Math.Round(result,2);
        }

        private static int[] Others(int i, int j)
        {
            if (i == 0 && j == 1)
                return new int[]{2,3,4};
            else if (i == 0 && j == 2)
                return new int[] { 1, 3, 4 };
            else if (i == 0 && j == 3)
                return new int[] { 1, 2, 4 };
            else if (i == 0 && j == 4)
                return new int[] { 1, 2, 3 };
            else if (i == 1 && j == 2)
                return new int[] { 0, 3, 4 };
            else if (i == 1 && j == 3)
                return new int[] { 0, 2, 4 };
            else if (i == 1 && j == 4)
                return new int[] { 0, 2, 3 };
            else if (i == 2 && j == 3)
                return new int[] { 0, 1, 4 };
            else if (i == 2 && j == 4)
                return new int[] { 0, 1, 3 };
            else if (i == 3 && j == 4)
                return new int[] { 0, 1, 2 };
            else return new int[] { 0, 1, 2 };
        }

    }
}
