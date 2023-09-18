using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Calculation
    {
        public static char[] OperatorsList = { '*', '/', '+', '-' };
        private string? _result;
        private bool _exceptionFound = false;
        private string _splittingPattern = @"([-+*/])";
        protected string _inputExceptionMessage = "Помилка. Неправильний ввід";
        protected string _divideByZeroExceptionMessage = "Помилка. Ділення на 0";

        protected void ExceptionFound(string message)
        {
            _result = message;
            _exceptionFound = true;
            return;
        }

        /// <summary>
        /// Converts the received string into an array of elements and operators
        /// </summary>
        /// <param name="input">Input string to be converted</param>
        /// <returns>An array of elements and expression operators</returns>
        protected string[] ParseExpression(string? input)
        {
            _result = null;
            string[] splittedInput = Regex.Split(input, _splittingPattern);
            List<string> parsedExpression = new List<string>();
            for (int i = 0; i < splittedInput.Length; i++)
            {
                if (string.IsNullOrEmpty(splittedInput[i]))
                {
                    continue;
                }
                if (i == 1 && splittedInput[i] == "-" && splittedInput[i-1] == "")
                {
                    parsedExpression.Add(string.Concat("-", splittedInput[i + 1]));
                    i++;
                    continue;
                }
                if (i > 1 && splittedInput[i] == "-" && splittedInput[i - 1] == "" && IsOperator(splittedInput[i - 2]))
                {
                    parsedExpression.Add(string.Concat("-", splittedInput[i + 1]));
                    i++;
                    continue;
                }
                parsedExpression.Add(splittedInput[i]);
            }
            return parsedExpression.ToArray();
        }

        /// <summary>
        /// Calculates the entire expression as a whole
        /// </summary>
        /// <param name="elements">An array of elements and expression operators</param>
        /// <returns>The text representation of the calculation result</returns>
        protected string Calculate(string[] elements)
        {
            Stack<double> stack = new Stack<double>();
            Stack<char> operators = new Stack<char>();
            string? result = null;
            _exceptionFound = false;
            try
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    double itemNum = 0;
                    if (IsOperator(elements[i]))
                    {
                        char currentOperation = elements[i][0];
                        result = CalculationLoop(ref stack, ref operators, currentOperation);
                        if (_exceptionFound)
                        {
                            return _result;
                        }
                        operators.Push(currentOperation);
                    }
                    else if (IsNum(elements[i], ref itemNum))
                    {
                        stack.Push(itemNum);
                    }
                }
                CalculationLoop(ref stack, ref operators);
                if (_exceptionFound)
                {
                    return _result;
                }
                return stack.Pop().ToString();
            }
            catch
            {
                return _inputExceptionMessage;
            }
        }

        /// <summary>
        /// Calculates each operation of the expression
        /// </summary>
        /// <param name="stack">Stack of numeric elements of an expression</param>
        /// <param name="operators">Stack of expression operators</param>
        /// <param name="currentOperation">Operator of the current operation</param>
        /// <returns>Report an error if it occurs</returns>
        protected string? CalculationLoop(ref Stack<double> stack, ref Stack<char> operators, char? currentOperation = null)
        {
            while (operators.Count > 0 && ((currentOperation != null) ? IsHigherPrecedence(operators.Peek(), (char)currentOperation) : true))
            {
                try
                {
                    double num2 = stack.Pop();
                    double num1 = stack.Pop();
                    char op = operators.Pop();
                    double? result = PerformOperation(num1, num2, op);
                    stack.Push((double)result);
                }
                catch (DivideByZeroException)
                {
                    ExceptionFound(_divideByZeroExceptionMessage);
                    return _result;
                }
                catch {
                    ExceptionFound(_inputExceptionMessage);
                    return _result;
                }
            }
            return null;
        }


        /// <summary>
        /// Checks if a string can be converted to a number
        /// </summary>
        /// <param name="str">Text value</param>
        /// <param name="num">Resulting number</param>
        /// <returns>Is it possible to convert a string to a number</returns>
        protected bool IsNum(string str, ref double num)
        {
            try
            {
                num = double.Parse(str);
                return true;
            }
            catch
            {
                ExceptionFound(_inputExceptionMessage);
                return false;
            }
        }

        /// <summary>
        /// Checks if a string can be converted to a operator
        /// </summary>
        /// <param name="str">Text value</param>
        /// <returns>Can this text be an operator</returns>
        protected bool IsOperator(string str)
        {
            return str.Length == 1 && OperatorsList.Contains(str[0]);
        }

        /// <summary>
        /// Performs a certain operation on two numbers
        /// </summary>
        /// <param name="num1">First number</param>
        /// <param name="num2">Second number</param>
        /// <param name="operation">Operation</param>
        /// <returns>Result of the operation</returns>
        protected double? PerformOperation(double num1, double num2, char operation)
        {
            switch (operation)
            {
                case '*':
                    return num1 * num2;
                case '/':
                    if (num2 == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    return num1 / num2;
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Checks whether an operation has a higher priority than another
        /// </summary>
        /// <param name="op1">First operation</param>
        /// <param name="op2">Second operation</param>
        /// <returns>Whether an operation has a higher priority than another</returns>
        protected bool IsHigherPrecedence(char? op1, char op2)
        {
            if (op1 == '*' || op1 == '/')
            {
                return true;
            }

            if (op1 == '+' || op1 == '-')
            {
                return op2 == '+' || op2 == '-';
            }

            return false;
        }
    }
}
