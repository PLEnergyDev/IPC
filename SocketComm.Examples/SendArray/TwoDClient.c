#include "../../CSocketComm/scomm.h"
#include "../../CSocketComm/simpleConversion.h"
#include <stdio.h>
#include <stdlib.h>

int Bench(const int* i, int length){
    int sum = 0;
    for(int j = 0; j < length; j++){
        sum += i[j];
    }
    return sum;
}

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = connectTo("/tmp/TwoDArray.pipe");
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
        int* i = (int*)receiveValue(s,bytesToArrayGeneric, sizeof(int));
        printf("Received [");
        for(int j = 0; j < 5; j++ )
        {
            printf("[");
            for(int k = 0; k < 2; k++){
                printf("%d, ", i[j*2+k]);
            }
            printf("],");
        }
        printf("]\n");
        writeCmd(s, Ready);
        expectCmd(s,Go);
        int res = Bench(i, 10);
        writeCmd(s, Done);
        printf("Sending %d\n", res);
        sendValue(s, &res, sizeof(int), numberToByteGeneric);
        c = readCmd(s);
        free(i);
    }
    closeSocket(s);
}
