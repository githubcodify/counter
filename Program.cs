using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp22
{
    internal class Program
    {
        static CancellationTokenSource cancelToken1 = new CancellationTokenSource();
        static CancellationTokenSource cancelToken2 = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Task task1 = new Task(() =>
            {
                int c1 = 0;
                while (true)
                {
                    c1++;
                    Console.WriteLine("c1 = " + c1);

                    if (c1 == 100000)
                    {
                        Task task2 = new Task(() =>
                        {
                            int c2 = 0;
                            while (true)
                            {
                                c2++;
                                Console.WriteLine("\t\tc2 = " + c2);
                                if (cancelToken2.IsCancellationRequested)
                                {
                                    break;
                                }
                            }
                        }, cancelToken2.Token);

                        task2.Start();
                    }

                    if (c1 == 150000)
                    {
                        cancelToken2.Cancel();
                    }

                    if (c1 == 200000)
                    {
                        cancelToken1.Cancel();
                        if (cancelToken1.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }, cancelToken1.Token);

            task1.Start();
            Console.ReadLine();
        }
    }
}
