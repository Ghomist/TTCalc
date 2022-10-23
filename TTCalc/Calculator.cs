using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCalc;

internal class Calculator
{
    private bool _isEnable = false;
    private string _expression = "";
    private HashSet<string> _symbols;
    private Queue<string> _expQueue;

    internal bool IsEnable => _isEnable;
    //internal Queue<string> ExpQueue => _expQueue;
    internal string ReversePolish
    {
        get
        {
            if (!_isEnable) return "";
            var sb = new StringBuilder();
            var queue = new Queue<string>(_expQueue);
            while (queue.TryDequeue(out var s)) sb.Append(s);
            return sb.ToString();
        }
    }
    internal string Expression
    {
        get => _expression;
        set
        {
            _expression = value;
            var optionStack = new Stack<OptionType>();
            var expQueue = new Queue<string>();
            var symbols = new HashSet<string>();
            int i = 0;
            try
            {
                while (i < value.Length)
                {
                    char c = value[i];
                    if (OptionHelper.TryParse(c, out var current))
                    {
                        if (current == OptionType.RightBracket)
                        {
                            while (optionStack.TryPop(out var option) && option != OptionType.LeftBracket)
                            {
                                expQueue.Enqueue(OptionHelper.GetChar(option).ToString());
                            }
                        }
                        else if (current == OptionType.LeftBracket)
                        {
                            optionStack.Push(current);
                        }
                        else
                        {
                            while (optionStack.TryPeek(out var option) && OptionHelper.ComparePriority(option, current) >= 0)
                            {
                                optionStack.Pop();
                                expQueue.Enqueue(OptionHelper.GetChar(option).ToString());
                            }
                            optionStack.Push(current);
                        }
                        i++;
                    }
                    else
                    {
                        int start = i;
                        i++;
                        while (i < value.Length && OptionHelper.Parse(value[i]) == OptionType.None) i++;
                        var symbol = value[start..i];
                        expQueue.Enqueue(symbol);
                        symbols.Add(symbol);
                    }
                }
                while (optionStack.TryPop(out var option))
                {
                    expQueue.Enqueue(OptionHelper.GetChar(option).ToString());
                }

                _expQueue = expQueue;
                _symbols = symbols;
                _isEnable = true;
            }
            catch (Exception) { _isEnable = false; }
        }
    }
    internal List<string> Symbols => _symbols.ToList();
    internal bool[] Answers
    {
        get
        {
            if (!_isEnable) return new bool[0];
            var symbols = Symbols;
            int max = 1 << _symbols.Count;
            var ans = new bool[max];
            try
            {
                for (int i = 0; i < max; i++)
                {
                    var ansStack = new Stack<bool>();
                    var exp = new Queue<string>(_expQueue);
                    while (exp.TryDequeue(out var str))
                    {
                        if (OptionHelper.TryParse(str[0], out var option))
                        {
                            if (option == OptionType.Invert)
                            {
                                ansStack.Push(!ansStack.Pop());
                            }
                            else
                            {
                                var a = ansStack.Pop();
                                var b = ansStack.Pop();
                                ansStack.Push(option switch
                                {
                                    OptionType.And => a && b,
                                    OptionType.Or => a || b,
                                    OptionType.Xor => a ^ b,
                                    _ => false
                                });
                            }
                        }
                        else
                        {
                            switch (str)
                            {
                                case "0": ansStack.Push(false); break;
                                case "1": ansStack.Push(true); break;
                                default: ansStack.Push(((1 << (symbols.Count - 1 - symbols.IndexOf(str))) & i) > 0); break;
                            }
                        }
                    }
                    ans[i] = ansStack.Pop();
                }
                return ans;
            }
            catch (Exception) { return new bool[max]; }
        }
    }
}

