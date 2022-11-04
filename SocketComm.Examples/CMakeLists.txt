cmake_minimum_required(VERSION 3.23)
project(Examples C)

set(CMAKE_C_STANDARD 11)

add_executable(Examples
        RunMethod/client.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h)
