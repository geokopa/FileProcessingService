using System;
using System.Collections.Generic;
using System.Linq;

namespace FileProcessingService.ConsoleApp
{
    public class ExtendedConsole
    {
        public List<Option> Options { get; private set; }

        public ExtendedConsole()
        {
            // Create options that you want your menu to have
            Options = new List<Option>();
        }

        public void Init()
        {
            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(Options, Options[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < Options.Count)
                    {
                        index++;
                        WriteMenu(Options, Options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(Options, Options[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.Q);
        }

        public void ClearMainMenu()
        {
            this.Options.Clear();
        }

        public void AddExit()
        {
            AddOption(new Option("Exit", () => { Environment.Exit(0); }));
        }


        public void AddOption(Option option)
        {
            Options.Add(option);
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }
    }
}
