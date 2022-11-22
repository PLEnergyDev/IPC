#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>
#include <float.h>

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = serveSingleClient("/tmp/floatvalue.pipe");

    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    printf("Connection successful!\n");

    expectCmd(s,Ready);
    float i = 1.1f;
    while (i < FLT_MAX && i > 1)
    {
        printf("Sending %f\n", i);
        sendValue(s, &i, sizeof(float), numberToByteGeneric);
        expectCmd(s, Ready);
        writeCmd(s, Go);
        expectCmd(s, Done);
        expectCmd(s, Receive);
        float* res = (float*) receiveValue(s, byteToNumberGeneric, sizeof(float));
        i = *res;
        free(res);
        printf("Received %f\n", i);
    }

    closeSocket(s);
}