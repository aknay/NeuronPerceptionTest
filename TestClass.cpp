#include "TestClass.h"

static const std::string SERVER_ADDRESS = "localhost"; //should use 'localhost' since Axis Neuron has to be run first
static const int SERVER_PORT = 7001; //default port for TCP Axis Neuron server and again should use this as default.

/** If you encountered the following error, just click 'build project' again*/
/** error: LNK1201: error writing to program database ... check for insufficient disk space, invalid path, or insufficient privilege*/

static void frameDataReceived(void * customObject, SOCKET_REF sockRef, BvhDataHeader * header, float * data)
{
    TestClass *self = reinterpret_cast<TestClass*>(customObject);
    self->showBvhBoneInfo(sockRef,header,data);
}


TestClass::TestClass()
{
    BRRegisterFrameDataCallback(this, FrameDataReceived(frameDataReceived));
}

void TestClass::testConnection()
{

    mSocketRef = BRConnectTo((char*)SERVER_ADDRESS.c_str(), SERVER_PORT);

    if (mSocketRef){
        qDebug() << "connected";
    }
    else{
        qDebug() << "disconnected";
    }

    SocketStatus status = BRGetSocketStatus(mSocketRef);
    qDebug() << "status" << status;
}

void TestClass::showBvhBoneInfo(SOCKET_REF sender, BvhDataHeader* header, float* data)
{
    int dataIndex = 0;
    /** The number 15 is right fore arm // Ref:: Appendix A: Skeleton Data Sequence in Array From "Neuron Data Reader Runtime API Documentation" */
    int currentBoneNumber = 15;
    if (header->WithDisp)
    {
        dataIndex = currentBoneNumber * 6;
        if (header->WithReference)
        {
            dataIndex += 6;
        }

        float angleX = data[dataIndex + 4];
        float angleY = data[dataIndex + 3];
        float angleZ = data[dataIndex + 5];

        qDebug() << "Angle X" << angleX << ", Angle Y" << angleY << ", Angle Z" << angleZ;
    }
}

