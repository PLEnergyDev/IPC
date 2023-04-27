package JSocketComm;

import javax.lang.model.type.ArrayType;
import java.lang.reflect.Array;
import java.nio.ByteBuffer;
import java.util.Arrays;
import java.util.function.Consumer;
import java.util.function.Function;

public class SimpleConversion {

    //Assumes equally sized 2d array
    public static <T> ByteBuffer ArrayToBytes(T[][] arr, Function<T,ByteBuffer> converter){
        var rows = arr.length;
        var columns = arr[0].length;
        var elements = rows * columns;
        var byteSize = converter.apply(arr[0][0]).capacity();
        var result = ByteBuffer.allocate(3 * Integer.BYTES + elements * byteSize);
        result.putInt(2);
        result.putInt(rows);
        result.putInt(columns);
        for (T[] row : arr) {
            for (var e : row) {
                result.put(converter.apply(e).rewind());
            }
        }
        return result;
    }

    public static <T> ByteBuffer ArrayToBytes(T[] arr, Function<T,ByteBuffer> converter){
        var len = arr.length;
        var byteSize = converter.apply(arr[0]).capacity();
        var result = ByteBuffer.allocate(3 * Integer.BYTES + len * byteSize);
        result.putInt(1);
        result.putInt(len);
        for (var e : arr) {
            result.put(converter.apply(e).rewind());
        }
        return result;
    }

    public static <T> Object BytesToArray(ByteBuffer buf, Function<ByteBuffer,T> converter){
        var rank =  buf.getInt();
        var dimension = new int[rank];
        var elements = 1;
        for(var i = 0; i < rank; i ++){
            var d = buf.getInt();
            dimension[i] = d;
            elements *= d;
        }
        var pos = buf.position();
        var obj = converter.apply(buf);
        buf = buf.position(pos);
        var result = Array.newInstance(obj.getClass(), dimension);


        var position = new int[rank];

        //TODO: works only when all elements have the same size
        var size = (buf.capacity() - buf.position()) / elements;
        boolean stop = false;
        for (int i = 0; i < elements; i++)
        {
            var e = converter.apply(buf);
            Consumer<int[]> arraySet = (int[] dims) -> {
                var d = 0;
                var ar = result;
                while(ar.getClass().isArray()){
                    var v =  ((Object[])ar)[dims[d]];
                    if(v != null){
                        ar = v;
                    }
                    else{
                        break;
                    }
                    d++;
                }
                ((Object[])ar)[dims[d]] = e;
            };
            arraySet.accept(position);
            position[rank-1]++;
            for (var j = rank -1; j >= 0; j--)
            {
                if (position[j] >= dimension[j])
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
                        throw new ArithmeticException("Number of elements in array does not match up");
                    }
                    break;
                }
            }
        }
        return result;
    }


}
