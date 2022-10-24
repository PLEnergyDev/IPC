//
// Created by adrian on 10/18/22.
//
#include <stdlib.h>
#include <string.h>
#include <stddef.h>
#ifndef CSOCKETCOMM_SIMPLECONVERSION_H
#define CSOCKETCOMM_SIMPLECONVERSION_H
typedef char*(*converter) (char*, size_t);
void* byteToNumberGeneric(char* buf, size_t size);
char* numberToByteGeneric(void* value, size_t size);
void* bytesToArrayGeneric(char* buffer, size_t size);
converter arrayToBytesGeneric( int rank, int* dimensions);

#endif //CSOCKETCOMM_SIMPLECONVERSION_H
