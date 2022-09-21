
namespace Math.Expression.Solver.Application.Parser
{
    public class ExpressionParser
    {
        private static readonly char[] _operations = new[] { '+', '-', '*', '/', '^' };

        public static double Parse(string expression)
        {
            if (expression.Contains('('))
                return ParseParentheses(expression);
            else
                return ParseOperation(expression);
        }

        private static double ParseParentheses(string expression)
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

        private static double ParseOperation(string expression)
        {
            foreach (var operation in _operations)
            {
                var splitedExpression = expression.Split(operation);
                var skipMinus = false;
                if (operation == '-' && splitedExpression[0].Any() && splitedExpression[0].Last() < 48)
                    skipMinus = true;

                if (splitedExpression.Length > 1 && skipMinus is false)
                {
                    var firstHalf = splitedExpression[0];
                    var secondHalf = splitedExpression.TakeLast(splitedExpression.Length - 1);

                    if (firstHalf == string.Empty)
                        firstHalf = "0";

                    var a = ParseOperation(firstHalf);
                    var b = ParseOperation(string.Join(operation, secondHalf));
                    return ExecuteOperation(operation, a, b);
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
