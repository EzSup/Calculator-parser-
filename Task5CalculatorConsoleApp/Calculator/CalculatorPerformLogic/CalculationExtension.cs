using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class CalculationExtension : Calculation
    {
        protected string? _result;
        protected string _format = "{0} -> {1}";

        /// <summary>
        /// Checks the result for an error message and formats it
        /// </summary>
        /// <param name="resultingString">The result string</param>
        protected void CheckForException(ref string resultingString)
        {
            resultingString = resultingString.Contains(base._inputExceptionMessage) ? _inputExceptionMessage : resultingString;
            resultingString = resultingString.Contains(base._divideByZeroExceptionMessage) ? _divideByZeroExceptionMessage : resultingString;
        }

        /// <summary>
        /// Runs methods to process an expression
        /// </summary>
        /// <param name="readedExpression">Expression stirng</param>
        /// <returns>result of processing</returns>
        protected string? ProcessExpressionLine(string readedExpression)
        {
            _result = BaseCalculationMethods(ReplaceExpressionsInBrackets(readedExpression));
            CheckForException(ref _result);
            return string.Format(_format, readedExpression, _result);
        }

        /// <summary>
        /// Shortly calls base method Calculate and ParseExpression
        /// </summary>
        /// <param name="input">Input expression</param>
        /// <returns>Result of Processing</returns>
        protected string BaseCalculationMethods(string input)
        {
            return base.Calculate(base.ParseExpression(input));
        }

        /// <summary>
        /// Evaluates expressions in parentheses and replaces the expression with the result of its calculation
        /// </summary>
        /// <param name="input">Input expression</param>
        /// <returns>Expression with replaced brackets</returns>
        protected string ReplaceExpressionsInBrackets(string input)
        {
            string result = input;
            int openingBracketIndex = result.LastIndexOf('(');
            while (openingBracketIndex != -1)
            {
                int closingBracketIndex = FindClosingBracketIndex(result, openingBracketIndex);
                if (closingBracketIndex != -1)
                {
                    string expressionInBrackets = result.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1);
                    string expressionResult = BaseCalculationMethods(expressionInBrackets);
                    result = result.Substring(0, openingBracketIndex) + expressionResult + result.Substring(closingBracketIndex + 1);
                }

                openingBracketIndex = result.LastIndexOf('(');
            }

            return result;
        }

        /// <summary>
        /// Looks for the index of the closing bracket
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="openingBracketIndex">Index of the opening bracket</param>
        /// <returns>Index of the closing bracket</returns>
        protected int FindClosingBracketIndex(string input, int openingBracketIndex)
        {
            int bracketCounter = 1;
            for (int i = openingBracketIndex + 1; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    bracketCounter++;
                }
                else if (input[i] == ')')
                {
                    bracketCounter--;
                    if (bracketCounter == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
