using minesweeper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using System.Reflection.Emit;

namespace ConsoleApp1
{

    static class Global_vars
    {
        public static List<int> boardsize = new() { 8, 8 }; // y, x
        public static int fieldsclear = 0;
        public static List<List<int>> fields_opened = new();
        public static List<List<int>> flaglist = new();
        public static List<List<int>> nums = new();
        public static List<ConsoleColor> colors = new()
        {
            ConsoleColor.Gray,
            ConsoleColor.Blue,
            ConsoleColor.Cyan,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Magenta,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
        };
        public static Dictionary<string, dynamic> jsontransfer = new();
    }
    public class Keyboardhandler
    {
    }
    internal class Program
    {

        static int Accseslist(List<List<int>> list2check, int x, int y)
        {
            int counter = 0;
            try
            {
                if (list2check[y][x] == 1)
                {
                    counter = 1;
                }

            }
            catch
            {

            }
            return counter;
        }
        static string Checkpos(List<List<int>> list2check, int x, int y)
        {
            int counter = 0;

             if (list2check[y][x] == 1)
            {
                return "M";
            }

            counter += Accseslist(list2check, x - 1, y + 1);
            counter += Accseslist(list2check, x, y + 1);
            counter += Accseslist(list2check, x + 1, y + 1);
            counter += Accseslist(list2check, x - 1, y);
            counter += Accseslist(list2check, x, y);
            counter += Accseslist(list2check, x + 1, y);
            counter += Accseslist(list2check, x - 1, y - 1);
            counter += Accseslist(list2check, x, y - 1);
            counter += Accseslist(list2check, x + 1, y - 1);

            string count = counter.ToString();
            return count;
        }

        static List<List<int>> MoveAndPrint(List<List<int>> coordinatelist, int x, int y)
        {
            List<List<int>> ints = new();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        string tempstorage = Checkpos(coordinatelist, x - j, y - i);
                        if (Int32.Parse(tempstorage) == 0)
                        {
                            List<int> templist = new()
                            {
                                x - j,
                                y - i
                            };
                            ints.Add(new List<int>(templist));
                        }
                        Console.SetCursorPosition((x - j) * 2, y - i);
                        Console.ForegroundColor = Global_vars.colors[Int32.Parse(tempstorage)];
                        Global_vars.nums[x - j][y - i] = Int32.Parse(tempstorage);
                        Console.Write(tempstorage);
                        Global_vars.fields_opened[y - i][x - j] = 1;
                    }
                    catch
                    {

                    }

                }
            }
            return ints;
        }

        static bool Open0(List<List<int>> coordinatelist, int x, int y)
        {
            List<List<int>> checkedlist = new();
            int o_x = x; int o_y = y;
            List<List<int>> list2check0 = new();
            if (coordinatelist[y][x] == 0)
            {
                list2check0 = MoveAndPrint(coordinatelist, x, y);
                List<int> templist = new() { o_x, o_y };
                checkedlist.Add(new List<int>(templist));

            }
        check40:
            List<List<int>> list2check0temp = new();

            if (list2check0.Count != 0)
            {
                foreach (List<int> item in list2check0)
                {
                    List<int> templist = new();
                    int found = 0;

                    foreach (List<int> checkitem in checkedlist)
                    {
                        if (checkitem[0] == item[0] && checkitem[1] == item[1])
                        {
                            found = 1;
                            goto skip_foreach;
                        }

                    }
                    if (found == 0)
                    {
                        checkedlist.Add(new List<int> { item[0], item[1] });
                    }

                    List<List<int>> list2check0temp2 = MoveAndPrint(coordinatelist, item[0], item[1]);
                    foreach (List<int> ints in list2check0temp2)
                    {
                        list2check0temp.Add(ints);
                    }
                skip_foreach:
                    int skip = 0;
                }
            }

            if (list2check0temp.Count != 0)
            {
                list2check0 = list2check0temp;
                list2check0temp.Clear();
                goto check40;
            }
            return true;
        }
        static void Checklist(List<List<int>> mainlist)
        {
            for (int y1 = 0; y1 < mainlist.Count(); y1++)
            {
                for (int x1 = 0; x1 < mainlist[y1].Count(); x1++)
                {
                    int count = 0;
                    if (mainlist[y1][x1] == 1)
                    {

                        Console.Write("M");
                        Console.Write(" ");
                    }
                    else
                    {
                        count += Accseslist(mainlist, x1 - 1, y1 + 1);
                        count += Accseslist(mainlist, x1, y1 + 1);
                        count += Accseslist(mainlist, x1 + 1, y1 + 1);
                        count += Accseslist(mainlist, x1 - 1, y1);
                        count += Accseslist(mainlist, x1, y1);
                        count += Accseslist(mainlist, x1 + 1, y1);
                        count += Accseslist(mainlist, x1 - 1, y1 - 1);
                        count += Accseslist(mainlist, x1, y1 - 1);
                        count += Accseslist(mainlist, x1 + 1, y1 - 1);

                        Console.Write(count);
                        Console.Write(" ");
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();

            }
        }
        static bool CheckEnd()
        {
            Global_vars.fieldsclear = 0;

            foreach (List<int> sublist in Global_vars.fields_opened)
            {
                foreach (int value in sublist)
                {
                    if (value == 1)
                    {
                        Global_vars.fieldsclear++;
                    }
                }
            }
            foreach (List<int> sublist in Global_vars.flaglist)
            {
                foreach (int value in sublist)
                {
                    if (value == 1)
                    {
                        Global_vars.fieldsclear++;
                    }
                }
            }
            if (Global_vars.fieldsclear / (Global_vars.boardsize[0] * Global_vars.boardsize[1]) == 1)
            {
                Console.SetCursorPosition(0, Global_vars.boardsize[1] + 3);
                Console.WriteLine("winner winner chicken dinner");
                return true;
            }
            return false;
        }

        static (bool, int xF, int yF) Openaround(List<List<int>> coordinatelist, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (Global_vars.fields_opened[y - i][x - j] == 0)
                        {

                            string tempstring = Checkpos(coordinatelist, x - j, y - i);
                            if (tempstring == "M" && Global_vars.flaglist[y - i][x - j] == 0)
                            {
                                return (true, x - j, y - i);
                            }
                            else if (tempstring != "M" && Global_vars.flaglist[y - i][x - j] == 0)
                            {
                                Console.SetCursorPosition((x - j) * 2, y - i);
                                Console.ForegroundColor = Global_vars.colors[Int32.Parse(tempstring)];

                                Console.Write(tempstring);
                                Global_vars.fields_opened[y - i][x - j] = 1;
                            }
                        }
                    }
                    catch { }
                }
            }

            return (false, x, y);
        }
        public class ResFile
        {
            public string Hash { set; get; }
            public int Size { set; get; }
        }

        public class ResRoot
        {
            public Dictionary<string, ResFile> Files { set; get; }
        }

        public class Jsontest
        {
            public int Id { set; get; }
            public dynamic Val { set; get; }
            public List<int> Boardsize { set; get; }
            public List<List<int>> Flaglist { set; get; }
            public List<List<int>> Fields_opened { set; get; }
            public List<List<int>> CoordinateList { set; get; }
            public List<List<int>> nums { set; get; }

        }

        static bool CheckAction(string Recived)
        {
            return true;
        }

        static bool ipc_update(BinaryReader br, BinaryWriter bw)
        {

            Jsontest jsontest = new()
            {
                Id = 1,
                Val = Global_vars.jsontransfer["seed"],
                Boardsize = Global_vars.jsontransfer["boardsize"],
                Flaglist = Global_vars.flaglist,
                Fields_opened = Global_vars.fields_opened,
                CoordinateList = Global_vars.jsontransfer["cnl"],
            };

            while(true)
            {
                try
                {

                    var len = (int)br.ReadUInt32();            // Read string length
                    var str = new string(br.ReadChars(len));    // Read string

                    //use functions
                    string tempstr = str.Substring(3);
                    if (Int32.TryParse(tempstr, out int xy))
                    {
                        //TODO: find a way to get from the index of the button to the xy position
                        int x = (xy) % Global_vars.boardsize[1];
                        int y = (xy) / Global_vars.boardsize[1];
                        Buttonpress(x, y);
                    }

                    string temp = JsonConvert.SerializeObject(jsontest);
                    temp = "    " + temp;
                    var buf = Encoding.ASCII.GetBytes(temp);     // Get ASCII byte array
                    bw.Write(buf);                              // Write string

                    break;
                }
                catch (Exception ex){}
            }



            return true;
        }
        static bool End()
        {
            Environment.Exit(0);
            return true;
        }

        static bool Endwhile()
        {
            End();
            return true;
        }

        static bool Buttonpress(int x, int y)
        {
            string current_pos = Checkpos(Global_vars.jsontransfer["cnl"], x, y);

            if (current_pos == "0")
            {
                Open0(Global_vars.jsontransfer["cnl"], x, y);
            }
            else if (Global_vars.fields_opened[y][x] == 1)
            {
                var temp = Openaround(Global_vars.jsontransfer["cnl"], x, y);
                if (temp.Item1)
                {

                    Console.SetCursorPosition(temp.xF * 2, temp.yF);
                    Console.Write("M");
                    Endwhile();
                }
            }
            else if (Global_vars.flaglist[y][x] == 1)
            {
                End();
            }
            else if (current_pos == "M")
            {
                for (int i = 0; i < Global_vars.jsontransfer["cnl"].Count; i++)
                {
                    for (int j = 0; j < Global_vars.jsontransfer["cnl"][i].Count; j++)
                    {
                        if (Global_vars.jsontransfer["cnl"][i][j] == 1)
                        {
                            Console.SetCursorPosition(j * 2, i);
                            Console.Write("M");
                        }
                    }
                }

                Endwhile();
            }
            else
            {
                Global_vars.fields_opened[y][x] = 1;

                if (CheckEnd())
                {
                    Endwhile();
                }

            }
            return true;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var server = new NamedPipeServerStream("NPtest", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();

            Console.WriteLine("Connected.");
            var br = new BinaryReader(server);
            var bw = new BinaryWriter(server);
            string x = "12345";

        Reset:
            Random rnd = new();
            Global_vars.boardsize[0] = rnd.Next(8, 20);
            Global_vars.boardsize[1] = rnd.Next(8, 20);
            Global_vars.jsontransfer.Clear();
            Global_vars.jsontransfer.Add("boardsize", Global_vars.boardsize);

            Console.ResetColor();
            Console.Clear();
            Console.SetWindowSize(Global_vars.boardsize[0] * 2 + 3, Global_vars.boardsize[1] + 4);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.White;
            Test Test = new();
            List<List<int>> coordinatelist = Test.Function();
            
            Global_vars.jsontransfer.Add("cnl", coordinatelist);

            Checklist(coordinatelist);
            for (int i = 0; i < Global_vars.boardsize[1] + 1; i++)
            {
                Console.SetCursorPosition(Global_vars.boardsize[0] * 2 - 1, i);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(0, Global_vars.boardsize[1]);
            for (int i = 0; i < Global_vars.boardsize[0] * 2; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("Overcomplicated Minesweeper\nv0.5");

            List<int> templist = new();
            for (int i = 0; i < Global_vars.boardsize[0]; i++)
            {
                templist.Add(0);
            }
            Global_vars.fields_opened.Clear();
            Global_vars.flaglist.Clear();
            for (int i = 0; i < Global_vars.boardsize[1]; i++)
            {
                Global_vars.fields_opened.Add(new List<int>(templist));
                Global_vars.flaglist.Add(new List<int>(templist));
            }
            Global_vars.jsontransfer.Add("fOpen", Global_vars.fields_opened);
            Global_vars.jsontransfer.Add("fList", Global_vars.flaglist);
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i <= coordinatelist.Count - 1; i++)
            {
                for (int j = 0; j <= coordinatelist[0].Count - 1; j++)
                {
                    if (coordinatelist[i][j] == 0)
                    {
                        if (int.Parse(Checkpos(coordinatelist, j, i)) == 0)
                        {
                            Open0(coordinatelist, j, i);
                            Console.SetCursorPosition(j * 2, i);
                            goto start;
                        }

                    }
                }
            }

        start:
            while (true)
            {
                ipc_update(br, bw);/*
                while (Keyboard.IsKeyDown(Key.Up))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    try
                    {
                        Console.SetCursorPosition(consolex, consoley - 1);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Down))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (consoley >= Global_vars.boardsize[1] - 1)
                    {
                        continue;
                    }
                    try
                    {
                        Console.SetCursorPosition(consolex, consoley + 1);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Left))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    try
                    {
                        Console.SetCursorPosition(consolex - 2, consoley);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Right))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (consolex / 2 >= Global_vars.boardsize[0] - 1)
                    {
                        continue;
                    }
                    try
                    {
                        Console.SetCursorPosition(consolex + 2, consoley);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }
                while (Keyboard.IsKeyDown(Key.Space))
                {
                    while (!Keyboard.IsKeyUp(Key.Space))
                    {
                    }
                    Console.ResetColor();
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    string current_pos = Checkpos(coordinatelist, consolex / 2, consoley);

                    if (current_pos == "0")
                    {
                        Open0(coordinatelist, consolex / 2, consoley);
                    }
                    else if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                    {
                        var temp = Openaround(coordinatelist, consolex / 2, consoley);
                        if (temp.Item1)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(temp.xF * 2, temp.yF);
                            Console.Write("M");
                            goto Endwhile;
                        }
                    }
                    else if (Global_vars.flaglist[consoley][consolex / 2] == 1)
                    {
                        break;
                    }
                    else if (current_pos == "M")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        for (int i = 0; i < coordinatelist.Count; i++)
                        {
                            for (int j = 0; j < coordinatelist[i].Count; j++)
                            {
                                if (coordinatelist[i][j] == 1)
                                {
                                    Console.SetCursorPosition(j * 2, i);
                                    Console.Write("M");
                                }
                            }
                        }
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(current_pos);

                        goto Endwhile;
                    }
                    else
                    {
                        Console.ForegroundColor = Global_vars.colors[Int32.Parse(current_pos)];

                        Console.Write(current_pos);
                        Global_vars.fields_opened[consoley][consolex / 2] = 1;

                        if (CheckEnd())
                        {
                            goto Endwhile;
                        }
                        //TODO: test

                    }
                    Console.SetCursorPosition(consolex, consoley);

                }
                while (Keyboard.IsKeyDown(Key.F))
                {
                    while (!Keyboard.IsKeyUp(Key.F)) { }
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                    {
                        break;
                    }
                    else if (Global_vars.flaglist[consoley][consolex / 2] == 1)
                    {
                        Global_vars.flaglist[consoley][consolex / 2] = 0;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        string tempvar = Checkpos(coordinatelist, consolex / 2, consoley);

                        if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                        {
                            Console.ForegroundColor = Global_vars.colors[Int32.Parse(tempvar)];

                            Console.Write(tempvar);
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                        Global_vars.flaglist[consoley][consolex / 2] = 1;
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("F");
                        if (CheckEnd())
                        {
                            goto Endwhile;
                        }
                    }

                    Console.SetCursorPosition(consolex, consoley);
                }
                while (Keyboard.IsKeyDown(Key.Escape))
                {
                    goto End;
                }*/
            }
        Endwhile:
            Console.ResetColor();
            Console.SetCursorPosition(0, coordinatelist.Count + 1);
            Console.WriteLine("ENDED");
            while (true)
            {
                while (Keyboard.IsKeyDown(Key.Space))
                {
                    goto Reset;
                }
                while (Keyboard.IsKeyDown(Key.Escape))
                {
                    goto End;
                }
                

            }

        End:
            Console.WriteLine("Closed");
            server.Close();
            server.Dispose();
        }
    }
}

/* savespace
            Testmethod();
            Console.WriteLine("Please enter length");
            double length = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Please enter width");
            double width = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("something: {0}", width * length);
            Console.ReadLine();



            string length;
            double lengthint;
            string width;
            double widthint;
            string heigth;
            double heightint;
            Console.WriteLine("Volumenberechnung.");
            Console.WriteLine("Geben sie die Länge ein");
            while (true)
            {
                length = Console.ReadLine();

                if(double.TryParse(length, out lengthint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else{ Console.WriteLine("please try again"); }
            }

            Console.WriteLine("Geben sie die Breite ein");
            while (true)
            {
                width = Console.ReadLine();

                if (double.TryParse(width, out widthint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else { Console.WriteLine("please try again"); }
            }

            Console.WriteLine("Geben sie die Höhe ein");
            while (true)
            {
                heigth = Console.ReadLine();

                if (double.TryParse(heigth, out heightint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else { Console.WriteLine("please try again"); }
            }
            double fläche = lengthint * widthint;
            Console.WriteLine("Fläche: {0}", fläche);
            Console.WriteLine("Volumen: {0}", fläche*heightint);
*/