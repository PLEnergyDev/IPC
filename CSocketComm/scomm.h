#include "cmd.h"
#ifndef SCOMM_H
#define SCOMM_H
void writeCmd(int socket, CMD cmd);
CMD readCmd(int socket);
int connectTo(char *path);
int serveSingleClient(char *path);
int shakeHands(int socket);
int receiveHandshake(int socket);
void closeSocket(int socket);
#endif