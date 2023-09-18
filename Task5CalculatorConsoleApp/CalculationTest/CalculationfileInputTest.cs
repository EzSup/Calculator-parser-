using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Calculator;

namespace CalculationTest
{
    [TestClass]
    public class CalculationfileInputTest
    {
        [TestMethod]
        public void WriteToFile_OutputFileCreated()
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            var outputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTestSolved.txt");
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            calculator.WriteToFile();
            //assert
            Assert.IsTrue(File.Exists(outputFile));
        }
        [TestMethod]
        public void ReadFile_FileExists_CorrectResult()
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            string[] expected = new string[]{ "1+2*(3+2)" };
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            string[] result = RunReadFileMethod(calculator);
            //assert
            CollectionAssert.AreEqual(result, expected);
        }

        [TestMethod]
        public void ReadFile_ProcessData_CorrectResult()
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            string[] input = new string[] { "1+2*(3+2)" , "2+15/3+4*2"};
            string[] expected = new string[] { "1+2*(3+2) -> 11" , "2+15/3+4*2 -> 15"};
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            string[] result = RunProcessDataMethod(calculator, input);
            //assert
            CollectionAssert.AreEqual(result, expected);
        }

        [TestMethod]
        public void ProcessExpressionLine_ValidInput_ReturnsCorrectResult()
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            string input = "1+2*(3+2)";
            string expected = "1+2*(3+2) -> 11";
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            string? result = RunProcessExpressionLineMethod(calculator, input);
            //assert
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void BaseCalculationMethod_ValidOutput() 
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            string input = "1+2";
            string expected = "3";
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            string? result = RunBaseCalculationMethod(calculator, input);
            //assert
            Assert.AreEqual(result, expected);
        }

        [DataTestMethod]
        [DataRow("1+2*(3+2)","1+2*5")]
        [DataRow("(5+5)*(11-2)","10*9")]
        [DataRow("4*(8+(13+2))", "4*23")]
        [DataRow("4*4", "4*4")]
        public void ReplaceExpressionsInBrackets_ValidInput_ReturnsCorrectResult(string input, string expected)
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            string result = RunReplaceExpressionsInBracketsMethod(calculator, input);
            //assert
            Assert.AreEqual(result, expected);
        }

        [DataTestMethod]
        [DataRow("4+(5-2)",2,6)]
        [DataRow("4+(3-4*(5-2))",2,12)]
        [DataRow("(5+2)", 0, 4)]
        public void FindClosingBracketIndex_ValidInput_ReturnsCorrectResult(string input, int openingBracketIndex, int expected)
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            int result = RunFindClosingBracketIndexMethod(calculator, input, openingBracketIndex);
            //assert
            Assert.AreEqual(result, expected);
        }

        [DataTestMethod]
        [DataRow("4+2", 0, -1)]
        [DataRow("(-15", 0, -1)]
        [DataRow("-15)", 0, 3)]
        public void FindClosingBracketIndex_InvalidInput_ReturnsExpectedResult(string input, int openingBracketIndex, int expected)
        {
            //arrange
            var inputFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ExpressionsTest.txt");
            CalculationFileInput calculator = new CalculationFileInput(inputFile);
            //act
            int result = RunFindClosingBracketIndexMethod(calculator, input, openingBracketIndex);
            //assert
            Assert.AreEqual(result, expected);
        }

        //---------------------------------------------------------------------------------------------------

        private static int RunFindClosingBracketIndexMethod(object instance, string input, int openingBracketIndex)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("FindClosingBracketIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input, openingBracketIndex});
            return (int)result;
        }

        private static string RunReplaceExpressionsInBracketsMethod(object instance, string input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("ReplaceExpressionsInBrackets", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (string)result;
        }

        private static string RunBaseCalculationMethod(object instance, string input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("BaseCalculationMethods", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (string)result;
        }

        private static string? RunProcessExpressionLineMethod(object instance, string input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("ProcessExpressionLine", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (string?)result;
        }

        private static object RunPrivateMethod(object instance, string methodName, object[] input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return result;
        }

        private static string[] RunReadFileMethod(object instance)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("ReadFile", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { });
            return (string[])result;
        }

        private static string[] RunProcessDataMethod(object instance, string[] input)
        {
            var type = instance.GetType();
            var method = type?.GetMethod("ProcessData", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method?.Invoke(instance, new object[] { input });
            return (string[])result;
        }
    }
}
