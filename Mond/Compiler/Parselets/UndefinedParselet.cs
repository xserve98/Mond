﻿using Mond.Compiler.Expressions;

namespace Mond.Compiler.Parselets
{
    class UndefinedParselet : IPrefixParselet
    {
        public Expression Parse(Parser parser, Token token)
        {
            return new UndefinedExpression(token);
        }
    }
}
