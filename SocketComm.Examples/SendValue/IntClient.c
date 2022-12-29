#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>

int Bench(int i){
    return i * 2;
}

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = connectTo("/tmp/intvalue.pipe");
    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    printf("Connection successful!\n");

    writeCmd(s,Ready);

    CMD c;
    c = readCmd(s);
    while (c == Receive)
    {
        int* res = (int*) receiveValue(s, byteToNumberGeneric, sizeof(int));
        int i = *res;
        free(res);
        printf("Received %d\n", i);

        writeCmd(s,Ready);
        expectCmd(s,Go);
        i = Bench(i);
        writeCmd(s,Done);
        printf("Sending %d\n", i);
        sendValue(s, &i, sizeof(int), numberToByteGeneric);
        c = readCmd(s);
    }
    closeSocket(s);
}