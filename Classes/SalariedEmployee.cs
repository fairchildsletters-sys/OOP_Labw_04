/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;

namespace PayrollSystem.Classes
{
    /// <summary>
    /// Represents an employee who receives a fixed monthly salary and an optional responsibility allowance.
    /// </summary>
    public class SalariedEmployee : Employee
    {
        private double _monthlySalary;
        private double _responsibilityAllowance;

        public double MonthlySalary
        {
            get => _monthlySalary;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Monthly salary cannot be negative.");
                }
                _monthlySalary = value;
            }
        }

        public double ResponsibilityAllowance
        {
            get => _responsibilityAllowance;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Responsibility allowance cannot be negative.");
                }
                _responsibilityAllowance = value;
            }
        }

        /// <summary>
        /// Shortened constructor.
        /// </summary>
        public SalariedEmployee(string employeeId, string fullName, double monthlySalary)
            : this(employeeId, fullName, "Unassigned", monthlySalary, 0.0)
        {
        }

        /// <summary>
        /// Full constructor. Calls the base class constructor to handle common attributes.
        /// </summary>
        public SalariedEmployee(string employeeId, string fullName, string department, double monthlySalary, double responsibilityAllowance)
            : base(employeeId, fullName, department)
        {
            MonthlySalary = monthlySalary;
            ResponsibilityAllowance = responsibilityAllowance;
        }




        // --- Derived Class Methods ---
        
        public override double CalculateGrossPay()
        {
            return MonthlySalary + ResponsibilityAllowance + MonthlyBonus;
        }

        public override string GetEmployeeType()
        {
            return "Salaried";
        }

        public override void DisplayPayrollInfo()
        {
            base.DisplayPayrollInfo();
            Console.WriteLine($"  Monthly Salary: {MonthlySalary:N0}");
            Console.WriteLine($"  Allowance: {ResponsibilityAllowance:N0}");
            Console.WriteLine($"  Gross Pay: {CalculateGrossPay():N0}");
            Console.WriteLine(new string('-', 40));
        }
    }
}