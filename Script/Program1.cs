using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenMacroInputComAPI;

namespace OpenMacroInputComTDD
{
  
        public class Program
        {

            static void Main(string[] args)
            {

                ConsoleKeyInfo keyInfo;
                Random r = new Random();

                do
                {
                    Console.WriteLine("Sender press (s) - Receiver press (r) - Boolean set (b) ?");
                    keyInfo = Console.ReadKey();


                    if (keyInfo.Key == ConsoleKey.B)
                    {


                        BooleanRawRegister defaultRegister = new BooleanRawRegister(BooleanRawRegister.Size._4x4);
                        BooleanRawRegisterIPC defaultMemoryPointer = new BooleanRawRegisterIPC(ref defaultRegister, "test");
                        

                        int index = 0;
                        do
                        {


                            Console.WriteLine("Get Save ?");
                            Console.ReadKey();
                            defaultMemoryPointer.ImportRegisterStateFromMemoryValue();
                            Console.WriteLine(">" + defaultMemoryPointer.m_linkedRegisterCompressor.GetIdsCompressed());
                            Console.WriteLine(">" + defaultMemoryPointer.m_linkedRegisterCompressor.GetBooleansCompressed());

                            index += r.Next() % 5;

                            defaultRegister.SetIndexName(index, "T" + index % 1000);
                            defaultRegister.SetIndexValue(index, ! defaultRegister.GetState((uint)index));

                            Console.WriteLine("Check Load ?");
                            Console.ReadKey();
                            defaultMemoryPointer.OverrideMemoryValueWithRegister();
                            Console.WriteLine(">" + defaultMemoryPointer.m_linkedRegisterCompressor.GetIdsCompressed());
                            Console.WriteLine(">" + defaultMemoryPointer.m_linkedRegisterCompressor.GetBooleansCompressed());



                            Console.WriteLine("Continue ?");
                            keyInfo = Console.ReadKey();

                        } while (keyInfo.Key != ConsoleKey.Escape);

                    }

                    if (keyInfo.Key == ConsoleKey.S)
                    {
                        CommandLinesSenderIPC sender = new CommandLinesSenderIPC("CommandLinesStacker");

                        while (true)
                        {
                            //Console.WriteLine("What to send ?");
                            //sender.Add(Console.ReadLine());
                            //sender.PushCommandsWaiting();

                            for (int i = 0; i < 1000; i++)
                            {
                                sender.Add("Ping " + i);
                                Console.WriteLine("Ping " + i);
                                sender.PushCommandsWaiting();

                                Thread.Sleep(10);
                            }

                            Thread.Sleep(10000);

                        }

                    }
                    if (keyInfo.Key == ConsoleKey.R)
                    {

                        CommandLinesReceiverIPC receiver = new CommandLinesReceiverIPC("CommandLinesStacker");
                        while (true)
                        {
                            Queue<string> found = new Queue<string>();
                            receiver.CheckForContent(out found, true);


                            Console.WriteLine(string.Format(">>{0:00}%:{1}", receiver.GetLastCheckSizeInPourcent() * 100.0, receiver.GetLastCheckSize()));
                            Thread.Sleep(1000);
                            while (found.Count > 0)
                            {
                                string s = found.Dequeue();
                                Console.WriteLine(">:" + s);

                            }


                        }

                    }

                    Console.WriteLine("Continue ?");
                    keyInfo = Console.ReadKey();

                } while (keyInfo.Key != ConsoleKey.Escape);
            }
        }






    }

