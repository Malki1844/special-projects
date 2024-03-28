using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace fib
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bundleCommand = new Command("bundle", "bundle code files into a single file");
            bundleCommand.AddAlias("bu");
            var outputOption = new Option<string>("--output", "file path and name");
            outputOption.AddAlias("-o");
            var languageOption = new Option<string>("--language", "file path and name") { IsRequired = true };
            languageOption.AddAlias("-l");
            var noteOption = new Option<bool>("--note", "write the source code in a note in the new file");
            noteOption.AddAlias("-n");

            var authorOption = new Option<string>("--author", "file creator name");
            authorOption.AddAlias("-a");
            var sortOption = new Option<bool>("--sort", "sort the files according to the type of code");
            sortOption.AddAlias("-s");
            var removeOption = new Option<bool>("--remove-empty-lines", "remove empty lines");
            removeOption.AddAlias("-r");
            bundleCommand.AddOption(outputOption);
            bundleCommand.AddOption(languageOption);
            bundleCommand.AddOption(noteOption);
            bundleCommand.AddOption(authorOption);
            bundleCommand.AddOption(sortOption);
            bundleCommand.AddOption(removeOption);
            bundleCommand.SetHandler((output, language, note, author, sort, remove) =>
            {
                try
                {
                    string[] pExten = { ".java", ".py", ".cs", ".c", ".html", ".js", ".css", ".jsx", "sql", "c++" };
                    string extension = null;
                    if (language.ToLower()=="all")

                    {
                        List<string> filesAll = new List<string>();

                        foreach (string file in Directory.GetFiles(".", "*.*", SearchOption.AllDirectories))
                        {
                            if (output==null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You did not choose a name for the file, if you want so try again");
                                Console.ResetColor();
                                Environment.Exit(1);
                            }

                            string extensionAllFile = Path.GetExtension(file);
                            if (pExten.Contains(extensionAllFile))
                            {
                                filesAll.Add(file);
                            }
                        }
                        if (filesAll.Count ==0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No programming files found in the current directory.");
                            Console.ResetColor(); 
                            return;
                        }
                        if (sort)
                        {
                            filesAll = filesAll.OrderBy(f => Path.GetExtension(f)).ToList();
                        }
                        else
                        {
                            filesAll.Sort();

                        }
                        string txtPath = output + '.'+"txt";

                        using (StreamWriter writer = new StreamWriter(txtPath))
                        {
                            if (author!=null)
                            {
                                writer.WriteLine("the name creator file is:");
                                writer.WriteLine(author);
                            }
                            if (remove)
                            {
                                foreach (string file in filesAll)
                                {
                                    if (note)
                                    {
                                        writer.WriteLine("Source code:");
                                        writer.WriteLine("File: " + file);
                                    }

                                    string code = File.ReadAllText(file);
                                    string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                    List<string> nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                                    string cleanedCode = string.Join(Environment.NewLine, nonEmptyLines);

                                    writer.WriteLine(cleanedCode);
                                }
                            }
                            else
                            {
                                foreach (string file in filesAll)
                                {
                                    if (note)
                                    {
                                        writer.WriteLine("Source code:");
                                        writer.WriteLine("File: " + file);
                                    }

                                    writer.WriteLine(File.ReadAllText(file));
                                }
                            }

                        }


                        Console.WriteLine("Files were bundled into a single file: " + txtPath);
                    }
                    else
                    {
                        List<string> files = new List<string>();

                        foreach (string file in Directory.GetFiles(".", $"*.{language}", SearchOption.AllDirectories))
                        {
                            if (output==null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You did not choose a name for the file, if you want so try again");
                                Console.ResetColor();
                                Environment.Exit(1);
                            }
                            
                            string extensionAllFile = Path.GetExtension(file);
                            if (pExten.Contains(extensionAllFile))
                            {
                                files.Add(file);
                            }
                        }
                        if (files.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No programming files found in the current directory.");
                            Console.ResetColor();
                            return;
                        }

                        else
                        {

                            files.Sort();

                            string txtPath = output + ".txt";

                            using (StreamWriter writer = new StreamWriter(txtPath))
                            {
                                if (author!=null)
                                {
                                    writer.WriteLine("the name creator file is:");
                                    writer.WriteLine(author);
                                }
                                if (remove)
                                {
                                    foreach (string file in files)
                                    {
                                        string code = File.ReadAllText(file);
                                        string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                        List<string> nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                                        string cleanedCode = string.Join(Environment.NewLine, nonEmptyLines);
                                        File.WriteAllText(file, cleanedCode);
                                    }
                                }

                                foreach (string file in files)
                                {
                                    if (note)
                                    {
                                        writer.WriteLine("Source code:");
                                        writer.WriteLine("File: " + file);
                                    }

                                    writer.WriteLine(File.ReadAllText(file));
                                }

                            }

                            Console.WriteLine("Files were bundled into a single file: " + txtPath);
                        }
                    }

                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Folder path is invalid");
                    Console.ResetColor();
                }
            }, outputOption, languageOption, noteOption, authorOption, sortOption, removeOption);

            var rspCommand = new Command("create-rsp", "Create response file");
            rspCommand.AddAlias("crsp");
            string rspFilePath = "fileRun.rsp";
            rspCommand.AddOption(outputOption);
            var rspLanguageOption = new Option<string>("--language", "file path and name");
            rspCommand.AddOption(rspLanguageOption);
            rspCommand.AddOption(noteOption);
            rspCommand.AddOption(authorOption);
            rspCommand.AddOption(sortOption);
            rspCommand.AddOption(removeOption);
            string[] pExten = { "java", "py", "cs", "c", "html", "js", "css", "jsx", "sql", "c++" };
            rspCommand.SetHandler(() =>
            {
                using (StreamWriter writer = File.CreateText(rspFilePath))
                {
                    Console.WriteLine("Enter the name of the file for output:");
                    string output = Console.ReadLine();
                    while (string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("please insert a name of file");
                        output = Console.ReadLine();
                    }
                    writer.WriteLine("bundle");
                    writer.WriteLine("--output " + output);
                }
                Console.WriteLine("Enter the language:");
                string language = Console.ReadLine();
                while (!pExten.Contains(language.ToLower()) && language.ToLower() != "all")
                {
                    Console.WriteLine("Please enter a programming language:");
                    language = Console.ReadLine();

                    if (!pExten.Contains(language.ToLower()) && language.ToLower() != "all")
                    {
                        Console.WriteLine("Invalid language. Please try again.");
                    }
                }

                using (StreamWriter languageWriter = File.AppendText(rspFilePath))
                {
                    languageWriter.WriteLine("--language " + language);
                }

                Console.WriteLine("Do you want to add a note? (yes/no):");
                string addNote = Console.ReadLine();
                while (addNote.ToLower()!="yes"&&addNote.ToLower()!="no")
                {
                    Console.WriteLine("your input is not good pleas insert again");
                    addNote = Console.ReadLine();
                }
                if (addNote.ToLower() == "yes")
                {
                    using (StreamWriter noteWriter = File.AppendText(rspFilePath))
                    {
                        noteWriter.WriteLine("--note");
                    }
                }

                Console.WriteLine("Enter the author (leave empty if not applicable):");
                string author = Console.ReadLine();
                if (!string.IsNullOrEmpty(author))
                {
                    using (StreamWriter authorWriter = File.AppendText(rspFilePath))
                    {
                        authorWriter.WriteLine("--author " + author);
                    }
                }
                Console.WriteLine("Do you want to sort the output? (yes/no):");
                string sortOutput = Console.ReadLine();
                while (sortOutput.ToLower()!="yes"&&sortOutput.ToLower()!="no")
                {
                    Console.WriteLine("your input is not good");
                    sortOutput = Console.ReadLine();
                }
                if (sortOutput.ToLower() == "yes")
                {
                    using (StreamWriter sortWriter = File.AppendText(rspFilePath))
                    {
                        sortWriter.WriteLine("--sort");
                    }
                }

                Console.WriteLine("Do you want to remove the output? (yes/no):");
                string removeOutput = Console.ReadLine();
                while (removeOutput.ToLower()!="yes"&&removeOutput.ToLower()!="no")
                {
                    Console.WriteLine("your input is not good");
                    removeOutput=Console.ReadLine();
                }
                if (removeOutput.ToLower() == "yes")
                {
                    using (StreamWriter removeWriter = File.AppendText(rspFilePath))
                    {
                        removeWriter.WriteLine("--remove");
                    }
                }
            }
            );
            var rootCommand = new RootCommand("root command for file bundle cli");
            rootCommand.AddCommand(bundleCommand);
            rootCommand.AddCommand(rspCommand);
            rootCommand.InvokeAsync(args).Wait();
        }
    }
}