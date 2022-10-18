#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "scomm.h"
#include "cmd.h"
#include "simpleConversion.h"


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
        double* i = (double*)receiveValue(s,byteToNumberGeneric);
        printf("Client received: %f\n", *i);
        *i += 1;
        sendValue(s, i, sizeof(*i),numberToByteGeneric );
        printf("Client sent: %f\n", *i);
        c = readCmd(s);
        free(i);
    }
    closeSocket(s);
}
