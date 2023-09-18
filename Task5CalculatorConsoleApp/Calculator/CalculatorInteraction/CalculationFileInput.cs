using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class CalculationFileInput : CalculationExtension
    {
        private string _filePathInput = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Expressions.txt");
        private string _filePathOutput;
        private string[] _inputLines;
        private string[] _outputLines;
        public CalculationFileInput(string filePath)
        {
            if (File.Exists(filePath))
            {
                _filePathInput = filePath;
            }
            _filePathOutput = _filePathInput.Substring(0, _filePathInput.Length-4);
            _filePathOutput += "Solved.txt";
            _inputLines = ReadFile();
            _outputLines = ProcessData(_inputLines);
        }

        private string[] ProcessData(string[] input)
        {
            string[] output = new string[input.Length];
            for(int i = 0; i < input.Length; i++)
            {
                output[i] = base.ProcessExpressionLine(input[i]);
            }
            return output;
        }

        public void WriteToFile()
        {
            File.WriteAllLines(_filePathOutput, _outputLines);
        }

        private string[] ReadFile()
        {
            return File.ReadAllLines(_filePathInput);
        }
    }
}
