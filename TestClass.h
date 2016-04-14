#ifndef TESTCLASS_H
#define TESTCLASS_H

#include "Windows/include/NeuronDataReader.h"
#include <QDebug>
class TestClass
{
public:
    TestClass();
    void testConnection();
    void showBvhBoneInfo(void *sender, BvhDataHeader *header, float *data);

private:
    SOCKET_REF mSocketRef;

};

#endif // TESTCLASS_H
