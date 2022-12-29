#include "../../CSocketComm/scomm.h"
#include <stdio.h>

int main(int argc, char **argv){

    printf("Connecting...\n");

    int s = serveSingleClient("/tmp/hello.pipe");

    if(s<0){
        printf("Bad socket...");
        exit(EXIT_FAILURE);
    }
    printf("Connection successful!\n");
    expectCmd(s,Ready);

    while(1)
    {
        expectCmd(s,Ready);
        writeCmd(s, Go);
        expectCmd(s, Done);
        break;
    }
    writeCmd(s, Exit);
    closeSocket(s);
}