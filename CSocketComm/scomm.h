#include "cmd.h"
#ifndef SCOMM_H
#define SCOMM_H
void writeCmd(int socket, CMD cmd);
CMD readCmd(int socket);
int expectCmd(int socket, CMD cmd);
void sendValue(int socket, void* value, size_t size, char*(*converter)(void*, size_t));
void* receiveValue(int socket, void*(*converter)(char*, size_t));
int connectTo(char *path);
int serveSingleClient(char *path);
int shakeHands(int socket);
int receiveHandshake(int socket);
void closeSocket(int socket);
int isLittleEndian();
void reverseArray(char* arr, size_t len);
#endif