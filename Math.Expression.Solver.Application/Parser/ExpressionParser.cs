
namespace Math.Expression.Solver.Application.Parser
{
    public class ExpressionParser
    {
        private Func<char, double, double, double> _executeOperation;

        public ExpressionParser(StepsWriter? stepsWriter = null)
        {
            _executeOperation = ExecuteOperation;

            if (stepsWriter is not null)
            {
                _executeOperation = (symbol, a, b) =>
                {
                    stepsWriter.Write($"{a}{symbol}{b}");
                    return ExecuteOperation(symbol, a, b);
                };
            }
        }

        private static readonly char[] _operations = new[] { '+', '-', '*', '/', '^' };

        public double Parse(string expression)
        {
            if (expression.Contains('('))
                return ParseParentheses(expression);
            else
                return ParseOperation(expression);
        }

        private double ParseParentheses(string expression)
        {
            var lastBeginParentheses = expression.LastIndexOf('(');

            if (lastBeginParentheses == -1)
                return ParseOperation(expression);

            var splitedExpression = expression.Substring(lastBeginParentheses, expression.Length - lastBeginParentheses);
            var firstEndParentheses = splitedExpression.IndexOf(')');

            var atomicOperation = splitedExpression[..(firstEndParentheses + 1)];
            var a = atomicOperation.Replace("(", "").Replace(")", "");
            var result = ParseOperation(a);

            expression = expression.Replace(atomicOperation, result.ToString());
            return ParseParentheses(expression);
        }

        private double ParseOperation(string expression)
        {
            foreach (var operation in _operations)
            {
                var OperationIndex = expression.LastIndexOf(operation);
                if (OperationIndex is -1) continue;

                var firstHalf = expression[..OperationIndex];
                var secondHalf = expression[(OperationIndex + 1)..];
                var skipMinus = false;

                if (operation == '-') //minus sign special cases
                {
                    if (string.IsNullOrEmpty(firstHalf)) //ex:-4*2
                        skipMinus = true;
                    else if (firstHalf.Last() == operation) //ex:3--4
                    {
                        firstHalf = firstHalf.Remove(firstHalf.LastIndexOf(operation));
                        secondHalf = $"-{secondHalf}";
                    }
                    else if (firstHalf.Any() && firstHalf.Last() < 48) //ex:3*-4
                        skipMinus = true;
                }

                if (skipMinus is false)
                {
                    var a = ParseOperation(firstHalf);
                    var b = ParseOperation(secondHalf);
                    return _executeOperation(operation, a, b);
                }
            }
            return Convert.ToDouble(expression);
        }

        private static double ExecuteOperation(char symbol, double a, double b)
        => symbol switch
        {
            '+' => a + b,
            '-' => a - b,
            '/' => a / b,
            '*' => a * b,
            '^' => System.Math.Pow(a, b),
            _ => 0,
        };
    }
}
