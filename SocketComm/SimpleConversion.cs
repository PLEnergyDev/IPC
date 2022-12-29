using System.Collections;
using System.Runtime.CompilerServices;

namespace SocketComm;

public static class SimpleConversion
{
    public static byte[] NumberToBytes<T>(T value)
    {
        throw new InvalidOperationException($"Value must be a number type. Is of type {typeof(T)}");
    }
    
    //TODO: can maybe made generic with the upcoming generic math feature, see https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/ 
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

    /// <summary>
    /// Coverts the array <paramref name="arr"/> to a byte array containing first an int representing the rank of the array, then an int for the
    /// size of each dimension. Then a concatenation of the byte array for each element as the result of applying
    /// <paramref name="converter"/>
    /// </summary>
    /// <param name="arr">Array</param>
    /// <param name="converter">Function the converts object of type T to byte arrays</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static byte[] ArrayToBytes<T>(Array arr, Func<T,byte[]> converter)
    {
        //TODO: how to handle ragged arrays
        IEnumerable<byte> results = new List<byte>();
        var rank = arr.Rank;
        results = results.Concat(NumberToBytes(rank));
        for (int i = 0; i < rank; i++)
        {
            results = results.Concat(NumberToBytes(arr.GetLength(i)));
        }
        
        foreach (var e in arr)
        {
            results = results.Concat(converter((T)e));
        }

        return results.ToArray();
    }

    public static Array BytesToArray<T>(byte[] buffer, Func<byte[], T> converter)
    {
        int offset = 0;
        var rank = BytesToNumber<int>(buffer[..(offset+=sizeof(int))]);
        var dimensions = new int[rank];
        var elements = 1;
        for (int i = 0; i < rank; i++)
        {
            var v = BytesToNumber<int>(buffer[offset..(offset+=sizeof(int))]);
            dimensions[i] = v;
            elements *= v;
        }

        var result = Array.CreateInstance(typeof(T),dimensions);

        var position = new int[rank];
        
        //TODO: works only when all elements have the same size
        var size = (buffer.Length - offset) / elements;
        bool stop = false;
        for (int i = 0; i < elements; i++)
        {
            var e = converter(buffer[offset..(offset+=size)]);
            result.SetValue(e, position);
            position[rank-1]++;
            for (var j = rank -1; j >= 0; j--)
            {
                if (position[j] >= dimensions[j])
                {
                    position[j] = 0;
                    if (j > 0)
                    {
                        position[j - 1]++;
                    }
                    else
                    {
                        stop = true;
                    }
                    
                }
                else
                {
                    break;
                }

                if (stop)
                {
                    if (++i != elements)
                    {
                        throw new FormatException("Reached end of result array but not end of byte array");
                    }
                    break;
                }
            }
        }
        return result;
    }
}