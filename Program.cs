using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT_LR2
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = @"DB.txt";
            try
            {
                string[] readtext = File.ReadAllLines(path);
                foreach (string line in readtext)
                {
                    Console.WriteLine(line);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Файл не найден");
            }
            Console.WriteLine("Нажмите любую кнопку, чтобы закрыть приложение... ");
            Console.ReadKey(true);
        }
    }
}
