using System.Text;
using Calculator;

namespace Task5CalculatorConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            while (true)
            {
                Console.WriteLine("Який режим вводу оберете?\n\t1 - Ввід з консолі\n\t2 - зчитування з файла");
                int answer;
                if(int.TryParse(Console.ReadLine(), out answer))
                {
                    if (answer == 1)
                    {
                        RunConsole(); break;
                    }
                    if (answer == 2)
                    {
                        RunFile(); break;
                    }
                    Console.WriteLine("Повторіть спробу");
                }
            }

            CalculationFileInput calculationFile = new CalculationFileInput(Path.Combine(AppContext.BaseDirectory, "..","..","..","Expression.txt"));
            calculationFile.WriteToFile();
        }
        static void RunConsole()
        {
            var calculationUser = new CalculationUserInput();
            string input;
            do
            {
                Console.Write("Введіть вираз:");
                input = Console.ReadLine();
                calculationUser.EnterExpression(input);
                calculationUser.OutResult();
            }
            while (input != "стоп");
        }
        static void RunFile()
        {
            Console.WriteLine("Введіть шлях до файлу");
            string filePath = Console.ReadLine();
            CalculationFileInput calculationFile = new CalculationFileInput(filePath);
            calculationFile.WriteToFile();
        }
    }
}