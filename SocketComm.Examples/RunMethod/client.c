#include "../../CSocketComm/scomm.h"
#include <stdio.h>

int main(int argc, char **argv){

    int s = connectTo("/tmp/hello.pipe");
    writeCmd(s,Ready);

    CMD c;


    do{
        writeCmd(s, Ready);
        c = readCmd(s);
        if(c == Go){
            printf("Hello World!\n");
            writeCmd(s, Done);
        }else{
            writeCmd(s, Error);
            break;
        }
        c = readCmd(s);
    }while(c == Ready );

    closeSocket(s);
}