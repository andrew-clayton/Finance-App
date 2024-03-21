using Finance.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;


namespace Finance.View_Models
{
    public class SharedViewModel : INotifyPropertyChanged
    {
        private TransactionService transactionService = new TransactionService();
        public ObservableCollection<ATransaction> Transactions = new ObservableCollection<ATransaction>();
        public IEnumerable<ATransaction> Expenses => Transactions.Where(t => t.Value < 0);
        public IEnumerable<ATransaction> Revenues => Transactions.Where(t => t.Value >= 0);
        public float netRevenues
        {
            get
            {
                float total = 0;
                foreach (var transaction in Revenues)
                {
                    total += transaction.Value;
                }
                return total;
            }
        }
        public float netExpenses
        {
            get
            {
                float total = 0;
                foreach (var transaction in Expenses)
                {
                    total += transaction.Value;
                }
                return total;
            }
        }
        public float NetIncome
        {
            get
            {
                return netRevenues + netExpenses;
            }
        }

        public SeriesCollection PieChartData { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SharedViewModel()
        {
            LoadTransactionsFromDatabase();
            InitializePieChartData();
        }

        private void InitializePieChartData()
        {
            // Example data initialization
            PieChartData = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Earned",
                    Values = new ChartValues<float> { netRevenues },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Spent",
                    Values = new ChartValues<float> { Math.Abs(netExpenses) },
                    DataLabels = true
                }
            };

            OnPropertyChanged(nameof(PieChartData));
        }

        private async void LoadTransactionsFromDatabase()
        {
            var transactionList = await transactionService.GetAllTransactionsAsync();
            foreach (var transaction in transactionList)
            {
                Transactions.Add(transaction);

                //if (transaction.Value >= 0)
                //{
                //    Revenues.Add(transaction);
                //}
                //else
                //{
                //    Expenses.Add(transaction);
                //}

            }
            OnPropertyChanged(nameof(Transactions));
        }

        private async void Refresh()
        {
            Transactions.Clear();
            //Expenses.Clear();
            //Revenues.Clear();

            var transactionList = await transactionService.GetAllTransactionsAsync();
            foreach (var transaction in transactionList)
            {
                Transactions.Add(transaction);

                //if (transaction.Value >= 0)
                //{
                //    Revenues.Add(transaction);
                //}
                //else
                //{
                //    Expenses.Add(transaction);
                //}

            }
            OnPropertyChanged(nameof(Transactions));
            //OnPropertyChanged(nameof(Expenses));
            //OnPropertyChanged(nameof(Revenues));
        }

        private async Task AddTransaction(ATransaction newTransaction)
        {
            await transactionService.AddTransaction(newTransaction);
            Transactions.Add(newTransaction);

            //if (newTransaction.Value >= 0) Revenues.Add(newTransaction);
            //else Expenses.Add(newTransaction);

            OnPropertyChanged(nameof(Transactions));
            //OnPropertyChanged(nameof(Expenses));
            //OnPropertyChanged(nameof(Revenues));
        }

        private void RemoveTransaction(ATransaction oldTransaction)
        {
            transactionService.DeleteTransaction(oldTransaction.Id);
            Transactions.Remove(oldTransaction);

            //if (oldTransaction.Value >= 0) Revenues.Add(oldTransaction);
            //else Expenses.Add(oldTransaction);

            OnPropertyChanged(nameof(Transactions));
            //OnPropertyChanged(nameof(Expenses));
            //OnPropertyChanged(nameof(Revenues));
        }


        private void UpdateTransaction(ATransaction transaction)
        {
            transactionService.UpdateTransaction(transaction);
            Refresh();
        }

        private int[] FindNetTransactionsForMonth(DateTime date)
        {
            // Filter transactions for the given month and year.
            var monthTransactions = Transactions.Where(t => t.TimeStamp.Month == date.Month && t.TimeStamp.Year == date.Year);

            // Calculate the sum of revenues (transactions greater than 0).
            int totalRevenues = (int)monthTransactions.Where(t => t.Value > 0).Sum(t => t.Value);

            // Calculate the sum of expenses (transactions less than 0).
            int totalExpenses = (int)monthTransactions.Where(t => t.Value < 0).Sum(t => t.Value);

            // Return an array with total revenues and total expenses.
            return new int[] { totalRevenues, totalExpenses };
        }
    }
}
