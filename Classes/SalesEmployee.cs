/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;

namespace PayrollSystem.Classes
{
    /// <summary>
    /// Represents an employee whose income includes a base salary and a commission based on sales revenue.
    /// </summary>
    public class SalesEmployee : Employee
    {
        private double _baseSalary;
        private double _salesRevenue;
        private double _commissionRate;

        public double BaseSalary
        {
            get => _baseSalary;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Base salary cannot be negative.");
                }
                _baseSalary = value;
            }
        }

        public double SalesRevenue
        {
            get => _salesRevenue;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Sales revenue cannot be negative.");
                }
                _salesRevenue = value;
            }
        }

        public double CommissionRate
        {
            get => _commissionRate;
            private set
            {
                if (value < 0.0 || value > 0.3)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Commission rate must be between 0 and 0.3 (30%).");
                }
                _commissionRate = value;
            }
        }

        /// <summary>
        /// Shortened constructor.
        /// </summary>
        public SalesEmployee(string employeeId, string fullName, double baseSalary, double commissionRate)
            : this(employeeId, fullName, "Unassigned", baseSalary, 0.0, commissionRate)
        {
        }

        /// <summary>
        /// Full constructor.
        /// </summary>
        public SalesEmployee(string employeeId, string fullName, string department, double baseSalary, double salesRevenue, double commissionRate)
            : base(employeeId, fullName, department)
        {
            BaseSalary = baseSalary;
            SalesRevenue = salesRevenue;
            CommissionRate = commissionRate;
        }




        // --- Derived Class Methods ---
        
        /// <summary>
        /// Updates the sales revenue in a controlled manner.
        /// </summary>
        /// <param name="revenue">The new sales revenue amount.</param>
        public void UpdateSalesRevenue(double revenue)
        {
            SalesRevenue = revenue; // This triggers the validation in the property setter
        }

        public override double CalculateGrossPay()
        {
            return BaseSalary + (SalesRevenue * CommissionRate) + MonthlyBonus;
        }

        public override string GetEmployeeType()
        {
            return "Sales";
        }

        public override void DisplayPayrollInfo()
        {
            base.DisplayPayrollInfo();
            Console.WriteLine($"  Base Salary: {BaseSalary:N0}");
            Console.WriteLine($"  Sales Revenue: {SalesRevenue:N0}");
            Console.WriteLine($"  Commission Rate: {CommissionRate:P0}"); // :P0 formats as percentage (e.g., 5%)
            Console.WriteLine($"  Gross Pay: {CalculateGrossPay():N0}");
            Console.WriteLine(new string('-', 40));
        }
    }
}