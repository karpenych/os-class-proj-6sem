using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace os_class_proj
{
    internal class Lab4
    {

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); ++i)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        //------------------------------------------МЕТОД 1------------------------------------------------//

        class ThreadClass1
        {
            [ThreadStatic] static int amount = 0;
            [ThreadStatic] static int ticks = 0;

            // общие разделяемые всеми потоками переменные
            static object lockObj = new();
            static public int totalNumbers = 0;
            static int N = 200000;


            public static void ThreadMethod(object? num)           
            {
                Console.WriteLine("Поток {0} стартовал", num);
                ticks = Environment.TickCount;
                for (int i = 0; i < N; ++i) 
                {
                    if (IsPrime(i))
                    {
                        lock (lockObj)
                            ++totalNumbers;
                        ++amount;
                    }
                }
                Console.WriteLine("Поток {0} завершился - найдено чисел: {1}, время: {2}", num, amount, Environment.TickCount - ticks);
            }
        }

        public class ThreadClass1Test 
        {
            public static void Test() 
            {
                Console.WriteLine("Метод 1\n");

                int startTick = Environment.TickCount;

                for (int i = 1; i < 11; ++i)
                {
                    Thread th = new(new ParameterizedThreadStart(ThreadClass1.ThreadMethod));
                    th.Start(i);
                }
                
                Thread.Sleep(1000);
                Console.WriteLine("\nОбщее время {0}. Всего найдено чисел {1:N0}\n", Environment.TickCount - startTick, ThreadClass1.totalNumbers);
            }
        }

        //------------------------------------------МЕТОД 2------------------------------------------------//


        public class ThreadClass2
        {
            public static ThreadLocal<double> amount = new(() => 0, trackAllValues: true);
            public static ThreadLocal<double> ticks = new(() => 0, trackAllValues: true);

            // общие разделяемые всеми потоками переменные
            static object lockObj = new();
            static public int totalNumbers = 0;
            static int N = 200000;


            public static void ThreadMethod(object? num)
            {
                Console.WriteLine("Поток {0} стартовал", num);
                ticks.Value = Environment.TickCount;
                for (int i = 0; i < N; ++i)
                {
                    if (IsPrime(i))
                    {
                        lock (lockObj)
                            ++totalNumbers;
                        ++amount.Value;
                    }
                }
                ticks.Value = Environment.TickCount - ticks.Value;
            }
        }

        public class ThreadClass2Test
        {
            public static void Test()
            {
                Console.WriteLine("Метод 2\n");

                int startTick = Environment.TickCount;

                Thread[] threads = new Thread[10];

                for (int i = 1; i < 11; ++i)
                {
                    threads[i - 1] = new(new ParameterizedThreadStart(ThreadClass2.ThreadMethod));
                    threads[i - 1].Start(i);
                }

                Thread.Sleep(1000);
                Console.WriteLine("\nОбщее время {0}. Всего найдено чисел {1:N0}\n", Environment.TickCount - startTick, ThreadClass2.totalNumbers);

                for (int j = 0; j < 10; j++)
                {
                    Console.WriteLine($"Поток {j + 1} завершился - найдено чисел: {ThreadClass2.amount.Values[j]}, время: {ThreadClass2.ticks.Values[j]}");
                }

            }
        }

        //------------------------------------------МЕТОД 3------------------------------------------------//

        public class ThreadClass3 
        {
            static LocalDataStoreSlot localSlot_amount = Thread.AllocateDataSlot();
            static LocalDataStoreSlot localSlot_ticks = Thread.AllocateDataSlot();

            static int Ticks
            {
                get
                {
                    object data = Thread.GetData(localSlot_ticks);
                    return Convert.ToInt32(data);
                }
                set
                {
                    Thread.SetData(localSlot_ticks, value);
                }
            }

            static int Amount
            {
                get
                {
                    object data = Thread.GetData(localSlot_amount);
                    return Convert.ToInt32(data);
                }
                set
                {
                    Thread.SetData(localSlot_amount, value);
                }
            }

            // общие разделяемые всеми потоками переменные
            static object lockObj = new();
            static public int totalNumbers = 0;
            static int N = 200000;


            public static void ThreadMethod(object? num)
            {
                Console.WriteLine("Поток {0} стартовал", num);
                Ticks = Environment.TickCount;
                for (int i = 0; i < N; ++i)
                {
                    if (IsPrime(i))
                    {
                        lock (lockObj)
                            ++totalNumbers;
                        ++Amount;
                    }
                }
                Console.WriteLine($"Поток {num} завершился - найдено чисел: {Amount}, время: {Environment.TickCount - Ticks}");
            }
        }

        public class ThreadClass3Test
        {
            public static void Test()
            {
                Console.WriteLine("Метод 3\n");
                int startTick = Environment.TickCount;

                for (int i = 1; i < 11; ++i)
                {
                    Thread th = new(new ParameterizedThreadStart(ThreadClass3.ThreadMethod));
                    th.Start(i);
                }

                Thread.Sleep(1000);
                Console.WriteLine("\nОбщее время  {0}. Всего найдено чисел {1:N0}\n", Environment.TickCount - startTick, ThreadClass3.totalNumbers);
            }
        }
    }
}
