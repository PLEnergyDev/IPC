//
// Created by adrian on 10/18/22.
//
#include "scomm.h"
#include "simpleConversion.h"

/// Converts a byte array as char* to a void pointer that can be cast to a number type
/// \param buf char* containing the byte array to be converted, assumes big-endian
/// \param size the size in bytes of the desired result
/// \return void* to the result
void* byteToNumberGeneric(char* buf, size_t size){
    int* result = malloc(size);
    if(isLittleEndian())
        reverseArray(buf, size);
    memcpy(result, buf, size);
    return result;
}

/// Converts a void* containing a single numeric value into a char* containing a byte array representing the value
/// \param value a void* to the value
/// \param size the size of the value
/// \return a char* containing the byte array representing the value, big-endian
char* numberToByteGeneric(void* value, size_t size){
    char* buf = malloc(size);
    memcpy(buf, value, size);
    if(isLittleEndian())
        reverseArray(buf, size);
    return buf;
}

/// Convert an array of single numeric values into a char* containing a byte array representing the array.
/// Assumes a row-first array layout. Single values will be big-endian.
/// \param array void* containing array to be converted
/// \param element_size size of each element in the array
/// \param rank the rank (number of dimensions) of the array
/// \param dimensions an int array of size rank containing the size of each dimension
/// \return a char* containing [rank, dimension sizes, values] big-endian
char* arrayToBytesGeneric(void* array, size_t element_size, int rank, int* dimensions) {
    int result_size = 0;
    result_size += sizeof(int) * (rank + 1);
    int elements = 1;
    for (int i = 0; i < rank; i++) {
        elements *= dimensions[i];
    }
    result_size += elements * element_size;
    char *result = malloc(result_size);
    int offset = 0;
    //TODO: flip in little-endian
    memcpy(result, &rank, sizeof(int));
    offset += sizeof(int);
    memcpy(result + offset, dimensions, sizeof(int) * rank);
    offset += sizeof(int) * rank;
    memcpy(result + offset, array, element_size * elements);
    if(isLittleEndian()){
        for(int i = 0; i < elements; i++){
            reverseArray(result + offset + (i * element_size), element_size);
        }
    }
    return result;
}


void* bytesToArrayGeneric(char* buffer, size_t size){
    int offset = 0;
    int rank = 0;
    memcpy(&rank, buffer, sizeof(int));
    //TODO: flip if little-endian
    offset += sizeof(int);
    int* dimension = malloc(rank * sizeof(int));
    memcpy(dimension, buffer + offset, sizeof(int) * rank);
    offset += sizeof(int) * rank;
    int result_size = 1;
    for(int i = 0; i < rank; i++){
        result_size *= dimension[i];
    }
    void* result = malloc(result_size * size);
    memcpy(result, buffer + offset, result_size * size);
    if(isLittleEndian()){
        for(int i = 0; i < result_size; i++){
            reverseArray(result + (i * size), size);
        }
    }

    return result;
}