﻿using Finance.Models;
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
        //AddTransactionViewModel()
        //{
        //    SaveCommand = new RelayCommand(SaveTransaction, CanSaveTransaction);
        //    CancelCommand = new RelayCommand(CancelTransaction);

        //    var budgetOptions = Enum.GetValues(typeof(Category));
        //    SelectedBudget = BudgetOptions[0];
        //}

        public AddTransactionViewModel(bool isExpense)
        {
            this.isExpense = isExpense;
            SaveCommand = new RelayCommand(SaveTransaction, CanSaveTransaction);
            CancelCommand = new RelayCommand(CancelTransaction);

            var budgetOptions = Enum.GetValues(typeof(Category));
            foreach (var option in budgetOptions)
            {
                if (option is Category categoryOption)
                    BudgetOptions.Add(categoryOption);
                else throw new NotImplementedException();
            }
            SelectedBudget = BudgetOptions[0];
        }

        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        // This method checks if all parameters are valid, so I can check this in each property's setter
        private bool CanSaveTransaction(object obj)
        {
            if (String.IsNullOrEmpty(SelectedLabel) || String.IsNullOrEmpty(SelectedValue.ToString()))
            {
                return false;
            }
            return true;
        }

        // Save the new transaction to the database
        private void SaveTransaction(object obj)
        {
            ATransaction newTransaction = new ATransaction();
            if (!isExpense) newTransaction.Value = SelectedValue;
            else newTransaction.Value = SelectedValue * -1;
            newTransaction.TimeStamp = SelectedTimeStamp;
            newTransaction.Title = SelectedLabel;
            newTransaction.Budget = SelectedBudget;

            var transactionService = new TransactionService();
            transactionService.AddTransaction(newTransaction);
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
        private float _valueSelection;
        public float SelectedValue
        {
            get => _valueSelection;
            set
            {
                if (float.TryParse(value.ToString(), out float parsedNum))
                {
                    // it is a number
                    _valueSelection = parsedNum;
                    OnPropertyChanged(nameof(SelectedValue));
                    CommandManager.InvalidateRequerySuggested(); // notify re-evaluation of CanExecute

                }
                else { }
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

        public ObservableCollection<Category> BudgetOptions = new ObservableCollection<Category>();

        bool isExpense;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}