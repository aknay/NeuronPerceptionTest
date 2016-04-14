using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using NeuronDataReaderWraper;

namespace demo_cs
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int                    _frameCount;
        SocketStatusChanged _SocketStatusChanged;
        FrameDataReceived _DataReceived;
        FrameDataReceived _CalcDataReceived;

        BvhDataHeader          _bvhHeader;
        private float[]        _valuesBuffer = new float[354];

        CalcDataHeader  _calcHeader;
        private float[] _valuesBufferCalc = new float[338];

        IntPtr sockTCPRef = IntPtr.Zero;
        IntPtr sockUDPRef = IntPtr.Zero;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)    //在程序加载期间做一些初始化操作
        {
            _DataReceived = new FrameDataReceived(bvhDataReceived);
            NeuronDataReader.BRRegisterFrameDataCallback(IntPtr.Zero, _DataReceived);

            _CalcDataReceived = new FrameDataReceived(calcDataReceived);
            NeuronDataReader.BRRegisterCalculationDataCallback(IntPtr.Zero, _CalcDataReceived); 

            _SocketStatusChanged = new SocketStatusChanged(socketStatusChanged);
            NeuronDataReader.BRRegisterSocketStatusCallback(IntPtr.Zero, _SocketStatusChanged);

            // 更新接收BVH数据时的BoneID界面UI
            {
                cbBoneID.Items.Clear();
                for (int i = 0; i < 59; i++)
                {
                    string Bone = "Bone";
                    Bone += i.ToString();
                    Bone += "\n";
                    cbBoneID.Items.Add(Bone);
                }
                cbBoneID.SelectedIndex = 0;
            }

            // 更新接收Calc数据时的BoneID界面UI
            {
                cbBoneID2.Items.Clear();
                for (int i = 0; i < 21; i++)
                {
                    string Bone = "Bone";
                    Bone += i.ToString();
                    Bone += "\n";
                    cbBoneID2.Items.Add(Bone);
                }
                cbBoneID2.SelectedIndex = 0;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (sockTCPRef != IntPtr.Zero)
            {
                NeuronDataReader.BRCloseSocket(sockTCPRef);
                sockTCPRef = IntPtr.Zero;
            }
                     
            if (sockUDPRef != IntPtr.Zero)
            {
                NeuronDataReader.BRCloseSocket(sockUDPRef);
                sockUDPRef = IntPtr.Zero;
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (NeuronDataReader.BRGetSocketStatus(sockTCPRef) == SocketStatus.CS_Running)
            {
                NeuronDataReader.BRCloseSocket(sockTCPRef);
                sockTCPRef = IntPtr.Zero;

                btnConnect.Content = "Connect";
            }
            else
            {
                sockTCPRef = NeuronDataReader.BRConnectTo(txtIP.Text, int.Parse(txtPort.Text));
                if (sockTCPRef == IntPtr.Zero)
                {
                    string msg = NeuronDataReader.strBRGetLastErrorMessage();
                    MessageBox.Show(msg, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                btnConnect.Content = "Disconnect";

                _frameCount = 0;
            }
        }

        private void UpdateCalcDataUI(float[] calcData, int boneId)
        {
            if (boneId > 20 || boneId < 0) return;

            tbp_x.Text = calcData[boneId * 16 + 0].ToString();
            tbp_y.Text = calcData[boneId * 16 + 1].ToString();
            tbp_z.Text = calcData[boneId * 16 + 2].ToString();

            tbv_x.Text = calcData[boneId * 16 + 3].ToString();
            tbv_y.Text = calcData[boneId * 16 + 4].ToString();
            tbv_z.Text = calcData[boneId * 16 + 5].ToString();

            tbq_w.Text =calcData[boneId * 16 + 6].ToString();
            tbq_x.Text = calcData[boneId * 16 + 7].ToString();
            tbq_y.Text = calcData[boneId * 16 + 8].ToString();
            tbq_z.Text = calcData[boneId * 16 + 9].ToString();

            tba_x.Text = calcData[boneId * 16 + 10].ToString();
            tba_y.Text = calcData[boneId * 16 + 11].ToString();
            tba_z.Text = calcData[boneId * 16 + 12].ToString();

            tbg_x.Text = calcData[boneId * 16 + 13].ToString();
            tbg_y.Text = calcData[boneId * 16 + 14].ToString();
            tbg_z.Text = calcData[boneId * 16 + 15].ToString();
        }

        private void UpdateBvhDataUI(float[] bvhData, int boneId, bool withDisp)
        {
            if (withDisp)
            {
                tbdisp_x.Text = bvhData[boneId * 6 + 0].ToString();
                tbdisp_y.Text = bvhData[boneId * 6 + 1].ToString();
                tbdisp_z.Text = bvhData[boneId * 6 + 2].ToString();

                tbrt_x.Text = bvhData[boneId * 6 + 3].ToString();
                tbrt_y.Text = bvhData[boneId * 6 + 4].ToString();
                tbrt_z.Text = bvhData[boneId * 6 + 5].ToString();
            }
            else 
            {
                if (boneId == 0)
                {
                    tbdisp_x.Text = bvhData[boneId * 6 + 0].ToString();
                    tbdisp_y.Text = bvhData[boneId * 6 + 1].ToString();
                    tbdisp_z.Text = bvhData[boneId * 6 + 2].ToString();

                    tbrt_x.Text = bvhData[boneId * 6 + 3].ToString();
                    tbrt_y.Text = bvhData[boneId * 6 + 4].ToString();
                    tbrt_z.Text = bvhData[boneId * 6 + 5].ToString();
                }
                else 
                {
                    tbrt_x.Text = bvhData[3 + boneId * 3 + 0].ToString();
                    tbrt_y.Text = bvhData[3 + boneId * 3 + 1].ToString();
                    tbrt_z.Text = bvhData[3 + boneId * 3 + 2].ToString();

                    tbdisp_x.Text = "0";
                    tbdisp_y.Text = "0";
                    tbdisp_z.Text = "0";
                }
            }
        }

        private void calcDataReceived(IntPtr customObject, IntPtr sockRef, IntPtr header, IntPtr data)
        {
            _calcHeader = (CalcDataHeader)Marshal.PtrToStructure(header, typeof(CalcDataHeader));
            // Change the buffer length if necessary
            if (_calcHeader.DataCount != _valuesBufferCalc.Length)
            {
                _valuesBufferCalc  =  new float[_calcHeader.DataCount];
            }

            Marshal.Copy(data, _valuesBufferCalc, 0, (int)_calcHeader.DataCount);

            if (sockRef == this.sockTCPRef)
            {
                _frameCount++;

                this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    int index = cbBoneID2.SelectedIndex;
                    UpdateCalcDataUI(_valuesBufferCalc, index);
                    txtLog.Text = _frameCount.ToString();
                }));
            }
            
            if (sockRef == this.sockUDPRef)
            {
                _frameCount++;

                this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    int index = cbBoneID2.SelectedIndex;
                    UpdateCalcDataUI(_valuesBufferCalc, index);
                    txtLog.Text = _frameCount.ToString();
                }));
            }
        }
        
        private void bvhDataReceived(IntPtr customObject, IntPtr sockRef, IntPtr header, IntPtr data)
        {
            _bvhHeader = (BvhDataHeader)Marshal.PtrToStructure(header, typeof(BvhDataHeader));

            // Change the buffer length if necessary
            if (_bvhHeader.DataCount != _valuesBuffer.Length)
            {
                _valuesBuffer = new float[_bvhHeader.DataCount];
            }

            Marshal.Copy(data, _valuesBuffer, 0, (int)_bvhHeader.DataCount);
            //_valuesBuffer = (float[])Marshal.PtrToStructure(data, typeof(float[_bvhHeader.DataCount]));

            if (sockRef == this.sockTCPRef)
            {
                _frameCount++;

                this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                     int index = cbBoneID.SelectedIndex;
                    UpdateBvhDataUI(_valuesBuffer, index, _bvhHeader.bWithDisp == 1 ? true : false);
                     txtLog.Text = _frameCount.ToString();
                }));
            }
                        
            if (sockRef == this.sockUDPRef)
            {
                _frameCount++;

                this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                     int index = cbBoneID.SelectedIndex;
                     UpdateBvhDataUI(_valuesBuffer, index, _bvhHeader.bWithDisp == 1 ?  true : false);
                   txtLog.Text = _frameCount.ToString();
                }));
            }

        }

        private void socketStatusChanged(IntPtr customObject, IntPtr sockRef, SocketStatus status, [MarshalAs(UnmanagedType.LPStr)]string msg)
        {
            this.Dispatcher.Invoke(new Action(delegate()
            {
                txtSockLog.Text = msg;
            }));
        }

        private void btnStartUDPService_Click(object sender, RoutedEventArgs e)
        {
            int nPort = 0;
            try 
	        {	        
		        nPort = int.Parse(txtUDPPort.Text);
	        }
	        catch (Exception)
	        {
                MessageBox.Show("Wrong port number.", "UDP Service", MessageBoxButton.OK, MessageBoxImage.Error);
		        return;
	        }

            if (btnStartUDPService.Content.ToString() == "Connect")
            {
                sockUDPRef = NeuronDataReader.BRStartUDPServiceAt(nPort);
                if (sockUDPRef!=IntPtr.Zero)
                {
                    btnStartUDPService.Content = "DisConnect";
                }
                else
                {
                    btnStartUDPService.Content = "Connect";
                }
            }
            else
            {
                NeuronDataReader.BRCloseSocket(sockUDPRef);
                btnStartUDPService.Content = "Connect";
            }
        }
        
        private void btnExitWindows(object sender, RoutedEventArgs e)
        {
            //添加状态判断，当TCP服务未断开时直接点击exit，会先关闭TCP连接再退出，避免程序崩溃
            if (NeuronDataReader.BRGetSocketStatus(sockTCPRef) == SocketStatus.CS_Running)
            {
                NeuronDataReader.BRCloseSocket(sockTCPRef);              
            }
             //添加状态判断，当UDP服务未断开时直接点击exit，会先关闭UDP连接再退出，避免程序崩溃
            if(sockUDPRef != IntPtr.Zero)
            {
                NeuronDataReader.BRCloseSocket(sockUDPRef);
            }

            Environment.Exit(0);    //立即
        }
    }
}
