
namespace Math.Expression.Solver.Application.Parser
{
    public class StepsWriter
    {
        private readonly List<string> _steps;

        public StepsWriter() => _steps = new List<string>();

        public void Write(string step) => _steps.Add(step);

        public override string ToString()
        {
            for (int i = 0; i < _steps.Count; i++)
                _steps[i] = $"step {i + 1} => {_steps[i]}";

            return string.Join('\n', _steps);
        }

        public static implicit operator string(StepsWriter stepsWriter) 
            => stepsWriter.ToString();
    }
}
