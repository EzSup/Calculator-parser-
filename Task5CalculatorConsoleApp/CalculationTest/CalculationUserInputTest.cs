using Calculator;
using System.Linq.Expressions;
using System.Reflection;

namespace CalculationTest
{
    [TestClass]
    public class CalculationUserInputTest
    {
        [DataTestMethod]
        [DataRow("2+2","2+2 -> 4")]
        [DataRow("2+2*2", "2+2*2 -> 6")]
        [DataRow("-2+-5", "-2+-5 -> -7")]
        [DataRow("-5-5", "-5-5 -> -10")]
        [DataRow("-5+5", "-5+5 -> 0")]
        [DataRow("-5*((5/2,5)+8,32)", "-5*((5/2,5)+8,32) -> -51,6")]
        public void EnterExpression_ValidInput_ReturnsCorrectResult(string? input, string expected)
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            //act
            calculation.EnterExpression(input);
            string result = GetPrivateField<string>(calculation, "_result");
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetResult_ReturnsCorrectResult()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            string expected = GetPrivateField<string>(calculation, "_result");
            //act
            string result = calculation.GetResult();
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseExpression_SimpleExpression_ResturnsCorrectResult()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            string input = "2+2";
            string[] expected = new string[] { "2", "+", "2" };
            //act
            string[] result = (string[])RunParseExpression(calculation, input);
            //assert
            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public void ParseExpression_OnlyOneNumber_ResturnsCorrectResult()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            string input = "123456";
            string[] expected = new string[] { "123456"};
            //act
            string[] result = (string[])RunParseExpression(calculation, input);
            //assert
            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public void ParseExpression_LongExpression_ResturnsCorrectResult()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            string input = "12+34-56*78/90";
            string[] expected = new string[] { "12", "+", "34","-", "56", "*", "78", "/", "90" };
            //act
            string[] result = (string[])RunParseExpression(calculation, input);
            //assert
            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public void ParseExpression_WithNegativeNumbers_ResturnsCorrectResult()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            string input = "12+-34*-56/-78+90";
            string[] expected = new string[] { "12", "+", "-34", "*", "-56", "/", "-78", "+", "90" };
            //act
            string[] result = (string[])RunParseExpression(calculation, input);
            //assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_ValidInput_ReturnsCorrectResult()
        {
            //arrange
            var calculator = new CalculationUserInput();
            var elements = new string[] { "2", "+", "3", "*", "4", "-", "5" };

            //act
            var result = RunCalculateMethod(calculator,elements);

            //assert
            Assert.AreEqual("9", result);
        }

        [TestMethod]
        public void Calculate_DivideByZeroException_ReturnsDivideByZeroException()
        {
            //arrange
            var calculator = new CalculationUserInput();
            var elements = new string[] { "2", "/", "0" };
            string expected = GetPrivateField<string>(calculator, "_divideByZeroExceptionMessage");
            //act
            string result = RunCalculateMethod(calculator, elements);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_InputException_ReturnsInputExceptionMessage()
        {
            // Arrange
            var calculator = new CalculationUserInput();
            var elements = new string[] { "2", "%", "3" };
            string expected = GetPrivateField<string>(calculator, "_inputExceptionMessage");
            // Act
            var result = RunCalculateMethod(calculator, elements);
            // Assert
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void CalculationLoop_ValidInput_StackContainsCorrectResult()
        {
            // Arrange
            var calculator = new CalculationUserInput();
            var stack = new Stack<double>();
            stack.Push(2.0);
            stack.Push(3.0);
            var operators = new Stack<char>();
            operators.Push('+');

            // Act
            var result = RunCalculationLoopMethod(calculator, stack, operators, null);

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(5.0, stack.Peek());
        }

        [TestMethod]
        public void CalculationLoop_DivideByZeroException_ReturnsMessageAboutException()
        {
            // arrange
            var calculator = new CalculationUserInput();
            var stack = new Stack<double>();
            stack.Push(2.0);
            stack.Push(0.0);
            var operators = new Stack<char>();
            operators.Push('/');
            string expected = GetPrivateField<string>(calculator, "_divideByZeroExceptionMessage");

            // act
            string? result = RunCalculationLoopMethod(calculator, stack, operators, null);
            // assert
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void CalculationLoop_InputException_ReturnsNull()
        {
            // Arrange
            var calculator = new CalculationUserInput();
            var stack = new Stack<double>();
            stack.Push(2.0);
            stack.Push(3.0);
            var operators = new Stack<char>();
            operators.Push('%');
            string expected = GetPrivateField<string>(calculator, "_inputExceptionMessage");
            // Act
            var result = RunCalculationLoopMethod(calculator, stack, operators, null);

            // Assert
            Assert.AreEqual(result, null);
        }


        [DataTestMethod]
        [DataRow("123",true)]
        [DataRow("123,45", true)]
        [DataRow("-50", true)]
        [DataRow("-50,66", true)]
        [DataRow("0", true)]
        [DataRow("(-5)", false)]
        [DataRow("(2)", false)]
        [DataRow("number", false)]
        public void IsNum_CorrectValues_ReturnsCorrectResult(string stringInput, bool expected)
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            double numInput = 0;
            //act
            bool result = RunIsNumMethod(calculation, stringInput, numInput);
            //assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("+",true)]
        [DataRow("-",true)]
        [DataRow("*", true)]
        [DataRow("/", true)]
        [DataRow("?", false)]
        [DataRow(".", false)]
        [DataRow(":", false)]
        [DataRow("^", false)]
        [DataRow("#", false)]
        public void IsOperator_Correctvalues_ReturnsCorrectResult(string input, bool expected)
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            //act
            bool result = (bool)RunIsOperatorMethod(calculation, input);
            //assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(2d,2d,'+',4d)]
        [DataRow(-15.2d, 16d, '+', 0.8d)]
        [DataRow(10d, 25d, '-', -15d)]
        [DataRow(-5d,5d,'-',-10d)]
        [DataRow(18d, 3d, '/', 6d)]
        [DataRow(-5d,2.5d,'/',-2d)]
        [DataRow(10d,10d,'*',100d)]
        [DataRow(-5d, 5d, '*', -25d)]
        public void PerformOperation_Correctvalues_ReturnsCorrectResult(double num1, double num2, char operation, double expected)
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            //act
            double result = (double)RunPerformOperation(calculation, num1, num2, operation );
            //assert
            Assert.AreEqual(expected, result,2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvocationExpression))]
        public void PerformOperation_DividingByZero_ThrowDivByZeroEx()
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            double num1 = 5, num2 = 0;
            char operation = '/';
            //act
            RunPerformOperation(calculation, num1, num2, operation);
        }

        [DataTestMethod]
        [DataRow('*', '+', true)]
        [DataRow('*', '-', true)]
        [DataRow('/', '+', true)]
        [DataRow('/', '-', true)]
        [DataRow('/', '*', false)]
        [DataRow('*', '/', false)]
        [DataRow('+', '-', false)]
        [DataRow('-', '+', false)]
        [DataRow('8', '3', false)]
        public void IsHigherPrecidence_Correctvalues_ReturnsCorrectResult(char op1, char op2, bool expected)
        {
            //arrange
            CalculationUserInput calculation = new CalculationUserInput();
            //act
            bool result = (bool)RunPrivateMethod(calculation, "IsHigherPrecidence", new object[] { op1, op2 });
            //assert
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void IsHigherPrecidence_Correct_ReturnsCorrectResult()
        {
            //arrange
            char op1 = '/', op2 = '+';
            bool expected = true;
            CalculationUserInput calculation = new CalculationUserInput();
            //act
            bool result = (bool)RunPrivateMethod(calculation, "IsHigherPrecidence", new object[] { op1, op2 });
            //assert
            Assert.AreEqual(result, expected);
        }

        //----------------------------------------------------------------------------------------------


        private static object RunParseExpression(CalculationUserInput instance, string input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("ParseExpression", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return result;
        }

        private static object RunPerformOperation(object instance, double num1, double num2, char op)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("PerformOperation", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { num1, num2, op });
            return result;
        }
        private static object RunPrivateMethod(object instance, string methodName, object[] input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return result;
        }

        private static bool RunIsOperatorMethod(object instance, string input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("IsOperator", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (bool)result;
        }

        private static bool RunIsNumMethod(object instance, string input, double parsedNum)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("IsNum", BindingFlags.NonPublic | BindingFlags.Instance);
            var numParameter = new object[] { input, 0.0 };
            var result = method?.Invoke(instance, numParameter);
            return (bool)result;
        }

        private static string? RunCalculationLoopMethod(object instance, Stack<double> stack, Stack<char> operators, char? currentOperation)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("CalculationLoop", BindingFlags.NonPublic | BindingFlags.Instance);
            var stackParameter = stack;
            var operatorsParameter = operators;
            var currentOperationParameter = currentOperation;
            var result = method?.Invoke(instance, new object[] { stackParameter, operatorsParameter, currentOperationParameter });
            return (string?)result;
        }

        private static string RunCalculateMethod(object instance, string[] input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("Calculate", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (string)result;
        }

        private static void SetPrivateFieldValue(object instance, string fieldName, object value)
        {
            var type = instance.GetType();
            type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, value);
        }

        private static T GetPrivateField<T>(object instance, string fieldName)
        {
            var type = instance.GetType();
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(instance);
        }

    }
}