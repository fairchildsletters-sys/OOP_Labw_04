/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;

namespace PayrollSystem.Classes
{
    /// <summary>
    /// Represents an employee paid based on an hourly rate, with overtime provisions.
    /// </summary>
    public class HourlyEmployee : Employee
    {
        private double _hourlyRate;
        private double _workedHours;

        public double HourlyRate
        {
            get => _hourlyRate;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Hourly rate cannot be negative.");
                }
                _hourlyRate = value;
            }
        }

        public double WorkedHours
        {
            get => _workedHours;
            private set
            {
                if (value < 0 || value > 250)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Worked hours must be between 0 and 250.");
                }
                _workedHours = value;
            }
        }

        /// <summary>
        /// Shortened constructor.
        /// </summary>
        public HourlyEmployee(string employeeId, string fullName, double hourlyRate)
            : this(employeeId, fullName, "Unassigned", hourlyRate, 0.0)
        {
        }

        /// <summary>
        /// Full constructor.
        /// </summary>
        public HourlyEmployee(string employeeId, string fullName, string department, double hourlyRate, double workedHours)
            : base(employeeId, fullName, department)
        {
            HourlyRate = hourlyRate;
            WorkedHours = workedHours;
        }




        // --- Derived Class Methods ---
        
        public override double CalculateGrossPay()
        {
            double basePay;

            if (WorkedHours <= 160)
            {
                basePay = WorkedHours * HourlyRate;
            }
            else
            {
                basePay = (160 * HourlyRate) + ((WorkedHours - 160) * HourlyRate * 1.5);
            }

            return basePay + MonthlyBonus;
        }

        public override string GetEmployeeType()
        {
            return "Hourly";
        }

        public override void DisplayPayrollInfo()
        {
            base.DisplayPayrollInfo();
            Console.WriteLine($"  Hourly Rate: {HourlyRate:N0}");
            Console.WriteLine($"  Worked Hours: {WorkedHours}");
            Console.WriteLine($"  Gross Pay: {CalculateGrossPay():N0}");
            Console.WriteLine(new string('-', 40));
        }
    }
}