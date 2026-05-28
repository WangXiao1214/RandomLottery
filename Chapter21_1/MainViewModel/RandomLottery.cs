using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Chapter21_1.Tools;

namespace Chapter21_1.MainViewModel
{
    public class RandomLottery : INotifyPropertyChanged
    {
        private string _status;
        private string _phenoNumberDisplay;
        private static Random _random = new Random();

        private Thread _workerThread;
        private volatile bool _shouldStop = false;  // volatile 确保线程间可见性
        private bool _isRunning = false;  // 添加运行状态标志

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnProperyChanged();
            }
        }
        public string PhoneNumberDisplay
        {
            get => _phenoNumberDisplay;
            set
            {
                _phenoNumberDisplay = value;
                OnProperyChanged();
            }
        }

        private List<string> _phoneNumberList = PhoneNumberGenerator.Generate(100);

        public RandomLottery()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(End);
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnProperyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Start()
        {
            if (_isRunning) return;  // 防止重复启动

            _shouldStop = false;
            _isRunning = true;

            _workerThread = new Thread(new ThreadStart(GetNum));
            _workerThread.IsBackground = true;  // 设置为后台线程
            _workerThread.Start();
            Status = "抽取中";
        }

        private void End()
        {
            if (!_isRunning) return;  // 如果没有运行，直接返回

            _shouldStop = true;  // 请求停止线程

            // 等待线程结束（可选，设置超时）
            if (_workerThread != null && _workerThread.IsAlive)
            {
                _workerThread.Join(1000);  // 等待1秒
            }

            _isRunning = false;
            Status = "停止";
        }


        private void GetNum()
        {
            if (_phoneNumberList == null || _phoneNumberList.Count == 0)
            {
                // 需要在UI线程显示消息框
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("没有参与抽奖人员");
                });
                return;
            }

            while (!_shouldStop)  // 检查停止标志
            {
                int index = _random.Next(0, _phoneNumberList.Count);

                // 更新UI需要在UI线程执行
                Application.Current.Dispatcher.Invoke(() =>
                {
                    string temp = _phoneNumberList[index];
                    string header = temp.Substring(0, 3);
                    string tail = temp.Substring(7,4);
                    PhoneNumberDisplay = header + "****" + tail;
                });

                Thread.Sleep(50);  // 添加延迟，避免刷新太快
            }
        }

    }


    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;


        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException();
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
