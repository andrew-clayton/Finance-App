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
        AddTransactionViewModel()
        {
            SaveCommand = new RelayCommand(SaveTransaction);
            CancelCommand = new RelayCommand(CancelTransaction);
            
            var budgetOptions = Enum.GetValues(typeof(Category));
            SelectedBudget = BudgetOptions[0];
        }

        // Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #region properties

        private string _labelSelection;
        public string LabelSelection
        {
            get => _labelSelection;
            set
            {
                _labelSelection = value;
            }

        }
        private float _valueSelection;
        public float ValueSelection
        {
            get => _valueSelection;
            set
            {
                if (float.TryParse(value.ToString(), out float parsedNum))
                {
                    // it is a number
                    _valueSelection = parsedNum;                    
                }
                else // it is not a number
                {
                    //handle this somehow
                }
            }
        }
        private DateTime _timeStampSelection;
        public DateTime TimeStampSelection
        {
            get => _timeStampSelection;
            set
            {
                _timeStampSelection = value;
            }
        }
        private string _selectedBudget;
        public string SelectedBudget
        {
            get => _selectedBudget;
            set
            {
                _selectedBudget = value;
            }
        }


        private ObservableCollection<string> _budgetOptions;
        public ObservableCollection<string> BudgetOptions
        {
            get => _budgetOptions;
            set
            {
                _budgetOptions = value;
                OnPropertyChanged(nameof(BudgetOptions));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void SaveTransaction(object obj)
        {
            throw new NotImplementedException();
        }

        private void Cancel(object obj)
        {
            throw new NotImplementedException();
        }

    }
}
