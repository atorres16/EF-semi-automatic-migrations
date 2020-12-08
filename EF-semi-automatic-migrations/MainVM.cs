using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using S = EF_semi_automatic_migrations.Properties.Settings;

namespace EF_semi_automatic_migrations
{
    public class MainVM : INotifyPropertyChanged
    {


        public string ConnString
        {
            get
            {
                return S.Default.ConnString;
            }
            set
            {
                S.Default.ConnString = value;
                S.Default.Save();
                this.OnPropertyChanged(nameof(ConnString));

            }
        }
        public void Initialize()
        {

            //Check if String Conn is set
            if (string.IsNullOrWhiteSpace(ConnString))
            {
                MessageBox.Show("Set Db Connection");
                return;
            }
            using (Db db = new Db(this.ConnString))
            {
                //Check if DB is created
                if (!db.Database.Exists())
                {
                    MessageBox.Show("Db not created yet, or not found");
                    return;
                }
                if (!db.Database.CompatibleWithModel(false))
                {
                    MessageBox.Show("Db is not updated");
                    return;
                }
            }

            //Check if DB is in Sync
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;



        private ICommand createDbCommand;
        public ICommand CreateDbCommand
        {
            get
            {
                if (createDbCommand == null)
                {
                    createDbCommand = new RelayCommand(
                    (p) => { CreateDb(); },
                    (p) =>
                    {

                        return true;
                    }
                    );
                }
                return createDbCommand;
            }
        }

        private void CreateDb()
        {
            try
            {
                using (Db db = new Db(ConnString))
                {
                    db.Database.CreateIfNotExists();
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<Db, EF_semi_automatic_migrations.Migrations.Configuration>());
                    db.Database.Initialize(false);
                }
                MessageBox.Show("Db Created OK, restart app");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private ICommand updateDbCommand;
        public ICommand UpdateDbCommand
        {
            get
            {
                if (updateDbCommand == null)
                {
                    updateDbCommand = new RelayCommand(
                    (p) => { UpdateDb(); },
                    (p) =>
                    {

                        return true;
                    }
                    );
                }
                return updateDbCommand;
            }
        }

        private void UpdateDb()
        {
            try
            {
                using (Db db = new Db(ConnString))
                {
                    var Migrator = new DbMigrator(new Migrations.Configuration() { TargetDatabase = new DbConnectionInfo(ConnString, "System.Data.SqlClient") });
                    IEnumerable<string> PendingMigrations = Migrator.GetPendingMigrations();
                    
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Updates to be applied: ");
                    foreach (var migration in PendingMigrations)
                    {
                        sb.AppendLine(migration);
                    }
                    MessageBox.Show(sb.ToString());
                    foreach (var Migration in PendingMigrations)
                    {
                        Migrator.Update(Migration);
                    }
                }
                MessageBox.Show("Db updated, restart app");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
