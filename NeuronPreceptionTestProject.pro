#-------------------------------------------------
#
# Project created by QtCreator 2016-04-13T15:34:34
#
#-------------------------------------------------

!include( NeuronPeception.pri ) {
    error( "Couldn't find the protobuff.pri file!" )
}


QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = NeuronPreceptionTestProject
TEMPLATE = app

#INCLUDEPATH += $$PWD/Windows/include

SOURCES += main.cpp\
        mainwindow.cpp \
        TestClass.cpp

HEADERS  += mainwindow.h \
            TestClass.h

FORMS    += mainwindow.ui

DISTFILES +=

#win32:CONFIG(release, debug|release): LIBS += -L$$PWD/Windows/lib/x86/ -lNeuronDataReader
#else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/Windows/lib/x86/ -lNeuronDataReader

#INCLUDEPATH += $$PWD/Windows/lib/x86
#DEPENDPATH += $$PWD/Windows/lib/x86


