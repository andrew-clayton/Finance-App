This is an app for managing finances. To use it, you can just download all of the files and build the solution. 
You can enter your transactions into the app by right-clicking on the "Expenses" and "Revenues" tables. This will show you these for
the select month (in the top right corner). You can also select to view all months to see all tranasctions, and not just ones in the selected month.
On the top row of the app, you can view transactions associated with a particular budget as selected. 





If you want to change the budgets that are available to use, go into 

A database will automatically be created Finance/Models/Category.cs, 
and change the listing of items inside "public enum Category"'s braces to match the budgets you want. 

The budgets in the database are automatically created based on what it finds in there. 
Currently, the app only generates budgets in batches. This means you can't add a new budget partway through the month,
it wouldn't be created in the database. However, when an uninitialized month is selected (a month that you haven't selected yet,
such as when it becomes a new month. assuming you haven't clicked ahead to it),
the budget would be created alongside the other ones for the month.