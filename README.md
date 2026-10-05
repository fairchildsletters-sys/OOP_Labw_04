# Hệ thống Tính lương và Thưởng Nhân sự (Enterprise Payroll System)

Chương trình tính thu nhập và quản lý bảng lương hàng tháng cho doanh nghiệp bằng ngôn ngữ C# (.NET), áp dụng các nguyên lý lập trình hướng đối tượng (OOP): Đóng gói, Kế thừa, Nạp chồng, Đa hình và Kết tập.

**Thông tin sinh viên:**
* **Mã sinh viên:** 202418838
* **Họ tên:**       Bùi Tuấn Anh

---

## 1. Kiến trúc và Các nguyên lý OOP

1. **Quan hệ Kế thừa (Inheritance):**
   * Lớp trừu tượng `Employee` đóng vai trò lớp cơ sở.
   * Các lớp dẫn xuất `SalariedEmployee`, `HourlyEmployee`, `SalesEmployee` kế thừa `Employee`, mở rộng các thuộc tính và công thức tính thu nhập riêng biệt.

2. **Kỹ thuật Nạp chồng (Overloading):**
   * **Nạp chồng Constructor:** Áp dụng kỹ thuật ủy quyền constructor (`: this(...)` và `: base(...)`) nhằm triệt tiêu sự lặp lại logic kiểm tra tính hợp lệ của dữ liệu.
   * **Nạp chồng Phương thức (`AddBonus`):** Cung cấp 3 phiên bản xử lý thưởng linh hoạt (thưởng cố định, thưởng kèm lý do, và thưởng theo tỷ lệ phần trăm của giá trị tham chiếu). Liên kết tĩnh diễn ra tại thời điểm biên dịch (compile-time).

3. **Tính Đa hình và Ghi đè (Polymorphism & Overriding):**
   * Các phương thức `CalculateGrossPay()`, `GetEmployeeType()`, `DisplayPayrollInfo()` được khai báo `abstract`/`virtual` tại `Employee` và được ghi đè (`override`) ở các lớp con.
   * Lớp quản lý `Payroll` kích hoạt các phương thức tính toán thông qua cơ chế liên kết động (dynamic dispatch / vtable) tại thời điểm chạy (runtime) mà không sử dụng chuỗi rẽ nhánh điều kiện (`if/else`) theo kiểu đối tượng.

4. **Quan hệ Kết tập không sở hữu (Aggregation):**
   * Lớp `Payroll` quản lý tập hợp `List<Employee>`. `Payroll` không kế thừa từ `Employee`; vòng đời của các đối tượng nhân sự độc lập hoàn toàn với vòng đời của bảng lương.

5. **Tính Đóng gói và Ràng buộc Bất biến (Invariants):**
   * Dữ liệu được bảo vệ qua các trường `private` và thuộc tính có kiểm soát (`get`, `private set`).
   * Ràng buộc chuỗi: `EmployeeId`, `FullName`, `Department`, `Period` không được rỗng hoặc chỉ chứa khoảng trắng.
   * Ràng buộc tài chính & nghiệp vụ:
     * Tiền thưởng không được âm; tỷ lệ thưởng tham chiếu $\in (0, 0.5]$.
     * Lương tháng, phụ cấp, đơn giá giờ, doanh số bán hàng $\ge 0$.
     * Số giờ làm việc trong tháng $\in [0, 250]$.
     * Tỷ lệ hoa hồng doanh số $\in [0, 0.3]$.
   * Ràng buộc toàn vẹn danh sách: Từ chối thêm nhân sự có trùng `EmployeeId` trong cùng một kỳ lương.

---

## 2. Cấu trúc thư mục

```text
.
├── Classes/
│   ├── Employee.cs            # Lớp trừu tượng cơ sở định nghĩa nhân sự
│   ├── HourlyEmployee.cs      # Nhân viên hưởng lương theo giờ và giờ làm thêm
│   ├── Payroll.cs             # Lớp quản lý bảng lương và tổng hợp thu nhập
│   ├── SalariedEmployee.cs    # Nhân viên hưởng lương cố định và phụ cấp
│   └── SalesEmployee.cs       # Nhân viên kinh doanh theo lương cứng và hoa hồng
├── Test/
│   └── Test.cs                # Chương trình thực thi kiểm thử và xác minh hệ thống
├── OOP_Exercise_04.csproj     # File cấu hình dự án .NET
└── README.md                  # Tài liệu hướng dẫn và đặc tả hệ thống
```

---

## 3. Hướng dẫn biên dịch và thực thi

### Yêu cầu môi trường
* .NET SDK 6.0 trở lên (hoặc môi trường quản lý gói Pixi).

### Lệnh thực thi

Sử dụng .NET CLI tiêu chuẩn:
```bash
dotnet run
```

Hoặc thực thi qua Pixi:
```bash
pixi run dotnet run
```

---

## 4. Kịch bản kiểm thử (Verification Suite)

Chương trình kiểm thử tập trung tại `Test/Test.cs` được chia thành hai giai đoạn độc lập:

1. **Kiểm thử Nghiệp vụ Chuẩn (Canonical Benchmark):**
   * Khởi tạo bảng lương kỳ `2026-09` với 4 đối tượng mẫu (`E001` đến `E004`).
   * Sử dụng đầy đủ 3 phiên bản nạp chồng của `AddBonus()`.
   * Hiển thị bảng lương bằng cơ chế đa hình; đối chiếu tổng chi phí lương toàn doanh nghiệp (70.000.000), tổng chi phí phòng ban "Hỗ trợ" (33.000.000), và trích xuất nhân sự có thu nhập cao nhất (`E004` - 19.000.000).

2. **Kiểm thử Biên và Ngoại lệ (10 Kịch bản Invariant Test):**
   * Bắt ngoại lệ phòng thủ (`try-catch`) qua phương thức hỗ trợ `ExecuteTest()`:
     * **TC01:** Chặn mã nhân sự rỗng (`ArgumentException`).
     * **TC02:** Chặn tiền thưởng âm (`ArgumentOutOfRangeException`).
     * **TC03:** Chặn tỷ lệ thưởng vượt quá 0.5 (`ArgumentOutOfRangeException`).
     * **TC04:** Chặn mức lương tháng âm (`ArgumentOutOfRangeException`).
     * **TC05:** Chặn số giờ làm vượt trần 250h (`ArgumentOutOfRangeException`).
     * **TC06:** Chặn tỷ lệ hoa hồng vượt ngưỡng 0.3 (`ArgumentOutOfRangeException`).
     * **TC07:** Từ chối thêm nhân viên trùng mã `E001` vào bảng lương (trả về `false`).
     * **TC08:** Chặn thêm tham chiếu `null` vào bảng lương (`ArgumentNullException`).
     * **TC09:** Chặn truy vấn phòng ban với chuỗi rỗng (`ArgumentException`).
     * **TC10:** Kiểm tra tính an toàn khi truy vấn bảng lương rỗng (trả về `null` và tổng lương bằng `0`).