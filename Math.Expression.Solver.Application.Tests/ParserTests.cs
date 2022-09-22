using Math.Expression.Solver.Application.Parser;

namespace Math.Expression.Solver.Application.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("2*4-9/5+2-1/3*44", -6.4667)]
        [InlineData("44*2/3+5-1+4/5*4*6/3", 39.7333)]
        [InlineData("-44*2/3+5-1+4/5*4*6/3", -18.9333)]
        [InlineData("2*-2+2/2", -3)]
        [InlineData("-2*-2+2/2", 5)]
        [InlineData("(2+2)*4*(2-3)", -16)]
        [InlineData("(2+2)*4", 16)]
        [InlineData("(((3+4)/2*(7.5))+2*(3-7))", 254.5)]
        [InlineData("-1*(((3+4)/2*(7.5))+2*(3-7))", -254.5)]
        [InlineData("(1+3)*(4+3)", 28)]
        [InlineData("((2*4/3)*(33^(1/2)+2)/4)/3+(4^2--6)", 23.7210)]
        [InlineData("(1+3)^(4+3)", 16384)]
        [InlineData("1+(3*2)+((2-3)*3)+1", 5)]
        [InlineData("-6*-8+-5/-1/-2+4--7", 56.5)]
        [InlineData("7--3", 10)]
        [InlineData("-1--2--3--4-4-5", -1)]
        [InlineData("3*-3", -9)]
        public void Should_return_correct_result_after_parsing(string expression, double expected)
        {
            //Arrange
            var parser = new ExpressionParser();

            //Act
            var result = parser.Parse(expression);

            //Assert
            Assert.Equal(expected, result, 4);
        }
    }
}