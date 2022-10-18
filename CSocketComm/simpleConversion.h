//
// Created by adrian on 10/18/22.
//
#include <stdlib.h>
#include <string.h>
#ifndef CSOCKETCOMM_SIMPLECONVERSION_H
#define CSOCKETCOMM_SIMPLECONVERSION_H
void* byteToNumberGeneric(char* buf, size_t size);
char* numberToByteGeneric(void* value, size_t size);

#endif //CSOCKETCOMM_SIMPLECONVERSION_H
