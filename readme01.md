# Enterprise Payroll and Bonus Management System

A monthly corporate remuneration calculation and payroll management application developed in C# (.NET), implementing core Object-Oriented Programming (OOP) principles: Encapsulation, Inheritance, Overloading, Polymorphism, and Aggregation.

---

## 1. Architecture and OOP Principles

1. **Inheritance Relationships (Inheritance):**
   * The abstract class `Employee` serves as the common base class.
   * Derived classes `SalariedEmployee`, `HourlyEmployee`, and `SalesEmployee` inherit from `Employee`, extending domain-specific attributes and discrete gross earnings calculation logic.

2. **Overloading Techniques (Overloading):**
   * **Constructor Overloading:** Employs constructor chaining (`: this(...)` and `: base(...)`) to eliminate redundant input validation logic.
   * **Method Overloading (`AddBonus`):** Provides three versatile bonus processing overloads (fixed bonus, fixed bonus with justification, and percentage-based bonus derived from a reference amount). Static binding occurs at compile time.

3. **Polymorphism and Overriding (Polymorphism & Overriding):**
   * The methods `CalculateGrossPay()`, `GetEmployeeType()`, and `DisplayPayrollInfo()` are declared `abstract`/`virtual` in `Employee` and overridden (`override`) within subclasses.
   * The `Payroll` management class invokes calculation routines via dynamic dispatch (`vtable`) at runtime without relying on conditional type-branching (`if/else`).

4. **Non-owning Aggregation (Aggregation):**
   * The `Payroll` class maintains a `List<Employee>` collection. `Payroll` does not inherit from `Employee`; the lifecycle of individual employee instances remains entirely independent of the payroll period.

5. **Encapsulation and Invariants:**
   * State is encapsulated through `private` backing fields and validated properties (`get`, `private set`).
   * String constraints: `EmployeeId`, `FullName`, `Department`, and `Period` must neither be empty nor consist solely of whitespace.
   * Financial & operational invariants:
     * Bonus amounts must be non-negative; reference bonus rates $\in (0, 0.5]$.
     * Monthly salary, allowances, hourly rates, and sales turnover $\ge 0$.
     * Logged monthly working hours $\in [0, 250]$.
     * Sales commission rate $\in [0, 0.3]$.
   * Collection integrity: Enforces uniqueness by rejecting duplicate `EmployeeId` records within a single payroll period.

---

## 2. Directory Structure

```text
.
├── Classes/
│   ├── Employee.cs            # Abstract base class defining employee entity
│   ├── HourlyEmployee.cs      # Hourly-waged employee with overtime tracking
│   ├── Payroll.cs             # Payroll management and gross remuneration aggregation
│   ├── SalariedEmployee.cs    # Salaried employee with responsibility allowance
│   └── SalesEmployee.cs       # Commission-based sales employee
├── Test/
│   └── Test.cs                # System verification and test execution suite
├── Design.pdf                 # Analysis, class design specifications, and test report
├── OOP_Exercise_04.csproj     # .NET project configuration file
└── README.md                  # Documentation and system specifications
```

---

## 3. Build and Execution Instructions

### Environment Prerequisites
* .NET SDK 6.0 or higher (or the Pixi package environment).

### Execution Commands

Using the standard .NET CLI:
```bash
dotnet run
```

Or running via Pixi:
```bash
pixi run dotnet run
```

---

## 4. Test Scenarios (Verification Suite)

The verification suite consolidated in `Test/Test.cs` is executed across two distinct stages:

1. **Canonical Business Benchmark (Canonical Benchmark):**
   * Initialises the `2026-09` payroll period with four sample entities (`E001` to `E004`).
   * Exercises all three overloaded variants of `AddBonus()`.
   * Renders the payroll breakdown via polymorphic dispatch; verifies total enterprise payroll expenditure, total expenditure for the "Support" department, and extracts the highest-earning employee (`E004`).

2. **Boundary and Exception Testing (Invariant Test Scenarios):**
   * Defensive exception handling (`try-catch`) evaluated through the helper method `ExecuteTest()`:
     * **TC01:** Rejects empty or whitespace-only employee identifiers (`ArgumentException`).
     * **TC02:** Rejects negative bonus amounts (`ArgumentOutOfRangeException`).
     * **TC03:** Rejects bonus rates exceeding the 0.5 threshold (`ArgumentOutOfRangeException`).
     * **TC04:** Rejects negative monthly base salaries (`ArgumentOutOfRangeException`).
     * **TC05:** Rejects logged hours exceeding the 250h threshold (`ArgumentOutOfRangeException`).
     * **TC06:** Rejects commission rates exceeding the 0.3 ceiling (`ArgumentOutOfRangeException`).
     * **TC07:** Rejects duplicate employee ID `E001` additions to the payroll (returns `false`).
     * **TC08:** Rejects null object references when adding to the payroll (`ArgumentNullException`).
     * **TC09:** Rejects department queries with an empty string (`ArgumentException`).
     * **TC10:** Verifies defensive behaviour on empty payroll records (returns `null` and total gross pay of `0`).