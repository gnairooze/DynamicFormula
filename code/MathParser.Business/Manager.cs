using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Business
{
    public class Manager
    {
        public void Demo()
        {
            string formula = "2-(32-4)/(23+4/5)-(2-4)*(4+6-98.2)+4";

            Expression e = new Expression(formula);

            Console.WriteLine($"Formula is: {formula}");
            Console.WriteLine($"Output = {e.calculate()}");
        }

        public Result SimpleExpression(string formula)
        {
            Result result = new Result();
            result.Succeeded = true;

            StringBuilder bld = new StringBuilder();

            try
            {
                Expression e = new Expression(formula);
                result.Value = e.calculate();

                bld.AppendLine($"Formula is: {formula}");
                bld.AppendLine($"Formula output: {result.Value.ToString("R")}");

                result.Description = bld.ToString();
            }
            catch (Exception ex)
            {
                result.Succeeded = false;

                bld.Clear();
                bld.AppendLine("Exception Message");
                bld.AppendLine(ex.Message);
                bld.AppendLine("Exception StackTrace");
                bld.AppendLine(ex.StackTrace);

                result.Description = bld.ToString();
                result.Value = double.NaN;
            }

            return result;
        }

        private List<Argument> ConvertToArguments(Dictionary<string, Variable> variables)
        {
            List<Argument> arguments = new List<Argument>();

            //add free arguments
            foreach (var variable in variables)
            {
                Argument argument = null;

                if(variable.Value.DependentVariables.Count == 0)
                {
                    argument = new Argument(variable.Key, double.Parse(variable.Value.Value));
                    arguments.Add(argument);
                }
            }

            //add dependent arguments
            foreach (var variable in variables)
            {
                Argument argument = null;

                if (variable.Value.DependentVariables.Count > 0)
                {
                    List<Argument> dependencies = new List<Argument>();

                    foreach (var dependent in variable.Value.DependentVariables)
                    {
                        dependencies.Add(arguments.Find(a => a.getArgumentName() == dependent));
                    }

                    argument = new Argument(variable.Key, variable.Value.Value, dependencies.ToArray());
                    arguments.Add(argument);
                }
            }

            return arguments;
        }
        public Result ArgumentExpression(Dictionary<string,Variable> variables, string formula)
        {
            Result result = new Result();
            result.Succeeded = true;

            StringBuilder bld = new StringBuilder();

            try
            {
                Expression e = new Expression(formula);

                List<Argument> arguments = ConvertToArguments(variables);

                e.addArguments(arguments.ToArray());

                result.Value = e.calculate();

                bld.AppendLine($"Formula is: {formula}");

                foreach (var argument in arguments)
                {
                    bld.AppendLine($"{argument.getArgumentName()} [{argument.getArgumentExpressionString()}] = {argument.getArgumentValue().ToString("R")}");
                }

                bld.AppendLine($"Formula output: {result.Value.ToString("R")}");

                result.Description = bld.ToString();
            }
            catch (Exception ex)
            {
                result.Succeeded = false;

                bld.Clear();
                bld.AppendLine("Exception Message");
                bld.AppendLine(ex.Message);
                bld.AppendLine("Exception StackTrace");
                bld.AppendLine(ex.StackTrace);

                result.Description = bld.ToString();
                result.Value = double.NaN;
            }

            return result;
        }
    }
}
