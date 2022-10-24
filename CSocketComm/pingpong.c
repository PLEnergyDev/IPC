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

        int* i = (int*)receiveValue(s,bytesToArrayGeneric, sizeof(int));
        printf("Client received: \n");
        for(int j = 0; j < 4; j++){
            printf("%i,", i[j]);
        }
        int dimensions[] = {4};
        sendValue(s, i, 24, arrayToBytesGeneric(1, dimensions));
        free(i);
        writeCmd(s, Exit);
        c = readCmd(s);
    }
    closeSocket(s);
}
