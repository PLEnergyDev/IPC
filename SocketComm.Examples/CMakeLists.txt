cmake_minimum_required(VERSION 3.23)
project(Examples C)

set(CMAKE_C_STANDARD 11)

add_executable(RunMethod.Client
        RunMethod/client.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h)

add_executable(RunMethod.Server
        RunMethod/server.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h)

add_executable(SendValue.IntServer
        SendValue/IntServer.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendValue.IntClient
        SendValue/IntClient.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendValue.FloatClient
        SendValue/FloatClient.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendValue.FloatServer
        SendValue/FloatServer.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendArray.OneDClient
        SendArray/OneDClient.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendArray.OneDServer
        SendArray/OneDServer.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendArray.TwoDClient
        SendArray/TwoDClient.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

add_executable(SendArray.TwoDServer
        SendArray/TwoDServer.c ../CSocketComm/scomm.h ../CSocketComm/scomm.c ../CSocketComm/cmd.h ../CSocketComm/cmd.h ../CSocketComm/simpleConversion.c ../CSocketComm/simpleConversion.h)

