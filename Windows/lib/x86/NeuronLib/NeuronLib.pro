#-------------------------------------------------
#
# Project created by QtCreator 2016-04-13T17:53:47
#
#-------------------------------------------------

QT       -= gui

TARGET = NeuronLib
TEMPLATE = lib

DEFINES += NEURONLIB_LIBRARY

SOURCES += neuronlib.cpp

HEADERS += neuronlib.h\
        neuronlib_global.h

unix {
    target.path = /usr/lib
    INSTALLS += target
}
