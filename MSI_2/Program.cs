using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_2
{
    class Program
    {
        public const int M = 12;
        public const int U = 5;
        public const double p = L / U;
        public const double L = 4.8;
        public readonly double[] Lambdas = new double[] {2,4,6,8,10,12,14};

        static void Main(string[] args)
        {
            //Wszelkie wywołania funkcji

        }
        // funkcja obliczająca prawdopodobieństwa stanu systemu
        public double[] systemStateProbability()
        {
            double[] pi = new double[M + 1];
            for(int i=0;i<M+2;i++)
            {
                pi[i] = Math.Pow(p, i) * (1 - p) / (1 - Math.Pow(p, M + 2));
            }
            return pi;
        }

        // Funkcja obliczająca prawdopodobieństwa straty zgłoszeń
        public double[] looseProbability()
        {
            double[] looses = new double[Lambdas.Length];
            int i = 0;
            foreach(double p in calculatepI())
            {
                looses[i] = (Math.Pow(p, M + 1) * (1 - p)) / (1 - Math.Pow(p, M + 2));
                i++;
            }
            return looses;
        }
        //funkcja obliczająca srednia liczbe zgloszen na stanowisku obsługi
        public double[] averageNumberInPlace()
        {
            double[] number = new double[Lambdas.Length];
            int i = 0;
            foreach(double p in calculatepI())
            {
                number[i] = (p * (1 - Math.Pow(p, M + 1))) / (1 - Math.Pow(p, M + 2));
                i++;
            }
            return number;
        }
        //funkcja obliczająca srednia liczbe zadan w systemie
        public double[] averageNumberInSystem()
        {
            double[] number = new double[Lambdas.Length];
            int i = 0;
            foreach (double p in calculatepI())
            {
                number[i] = (Math.Pow(p, 2) * (1 + Math.Pow(p, M) * ((M * p) - (M + 1)))) / ((1 - Math.Pow(p, M + 2)) * (1 - p));
                i++;
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
                wait[i] =  (p*(1+Math.Pow(p,M)*(M*p-(M+1))))/(U*(1-p)*(1-Math.Pow(p,M+2)));
                i++;
            }
            return wait;
        }

        public double[] calculatepI()
        {
            double[] p = new double[Lambdas.Length];
            int i = 0;
            foreach(double l in Lambdas)
            {
                p[i] = l / U;
                i++;
            }
            return p;
        }
    }
}
