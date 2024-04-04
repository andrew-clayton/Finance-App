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
using SQLitePCL;
using Finance.Views;
using System.Windows.Input;


namespace Finance.ViewModels
{
    public class SharedViewModel : INotifyPropertyChanged
    {
        private TransactionService transactionService = new TransactionService();
        public ObservableCollection<ATransaction> Transactions = new ObservableCollection<ATransaction>();
        public IEnumerable<ATransaction> CurrentTransactions => Transactions.Where(t => ViewAllMonths || (t.TimeStamp.Month == selectedDate.Month &&
        selectedDate.Year == t.TimeStamp.Year && TransactionMatchesBudgetMonth(t, SelectedBudget)));
        public IEnumerable<ATransaction> CurrentBudgetTransactions => CurrentTransactions.Where(t => t.Budget == SelectedBudget.Type);

        public ObservableCollection<Budget> _budgets = new ObservableCollection<Budget>();
        public ObservableCollection<Budget> Budgets
        {
            get => _budgets;
            set
            {
                _budgets = value;
                OnPropertyChanged(nameof(Budgets));
            }
        }

        #region properties
        public ICommand OpenAddRevenueCommand { get; private set; }
        public ICommand OpenAddExpenseCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }

        public IEnumerable<ATransaction> Expenses => CurrentTransactions.Where(t => t.Value < 0);
        public IEnumerable<ATransaction> Revenues => CurrentTransactions.Where(t => t.Value >= 0);

        private DateTime _selectedDate = DateTime.Now;
        public DateTime selectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(selectedDate));
                RefreshCurrentTransactions();
            }
        }

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
        public float netIncome
        {
            get
            {
                return netRevenues + netExpenses;
            }
        }

        public SeriesCollection PieChartData { get; set; }
        private bool _viewAllMonths;
        public bool ViewAllMonths
        {
            get => _viewAllMonths;
            set
            {
                if (_viewAllMonths != value)
                {
                    _viewAllMonths = value;
                    OnPropertyChanged(nameof(ViewAllMonths));
                    // Trigger update of transactions when checkbox changes
                    RefreshCurrentTransactions();
                }
            }
        }

        private Budget _selectedBudget;
        public Budget SelectedBudget
        {
            get => _selectedBudget;
            set
            {
                if (value != _selectedBudget)
                {
                    _selectedBudget = value;
                    OnPropertyChanged(nameof(SelectedBudget));
                    OnPropertyChanged(nameof(CurrentBudgetTransactions));
                    OnPropertyChanged(nameof(AmountOfBudgetSpent));
                    OnPropertyChanged(nameof(PercentageOfBudgetSpent));
                }
            }
        }

        public double PercentageOfBudgetSpent
        {
            get
            {
                double totalBudgetSpent = CurrentBudgetTransactions.Sum(t => t.Value);
                return (totalBudgetSpent / SelectedBudget.AllotedAmount) * 100;
            }
        }

        public double AmountOfBudgetSpent
        {
            get
            {
                return CurrentBudgetTransactions.Sum(t => t.Value);
            }
        }



        #endregion

        // Used to refresh UI elements. This was needed since the checkbox to view all transactions was not functioning as intended
        private void RefreshCurrentTransactions()
        {
            OnPropertyChanged(nameof(CurrentTransactions));
            OnPropertyChanged(nameof(Expenses));
            OnPropertyChanged(nameof(Revenues));
            OnPropertyChanged(nameof(netRevenues));
            OnPropertyChanged(nameof(netExpenses));
            OnPropertyChanged(nameof(netIncome));
            OnPropertyChanged(nameof(CurrentBudgetTransactions));
            OnPropertyChanged(nameof(PercentageOfBudgetSpent));
            OnPropertyChanged(nameof(AmountOfBudgetSpent));
            UpdatePieChartData();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SharedViewModel()
        {
            LoadBudgetsFromDatabase();
            // if there is a budget for this month already, I want to have that set by default. otherwise a new one shall be created
            var currentBudget = Budgets.Where(b => b.TimeStamp.Month == DateTime.Now.Month && b.TimeStamp.Year == DateTime.Now.Year).FirstOrDefault();
            if (currentBudget == null)
            {
                // if there is not already a budget for this month, we shall create a new one
                var budgetService = new BudgetService();
                var newBudget = new Budget();
                budgetService.InitializeBudgetsForMonth(DateTime.Now);
            }

            SelectedBudget = Budgets.Where(budget => budget.TimeStamp.Month == DateTime.Now.Month && budget.TimeStamp.Year == DateTime.Now.Year && budget.Type == Category.None).FirstOrDefault();
            if (SelectedBudget == null) throw new Exception("SelectedBudget is null");

            LoadTransactionsFromDatabase();
            InitializePieChartData();
            OpenAddExpenseCommand = new RelayCommand(o => OpenAddTransactionView(true));
            OpenAddRevenueCommand = new RelayCommand(o => OpenAddTransactionView(false));
            DeleteTransactionCommand = new RelayCommand(DeleteSelectedTransaction, CanDeleteTransaction);
        }

        private void DeleteSelectedTransaction(object parameter)
        {
            if (parameter is ATransaction transaction)
            {
                RemoveTransaction(transaction);
            }
        }
        
        private bool CanDeleteTransaction(object parameter)
        {
            return parameter != null; // Ensure that a transaction is selected
        }

        private void OpenAddTransactionView(object isExpense)
        {
            var dialog = new AddTransactionView();
            bool isExpenseTransaction = (bool)isExpense;
            var viewModel = new AddTransactionViewModel(isExpenseTransaction);


            dialog.DataContext = viewModel;
            dialog.ShowDialog();
            LoadTransactionsFromDatabase();
            OnPropertyChanged(nameof(Transactions));
            RefreshCurrentTransactions();
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
            Transactions.Clear();
            var transactionList = await transactionService.GetAllTransactionsAsync();
            foreach (var transaction in transactionList)
            {
                Transactions.Add(transaction);
            }
            OnPropertyChanged(nameof(Transactions));
        }

        private async void LoadBudgetsFromDatabase()
        {
            var budgetService = new BudgetService();
            var budgetList = await budgetService.GetAllBudgetsAsync();
            foreach (var budget in budgetList)
            {
                Budgets.Add(budget);
            }
            OnPropertyChanged(nameof(Budgets));
        }

        private async void RefreshTransactions()
        {
            Transactions.Clear();

            var transactionList = await transactionService.GetAllTransactionsAsync();
            foreach (var transaction in transactionList)
            {
                Transactions.Add(transaction);
            }
            OnPropertyChanged(nameof(Transactions));
            RefreshCurrentTransactions();
        }

        private async Task AddTransaction(ATransaction newTransaction)
        {
            await transactionService.AddTransaction(newTransaction);
            Transactions.Add(newTransaction);

            OnPropertyChanged(nameof(Transactions));
            RefreshCurrentTransactions();

        }

        private void RemoveTransaction(ATransaction oldTransaction)
        {
            transactionService.DeleteTransaction(oldTransaction.Id);
            Transactions.Remove(oldTransaction);

            OnPropertyChanged(nameof(Transactions));
            RefreshCurrentTransactions();
        }


        private void UpdateTransaction(ATransaction transaction)
        {
            transactionService.UpdateTransaction(transaction);
            RefreshTransactions();
            RefreshCurrentTransactions();
        }

        private void UpdatePieChartData()
        {
            PieChartData[0].Values = new ChartValues<float> { netRevenues };
            PieChartData[1].Values = new ChartValues<float> { Math.Abs(netExpenses) };
            OnPropertyChanged(nameof(PieChartData));
        }

        private bool TransactionMatchesBudgetMonth(ATransaction transaction, Budget budget)
        {
            if (transaction.TimeStamp.Month == budget.TimeStamp.Month && transaction.TimeStamp.Year == budget.TimeStamp.Year) return true;
            else return false;
        }
    }
}
