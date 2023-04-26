# TimeClock
A TUI (text-based user interface) time clock app

Platforms:
-------------------------------------------------------------------
- Windows
- macOS
- Linux

Dependencies:
-------------------------------------------------------------------
- [Terminal.Gui](https://www.nuget.org/packages/Terminal.Gui)
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/)

Features:
-------------------------------------------------------------------
- Persistent storage of admin and employee information
- Guided creation of a single admin account 
- Management of employees including creating, editing and deleting of employees
- Employees can clock in and out, and start and end breaks by entering their unique and automatically generated employee ID.
- Editable information about employees include
  - Name
  - Hourly wage
  - Allowed breaks per shift
  - Details about past and current shifts
- Editable information about shifts include
  - Start and end time of the shift
  - Start and end time of all breaks
  - Wage paid for that shift 
  
Missing Features:
-------------------------------------------------------------------
  - Clock that shows at all times (due to limitations with the text user interface, constantly refreshing the display to show the time can cause issues)
  - Overtime (due to shifts having no prescribed length and all shifts being treated as separate and unrelated even within the same week, overtime is not able to be implemented currently)
  - Encryption or other protection of any stored data
  - Easy clock in and out of employees from admin view of an employee (currently to clock an employee in or out, or to start or end an employees break an admin must log in as the employee to perform this action)
  
How To Run:
-------------------------------------------------------------------
TODO
  
  

  
