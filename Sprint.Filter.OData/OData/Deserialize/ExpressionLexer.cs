using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using JetBrains.Annotations;
using Sprint.Filter.Extensions;
using Sprint.Filter.Helpers;
using Sprint.Filter.OData.Common;

namespace Sprint.Filter.OData.Deserialize
{
    internal class ExpressionLexer
    {
        private enum StringType
        {
            None,
            Binary,
            DateTime,
            Guid,
            Time,
            DateTimeOffset
        }

        private static readonly CultureInfo ParseCulture = CultureInfo.InvariantCulture;        
        private static readonly Regex GuidRegex = new Regex(@"([a-f0-9\-]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);        
        private static readonly Regex TimeSpanRegex = new Regex(@"(P.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly string _source;
        private int _offset;
        private int _current;

        public ExpressionLexer([NotNull]string source)
        {
            _source = source;
        }

        internal ODataLambdaExpression BuildLambdaExpression()
        {
            var parameter = ODataExpression.Parameter("t");

            var tokens = ConvertToRpn(Scan(parameter));

            var expression = CreateExpression(tokens);

            return new ODataLambdaExpression
            {
                Body = expression,
                Parameters = new[] { parameter }
            };
        }

        internal IEnumerable<ODataExpression> Scan(ODataParameterExpression parameter)
        {
            _offset = 0;
            _current = 0;

            var tokens = new List<ODataExpression>();
            ODataExpression token;

            while ((token = GetNext(parameter)) != null)
                tokens.Add(token);

            return tokens.ToArray();
        }

        private ODataExpression GetNext(ODataParameterExpression parameter, ODataExpression parent = null, IDictionary<string, ODataParameterExpression> lambdaParameters = null)
        {
            if (_offset >= _source.Length)
                return null;

            while (_offset < _source.Length && Char.IsWhiteSpace(_source[_offset]))
                _offset++;

            if (_offset >= _source.Length)
                return null;

            _current = _offset;

            var c = _source[_current];

            switch (c)
            {
                case '-':
                    return ParseSign(parameter, parent, lambdaParameters);

                case '\'':
                    return ParseString();

                case '(':
                case ')':
                case ',':
                case ':':                    
                        return ParseSyntax();                    
                default:
                    if (Char.IsNumber(c))
                    {
                        return ParseNumeric();
                    }
                    if (IsIdentifierStartChar(c))
                    {
                        return ParseIdentifier(false, parameter, parent, lambdaParameters);
                    }
                    throw new Exception(String.Format(
                        "Unexpecter character '{0}' at offset {1}.", c, _current
                        ));
            }

        }

        #region Parcers

        private Type GetType(string typeName)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == typeName);

            if (type == null)
                throw new FormatException("Could not load type " + typeName);

            return type;
        }

        private ODataConstantExpression ParceValue(string typeName, string rawValue)
        {
            var type = GetType(typeName);

            var value = TypeDescriptor.GetConverter(type).ConvertFromString(rawValue);

            return ODataExpression.Constant(value);            
        }

        private ODataConstantExpression ParseSpecialString(string value, StringType stringType)
        {
            switch (stringType)
            {
                case StringType.Binary:
                    return ParseBinaryString(value);

                case StringType.DateTime:
                    return ParseDateTimeString(value);

                case StringType.DateTimeOffset:
                    return ParseDateTimeOffsetString(value);

                case StringType.Guid:
                    return ParseGuidString(value);

                case StringType.Time:
                    return ParseTimeString(value);

                default:
                    throw new ArgumentOutOfRangeException("stringType");
            }
        }

        private ODataConstantExpression ParseBinaryString(string value)
        {
            if (value.Length % 2 == 0)
            {
                var result = new byte[value.Length / 2];

                for (var i = 0; i < result.Length; i++)
                {
                    if (Util.IsHex(value[i * 2]) && Util.IsHex(value[i * 2 + 1]))
                    {
                        result[i] = (byte)(Util.HexToInt(value[i * 2]) * 16 + Util.HexToInt(value[i * 2 + 1]));
                    }
                    else
                    {
                        throw new Exception(String.Format(
                            "Binary format is invalid at {0}.", _offset
                        ));
                    }
                }

                return ODataExpression.Constant(result);
            }

            throw new Exception(String.Format("Binary format is invalid at {0}.", _offset));
        }

        private ODataConstantExpression ParseDateTimeString(string value)
        {
            var dateTime = DateTimeHelper.Parse(value);

            return ODataExpression.Constant(dateTime);
        }

        private ODataConstantExpression ParseDateTimeOffsetString(string value)
        {
            var dateTimeOffset = XmlConvert.ToDateTimeOffset(value);
            return ODataExpression.Constant(dateTimeOffset);
        }

        private ODataConstantExpression ParseGuidString(string value)
        {
            var match = GuidRegex.Match(value);

            if (match.Success)
            {
                Guid guid;
                if (Guid.TryParse(match.Groups[1].Value, out guid))
                {
                    return ODataExpression.Constant(guid);
                }
            }

            throw new FormatException(String.Format("Could not read '{0}' as Guid at {1}.", value, _offset));            
        }

        private ODataConstantExpression ParseTimeString(string value)
        {
            var match = TimeSpanRegex.Match(value);
            if (match.Success)
            {
                try
                {
                    var timespan = XmlConvert.ToTimeSpan(match.Groups[1].Value);
                    return ODataExpression.Constant(timespan);
                }
                catch
                {
                    throw new FormatException("Could not read " + value + " as TimeSpan.");
                }
            }            

            throw new Exception(String.Format("Duration format is invalid at {0}.", _offset));
        }

        private ODataExpression ParseSyntax()
        {
            ODataExpression token;

            switch (_source[_current])
            {
                case '(': token = new ODataSyntaxExpression('('); break;
                case ')': token = new ODataSyntaxExpression(')'); break;
                case ',': token = new ODataSyntaxExpression(','); break;
                case ':': token = new ODataSyntaxExpression(':'); break;
                default: throw new InvalidOperationException("Unknown token");
            }

            _offset = _current + 1;

            return token;
        }

        private ODataConstantExpression ParseString()
        {
            var sb = new StringBuilder();
            var hadEnd = false;

            for (_current++; _current < _source.Length; _current++)
            {
                char c = _source[_current];

                if (c == '\'')
                {
                    // Two consecutive quotes translate to a single quote in
                    // the string. This is not in the spec (2.2.2), but seems
                    // the logical thing to do (and at StackOverflow on
                    // http://stackoverflow.com/questions/3979367 they seem
                    // to think the same thing).

                    if (
                        _current < _source.Length - 1 &&
                        _source[_current + 1] == '\''
                    )
                    {
                        _current++;
                        sb.Append('\'');
                    }
                    else
                    {
                        hadEnd = true;

                        break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (!hadEnd)
            {
                throw new Exception(String.Format(
                    "Unterminated string starting at {0}.", _offset
                ));
            }

            _offset = _current + 1;

            return ODataExpression.Constant(sb.ToString());
        }

        private ODataExpression ParseSign(ODataParameterExpression parameter, ODataExpression parent = null, IDictionary<string, ODataParameterExpression> lambdaParameters = null)
        {
            _current++;

            return Char.IsDigit(_source[_current])
                ? ParseNumeric()
                : ParseIdentifier(true, parameter, parent, lambdaParameters);
        }

        private ODataConstantExpression ParseNumeric()
        {
            var floating = false;
            char c;

            for (_current++; _current < _source.Length; _current++)
            {
                c = _source[_current];

                if (c == '.')
                {
                    if (floating)
                        break;

                    floating = true;
                }
                else
                {
                    if (!Char.IsDigit(c))
                        break;
                }
            }

            var haveExponent = false;

            if (_current < _source.Length)
            {
                c = _source[_current];

                if (c == 'E' || c == 'e')
                {
                    _current++;

                    if (_source[_current] == '-')
                        _current++;

                    var exponentEnd = _current == _source.Length ? null : SkipDigits(_current);

                    if (!exponentEnd.HasValue)
                    {
                        throw new Exception(String.Format(
                            "Expected digits after exponent at {0}.", _offset
                        ));
                    }

                    _current = exponentEnd.Value;

                    haveExponent = true;

                    if (_current < _source.Length)
                    {
                        c = _source[_current];

                        if (c == 'm' || c == 'M')
                        {
                            throw new Exception(String.Format(
                                "Unexpected exponent for decimal literal at {0}.", _offset
                            ));
                        }
                        if (c == 'l' || c == 'L')
                        {
                            throw new Exception(String.Format(
                                "Unexpected exponent for long literal at {0}.", _offset
                                ));
                        }
                    }
                }
            }

            var text = _source.Substring(_offset, _current - _offset);
            object value;


            if (_current < _source.Length)
            {
                c = _source[_current];

                switch (c)
                {
                    case 'F':
                    case 'f':
                        value = float.Parse(text, ParseCulture);
                        _current++;
                        break;

                    case 'D':
                    case 'd':
                        value = double.Parse(text, ParseCulture);
                        _current++;
                        break;

                    case 'M':
                    case 'm':
                        value = decimal.Parse(text, ParseCulture);
                        _current++;
                        break;

                    case 'L':
                    case 'l':
                        value = long.Parse(text, ParseCulture);
                        _current++;
                        break;

                    default:
                        if (floating || haveExponent)
                            value = double.Parse(text, ParseCulture);
                        else
                            value = int.Parse(text, ParseCulture);
                        break;
                }
            }
            else
            {
                if (floating || haveExponent)
                    value = double.Parse(text, ParseCulture);
                else
                    value = int.Parse(text, ParseCulture);
            }

            _offset = _current;

            return ODataExpression.Constant(value);
        }

        private int? SkipDigits(int current)
        {
            if (!Char.IsDigit(_source[current]))
                return null;

            current++;

            while (current < _source.Length && Char.IsDigit(_source[current]))
                current++;

            return current;
        }

        private bool IsIdentifierStartChar(char c)
        {
            // Definition for names taken from
            // http://msdn.microsoft.com/en-us/library/aa664670.aspx.
            // Only exception here is that we also include the '$'
            // sign. This is for working with '$value' which has a
            // special meaning.

            return c == '_' || c == '$' || c=='.' || Char.IsLetter(c);
        }
        private bool IsIdentifierChar(char c)
        {
            return IsIdentifierStartChar(c) || Char.IsDigit(c);
        }

        private ODataExpression ParseIdentifier(bool minus, ODataParameterExpression parameter, ODataExpression parent = null, IDictionary<string, ODataParameterExpression> lambdaParameters = null)
        {
            for (_current++; _current < _source.Length; _current++)
            {
                var c = _source[_current];

                if (!IsIdentifierChar(c))
                    break;
            }

            var name = _source.Substring(_offset, _current - _offset);

            var lastOffset = _offset;
            _offset = _current;

            switch (name)
            {
                case "INF":
                    return ODataExpression.Constant(double.PositiveInfinity);

                case "-INF":
                    return ODataExpression.Constant(double.NegativeInfinity);

                case "Nan":
                    return ODataExpression.Constant(double.NaN);

                case "true":
                    return ODataExpression.Constant(true);

                case "false":
                    return ODataExpression.Constant(false);

                case "null":
                    return ODataExpression.Constant(null, typeof(object));

                case "-":
                    {
                        return new ODataUnaryExpression(ExpressionType.Negate);
                    }

                default:
                    if (minus)
                    {
                        // Reset the offset.
                        _offset = lastOffset + 1;

                        return new ODataUnaryExpression(ExpressionType.Negate);
                    }
                    break;
            }

            if (_offset < _source.Length)
            {
                switch (_source[_offset])
                {
                    case '\'':
                        {
                            StringType stringType;

                            switch (name)
                            {
                                case "X": stringType = StringType.Binary; break;
                                case "binary": stringType = StringType.Binary; break;
                                case "datetime": stringType = StringType.DateTime; break;
                                case "guid": stringType = StringType.Guid; break;
                                case "time": stringType = StringType.Time; break;
                                case "datetimeoffset": stringType = StringType.DateTimeOffset; break;
                                default: stringType = StringType.None; break;
                            }

                            if (stringType != StringType.None && _source[_offset] == '\'')
                            {
                                var content = ParseString();

                                return ParseSpecialString((string)content.Value, stringType);
                            }

                            if (stringType == StringType.None)
                            {                                
                                var content = ParseString();

                                return ParceValue(name, (string)content.Value);                                
                            }

                            break;
                        }
                    case ':':
                    {
                        _offset++;

                        var depth = 0;

                        var p = ODataExpression.Parameter(name);

                        var lp = new Dictionary<string, ODataParameterExpression>(lambdaParameters ?? new Dictionary<string, ODataParameterExpression>());

                        lp[p.Name] = p;

                        var tokens = new List<ODataExpression>();
                      
                        while(true)
                        {                            
                            var token = GetNext(parameter, null, lp);

                            if(token == null)
                                break;

                            if (token.NodeType == ExpressionType.Default)
                            {
                                var syntaxExpressionToken = (ODataSyntaxExpression)token;

                                if(syntaxExpressionToken.Syntax == ',')
                                {
                                    _offset--;
                                    break;
                                }

                                if (syntaxExpressionToken.Syntax == '(')
                                    depth++;

                                if(syntaxExpressionToken.Syntax == ')')
                                {
                                    if(depth == 0)
                                    {
                                        _offset--;
                                        break;
                                    }

                                    depth--;
                                }
                            }

                            tokens.Add(token);
                        }

                        var body = CreateExpression(ConvertToRpn(tokens));

                        var lambdaExpression = new ODataLambdaExpression
                        {
                            Parameters = new[] { p },
                            Body = body
                        };                        
                        
                        return lambdaExpression;
                    }
                    case '/':
                        {
                            _offset++;

                            if (lambdaParameters != null && lambdaParameters.ContainsKey(name) && parent == null)
                                return ParseIdentifier(false, parameter, lambdaParameters[name], lambdaParameters);

                            return ParseIdentifier(false, parameter, ODataExpression.PropertyOrField(name, parent ?? parameter), lambdaParameters);
                        }
                    case '('://Если следующий элемент скобка, значит это функция
                        {
                            var depth = 0;
                            var comma = false;
                            var arguments = new List<ODataExpression>();
                            var temp = new List<ODataExpression>();                            

                            while (true)
                            {
                                var token = GetNext(parameter, null, lambdaParameters);

                                if (token == null)
                                    break;                                
                                var syntax = token as ODataSyntaxExpression;

                                if (syntax != null && syntax.Syntax == ',')
                                {
                                    if (temp.Any())
                                    {
                                        var tokens = ConvertToRpn(temp.ToArray());

                                        var expression = CreateExpression(tokens);

                                        arguments.Add(expression);

                                        temp = new List<ODataExpression>();

                                        comma = true;
                                    }
                                    else
                                        throw new Exception("extra comma");
                                }
                                else
                                {
                                    if (syntax != null && syntax.Syntax == '(')
                                    {
                                        if (comma)
                                            throw new Exception("extra comma");

                                        depth++;
                                    }

                                    if (syntax != null && syntax.Syntax == ')')
                                    {
                                        if (comma)
                                            throw new Exception("extra comma");

                                        depth--;
                                    }

                                    if (syntax == null ||
                                       !(syntax.Syntax == '(' && depth == 1) && !(syntax.Syntax == ')' && depth == 0))
                                        temp.Add(token);

                                    comma = false;

                                    if (depth == 0)
                                    {
                                        if (temp.Any())
                                        {
                                            var tokens = ConvertToRpn(temp.ToArray());

                                            var expression = CreateExpression(tokens);
                                            

                                            arguments.Add(expression);
                                        }

                                        break;
                                    }
                                }

                            }

                            if (depth != 0)
                                throw new Exception("Parenthesis mismatch");


                            var methodCallExpression = new ODataMethodCallExpression
                            {
                                Context = parent,
                                MethodName = name,
                                Arguments = arguments.ToArray()
                            };

                            if (_offset < _source.Length && _source[_offset] == '/')
                            {
                                _current++;

                                _offset = _offset + 1;


                                return ParseIdentifier(false, parameter, methodCallExpression, lambdaParameters);
                            }


                            return methodCallExpression;

                        }
                }
            }

            if (name.IsOperator())
            {
                var expressionType = name.GetExpressionType();

                if(name.IsUnaryOperator())
                    return ODataExpression.MakeUnary(expressionType, null);                    

                if(name.IsArithmeticOperator() || name.IsLogicalOperator())
                    return ODataExpression.MakeBinary(expressionType, null, null);
            }

            if(parent == null && lambdaParameters != null && lambdaParameters.ContainsKey(name))
                return lambdaParameters[name];

            if(name.Contains("."))
            {
                var type = GetType(name);
                return ODataExpression.Constant(type);
            }

            return ODataExpression.PropertyOrField(name, parent ?? parameter);
        }

        #endregion

        private static ODataExpression CreateExpression([NotNull] ODataExpression[] tokens)
        {
            if (tokens.Length == 0)
                return null;

            var queue = new Queue<ODataExpression>(tokens);

            var stack = new Stack<ODataExpression>();

            var token = queue.Dequeue();

            while (queue.Count >= 0)
            {
                if (token.NodeType == ExpressionType.Constant || token.NodeType == ExpressionType.MemberAccess || token.NodeType == ExpressionType.Parameter || token.NodeType == ExpressionType.Call || token.NodeType == ExpressionType.Lambda)
                    stack.Push(token);
                else
                {
                    if (token.NodeType.IsOperator())
                    {
                        if (token.NodeType.IsBinary())
                        {
                            var binaryExpressionToken = ((ODataBinaryExpression)token);
                            var right = stack.Pop();
                            var left = stack.Pop();

                            binaryExpressionToken.Left = left;
                            binaryExpressionToken.Right = right;
                        }
                        else
                        {
                            ((ODataUnaryExpression)token).Operand = stack.Pop();
                        }

                        stack.Push(token);
                    }
                }


                if (queue.Count > 0)
                    token = queue.Dequeue();
                else
                    break;
            }

            return stack.Pop();
        }

        private static ODataExpression[] ConvertToRpn([NotNull]IEnumerable<ODataExpression> tokens)
        {
            var result = new List<ODataExpression>();
            var stack = new Stack<ODataExpression>();

            foreach (var token in tokens)
            {
                var tokenType = token.NodeType;

                if (tokenType == ExpressionType.Call || tokenType == ExpressionType.Constant || tokenType == ExpressionType.Parameter || tokenType == ExpressionType.MemberAccess || tokenType == ExpressionType.Lambda)
                {
                    result.Add(token);
                }

                if (tokenType.IsOperator())
                {
                    var priority = token.NodeType.Priority();
                    var isLeftAssociative = tokenType.IsLeftAssociative();

                    while (stack.Any())
                    {
                        var sc = stack.First();

                        if (sc.NodeType.IsOperator() && ((isLeftAssociative && (priority <= sc.NodeType.Priority())) || (!isLeftAssociative) && (priority < sc.NodeType.Priority())))
                            result.Add(stack.Pop());
                        else
                            break;
                    }

                    stack.Push(token);
                }

                if (tokenType == ExpressionType.Default)
                {
                    var syntaxToken = (ODataSyntaxExpression)token;
                    if (syntaxToken.Syntax == '(')
                        stack.Push(token);

                    if (syntaxToken.Syntax == ')')
                    {
                        var pe = false;

                        while (stack.Any())
                        {

                            var sc = stack.First() as ODataSyntaxExpression;
                            if (sc != null && sc.Syntax == '(')
                            {
                                pe = true;
                                break;
                            }

                            result.Add(stack.Pop());
                        }

                        if (!pe)
                            throw new Exception("Error: parentheses mismatched\n");

                        stack.Pop();

                        if (stack.Any() && stack.First().NodeType == ExpressionType.Call)
                            stack.Push(stack.Pop());
                    }
                }
            }

            // Когда не осталось токенов на входе:
            // Если в стеке остались токены:
            while (stack.Any())
            {
                var sc = stack.First() as ODataSyntaxExpression;
                if (sc != null && (sc.Syntax == '(' || sc.Syntax == ')'))
                {
                    throw new Exception("Error: parentheses mismatched\n");
                }
                result.Add(stack.Pop());
            }

            return result.ToArray();
        }
    }
}
