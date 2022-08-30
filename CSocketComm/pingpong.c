#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "scomm.h"
#include "cmd.h"

void* byteToIntGeneric(char* buf, size_t size){
    int* result = malloc(size);
    if(isLittleEndian())
        reverseArray(buf, size);
    memcpy(result, buf, size);
    return result;
}

char* intToByteGeneric(void* value, size_t size){
    char* buf = malloc(size);
    memcpy(buf, value, size);
    if(isLittleEndian())
        reverseArray(buf, size);
    return buf;
}

int main(int argc, char **argv){
    printf("Is little endian: %d", isLittleEndian());
    if(argc<=1){
        fprintf(stderr,"\nUsage: %s [socket]\n\n", argv[0]);
        exit(EXIT_FAILURE);
    }
    int s = connectTo(argv[1]);
    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    writeCmd(s, Ready);
    CMD c = readCmd(s);
    while(c == Receive){
        int* i = (int*)receiveValue(s,byteToIntGeneric);
        printf("Client received: %d\n", *i);
        *i += 1;
        sendValue(s, i, sizeof(*i),intToByteGeneric );
        printf("Client sent: %d\n", *i);
        c = readCmd(s);
        free(i);
    }
    closeSocket(s);
}
