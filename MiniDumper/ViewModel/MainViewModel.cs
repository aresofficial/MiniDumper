using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MiniDumper.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        [DllImport("user32.dll")]
        static extern int MessageBoxA(int hWnd, int msg, string caption, int type);

        public RelayCommand SimpleExceptionCommand { get; set; }
        public RelayCommand PInvokeExceptionCommand { get; set; }

        public MainViewModel()
        {
            SimpleExceptionCommand = new RelayCommand(SimpleExceptionCommandClicked);
            PInvokeExceptionCommand = new RelayCommand(PInvokeExceptionCommandClicked);
        }

        public void SimpleExceptionCommandClicked()
        {
            throw new Exception("Simple exception.");
        }

        [HandleProcessCorruptedStateExceptions]
        public void PInvokeExceptionCommandClicked()
        {
            try
            {
                MessageBoxA(0, 1, "test", 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}