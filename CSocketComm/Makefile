
pingpong: pingpong.c scomm.o cmd.o conversion.o
	gcc -o pingpong pingpong.c scomm.o cmd.o conversion.o -O3

cmd.o: cmd.c cmd.h
	gcc -c cmd.c -o cmd.o

scomm.o: scomm.c scomm.h
	gcc -c scomm.c -o scomm.o

conversion.o: simpleConversion.c simpleConversion.h scomm.o
	gcc -c simpleConversion.c -o conversion.o

