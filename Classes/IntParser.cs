using System;
using FB.FBAttributes;

public class IntParser
{
    private UInt32 _value;
    public IntParser(UInt32 value)
    {
        _value = value;
    }

    public UInt32 getBits(int start, int end)
    {
        int bitsCount = end - start + 1;
        UInt32 valueMask = (((UInt32)1 << bitsCount) - 1) << start;
        return (_value & valueMask) >> start;
    }

    public bool getBit(int start)
    {
        return getBits(start, start) != 0;
    }
}