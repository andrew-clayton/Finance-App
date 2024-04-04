using Finance.Models;
using MahApps.Metro.Controls;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Finance.ViewModels
{
    public class AddTransactionViewModel : INotifyPropertyChanged
    {
        public AddTransactionViewModel(bool isExpense)
        {
            var budgetOptions = Enum.GetValues(typeof(Category));

            this.isExpense = isExpense;
            SaveCommand = new RelayCommand(SaveTransaction, CanSaveTransaction);
            CancelCommand = new RelayCommand(CancelTransaction);

            BudgetOptions = new ObservableCollection<Category>(Enum.GetValues(typeof(Category)).Cast<Category>());
            SelectedBudget = BudgetOptions[0];
        }

        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        // This method checks if all parameters are valid, so I can check this in each property's setter
        private bool CanSaveTransaction(object obj)
        {
            if (String.IsNullOrEmpty(SelectedLabel) || String.IsNullOrEmpty(SelectedValue) || !float.TryParse(SelectedValue, out var _))
            {
                return false;
            }
            return true;
        }

        // Save the new transaction to the database
        private async void SaveTransaction(object obj)
        {
            ATransaction newTransaction = new ATransaction();
            if (!isExpense) newTransaction.Value = float.Parse(SelectedValue);
            else newTransaction.Value = float.Parse(SelectedValue) * -1;
            newTransaction.TimeStamp = SelectedTimeStamp;
            newTransaction.Title = SelectedLabel;
            newTransaction.Budget = SelectedBudget;

            var transactionService = new TransactionService();
            await transactionService.AddTransaction(newTransaction);
            var window = obj as Window;
            window?.Close();
        }

        // exit the dialog somehow
        private void CancelTransaction(object obj)
        {
            var window = obj as Window;
            window?.Close();
        }
        #endregion

        #region properties

        private string _labelSelection;
        public string SelectedLabel
        {
            get => _labelSelection;
            set
            {
                _labelSelection = value;
                OnPropertyChanged(nameof(SelectedLabel));
                CommandManager.InvalidateRequerySuggested();
            }

        }
        private string _valueSelection;
        public string SelectedValue
        {
            get => _valueSelection;
            set
            {
                //if (float.TryParse(value.ToString(), out float parsedNum))
                //{
                // it is a number
                //_valueSelection = parsedNum;
                _valueSelection = value;
                OnPropertyChanged(nameof(SelectedValue));
                CommandManager.InvalidateRequerySuggested(); // notify re-evaluation of CanExecute

                //}
                //else { }
                // do nothing
            }
        }
        private DateTime _timeStampSelection = DateTime.Now;
        public DateTime SelectedTimeStamp
        {
            get => _timeStampSelection;
            set
            {
                _timeStampSelection = value;
                OnPropertyChanged(nameof(SelectedTimeStamp));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        private Category _selectedBudget;
        public Category SelectedBudget
        {
            get => _selectedBudget;
            set
            {
                _selectedBudget = value;
                OnPropertyChanged(nameof(SelectedBudget));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Category> BudgetOptions { get; set; }

        bool isExpense;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
