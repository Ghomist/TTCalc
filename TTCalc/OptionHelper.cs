using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCalc;

internal class OptionHelper
{
    private static readonly IDictionary<OptionType, int> s_options = new Dictionary<OptionType, int>()
    {
        { OptionType.LeftBracket, -1 },
        { OptionType.Invert,3 },
        { OptionType.And, 2 },
        { OptionType.Or, 1 },
        { OptionType.Xor, 2 },
        { OptionType.None, 0 },
    };

    internal static int ComparePriority(OptionType type, OptionType other) => s_options[type] - s_options[other];

    internal static OptionType Parse(char c) => c switch
    {
        '!' or '~' => OptionType.Invert,
        '|' or '+' => OptionType.Or,
        '&' or '*' => OptionType.And,
        '^' => OptionType.Xor,
        ')' => OptionType.RightBracket,
        '(' => OptionType.LeftBracket,
        _ => OptionType.None
    };

    internal static bool TryParse(char c, out OptionType option)
    {
        option = Parse(c);
        return option != OptionType.None;
    }

    internal static char GetChar(OptionType option) => option switch
    {
        OptionType.Or => '|',
        OptionType.Xor => '^',
        OptionType.And => '&',
        OptionType.Invert => '!',
        OptionType.LeftBracket => '(',
        OptionType.RightBracket => ')',
        _ => '\0'
    };
}

internal enum OptionType
{
    None, And, Or, Xor, Invert, LeftBracket, RightBracket
}
