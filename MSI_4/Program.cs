using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_4
{
    class Program
    {
        public const int N = 25;
        public const int U = 10;
        public const double p = L / U;
        public const double L = 2.5;
        public readonly double[] Lambdas = new double[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5 };

        static void Main(string[] args)
        {
            //Wszelkie wywołania funkcji

        }
        // funkcja obliczająca prawdopodobieństwa stanu systemu w zależności od parametru Lambda
        public double[] systemStateProbability(double Lambda)
        {
            double[] qi = new double[N + 1];
            double p = Lambda / U;
            for (int i = 0; i < N + 1; i++)
            {
                qi[i] = silnia(N) / (silnia(N - i) * Math.Pow(p, i));
            }
            double p0 = 0;
            for (int i = 0; i < N + 1; i++)
            {
                p0 = p0 + qi[i];
            }
            double[] pi = new double[N + 1];
            for (int i = 0; i < N + 1; i++)
            {
                pi[i] = qi[i] * (1 / p0);
            }
            return pi;
        }

        //funkcja obliczająca srednia liczbe zgloszen na stanowisku obsługi l
        public double[] averageNumberInPlace()
        {
            double[] number = new double[Lambdas.Length];
            double[] qi = new double[N + 1];
            double sum = 0;
            for(int i=0;i<Lambdas.Count();i++)
            {
                double p = Lambdas[i] / U;
                for (int j = 0; j < N + 1; j++)
                {
                    qi[j] = silnia(N) / (silnia(N - j) * Math.Pow(p, j));
                    sum = sum + qi[j];
                }
                number[i] = 1/sum;
                sum = 0;
            }
            //Obliczenie sredniej liczby zadan na stanowisku obsługi w zalezności od lambda
            for(int i=0;i<number.Length;i++)
            {
                number[i] = 1 - number[i];
            }
            return number;
        }
        //funkcja obliczająca srednia liczbe zadan w systemie n
        public double[] averageNumberInSystem()
        {
            double[] number = new double[Lambdas.Length];
            ArrayList pi = new ArrayList();

            // prawdopodobieństwa stanów systemu dla każdego lambda
            foreach(double l in Lambdas)
            {
                pi.Add(systemStateProbability(l));
            }
            double sum = 0;
            double m = 0;
            //srednia liczba zadan w systemie
            for(int i=0;i<pi.Count;i++)
            {
                sum = 0;
                double[] p = (double[])pi[i];
                for(int j=0;j<p.Length;j++)
                {
                    m = p[j] * j;
                    sum = sum + m;
                }
                number[i] = sum;
            }
            
            return number;
        }

        //funckja obliczająca srednia liczbe zadan w kolejce
        public double[] averageNumberInQueue()
        {
            double[] number = new double[Lambdas.Length];
            for(int i=0;i<number.Length;i++)
            {
                number[i] = averageNumberInSystem()[i] - averageNumberInPlace()[i];
            }
            return number;
        }

        //sredni czas oczekiwania w kolejce
        public double[] averageWaitTime()
        {
            double[] wait = new double[Lambdas.Length];
            int i = 0;
            foreach (double p in calculatepI())
            {
                wait[i] = (p * (1 + Math.Pow(p, M) * (M * p - (M + 1)))) / (U * (1 - p) * (1 - Math.Pow(p, M + 2)));
                i++;
            }
            return wait;
        }

        public double[] calculatepI()
        {
            double[] p = new double[Lambdas.Length];
            int i = 0;
            foreach (double l in Lambdas)
            {
                p[i] = l / U;
                i++;
            }
            return p;
        }

        static double silnia(int k)
        {
            int wynik = 1;
            for (int i = k; i > 1; i--)
            {
                wynik = wynik * i;
            }
            return wynik;
        }
    }
}
