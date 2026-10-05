/****************/
// Mã sinh viên: 202418838
// Họ tên:       Bùi Tuấn Anh
/****************/

using System;
using System.Text;
using PayrollSystem.Classes;

namespace PayrollSystem
{
    internal class Test
    {
        static void Main(string[] args)
        {
            // Ensure console correctly renders characters
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(new string('=', 70));
            Console.WriteLine("    ENTERPRISE PAYROLL & REMUNERATION MANAGEMENT SYSTEM");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();

            // =========================================================================
            // PART 1: OBJECT INSTANTIATION TEST
            // =========================================================================
            Console.WriteLine(">>> PART 1: CANONICAL PAYROLL PROCESSING (BENCHMARK DATA) <<<");
            Console.WriteLine();

            // 1. Initialise Payroll for period 2026-09
            Payroll payroll = new Payroll("2026-09");

            // 2. Instantiate and configure Employee 1: Salaried Employee
            // E001: Nguyễn Minh An | Dept: Đào tạo | Monthly: 15,000,000 | Allowance: 2,000,000 | Bonus: 1,000,000
            SalariedEmployee e001 = new SalariedEmployee(
                "E001",
                "Nguyễn Minh An",
                "Đào tạo",
                15000000,
                2000000
            );
            // Overloaded version 1: fixed bonus amount
            e001.AddBonus(1000000);
            payroll.AddEmployee(e001);

            // 3. Instantiate and configure Employee 2: Hourly Employee (Standard hours, no overtime)
            // E002: Trần Thu Bình | Dept: Hỗ trợ | Rate: 100,000 | Hours: 150 | Bonus: 500,000
            HourlyEmployee e002 = new HourlyEmployee(
                "E002",
                "Trần Thu Bình",
                "Hỗ trợ",
                100000,
                150
            );
            // Overloaded version 2: fixed bonus with explicit reason
            e002.AddBonus(500000, "Monthly performance bonus");
            payroll.AddEmployee(e002);

            // 4. Instantiate and configure Employee 3: Hourly Employee (With overtime exceeding 160 hours)
            // E003: Lê Hoàng Chi | Dept: Hỗ trợ | Rate: 100,000 | Hours: 170 | No bonus
            HourlyEmployee e003 = new HourlyEmployee(
                "E003",
                "Lê Hoàng Chi",
                "Hỗ trợ",
                100000,
                170
            );
            payroll.AddEmployee(e003);

            // 5. Instantiate and configure Employee 4: Sales Employee
            // E004: Phạm Quốc Dũng | Dept: Kinh doanh | Base: 8,000,000 | Revenue: 200,000,000 | Commission: 5%
            // Bonus: 2% of 50,000,000 = 1,000,000
            SalesEmployee e004 = new SalesEmployee(
                "E004",
                "Phạm Quốc Dũng",
                "Kinh doanh",
                8000000,
                200000000,
                0.05
            );
            // Overloaded version 3: rate applied to reference amount with justification
            e004.AddBonus(0.02, 50000000, "Quarterly revenue target achievement");
            payroll.AddEmployee(e004);

            // 6. Display complete payroll breakdown polymorphically
            payroll.DisplayPayroll();
            Console.WriteLine();

            // 7. Aggregate validations as per specification
            Console.WriteLine("--- AGGREGATE VERIFICATION CHECKS ---");

            double totalPayroll = payroll.CalculateTotalPayroll();
            Console.WriteLine($"Calculated Total Payroll : {totalPayroll:N0} (Expected: 70,000,000)");

            double supportDeptPayroll = payroll.CalculatePayrollByDepartment("Hỗ trợ");
            Console.WriteLine($"'Hỗ trợ' Dept Subtotal     : {supportDeptPayroll:N0} (Expected: 33,000,000)");

            Employee? highestEarner = payroll.FindHighestPaidEmployee();
            if (highestEarner != null)
            {
                Console.WriteLine($"Highest Compensated Staff: {highestEarner.FullName} ({highestEarner.EmployeeId}) " +
                                  $"- Gross Pay: {highestEarner.CalculateGrossPay():N0}");
            }

            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();

            // =========================================================================
            // PART 2: BOUNDARY AND EXCEPTION TEST SUITE
            // =========================================================================
            Console.WriteLine(">>> PART 2: INVARIANT, BOUNDARY & EXCEPTION TEST SUITE <<<");
            Console.WriteLine("Verifying defensive programming contracts and exception handling...\n");

            int testCounter = 1;

            // TC01: Empty or whitespace Employee ID
            ExecuteTest(testCounter++, "TC01: Reject creation of Employee with blank or whitespace ID", () =>
            {
                _ = new SalariedEmployee("   ", "Valid Name", 10000000);
            });

            // TC02: Negative Monthly Bonus via AddBonus(amount)
            ExecuteTest(testCounter++, "TC02: Reject negative bonus allocation in AddBonus(amount)", () =>
            {
                Employee sampleEmp = new SalariedEmployee("E101", "Sample Staff", 10000000);
                sampleEmp.AddBonus(-250000);
            });

            // TC03: Bonus rate exceeding permitted maximum threshold (> 0.5)
            ExecuteTest(testCounter++, "TC03: Reject bonus percentage exceeding 0.5 in AddBonus(rate, reference, reason)", () =>
            {
                Employee sampleEmp = new SalariedEmployee("E102", "Sample Staff", 10000000);
                sampleEmp.AddBonus(0.55, 10000000, "Excessive bonus rate");
            });

            // TC04: Negative Monthly Salary for SalariedEmployee
            ExecuteTest(testCounter++, "TC04: Reject negative monthly salary for SalariedEmployee", () =>
            {
                _ = new SalariedEmployee("E103", "Invalid Staff", -5000000);
            });

            // TC05: Hourly Employee worked hours exceeding maximum ceiling (> 250)
            ExecuteTest(testCounter++, "TC05: Reject worked hours exceeding monthly ceiling of 250 hours", () =>
            {
                _ = new HourlyEmployee("E104", "Overworked Staff", "Operations", 100000, 250.5);
            });

            // TC06: Commission rate exceeding maximum boundary of 0.3 (30%)
            ExecuteTest(testCounter++, "TC06: Reject commission rate exceeding 0.3 (30%) for SalesEmployee", () =>
            {
                _ = new SalesEmployee("E105", "Greedy Agent", "Sales", 8000000, 100000000, 0.35);
            });

            // TC07: Duplicate Employee ID detection and rejection in Payroll
            Console.WriteLine($"[Test {testCounter++}] TC07: Reject duplicate Employee ID insertion into Payroll");
            bool duplicateAdded = payroll.AddEmployee(new SalariedEmployee("E001", "Impostor An", 12000000));
            if (!duplicateAdded)
            {
                Console.WriteLine("  -> SUCCESS: Correctly rejected duplicate Employee ID 'E001' (returned false).");
            }
            else
            {
                Console.WriteLine("  -> FAILURE: Incorrectly accepted duplicate Employee ID 'E001'.");
            }
            Console.WriteLine();

            // TC08: Reject adding null Employee reference to Payroll
            ExecuteTest(testCounter++, "TC08: Reject adding null Employee reference to Payroll", () =>
            {
                payroll.AddEmployee(null!);
            });

            // TC09: Department salary aggregation with blank/whitespace query string
            ExecuteTest(testCounter++, "TC09: Reject blank department query string in CalculatePayrollByDepartment", () =>
            {
                payroll.CalculatePayrollByDepartment("   ");
            });

            // TC10: Safe query execution on an empty Payroll instance
            Console.WriteLine($"[Test {testCounter++}] TC10: Safe behaviour verification on completely empty Payroll instance");
            Payroll emptyPayroll = new Payroll("2026-10");
            Employee? emptyHighest = emptyPayroll.FindHighestPaidEmployee();
            double emptyTotal = emptyPayroll.CalculateTotalPayroll();

            if (emptyHighest == null && emptyTotal == 0.0)
            {
                Console.WriteLine("  -> SUCCESS: Returned null for highest earner and 0 for grand total.");
            }
            else
            {
                Console.WriteLine("  -> FAILURE: Unexpected state encountered on empty payroll instance.");
            }
            Console.WriteLine();

            Console.WriteLine(new string('=', 70));
            Console.WriteLine("        ALL VERIFICATION PROCEDURES CONCLUDED");
            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// Helper test runner to capture expected exception throwing and log outcome in British English.
        /// </summary>
        private static void ExecuteTest(int testNumber, string description, Action action)
        {
            Console.WriteLine($"[Test {testNumber}] {description}");
            try
            {
                action();
                Console.WriteLine("  -> FAILURE: Expected exception was not raised.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"  -> SUCCESS: Caught expected ArgumentOutOfRangeException [{ex.ParamName}]: {ex.Message.Split('\r', '\n')[0]}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  -> SUCCESS: Caught expected ArgumentNullException [{ex.ParamName}]: {ex.Message.Split('\r', '\n')[0]}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  -> SUCCESS: Caught expected ArgumentException: {ex.Message.Split('\r', '\n')[0]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  -> PARTIAL: Caught unexpected exception type ({ex.GetType().Name}): {ex.Message}");
            }
            Console.WriteLine();
        }
    }
}