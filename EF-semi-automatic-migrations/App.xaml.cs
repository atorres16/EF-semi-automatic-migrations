using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EF_semi_automatic_migrations
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow v = new EF_semi_automatic_migrations.MainWindow();
            MainVM vm = new MainVM();
            v.DataContext = vm;
            vm.Initialize();
            v.ShowDialog();
        }
    }
}
