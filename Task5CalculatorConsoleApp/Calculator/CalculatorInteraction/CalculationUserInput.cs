using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    public class CalculationUserInput : CalculationExtension
    {
        protected string? _result;

        /// <summary>
        /// Gets the value of the entered expression and calls methods to calculate the result
        /// </summary>
        /// <param name="enteredString">Entered expression string</param>
        public void EnterExpression(string? enteredString)
        {
            _result = base.ProcessExpressionLine(enteredString);
        }

        /// <summary>
        /// Displays the text representation of the expression and its result in the console
        /// </summary>
        public void OutResult()
        {
            Console.WriteLine(_result);
        }

        /// <summary>
        /// Returns the text representation of the result 
        /// </summary>
        /// <returns>The text representation of the result</returns>
        public string GetResult()
        {
            return _result;
        }
    }
}
