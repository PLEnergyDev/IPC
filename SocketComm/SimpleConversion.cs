using System.Collections;
using System.Runtime.CompilerServices;

namespace SocketComm;

public static class SimpleConversion
{
    public static byte[] NumberToBytes<T>(T value)
    {
        throw new InvalidOperationException($"Value must be a number type. Is of type {typeof(T)}");
    }
    public static byte[] NumberToBytes(int value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(short value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(bool value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(char value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(long value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(ushort value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(uint value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(ulong value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(float value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }
    public static byte[] NumberToBytes(double value)
    {
        var result =  BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            result = result.Reverse().ToArray();
        }
        return result;
    }

    public static T BytesToNumber<T>(byte[] buffer)
    {
        return (T) _bytesToNumber<T>(buffer);
    }

    public static T BytesToNumber<T>(byte[] buffer, int index)
    {
        return (T) _bytesToNumber<T>(buffer, buffer.Length - index -1);
    }
    
    private static object _bytesToNumber<T>(byte[] buffer, int index = 0)
    {
        if (BitConverter.IsLittleEndian)
        {
            buffer = buffer.Reverse().ToArray();
        }
        switch (typeof(T).Name)
        {
            case nameof(Char):
                return BitConverter.ToChar(buffer, index);
            case nameof(Boolean):
                return BitConverter.ToBoolean(buffer, index);
            case nameof(Int16):
                return BitConverter.ToInt16(buffer, index);
            case nameof(Int32):
                return BitConverter.ToInt32(buffer, index);
            case nameof(Int64):
                return BitConverter.ToInt64(buffer, index);
            case nameof(UInt16):
                return BitConverter.ToUInt16(buffer, index);
            case nameof(UInt32):
                return BitConverter.ToUInt32(buffer, index);
            case nameof(UInt64):
                return BitConverter.ToUInt64(buffer, index);
            case nameof(Single):
                return BitConverter.ToSingle(buffer, index);
            case nameof(Double):
                return BitConverter.ToDouble(buffer, index);
            default:
                throw new InvalidOperationException($"Conversion only valid for number types. Type {typeof(T)} is incompatible");
        }
    }

    public static byte[] ArrayToBytes<T>(T[] arr, Func<T,byte[]> converter)
    {
        IEnumerable<byte> results = new List<byte>();
        var rank = arr.Rank;
        results = results.Concat(NumberToBytes(rank));
        for (int i = 0; i < rank; i++)
        {
            results = results.Concat(NumberToBytes(arr.GetLength(i)));
        }
        
        foreach (var e in arr)
        {
            results = results.Concat(converter(e));
        }

        return results.ToArray();
    }

    public static T[] BytesToArray<T>(byte[] buffer, Func<byte[], T> converter)
    {
        var rank = BytesToNumber<int>(buffer,4);
        var dimensions = new int[rank];
        var elements = 1;
        for (int i = 0; i < rank; i++)
        {
            var v = BytesToNumber<int>(buffer, (1+i) * sizeof(int));
            dimensions[i] = v;
            elements *= v;
        }

        var result = new T[elements];
        var size = (buffer.Length - (rank + 1) * sizeof(int)) / elements;
        for (int i = 0; i < elements; i++)
        {
            result[i] = BytesToNumber<T>(buffer, (rank+1) * sizeof(int) + i * size);
        }
        return result;
    }
}