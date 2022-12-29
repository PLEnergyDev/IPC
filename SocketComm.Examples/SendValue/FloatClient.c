#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>

float Bench(float i){
    return i * 2;
}

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = connectTo("/tmp/floatvalue.pipe");
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
        float* res = (float*) receiveValue(s, byteToNumberGeneric, sizeof(float));
        float i = *res;
        free(res);
        printf("Received %f\n", i);

        writeCmd(s,Ready);
        expectCmd(s,Go);
        i = Bench(i);
        writeCmd(s,Done);
        printf("Sending %f\n", i);
        sendValue(s, &i, sizeof(float), numberToByteGeneric);
        c = readCmd(s);
    }
    closeSocket(s);
}