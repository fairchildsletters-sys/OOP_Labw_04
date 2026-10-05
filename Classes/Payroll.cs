/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;
using System.Collections.Generic;

namespace PayrollSystem.Classes
{
    /// <summary>
    /// Manages a collection of employees for a specific payroll period.
    /// Demonstrates aggregation and runtime polymorphism.
    /// </summary>
    public class Payroll
    {
        private string _period = string.Empty;

        /// <summary>
        /// Gets and sets the payroll period (e.g., "2026-09").
        /// </summary>
        public string Period
        {
            get => _period;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Payroll period must not be empty or contain only whitespace.");
                }
                _period = value.Trim();
            }
        }

        /// <summary>
        /// Gets the list of employees. 
        /// The setter is private to prevent external replacement of the entire list instance.
        /// </summary>
        public List<Employee> Employees { get; private set; }


        // --- Constructor ---

        /// <summary>
        /// Initialises a new payroll instance for the specified period.
        /// </summary>
        /// <param name="period">The payroll period identifier.</param>
        public Payroll(string period)
        {
            Period = period;
            Employees = new List<Employee>();
        }




        // --- Payroll Management Methods ---

        /// <summary>
        /// Adds an employee to the payroll if their ID is not already present.
        /// </summary>
        /// <param name="employee">The employee object to add.</param>
        /// <returns>True if added successfully; false if a duplicate ID exists.</returns>
        public bool AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Cannot add a null employee to the payroll.");
            }

            // Check for duplicate Employee ID
            foreach (var emp in Employees)
            {
                if (emp.EmployeeId == employee.EmployeeId)
                {
                    return false; // Duplicate found, rejection
                }
            }

            Employees.Add(employee);
            return true;
        }

        /// <summary>
        /// Searches for an employee by their unique identifier.
        /// </summary>
        /// <param name="employeeId">The ID to search for.</param>
        /// <returns>The matching Employee object, or null if not found.</returns>
        public Employee? FindEmployee(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return null;

            foreach (var emp in Employees)
            {
                if (emp.EmployeeId == employeeId.Trim())
                {
                    return emp;
                }
            }

            return null;
        }

        /// <summary>
        /// Calculates the total gross pay for all employees in the current payroll.
        /// Demonstrates polymorphic method dispatch without type-checking (no if/else).
        /// </summary>
        /// <returns>The total payroll amount.</returns>
        public double CalculateTotalPayroll()
        {
            double total = 0.0;

            foreach (var emp in Employees)
            {
                // Polymorphism in action: The correct CalculateGrossPay() is called automatically
                total += emp.CalculateGrossPay();
            }

            return total;
        }

        /// <summary>
        /// Calculates the total gross pay for a specific department.
        /// </summary>
        /// <param name="department">The target department name.</param>
        /// <returns>The total payroll amount for the department.</returns>
        public double CalculatePayrollByDepartment(string department)
        {
            // Invariant: Department must not be null or whitespace
            if (string.IsNullOrWhiteSpace(department))
            {
                throw new ArgumentException("Department name must not be empty or contain only whitespace.");
            }

            double total = 0.0;
            string targetDept = department.Trim();

            foreach (var emp in Employees)
            {
                if (emp.Department == targetDept)
                {
                    total += emp.CalculateGrossPay();
                }
            }

            return total;
        }

        /// <summary>
        /// Identifies the employee with the highest gross pay in the current period.
        /// </summary>
        /// <returns>The highest-paid Employee, or null if the list is empty.</returns>
        public Employee? FindHighestPaidEmployee()
        {
            if (Employees.Count == 0) return null;

            Employee highestPaid = Employees[0];
            double maxPay = highestPaid.CalculateGrossPay();

            for (int i = 1; i < Employees.Count; i++)
            {
                double currentPay = Employees[i].CalculateGrossPay();
                if (currentPay > maxPay)
                {
                    maxPay = currentPay;
                    highestPaid = Employees[i];
                }
            }

            return highestPaid;
            // The highest-paid employee found is the first one
        }
        
        /// <summary>
        /// Displays the complete payroll report, including individual details and the grand total.
        /// </summary>
        public void DisplayPayroll()
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"PAYROLL REPORT - PERIOD: {Period}");
            Console.WriteLine(new string('=', 50));

            if (Employees.Count == 0)
            {
                Console.WriteLine("No employees in this payroll period.");
                Console.WriteLine(new string('=', 50));
                return;
            }

            foreach (var emp in Employees)
            {
                // Polymorphism in action: The correct DisplayPayrollInfo() is called automatically
                emp.DisplayPayrollInfo();
            }

            Console.WriteLine($"GRAND TOTAL PAYROLL: {CalculateTotalPayroll():N0}");
            Console.WriteLine(new string('=', 50));
        }
    }
}