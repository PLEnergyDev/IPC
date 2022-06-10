#include<stdio.h>
#include<stdlib.h>
#include<sys/socket.h>
#include<sys/un.h>
#include<unistd.h>
#include "cmd.h"
#include "scomm.h"

void writeCmd(int socket, CMD cmd){
  if (write(socket, (char*)&cmd, 1) < 0) { 
      perror("error writing to socket");
  }
}

CMD readCmd(int socket){
  char ch;
  printf("Readcmd: ");
  int bytes_read = read(socket, &ch, sizeof(char));
  printf("bytesread: %d -- ", bytes_read);
  printf("ch: %d\n", ch);
  return (CMD)ch;
}

void closeSocket(int socket){
  close(socket);
}


struct sockaddr_un unix_sa(char *path){
  struct sockaddr_un address;
  memset(&address, 0, sizeof(address));
  strcpy(address.sun_path, path);
  address.sun_family = AF_UNIX;
  return address;
}

int unixsock(){
  int fd = socket(AF_UNIX, SOCK_STREAM, 0);
  if(fd <0){
    perror("socket failed");
    return -1;
  }
}

void rev(char *buf, int l){
  int n=l/2;
  for(int i=0;i<n;i++){
    char t = buf[i];
    buf[i]=buf[(l-i)-1];
    buf[(l-i)-1]=t;
  }
}

int serveSingleClient(char *path){
  if( access( path, F_OK ) >= 0)
  {
    fprintf(stderr,"File already exists: (%s)\n", path);
    return -1;
  }
  struct sockaddr_un address = unix_sa(path);
  int listensocket = unixsock();
  if( bind(listensocket, (struct sockaddr*)(&address), sizeof(struct sockaddr_un)) <0){
    perror("Bind failed");
    return -1;
  }
  if(listen(listensocket, 1)<0){
    perror("Listen failed");
    return -1;
  }
  socklen_t size = sizeof(struct sockaddr_un);
  int clientSocket = accept(listensocket, (struct sockaddr*)(&address), &size);
  close(listensocket);
  if(receiveHandshake(clientSocket)<0){
    fprintf(stderr, "Bad handsake...\n");
    return -1;
  }
  return clientSocket;
}

int connectTo(char *path){
  if( access( path, F_OK ) < 0)
  {
    fprintf(stderr,"File does not exists: (%s)\n", path);
    return -1;
  }
  struct sockaddr_un address = unix_sa(path);
  int socket = unixsock();
  if (connect(socket, (struct sockaddr*)&address, sizeof(struct sockaddr_un)) < 0) {
        perror("Connection Failed");
        return -1;
  }
  if(shakeHands(socket)<0){
    fprintf(stderr, "Bad handsake...\n");
    return -1;
  }
  return socket;
}

int receiveHandshake(int socket){
  int32_t k;
  if(4!=read(socket, (char*)&k, sizeof(int32_t))){
    printf("Error receiving handshake");
    return -2;
  }
  if(4!=write(socket, (char*)(&k), sizeof(int32_t))){
    printf("Error replying handshake!");
    return -1;
  }
}

int shakeHands(int socket){
  int32_t k = 25;
  int32_t result;
  if(4!=write(socket, (char*)(&k), sizeof(int32_t))){
    printf("Error initializing handshake!");
    return -1;
  }

  if(result!=k){
    printf("Bad handshake response..");
    return -3;
  }
  return 0;
}


// void main(){
//   int k = serveSingleClient("/tmp/foo.sock2");
//   if(k<0){
//     printf("Bad socket...");
//     exit(EXIT_FAILURE);
//   }
//   CMD cmd = Done;
//   for(int i = 0; i < 10; i++)
//   {
//     printf("sending: [%s]...", tos(cmd));
//     writeCmd(k,Done);
//     printf("OK... reading\n");
//     cmd = readCmd(k);
//     printf("received: [%s]...", tos(cmd));
//   }
//   printf("OK... reading\n");
//   readCmd(k);
//   closeSocket(k);
// }