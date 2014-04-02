using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;


namespace CPUPerformance
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Model> CPU { get; set; }
        public string ProcessorID { get; set; }
        
        ManagementClass mgt;
        ManagementObjectCollection procs;

        private string utilization;
        public string  Utilization 
        { 
            get {return utilization;}
            set { utilization = value; OnPropertyChanged(new PropertyChangedEventArgs("Utilization")); }
        }

        private string upTimeString;
        public string UpTime
        {
            get { return upTimeString; }
            set { upTimeString = value; OnPropertyChanged(new PropertyChangedEventArgs("UpTime")); }
        }

        private string speed;
        public string Speed
        {
            get { return speed; }
            set { speed = value; OnPropertyChanged(new PropertyChangedEventArgs("Speed")); }
        }
        private string maxSpeed;
        public string MaximumSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; OnPropertyChanged(new PropertyChangedEventArgs("MaximumSpeed")); }
        }

        private string sockets;
        public string Sockets
        {
            get { return sockets; }
            set { sockets = value; OnPropertyChanged(new PropertyChangedEventArgs("Sockets")); }
        }

        private string cores;
        public string Cores
        {
            get { return cores; }
            set { cores = value; OnPropertyChanged(new PropertyChangedEventArgs("Cores")); }
        }

        private string logicalProcessors;
        public string LogicalProcessors
        {
            get { return logicalProcessors; }
            set { logicalProcessors = value; OnPropertyChanged(new PropertyChangedEventArgs("LogicalProcessors")); }
        }

        private string virtualization;
        public string Virtualization
        {
            get { return virtualization; }
            set { virtualization = value; OnPropertyChanged(new PropertyChangedEventArgs("Virtualization")); }
        }

        private string l1Cache;
        public string L1Cache
        {
            get { return l1Cache; }
            set { l1Cache = value; OnPropertyChanged(new PropertyChangedEventArgs("L1Cache")); }
        }
        private string l2Cache;
        public string L2Cache
        {
            get { return l2Cache; }
            set { l2Cache = value; OnPropertyChanged(new PropertyChangedEventArgs("L2Cache")); }
        }
        private string l3Cache;
        public string L3Cache
        {
            get { return l3Cache; }
            set { l3Cache = value; OnPropertyChanged(new PropertyChangedEventArgs("L3Cache")); }
        }

        private Process[] processes;
        private double processCount;
        public double Processes
        { 
            get{return processCount;}
            set { processCount = value; OnPropertyChanged(new PropertyChangedEventArgs("Processes")); }
        }

        private double threadCount;
        public double ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; OnPropertyChanged(new PropertyChangedEventArgs("ThreadCount")); }
        }

        private double handleCount;
        public double HandleCount
        {
            get { return handleCount; }
            set { handleCount = value; OnPropertyChanged(new PropertyChangedEventArgs("HandleCount")); }
        }

        private DispatcherTimer timer;
        private DateTime time;        
        private Random random;

        public ViewModel()
        {
            mgt = new ManagementClass("Win32_Processor");
            procs = mgt.GetInstances();            
            
            CPU = new ObservableCollection<Model>();
            timer = new DispatcherTimer();
            random = new Random();
            time = DateTime.Now;
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";            
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            ProcessorID = GetProcessorID();
            processes = Process.GetProcesses();
            Processes = processes.Length;
            MaximumSpeed = GetMaxClockSpeed();
            LogicalProcessors = GetNumberOfLogicalProcessors();
            Cores = GetNumberOfCores();
            L2Cache = GetL2CacheSize();
            L3Cache = GetL3CacheSize();
            foreach (ManagementObject item in procs)
                L1Cache = ((UInt32)item.Properties["L2CacheSize"].Value / 2).ToString() + " KB";
            
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;            
            timer.Start();
            for (int i = 0; i < 60; i++)
            {
                CPU.Add(new Model(time, 0,0));
                time = time.AddSeconds(1);
            }
            
        }

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        
        public string getAvailableRAM()
        {
            return (int)ramCounter.NextValue() + "MB";
        }
        private string GetProcessorID()
        {           
            foreach (ManagementObject item in procs)
                return item.Properties["Name"].Value.ToString();
           
            return "Unknown";
        }
       
        private string GetMaxClockSpeed()
        {
            foreach (ManagementObject item in procs)
            {                                
                return (0.001 * (UInt32)(item.Properties["MaxClockSpeed"].Value)).ToString("0.00") + " GHz";
            }
            mgt.Dispose();
            return "Unknown";
        }
        private  string GetNumberOfLogicalProcessors()
        {           
            foreach (ManagementObject item in procs)
                return item.Properties["NumberOfLogicalProcessors"].Value.ToString();
            mgt.Dispose();
            return "Unknown";
        }

        private  string GetNumberOfCores()
        {           
            foreach (ManagementObject item in procs)
                return item.Properties["NumberOfCores"].Value.ToString();
            mgt.Dispose();
            return "Unknown";
        }

        private  string GetL2CacheSize()
        {
            foreach (ManagementObject item in procs)
                return (2 * (UInt32)item.Properties["L2CacheSize"].Value).ToString() + " KB";
            mgt.Dispose();
            return "Unknown";
        }
        private  string GetL3CacheSize()
        {          
            foreach (ManagementObject item in procs)
                return (0.001 * (UInt32)item.Properties["L3CacheSize"].Value).ToString("0.0")+" MB";
            mgt.Dispose();
            return "Unknown";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            int threadCount = 0;
            int handles = 0;
            processes = Process.GetProcesses();
            foreach (Process proc in processes)
            {
                threadCount = threadCount + proc.Threads.Count;
                handles += proc.HandleCount;
                
            }
            UpTime = GetUptime().ToString(@"dd\.hh\:mm\:ss");
            ThreadCount = threadCount;
            HandleCount = handles;
            Speed = GetCpuSpeedInGHz();
            CPU.RemoveAt(0);
            double percentage = cpuCounter.NextValue();
            Utilization = percentage.ToString(String.Format("0")) + "%";
            CPU.Add(new Model(time, percentage,0));
            time = time.AddSeconds(1);
        }
        public static TimeSpan GetUptime()
        {
            var mo = new ManagementObject(@"\\.\root\cimv2:Win32_OperatingSystem=@");
            var lastBootUp = ManagementDateTimeConverter.ToDateTime(mo["LastBootUpTime"].ToString());
            mo.Dispose();
            return DateTime.Now.ToUniversalTime() - lastBootUp.ToUniversalTime();
        }

        private string GetCpuSpeedInGHz()
        {
            string GHz = null;
            foreach (ManagementObject mo in procs)
            {
                GHz = (0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value).ToString("0.00") + " GHz";                
                break;
            }
            return GHz;
        }

        private void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, args);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }    
}
