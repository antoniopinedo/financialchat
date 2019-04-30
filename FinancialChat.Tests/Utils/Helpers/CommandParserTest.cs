using System;
using System.Text.RegularExpressions;
using FinancialChat.Utils.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialChat.Tests.Utils.Helpers
{
    [TestClass]
    public class CommandParserTest
    {
        [TestMethod]
        public void GetStockFromCommand_Match_OK()
        {
            // Shims can be used only in a ShimsContext:
            using (ShimsContext.Create())
            {
                // Arrange:
                string command = "TEST123";
                string expectedResult = "123";

                // Shim Regex.Match to return success:
                System.Text.RegularExpressions.Fakes.ShimRegex.MatchStringStringRegexOptions = (string a, string b, RegexOptions c) =>
                {
                    Match m = new Regex(@"([A-Z]+)([0-9]+)").Match(command);
                    return m;
                };               

                // Act:
                var result = CommandParser.GetStockFromCommand(command);

                // Assert:
                // This will always be true if the method is working:
                Assert.AreEqual(expectedResult, result);
            }
        }

        [TestMethod]
        public void GetStockFromCommand_NoMatch_OK()
        {
            // Shims can be used only in a ShimsContext:
            using (ShimsContext.Create())
            {
                // Arrange:
                string command = "...";
                string expectedResult = "";

                // Shim Regex.Match to return success:
                System.Text.RegularExpressions.Fakes.ShimRegex.MatchStringStringRegexOptions = (string a, string b, RegexOptions c) =>
                {
                    Match m = new Regex(@"([A-Z]+)([0-9]+)").Match(command);
                    return m;
                };

                // Act:
                var result = CommandParser.GetStockFromCommand(command);

                // Assert:
                // This will always be true if the method is working:
                Assert.AreEqual(expectedResult, result);
            }
        }

        [TestMethod]
        public void IsCommand_Returns_True()
        {
            // Shims can be used only in a ShimsContext:
            using (ShimsContext.Create())
            {
                // Arrange:
                var command = "COMMAND";

                // Shim Regex.Match to return success:
                System.Text.RegularExpressions.Fakes.ShimRegex.IsMatchStringStringRegexOptions = 
                    (string a, string b, RegexOptions c) => true;

                // Act:
                var result = CommandParser.IsCommand(command);

                // Assert:
                // This will always be true if the method is working:
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void IsCommand_Returns_False()
        {
            // Shims can be used only in a ShimsContext:
            using (ShimsContext.Create())
            {
                // Arrange:
                var command = "NOT_A_COMMAND";

                // Shim Regex.Match to return success:
                System.Text.RegularExpressions.Fakes.ShimRegex.IsMatchStringStringRegexOptions =
                    (string a, string b, RegexOptions c) => false;

                // Act:
                var result = CommandParser.IsCommand(command);

                // Assert:
                // This will always be true if the method is working:
                Assert.IsFalse(result);
            }
        }
    }
}
