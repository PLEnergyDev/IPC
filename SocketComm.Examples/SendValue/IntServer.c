#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>
#include <limits.h>

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = serveSingleClient("/tmp/intvalue.pipe");

    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    printf("Connection successful!\n");

    expectCmd(s,Ready);
    int i = 2;
    while (i < INT_MAX && i > 1)
    {
        printf("Sending %d\n", i);
        sendValue(s, &i, sizeof(int), numberToByteGeneric);
        expectCmd(s, Ready);
        writeCmd(s, Go);
        expectCmd(s, Done);
        expectCmd(s, Receive);
        int* res = (int*) receiveValue(s, byteToNumberGeneric, sizeof(int));
        i = *res;
        free(res);
        printf("Received %d\n", i);
    }

    closeSocket(s);
}