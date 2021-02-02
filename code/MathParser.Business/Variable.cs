using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Business
{
    public class Variable
    {
        private List<string> _DependentVariables = new List<string>();
        public string Name { get; set; }
        public string Value { get; set; }

        public List<string> DependentVariables
        {
            get
            {
                return this._DependentVariables;
            }
        }
    }
}
