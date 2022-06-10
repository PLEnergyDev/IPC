#ifndef CMD_H
#define CMD_H

extern char* UNKNOWN;
extern char* ERROR;
extern char* STOPPED;
extern char* EXIT;
extern char* GO;
extern char* DONE;
extern char* READY;
extern char* OK;

typedef enum{
    Unkown = -3,
    Error = -2,
    Stopped = -1,
    Exit = 0,
    Go=1,
    Done=2,
    Ready=3,
    Ok=4
} CMD;
char* tos(CMD cmd);
CMD parseCmd(char* cmd);
#endif