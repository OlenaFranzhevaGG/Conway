﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway
{
    public class CaclulationFunctionVM
    {
        public Func<decimal, decimal> FunctionForAllParsed(string FunctionForAll)
        {
            return FunctionReader.Parse(FunctionForAll);
        }
    }
}
