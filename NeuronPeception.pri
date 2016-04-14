DEFINES += NEURON_PRECEPTION_LIB

HEADERS += $$PWD/Windows/include/NeuronDataReader.h \
           $$PWD/Windows/include/DataType.h

contains(QT_ARCH, i386) {
    message("32-bit")

win32:CONFIG(release, debug|release): LIBS += -L$$PWD/Windows/lib/x86/ -lNeuronDataReader
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/Windows/lib/x86/ -lNeuronDataReader

INCLUDEPATH += $$PWD/Windows/lib/x86
DEPENDPATH += $$PWD/Windows/lib/x86
} else {
    message("64-bit")

win32:CONFIG(release, debug|release): LIBS += -L$$PWD/Windows/lib/x64/ -lNeuronDataReader
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/Windows/lib/x64/ -lNeuronDataReader

INCLUDEPATH += $$PWD/Windows/lib/x64
DEPENDPATH += $$PWD/Windows/lib/x64


}
