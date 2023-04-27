#!/bin/bash

cd src
javac ./JSocketComm/*.java
jar cf JSocketComm.jar JSocketComm/*.class
rm JSocketComm/*.class

