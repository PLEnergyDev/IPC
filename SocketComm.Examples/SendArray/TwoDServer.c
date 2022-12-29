#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>
#include <limits.h>

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = serveSingleClient("/tmp/TwoDArray.pipe");

    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    printf("Connection successful!\n");
    expectCmd(s, Ready);

    int i[] = {0,1,2,3,4,5,6,7,8,9}, dimension[] = {5,2};
    printf("Sending [");
    for(int j = 0; j < 5; j++ )
    {
        printf("[");
        for(int k = 0; k < 2; k++){
            printf("%d, ", i[j*2+k]);
        }
        printf("],");
    }
    printf("]\n");

    sendArray(s, i, sizeof(int), 2, dimension, arrayToBytesGeneric);
    expectCmd(s, Ready);
    writeCmd(s, Go);
    expectCmd(s, Done);
    expectCmd(s, Receive);
    int* res = (int*) receiveValue(s, byteToNumberGeneric, sizeof(int));
    printf("Received %d", *res);
    free(res);

    writeCmd(s, Exit);
    closeSocket(s);
}