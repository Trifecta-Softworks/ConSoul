using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Resources;
using System.Diagnostics;

namespace src
{
    public class Config
    {
        public bool FirstTimeSetup { get; set; }
        public ConsoleColor Color { get; set; }
        public bool Sound { get; set; }
    }

    public class Module
    {
        
    }
    
    public class Character
    {
        // Character Info
        public string? Name { get; set; }
        public string? Backstory { get; set; }
        public int[]? SkillPoints { get; set; }
        
        // Game Info
        public Module[]? EnabledModules { get; set; }
    }
    
    internal class Program
    {
        private static bool inUpdate = false;
        private static bool finishUpdate = false;

        static void PlayClick()
        {
            
        }

        static void LoadMenu()
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "ConSoul");
            Console.Clear();
            bool inMenu = false;
            
            Console.WriteLine(@"
     ██▓     ▒█████   ▄▄▄      ▓█████▄      ██████  ▄▄▄    ██▒   █▓▓█████ 
    ▓██▒    ▒██▒  ██▒▒████▄    ▒██▀ ██▌   ▒██    ▒ ▒████▄ ▓██░   █▒▓█   ▀ 
    ▒██░    ▒██░  ██▒▒██  ▀█▄  ░██   █▌   ░ ▓██▄   ▒██  ▀█▄▓██  █▒░▒███   
    ▒██░    ▒██   ██░░██▄▄▄▄██ ░▓█▄   ▌     ▒   ██▒░██▄▄▄▄██▒██ █░░▒▓█  ▄ 
    ░██████▒░ ████▓▒░ ▓█   ▓██▒░▒████▓    ▒██████▒▒ ▓█   ▓██▒▒▀█░  ░▒████▒
    ░ ▒░▓  ░░ ▒░▒░▒░  ▒▒   ▓▒█░ ▒▒▓  ▒    ▒ ▒▓▒ ▒ ░ ▒▒   ▓▒█░░ ▐░  ░░ ▒░ ░
    ░ ░ ▒  ░  ░ ▒ ▒░   ▒   ▒▒ ░ ░ ▒  ▒    ░ ░▒  ░ ░  ▒   ▒▒ ░░ ░░   ░ ░  ░
      ░ ░   ░ ░ ░ ▒    ░   ▒    ░ ░  ░    ░  ░  ░    ░   ▒     ░░     ░   
        ░  ░    ░ ░        ░  ░   ░             ░        ░  ░   ░     ░  ░
                                ░                              ░          
");
            Console.WriteLine();
            Console.WriteLine("Load Saves \nEnter The Save Slot Number Of The Save To Load - Will only list 5");
            Console.WriteLine();

            string[] Saves = new[] { "Empty", "Empty", "Empty", "Empty", "Empty"};

            int sub = 0;
            foreach (string file in Directory.EnumerateFiles( appData + "/saves", "*.*", SearchOption.AllDirectories))
            {
                Saves[sub] = Path.GetFileName(file);
                sub++;
                if (sub >= 5)
                {
                    break;
                }
            }
            
            Console.WriteLine(@"Saves:
    1 : {0}
    2 : {1}
    3 : {2}
    4 : {3}
    5 : {4}

    S : Know The Name?

    B : Back
            ", Saves[0], Saves[1], Saves[2], Saves[3], Saves[4] );
            
            inMenu = true;
            while (inMenu)
            {
                // do something
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        if(Saves[0] == "Empty")
                            break;
                        Console.WriteLine(@"Returning to {0}", Saves[0]);
                        inMenu = false;
                        break;
                    case ConsoleKey.D2:
                        if(Saves[1] == "Empty") 
                            break;
                        Console.WriteLine(@"Returning to {0}", Saves[1]);
                        inMenu = false;
                        break;
                    case ConsoleKey.D3:
                        if(Saves[2] == "Empty") 
                            break;
                        Console.WriteLine(@"Returning to {0}", Saves[2]);
                        inMenu = false;
                        break;
                    case ConsoleKey.D4:
                        if(Saves[3] == "Empty") 
                            break;
                        Console.WriteLine(@"Returning to {0}", Saves[3]);
                        inMenu = false;
                        break;
                    case ConsoleKey.D5:
                        if(Saves[4] == "Empty") 
                            break;
                        Console.WriteLine(@"Returning to {0}", Saves[4]);
                        inMenu = false;
                        break;
                    
                    case ConsoleKey.S:
                        Console.Write(@"
    Enter File Name
    >> ");
                        var fileName = Console.ReadLine();
                        if (!File.Exists(appData + "/saves" + "/" + fileName))
                        {
                            LoadMenu();
                            break;
                        }
                        Console.WriteLine(@"Returning to {0}", fileName);
                        inMenu = false;
                        break;

                    case ConsoleKey.B:
                        Console.WriteLine(@"Returning");
                        inMenu = false;
                        MainMenu();
                        break;
                }
            }
        }

        static void CharacterMenu()
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "ConSoul");
            Console.Clear();
            bool inMenu = false;
            
            Console.WriteLine(@"
     ▄████▄   ██░ ██  ▄▄▄       ██▀███   ▄▄▄       ▄████▄  ▄▄▄█████▓▓█████  ██▀███  
    ▒██▀ ▀█  ▓██░ ██▒▒████▄    ▓██ ▒ ██▒▒████▄    ▒██▀ ▀█  ▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒
    ▒▓█    ▄ ▒██▀▀██░▒██  ▀█▄  ▓██ ░▄█ ▒▒██  ▀█▄  ▒▓█    ▄ ▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒
    ▒▓▓▄ ▄██▒░▓█ ░██ ░██▄▄▄▄██ ▒██▀▀█▄  ░██▄▄▄▄██ ▒▓▓▄ ▄██▒░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄  
    ▒ ▓███▀ ░░▓█▒░██▓ ▓█   ▓██▒░██▓ ▒██▒ ▓█   ▓██▒▒ ▓███▀ ░  ▒██▒ ░ ░▒████▒░██▓ ▒██▒
    ░ ░▒ ▒  ░ ▒ ░░▒░▒ ▒▒   ▓▒█░░ ▒▓ ░▒▓░ ▒▒   ▓▒█░░ ░▒ ▒  ░  ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░
      ░  ▒    ▒ ░▒░ ░  ▒   ▒▒ ░  ░▒ ░ ▒░  ▒   ▒▒ ░  ░  ▒       ░     ░ ░  ░  ░▒ ░ ▒░
    ░         ░  ░░ ░  ░   ▒     ░░   ░   ░   ▒   ░          ░         ░     ░░   ░ 
    ░ ░       ░  ░  ░      ░  ░   ░           ░  ░░ ░                  ░  ░   ░     
    ░                                             ░                                 
     ▄████▄   ██▀███  ▓█████ ▄▄▄     ▄▄▄█████▓ ██▓ ▒█████   ███▄    █               
    ▒██▀ ▀█  ▓██ ▒ ██▒▓█   ▀▒████▄   ▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █               
    ▒▓█    ▄ ▓██ ░▄█ ▒▒███  ▒██  ▀█▄ ▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒              
    ▒▓▓▄ ▄██▒▒██▀▀█▄  ▒▓█  ▄░██▄▄▄▄██░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒              
    ▒ ▓███▀ ░░██▓ ▒██▒░▒████▒▓█   ▓██▒ ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░              
    ░ ░▒ ▒  ░░ ▒▓ ░▒▓░░░ ▒░ ░▒▒   ▓▒█░ ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒               
      ░  ▒     ░▒ ░ ▒░ ░ ░  ░ ▒   ▒▒ ░   ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░              
    ░          ░░   ░    ░    ░   ▒    ░       ▒ ░░ ░ ░ ▒     ░   ░ ░               
    ░ ░         ░        ░  ░     ░  ░         ░      ░ ░           ░               
    ░                                                                              
");
            Console.WriteLine();
            Console.WriteLine("Character Creation \nFollow the steps to create your character");
            Console.WriteLine();

            Character? newChar;
            
            if (!File.Exists(appData + "/tmpChar.json"))
            {
                var Defaults = new Character()
                {
                    Name = "None",
                    Backstory = "None",
                    SkillPoints = new []{0,0,0,0}
                };
            
                string jsonString = JsonSerializer.Serialize(Defaults);
                File.WriteAllText(appData + "/tmpChar.json", jsonString);
                newChar = Defaults;
            }
            else
            {
                string ReadString = File.ReadAllText(appData + "/tmpChar.json");
                newChar = JsonSerializer.Deserialize<Character>(ReadString);
            }

            Console.WriteLine(@"Character Creation:
	Character:
	N : Name : {0}
	D : Backstory : {1}
	A : Skill Points

	Game:
	M : Modules

	S : Save
	P : Save And Play

    B : Back", newChar.Name, newChar.Backstory);
            
            inMenu = true;
            while (inMenu)
            {
                // do something
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                
                key = Console.ReadKey(true);
                string jsonString;
                switch (key.Key)
                {
					case ConsoleKey.N:
						Console.Write(@"
	Enter Character Name >> ");
                        newChar.Name = Console.ReadLine();
                        
                        // Save Temp File
                        jsonString = JsonSerializer.Serialize(newChar);
                        File.WriteAllText(appData + "/tmpChar.json", jsonString);
                        inMenu = false;
                        CharacterMenu();
                        break;
                    
                    case ConsoleKey.D:
                        Console.Write(@"
    Enter Character Backstory >> ");
                        newChar.Backstory = Console.ReadLine();
                        // Save Temp File
                        jsonString = JsonSerializer.Serialize(newChar);
                        File.WriteAllText(appData + "/tmpChar.json", jsonString);
                        inMenu = false;
                        CharacterMenu();
                        break;
                    
                    case ConsoleKey.A:
                        Console.Write(@"
    Menu For Skill Points");
                        inMenu = false;
                        MainMenu();
                        break;
                    
                    case ConsoleKey.S:
                        Console.Write("Saving Character Returning");
                        jsonString = JsonSerializer.Serialize(newChar);
                        File.WriteAllText(appData + "/saves/" + newChar.Name, jsonString);
                        File.Delete(appData + "/tmpChar.json");
                        inMenu = false;
                        MainMenu();
                        break;
                    
                    case ConsoleKey.P:
                        // Probably wont get functionality for a while
                        Console.Write("Saving and Starting Game...");
                        File.Delete(appData + "/tmpChar.json");
                        inMenu = false;
                        MainMenu();
                        break;
                    
                    case ConsoleKey.B:
                        Console.WriteLine(@"Returning");
                        File.Delete(appData + "/tmpChar.json");
                        inMenu = false;
                        MainMenu();
                        break;
                }
            }
        }
        
        static void OptionsMenu()
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "ConSoul");
            string ReadMain = File.ReadAllText(appData + "/config.json");
            Config? ConfigMain = JsonSerializer.Deserialize<Config>(ReadMain);
            Console.Clear();
            bool inMenu = false;
            
            Console.WriteLine(@"
     ▒█████   ██▓███  ▄▄▄█████▓ ██▓ ▒█████   ███▄    █   ██████ 
    ▒██▒  ██▒▓██░  ██▒▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █ ▒██    ▒ 
    ▒██░  ██▒▓██░ ██▓▒▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒░ ▓██▄   
    ▒██   ██░▒██▄█▓▒ ▒░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒  ▒   ██▒
    ░ ████▓▒░▒██▒ ░  ░  ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░▒██████▒▒
    ░ ▒░▒░▒░ ▒▓▒░ ░  ░  ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░
      ░ ▒ ▒░ ░▒ ░         ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░░ ░▒  ░ ░
    ░ ░ ░ ▒  ░░         ░       ▒ ░░ ░ ░ ▒     ░   ░ ░ ░  ░  ░  
        ░ ░                     ░      ░ ░           ░       ░ 

");
            Console.WriteLine();
            Console.WriteLine("The Options Menu \nthis is where you can change your options\nMore will be added");
            Console.WriteLine();
            
            Console.WriteLine(@"Options:
    S : Sound - {0}
    C : Change Color

    B : Back
            ", ConfigMain.Sound);

            inMenu = true;
            while (inMenu)
            {
                // do something
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.B:
                        // Quiting
                        Console.WriteLine(@"Returning");
                        inMenu = false;
                        MainMenu();
                        break;
                    case ConsoleKey.S:
                        string readTextSound = File.ReadAllText(appData + "/config.json");
                        Config? configoldSound = JsonSerializer.Deserialize<Config>(readTextSound);
                        var configSound = new Config()
                        {
                            FirstTimeSetup = configoldSound.FirstTimeSetup,
                            Color = configoldSound.Color,
                            Sound = !configoldSound.Sound
                        };
                        string jsonStringSound = JsonSerializer.Serialize(configSound);
                        File.WriteAllText(appData + "/config.json", jsonStringSound);
                        inMenu = false;
                        OptionsMenu();
                        break;
                    case ConsoleKey.C:
                        Console.Write(@"Enter Text Color: 
    Magenta

    Blue

    Yellow

    White

    Green 
>> ");
                        var text = Console.ReadLine();
                        
                        if (Enum.TryParse(text, out ConsoleColor foreground))
                        {
                            Console.ForegroundColor = foreground;
                            string readText = File.ReadAllText(appData + "/config.json");
                            Config? configold = JsonSerializer.Deserialize<Config>(readText);
                            var config = new Config()
                            {
                                FirstTimeSetup = configold.FirstTimeSetup,
                                Color = foreground,
                                Sound = configold.Sound
                            };
                            string jsonString = JsonSerializer.Serialize(config);
                            File.WriteAllText(appData + "/config.json", jsonString);
                        }   
                        

                        inMenu = false;
                        OptionsMenu();
                        break;
                }
            }
        }
        
        static void MainMenu()
        {
            Console.Clear();
            bool inMenu = false;
            
            Console.WriteLine(@"
     ▄████▄   ▒█████   ███▄    █   ██████  ▒█████   █    ██  ██▓    
    ▒██▀ ▀█  ▒██▒  ██▒ ██ ▀█   █ ▒██    ▒ ▒██▒  ██▒ ██  ▓██▒▓██▒    
    ▒▓█    ▄ ▒██░  ██▒▓██  ▀█ ██▒░ ▓██▄   ▒██░  ██▒▓██  ▒██░▒██░    
    ▒▓▓▄ ▄██▒▒██   ██░▓██▒  ▐▌██▒  ▒   ██▒▒██   ██░▓▓█  ░██░▒██░    
    ▒ ▓███▀ ░░ ████▓▒░▒██░   ▓██░▒██████▒▒░ ████▓▒░▒▒█████▓ ░██████▒
    ░ ░▒ ▒  ░░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░░ ▒░▒░▒░ ░▒▓▒ ▒ ▒ ░ ▒░▓  ░
      ░  ▒     ░ ▒ ▒░ ░ ░░   ░ ▒░░ ░▒  ░ ░  ░ ▒ ▒░ ░░▒░ ░ ░ ░ ░ ▒  ░
    ░        ░ ░ ░ ▒     ░   ░ ░ ░  ░  ░  ░ ░ ░ ▒   ░░░ ░ ░   ░ ░   
    ░ ░          ░ ░           ░       ░      ░ ░     ░         ░  ░
    ░                                                               ");
            
            Console.WriteLine();
            Console.WriteLine("Welcome to ConSoul \nThis is a game of life your decisions matter and can cause a change in the story");
            Console.WriteLine();

            Console.WriteLine(@"Menu:
    P : Start A New Character
    L : Load From A Character Save

    O : Options
    Q : Quit
            ");
            
            inMenu = true;
            while (inMenu)
            {
                // do something
                ConsoleKeyInfo key = new ConsoleKeyInfo();
                
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        // Quiting
                        Console.WriteLine(@"Thanks for Playing ConSoul
We hope to see you again!");
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.P:
                        Console.WriteLine(@"Character Creation");
                        CharacterMenu();
                        inMenu = false;
                        break;
                    case ConsoleKey.L:
                        Console.WriteLine(@"Load From Save");
                        LoadMenu();
                        inMenu = false;
                        break;
                    case ConsoleKey.O:
                        inMenu = false;
                        OptionsMenu();
                        break;
                }
            }
        }

        static void firstTimeSetup()
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "ConSoul");
// Ensure the directory and all its parents exist.
            Directory.CreateDirectory(appData);
            string[] paths = new[] { "/saves", "/themes", "/modules" };
            using (var progress = new ProgressBar()) {
                for (int i = 0; i < paths.Length; i++) {
                    Directory.CreateDirectory(appData + paths[i]);
                    progress.Report((double) i / paths.Length);
                    Thread.Sleep(25);
                }
            }

            var config = new Config()
            {
                Sound = true,
                FirstTimeSetup = true,
                Color = ConsoleColor.White
            };
            string jsonString = JsonSerializer.Serialize(config);
            File.WriteAllText(appData + "/config.json", jsonString);
        }
        
        
        static void Main(string[] args)
        {
            string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "ConSoul");
            
            if (!Directory.Exists(appData))
            {
                Console.WriteLine("Performing First Time Setup");
                firstTimeSetup();
            }
            
            string readText = File.ReadAllText(appData + "/config.json");
            Config? config = JsonSerializer.Deserialize<Config>(readText);



            Console.ForegroundColor = config.Color;


                //Console.WriteLine(readText);
            
            Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                if (inUpdate)
                {
                    finishUpdate = true;
                }
            };
            Console.WriteLine("Checking For Updates... ");
            Thread.Sleep(1500);
            Console.Write("Updating Game : ");
            inUpdate = true;
            using (var progress = new ProgressBar()) {
                for (int i = 0; i <= 100; i++) {
                    progress.Report((double) i / 100);
                    Thread.Sleep(25);
                }
            }
            if (finishUpdate)
            {
                Console.WriteLine("\nUpdate Finished Closing");
                Environment.Exit(0);
            }
            Console.WriteLine("Launching Game!");
            MainMenu();
        }
    }
}