/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;

namespace PayrollSystem.Classes
{
    /// <summary>
    /// Abstract base class representing the common characteristics and behaviours of an employee.
    /// </summary>
    public abstract class Employee
    {
        // --- Private backing fields to protect invariants ---
        private string _employeeId = string.Empty;
        private string _fullName = string.Empty;
        private string _department = string.Empty;
        private double _monthlyBonus = 0.0;

        // --- Properties with encapsulated validation ---

        /// <summary>
        /// Gets the unique identifier for the employee. Immutable after initialisation.
        /// </summary>
        public string EmployeeId
        {
            get => _employeeId;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Employee ID must not be empty or contain only whitespace.");
                }
                _employeeId = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Full name must not be empty or contain only whitespace.");
                }
                _fullName = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets the department to which the employee belongs.
        /// </summary>
        public string Department
        {
            get => _department;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Department must not be empty or contain only whitespace.");
                }
                _department = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets the accumulated bonus amount for the current pay period.
        /// </summary>
        public double MonthlyBonus
        {
            get => _monthlyBonus;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Monthly bonus cannot be negative.");
                }
                _monthlyBonus = value;
            }
        }

        // --- Constructors (Overloading & Delegation) ---

        /// <summary>
        /// Shortened constructor initialising essential details with default values.
        /// Delegates to the full constructor to avoid duplicate validation logic.
        /// </summary>
        /// <param name="employeeId">The unique employee identifier.</param>
        /// <param name="fullName">The employee's full name.</param>
        public Employee(string employeeId, string fullName)
            : this(employeeId, fullName, "Unassigned")
        {
            // Body remains empty because logic is delegated to the primary constructor.
        }

        /// <summary>
        /// Full constructor initialising all base employee attributes.
        /// </summary>
        /// <param name="employeeId">The unique employee identifier.</param>
        /// <param name="fullName">The employee's full name.</param>
        /// <param name="department">The allocated department.</param>
        public Employee(string employeeId, string fullName, string department)
        {
            EmployeeId = employeeId;
            FullName = fullName;
            Department = department;
            MonthlyBonus = 0.0;
        }



        // --- Overloaded Bonus Methods ---

        /// <summary>
        /// Version 1: Adds a fixed bonus amount.
        /// </summary>
        /// <param name="amount">The bonus amount to add.</param>
        public void AddBonus(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Bonus amount must be strictly greater than zero.");
            }

            MonthlyBonus += amount;
        }

        /// <summary>
        /// Version 2: Adds a fixed bonus amount accompanied by a recorded reason.
        /// </summary>
        /// <param name="amount">The bonus amount to add.</param>
        /// <param name="reason">The rationale behind the bonus allocation.</param>
        public void AddBonus(double amount, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Bonus reason must not be empty or consist solely of whitespace.", nameof(reason));
            }

            // Reuse the existing validation logic from Version 1
            AddBonus(amount);
        }

        /// <summary>
        /// Version 3: Calculates and awards a bonus based on a specific percentage rate applied to a reference amount.
        /// </summary>
        /// <param name="rate">The bonus rate (must be in the range (0, 0.5]).</param>
        /// <param name="referenceAmount">The benchmark monetary amount.</param>
        /// <param name="reason">The rationale behind the bonus allocation.</param>
        public void AddBonus(double rate, double referenceAmount, string reason)
        {
            if (rate <= 0.0 || rate > 0.5)
            {
                throw new ArgumentOutOfRangeException(nameof(rate), "Bonus rate must be greater than 0 and cannot exceed 0.5 (50%).");
            }

            if (referenceAmount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(referenceAmount), "Reference amount must be strictly greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Bonus reason must not be empty or consist solely of whitespace.", nameof(reason));
            }

            double calculatedBonus = referenceAmount * rate;
            MonthlyBonus += calculatedBonus;
        }

        /// <summary>
        /// Resets the accumulated monthly bonus to zero at the start of a fresh payroll cycle.
        /// </summary>
        public void ResetBonus()
        {
            MonthlyBonus = 0.0;
        }

        // --- Polymorphic Methods ---

        /// <summary>
        /// Abstract method to calculate the gross income before any statutory deductions.
        /// Must be overridden by derived classes according to their specific remuneration scheme.
        /// </summary>
        /// <returns>Gross pay as a double.</returns>
        public abstract double CalculateGrossPay();

        /// <summary>
        /// Abstract method returning the descriptive category of the employee.
        /// </summary>
        /// <returns>The employee type string.</returns>
        public abstract string GetEmployeeType();

        /// <summary>
        /// Virtual method providing a foundational summary of the employee's payroll details.
        /// Can be extended by derived classes to append categoric specifics.
        /// </summary>
        public virtual void DisplayPayrollInfo()
        {
            Console.WriteLine($"ID: {EmployeeId} | Name: {FullName} | Dept: {Department} | Type: {GetEmployeeType()}");
            Console.WriteLine($"  Monthly Bonus: {MonthlyBonus:N0}");
        }
    }
}