using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimulIDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    
    public partial class App : Application
    {
        public App()
        {
            self = this;
        }

        public event Action<int, int, int> OnContentsChange;
        public static App Self() { return self; }

        public void ContentsChange(int a, int b, int c)
        {
            OnContentsChange?.Invoke(a,b,c);
        }

        private static App self = null;
    }

    
}
