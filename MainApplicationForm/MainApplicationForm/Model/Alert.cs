using System;
using System.IO;

namespace MainApplicationForm.Model
{
    class Alert
    {
        public void generateWarnings()
        {
            Console.Beep(1000, 900);
        }

        public static async void WriteTextAsync(string text)
        {
            // Set a variable to the My Documents path.
            string mydocpath = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string localPath = new Uri(mydocpath).LocalPath;
            string statsFilePath = Path.Combine(localPath, "DriverStatistics.txt");

            // Write the text asynchronously to a new file named "DriverStatistics.txt".
            using (StreamWriter outputFile = File.AppendText(statsFilePath))
            {
                await outputFile.WriteLineAsync(text);
            }
        }
    }
}
