using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexForWriters
{
    class Program
    {
        static void Main(string[] args)
        {
            /** Two suggested ways to use this program:
             * 1. Run directly from Visual Studio. Set a specific directory to run through.
             *  Good for testing or for projects where all the content is in one directory.
             * 2. Build application and run in specific directory. Tell program to use the directory where it is.
             *  In this scenario, you'll build the application from the build menu and then grab the .exe file from the bin folder.
             *  Then, place the exe in the folder you want to process and run it.
             *  */

            //For option 1, specify the directory the program will process.
            string directory = @"C:\Users\username\Desktop\SampleDocs";
            //For option 2, the program will use the directory where the .exe file is.
            //Use the following line instead of the previous one (line 25). 
            //string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
              
            //Starting message so you know the program is running
            Console.WriteLine("Processing files...\n");

            //update the files in the specified directory
            ProcessDirectory(directory);

            //Let the user know when the program is done running
            Console.WriteLine("All done! Press any key to exit.");
            Console.ReadKey();
        }

        public static void ProcessDirectory(string directory)
        {
            /** If you want to go through subfolders,
             * you don't need to do anything with this method
             * If you only want to modify files in the directory above,
             * comment out the foreach loop below
             * */

            //get all the files in the folder and process them
            string[] fileEntries = Directory.GetFiles(directory);

            List<string> file_list = new List<string>(fileEntries);
            try
            {
                Parallel.ForEach(file_list, file =>
                {
                    //you can apply edits to specific file types. 
                    //If you don't want to specify file types, delete lines 59 and 61.
                    if (Path.GetExtension(file) == ".txt" || (Path.GetExtension(file) == ".html")) {
                        ProcessFile(file);
                    }
                });
            }
            //if there's an error print it to the console
            catch (AggregateException e)
            {
                Console.WriteLine("Inner exception: " + e.InnerException);
            }

            //process all the subfolders in the directory.
            //If you don't want to go into subfolders, comment out or remove the foreach loop (lines 74-77)
            string[] subDirectoryEntries = Directory.GetDirectories(directory);

            foreach (string subDirectory in subDirectoryEntries)
            {
                ProcessDirectory(subDirectory);
            }
        }

        public static void ProcessFile(string file)
        {
            /** This method:
             * 1. Gets all the text in a file
             * 2. Performs a series of regular expressions
             * 3. Saves the file with the updated text
             * */

            //get all text
            string text = File.ReadAllText(file);

            //use regex.replace function and pass in the text to search, regex, and replacement string
            text = Regex.Replace(text, @"(?<=Greeting=).*", "Hello, world!");

            //you can remove text by passing in a blank replacement string
            //use regex options at the end if you want to ignore case in your match
            //if the regex has quotes in it, escape them as shown here
            text = Regex.Replace(text, @"<p.*?class="".*?unnecessary.*?"".*?>.*?<\/p>", "", RegexOptions.IgnoreCase);

            //if you want to do a simple text replacement without regex, use the string.replace method
            text = text.Replace(@"Old text", "New text");

            /**
             *  Add more regular expressions here, as many as you like. 
             * */

            //Finally, save the file with the updated text
            File.WriteAllText(file, text);
        }
    }
}
