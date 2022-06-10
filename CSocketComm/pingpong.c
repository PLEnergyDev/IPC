#include <stdlib.h>
#include <stdio.h>
#include "scomm.h"
#include "cmd.h"

int main(int argc, char **argv){
  if(argc<=1){
    fprintf(stderr,"\nUsage: %s [socket]\n\n", argv[0]);
    exit(EXIT_FAILURE);
  }
  int s = serveSingleClient(argv[1]);
  if(s<0){
    printf("Bad socket...");
    exit(EXIT_FAILURE);
  }
  CMD c;
  do{
    writeCmd(s, Go);
  } while ((c = readCmd(s))>= Go ); 
  closeSocket(s);
}
