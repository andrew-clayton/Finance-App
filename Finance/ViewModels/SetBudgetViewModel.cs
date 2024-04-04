using Finance.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Finance.ViewModels
{
    public class SetBudgetViewModel
    {
        public SetBudgetViewModel(Budget budget)
        {
            SaveCommand = new RelayCommand(SetBudget, CanSetBudget);
            CancelCommand = new RelayCommand(CancelDialog);
            SelectedBudget = budget;
        }
        public string SelectedValue { get; set; }
        private Budget SelectedBudget { get; set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool CanSetBudget(object obj)
        {
            if (string.IsNullOrEmpty(SelectedValue)) return false;
            if (!float.TryParse(SelectedValue, out _)) return false;
            return true;
        }

        public void SetBudget(object obj)
        {
            var budgetService = new BudgetService();
            SelectedBudget.AllottedAmount = float.Parse(SelectedValue);
            budgetService.UpdateBudget(SelectedBudget);

            var window = obj as Window;
            window?.Close();
        }

        public void CancelDialog(object obj)
        {
            var window = obj as Window;
            window?.Close();
        }
    }
}
