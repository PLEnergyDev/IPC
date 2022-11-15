cmake_minimum_required(VERSION 3.23)
project(Examples C)

set(CMAKE_C_STANDARD 11)

add_executable(RunMethod.Client
        RunMethod/client.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h)

add_executable(RunMethod.Server
        RunMethod/server.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h)