using System;
using System.Collections.Generic;

namespace MathParser.Draft
{
    class Program
    {
        static MathParser.Business.Manager manager = new Business.Manager();
        static void Main(string[] args)
        {
            Demo1();
            int counter = Demo2();
            Demo3(counter);
        }

        private static void Demo1()
        {
            Console.WriteLine("Demo 1");
            manager.Demo();
            Console.WriteLine("---------------------------------");
        }

        private static int Demo2()
        {
            int counter = 2;
            bool moreFormula = false;

            do
            {
                Console.WriteLine($"Demo {counter++}");

                Console.WriteLine("Write the formula: (ex. 34 + 43 / 34 * 399)");

                try
                {
                    string formula = Console.ReadLine();
                    MathParser.Business.Result result = manager.SimpleExpression(formula);

                    Console.WriteLine($"Is Succeeded? {result.Succeeded}");
                    Console.WriteLine("Result.Description");
                    Console.WriteLine(result.Description);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("!!! ERROR !!!");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                
                Console.WriteLine("do you want to create another formula? (Y/n)");
                ConsoleKeyInfo answer = Console.ReadKey();

                moreFormula = answer.Key == ConsoleKey.Y;

                Console.WriteLine();
                Console.WriteLine("---------------------------------");
            } while (moreFormula);

            return counter;
        }

        private static int GetVariableCount()
        {
            Console.WriteLine("Specify how many input variables");
            string variablesCountQuestion = Console.ReadLine();
            int variablesCount;
            bool succeededInParsingVariablesCount;

            do
            {
                succeededInParsingVariablesCount = int.TryParse(variablesCountQuestion, out variablesCount);

                if (!succeededInParsingVariablesCount)
                {
                    Console.WriteLine("Please write a valid number");
                }

            } while (!succeededInParsingVariablesCount);

            return variablesCount;
        }
        private static void Demo3(int counter)
        {
            int trialCounter = 1;
            bool moreFormula = false;
            bool moreTrial = true;
            Dictionary<string, MathParser.Business.Variable> variables = new Dictionary<string, MathParser.Business.Variable>();

            do
            {
                Console.WriteLine($"Demo {counter++}");

                int variablesCount = GetVariableCount();

                variables = GenerateVariables(variablesCount);

                Console.WriteLine("Write the formula:");
                string formula = Console.ReadLine();

                do
                {
                    Console.WriteLine($"Demo {counter - 1} - Trial {trialCounter++}");

                    variables = FillVariables(variables);

                    try
                    {
                        MathParser.Business.Result result = manager.ArgumentExpression(variables, formula);

                        Console.WriteLine();
                        Console.WriteLine("---RESULT---");
                        Console.WriteLine($"Is Succeeded? {result.Succeeded}");
                        Console.WriteLine($"Output value =  {result.Value}");
                        Console.WriteLine("Result.Description");
                        Console.WriteLine(result.Description);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!!! ERROR !!!");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }


                    Console.WriteLine("----------------------------");

                    Console.WriteLine("do you want to try other values? (Y/n)");
                    ConsoleKeyInfo anotherTrial = Console.ReadKey();

                    moreTrial = anotherTrial.Key == ConsoleKey.Y;
                    Console.WriteLine();
                    
                } while (moreTrial);
                

                Console.WriteLine("do you want to create another formula? (Y/n)");
                ConsoleKeyInfo answer = Console.ReadKey();

                moreFormula = answer.Key == ConsoleKey.Y;
            } while (moreFormula);
        }

        private static MathParser.Business.Variable GetVariableValue(string variableName)
        {
            MathParser.Business.Variable variable = new Business.Variable();
            variable.Name = variableName;
            bool otherDependencyExit = true;
            while (otherDependencyExit) 
            {
                int counter = 1;
                Console.WriteLine($"Does {variableName} have dependcies?(y/N)");
                ConsoleKeyInfo hasDependencyKey = Console.ReadKey();

                otherDependencyExit = hasDependencyKey.Key == ConsoleKey.Y;
                Console.WriteLine();
                
                if (otherDependencyExit)
                {
                    Console.WriteLine($"write dependency - {counter++}");
                    string dependency = Console.ReadLine();

                    variable.DependentVariables.Add(dependency);
                }
            }
            Console.WriteLine();

            Console.WriteLine($"{variableName}.Value = ");
            string answer = Console.ReadLine();

            double variablesValue;
            bool succeededInParsingVariablesValue = false;

            do
            {
                if(variable.DependentVariables.Count == 0)
                {
                    succeededInParsingVariablesValue = double.TryParse(answer, out variablesValue);

                    if (!succeededInParsingVariablesValue)
                    {
                        Console.WriteLine("Please write a valid value");
                    }
                    else
                    {
                        variable.Value = variablesValue.ToString("R");
                    }
                }
                else
                {
                    succeededInParsingVariablesValue = true;
                    variable.Value = answer;
                }

            } while (!succeededInParsingVariablesValue);

            return variable;
        }

        private static Dictionary<string, MathParser.Business.Variable> FillVariables(Dictionary<string, MathParser.Business.Variable> variables)
        {
            Dictionary<string, MathParser.Business.Variable> filledVariables = new Dictionary<string, Business.Variable>();

            foreach (var key in variables.Keys)
            {
                MathParser.Business.Variable filledVariable = GetVariableValue(key);
                filledVariables.Add(key, filledVariable);
            }

            return filledVariables;
        }

        private static Dictionary<string, MathParser.Business.Variable> GenerateVariables(int variablesCount)
        {
            Dictionary<string, MathParser.Business.Variable> variables = new Dictionary<string, MathParser.Business.Variable>();

            for (int i = 1; i <= variablesCount; i++)
            {
                string name = $"a{i}";
                double defaultValue = 1;

                variables.Add(name, new Business.Variable() { Name = name, Value = defaultValue.ToString(),});

                Console.Write($"{name} ");
            }

            Console.WriteLine();
            return variables;
        }
    }
}
