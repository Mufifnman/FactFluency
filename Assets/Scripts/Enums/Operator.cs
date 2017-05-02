public enum Operator
{
    Plus = '+',
    Minus = '-',
    Times = '*',
    Devide = '/',
    Equals = '=',
    None = '_'
};


public static class OperatorExtentionMethds
{
    public static string ToOperatorString(this Operator o)
    {
        // TODO: validation
        return ((char)o).ToString();
    }
}