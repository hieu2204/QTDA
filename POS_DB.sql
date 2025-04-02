CREATE DATABASE POS_DB
GO
USE POS_DB
GO
-- Bảng người dùng
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
	-- Thông tin đăng nhập
	Username VARCHAR(50) UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL, 
	-- Thông tin cá nhân
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE,
    Phone NVARCHAR(20) UNIQUE,
    Gender VARCHAR(20),  
    BirthDate DATE NULL,
	Address NVARCHAR(255),
	ImageURL NVARCHAR(255), 
	-- Thông tin công việc
    Salary DECIMAL(18,2) NULL, 
    Status TINYINT NOT NULL DEFAULT 1, -- 1: Hoạt động, 0: Đã nghỉ
    Role NVARCHAR(50) NOT NULL, -- Admin, Manager, Employee
	-- Thời gian tạo user và update
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- Bảng khách hàng
CREATE TABLE Customer (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone VARCHAR(15) UNIQUE,
    Email VARCHAR(255) UNIQUE,
    Address NVARCHAR(255),
	LoyaltyPoints INT DEFAULT 0,
	Status TINYINT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
-- Bảng nhà cung cấp
CREATE TABLE Supplier (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Phone VARCHAR(15) UNIQUE,
    Email VARCHAR(255) UNIQUE,
    Address NVARCHAR(255),
	Status TINYINT DEFAULT 1, -- 1 là Đang hợp tác 0 là không hợp tác
	CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
-- Bảng danh mục
CREATE TABLE Category (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500) NULL
);
-- Bảng sản phẩm
CREATE TABLE Product (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    CategoryID INT NOT NULL,
    SupplierID INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT DEFAULT 0,
    Description NVARCHAR(500),
	ImageURL NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);

/*
ALTER TABLE Product
ALTER COLUMN SupplierID INT NULL
*/
-- Bảng chương trình khuyến mãi
CREATE TABLE Promotion (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,  -- Tên chương trình khuyến mãi
    Description NVARCHAR(500),    -- Mô tả khuyến mãi
    DiscountType TINYINT NOT NULL, -- 1: Giảm giá theo % | 2: Giảm giá số tiền | 3: Tặng kèm sản phẩm
    DiscountValue DECIMAL(18,2) NULL, -- Giá trị giảm giá (nếu có)
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status TINYINT DEFAULT 1  -- 1: Đang áp dụng, 0: Ngừng áp dụng
);

CREATE TABLE PromotionDetail (
    PromotionDetailID INT IDENTITY(1,1) PRIMARY KEY,
    PromotionID INT NOT NULL,
    -- Điều kiện áp dụng
    ProductID INT NULL,            -- Sản phẩm cần mua (NULL nếu áp dụng cho đơn hàng)
    MinQuantity INT DEFAULT 1,    -- Số lượng sản phẩm cần mua
    AppliesToOrder BIT DEFAULT 0, -- 1: Áp dụng cho đơn hàng
    -- Thông tin tặng quà (chỉ dùng khi DiscountType = 3)
    GiftProductID INT NULL,       -- Sản phẩm được tặng
    GiftQuantity INT NULL,        -- Số lượng tặng
    FOREIGN KEY (PromotionID) REFERENCES Promotion(PromotionID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (GiftProductID) REFERENCES Product(ProductID)
);
SELECT * FROM PromotionDetail

ALTER TABLE Promotion
DROP COLUMN DiscountValue

ALTER TABLE PromotionDetail
ADD MinBillAmount DECIMAL(18,2)

ALTER TABLE Promotion
ALTER COLUMN DiscountType NVARCHAR(50)

ALTER TABLE PromotionDetail
ADD DiscountValue DECIMAL(18, 2) NULL

CREATE TABLE PaymentMethod (
    MethodID INT IDENTITY(1,1) PRIMARY KEY,
    MethodName NVARCHAR(50) NOT NULL UNIQUE, -- Ví dụ: Tiền mặt, Momo, ZaloPay
    Description NVARCHAR(255) NULL
);
-- Bảng hóa đơn bán hàng
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NULL,  -- Khách hàng (NULL nếu khách lẻ)
    UserID INT NOT NULL,  -- Nhân viên thực hiện đơn hàng
    OrderDate DATETIME DEFAULT GETDATE(), -- Ngày đặt hàng
    TotalAmount DECIMAL(18,2) NOT NULL, -- Tổng tiền trước khuyến mãi
    FinalTotalAmount DECIMAL(18,2) NULL, -- Tổng tiền sau khuyến mãi
    Status TINYINT DEFAULT 1, -- 1: Hoàn thành, 0: Hủy
	MethodID INT, 
    PaymentStatus TINYINT DEFAULT 1, -- 0: Chưa thanh toán, 1: Đã thanh toán, 2: Hoàn tiền
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (MethodID) REFERENCES PaymentMethod(MethodID)
);
-- Bảng Order cho phép áp dụng nhiều khuyến mãi
CREATE TABLE OrderPromotion (
    OrderID INT NOT NULL,
    PromotionID INT NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL, -- Số tiền giảm (tính toán sẵn)
    PRIMARY KEY (OrderID, PromotionID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (PromotionID) REFERENCES Promotion(PromotionID)
);
-- Bảng chi tiết hóa đơn bán hàng
CREATE TABLE OrderDetail (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
-- Bảng hóa đơn nhập kho
CREATE TABLE StockReceipt (
    ReceiptID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierID INT NOT NULL,
    UserID INT NOT NULL,
    ReceiptDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
-- Bảng chi tiết hóa đơn nhập kho
CREATE TABLE StockReceiptDetail (
    ReceiptDetailID INT IDENTITY(1,1) PRIMARY KEY,
    ReceiptID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ReceiptID) REFERENCES StockReceipt(ReceiptID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
ALTER TABLE StockReceipt DROP CONSTRAINT FK__StockRece__Suppl__56B3DD81;
ALTER TABLE StockReceiptDetail
ADD RemainingQuantity INT;

-- Cập nhật giá trị ban đầu cho RemainingQuantity bằng Quantity
UPDATE StockReceiptDetail
SET RemainingQuantity = Quantity;
ALTER TABLE StockReceipt
DROP COLUMN SupplierID

ALTER TABLE StockReceiptDetail
ADD SupplierID INT NOT NULL


--DROP DATABASE POS_DB;
/*
-- Xóa thủ tục

*/
IF OBJECT_ID('dbo.GetAllCustomer', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetAllCustomer;
GO
-- Tạo thủ tục


-- Customer
CREATE PROCEDURE GetAllCustomer
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT * FROM Customer
	WHERE Status = 1
	ORDER BY CustomerID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

EXEC GetAllCustomer 1, 8

CREATE PROCEDURE GetTotalPage
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Customer
END

EXEC GetTotalPage 8
IF OBJECT_ID('dbo.GetListCus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetListCus;
GO

CREATE PROCEDURE UpdateCus
	@ID INT,
	@Name NVARCHAR(100),
	@Phone VARCHAR(15),
	@Email VARCHAR(255),
	@Address NVARCHAR(255),
	@LoyaltyPoints INT,
	@Status INT,
	@UpdateAt DATETIME
AS
BEGIN
	UPDATE Customer
	SET
		FullName = @Name,
		Phone = @Phone,
		Email = @Email,
		Address = @Address,
		LoyaltyPoints = @LoyaltyPoints,
		Status = @Status,
		UpdatedAt = @UpdateAt
		WHERE
			CustomerID = @ID
END

ALTER TABLE Customer
ADD CONSTRAINT DF_Customer_Status
DEFAULT 1 FOR Status;
CREATE PROCEDURE InsertCus
	@Name NVARCHAR(255),
	@Phone VARCHAR(15),
	@Email VARCHAR(100),
	@Address NVARCHAR(500)
AS
BEGIN
	INSERT INTO Customer(FullName, Phone, Email, Address)
	VALUES(@Name, @Phone, @Email, @Address);
END


CREATE PROCEDURE CheckCustomerEmailOrPhoneExists
    @Email NVARCHAR(255),
    @Phone NVARCHAR(20),
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmailExists BIT = 0;
    DECLARE @PhoneExists BIT = 0;

    -- Kiểm tra email có bị trùng không (loại trừ chính nó khi cập nhật)
    IF EXISTS (SELECT 1 FROM Customer WHERE Email = @Email AND CustomerID <> @CustomerID)
    BEGIN
        SET @EmailExists = 1;
    END

    -- Kiểm tra số điện thoại có bị trùng không (loại trừ chính nó khi cập nhật)
    IF EXISTS (SELECT 1 FROM Customer WHERE Phone = @Phone AND CustomerID <> @CustomerID)
    BEGIN
        SET @PhoneExists = 1;
    END

    -- Trả về kết quả
    SELECT @EmailExists AS EmailExists, @PhoneExists AS PhoneExists;
END;

EXEC CheckCustomerEmailOrPhoneExists 
    @Email = 'test@example.com', 
    @Phone = '123456789', 
    @CustomerID = 1;



CREATE PROCEDURE GetListCus
AS
BEGIN
	SELECT 
		CustomerID,
		FullName,
		Phone,
		LoyaltyPoints
	FROM Customer
	WHERE Status = 1
END

CREATE PROCEDURE ArchiveCustomer
    @CustomerID INT
AS
BEGIN
    UPDATE Customer
    SET Status = 0
    WHERE CustomerID = @CustomerID;
END;







--User
SELECT * FROM Users

ALTER TABLE Users
ALTER COLUMN Gender NVARCHAR(20)
CREATE PROCEDURE InsertUser
	@Username VARCHAR(50),
	@PasswordHash NVARCHAR(255),
	@FullName NVARCHAR(100),
	@Email NVARCHAR(255),
    @Phone NVARCHAR(20),
	@Gender NVARCHAR(20),
	@BirthDate DATE,
	@Address NVARCHAR(255),
	@ImageURL NVARCHAR(255),
	@Salary DECIMAL(18, 2),
	@Role NVARCHAR(50)
AS
BEGIN
	INSERT INTO Users(Username, PasswordHash, FullName, Email, Phone, Gender, BirthDate, Address, ImageURL, Salary, Role)
	VALUES (@Username, @PasswordHash, @FullName,@Email,@Phone,@Gender,@BirthDate,@Address, @ImageURL,@Salary,@Role);
END


CREATE PROCEDURE CheckEmailExists
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT COUNT(*) FROM Users WHERE Email = @Email
END
GO

CREATE PROCEDURE CheckPhoneExists
    @Phone NVARCHAR(20)
AS
BEGIN
    SELECT COUNT(*) FROM Users WHERE Phone = @Phone
END
GO

CREATE PROCEDURE CheckEmailOrPhoneExists
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmailExists BIT = 0;
    DECLARE @PhoneExists BIT = 0;

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND UserID <> @UserID)
        SET @EmailExists = 1;

    IF EXISTS (SELECT 1 FROM Users WHERE Phone = @Phone AND UserID <> @UserID)
        SET @PhoneExists = 1;

    SELECT @EmailExists AS EmailExists, @PhoneExists AS PhoneExists;
END;

EXEC CheckEmailOrPhoneExists 'email@example.com', '0123456789', 5



IF OBJECT_ID('dbo.CheckEmailOrPhoneExists', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CheckEmailOrPhoneExists;


CREATE PROCEDURE GetAllUser
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT * FROM Users
	WHERE Status = 1
	ORDER BY UserID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

EXEC GetAllUser 1, 6

CREATE PROCEDURE GetListUser
AS
BEGIN
	SELECT * FROM Users
END



CREATE PROCEDURE GetTotalPageUser
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Users
END
EXEC GetTotalPageUser 6;

CREATE PROCEDURE GetUserName
	@Username VARCHAR(100)
AS
BEGIN
	SELECT  COUNT(*) FROM Users
	WHERE Username = @Username
END
EXEC GetUserName abc

CREATE PROCEDURE UpdateUser
	@ID INT,
	@Pass NVARCHAR(255),
	@Name NVARCHAR(100),
	@Email VARCHAR(100),
	@Phone VARCHAR(20),
	@Gender NVARCHAR(30),
	@BirthDate DATE,
	@Address NVARCHAR(255),
	@ImageURL NVARCHAR(255),
	@Salary DECIMAL(18, 2),
	@Status TINYINT,
	@Role VARCHAR(100)
AS
BEGIN
	UPDATE Users
	SET
		PasswordHash = @Pass,
		FullName = @Name,
		Email = @Email,
		Phone = @Phone,
		Gender = @Gender,
		BirthDate = @BirthDate,
		Address = @Address,
		ImageURL = @ImageURL,
		Salary = @Salary,
		Status = @Status,
		Role = @Role
	WHERE
		UserID = @ID
END
EXEC GetAllUser @PageNumber = 1, @PageSize = 10;


CREATE PROCEDURE SearchUser
    @Keyword NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Users 
    WHERE Status = 1
    AND (
        FullName LIKE '%' + @Keyword + '%'
        OR Phone LIKE '%' + @Keyword + '%'
        OR Email LIKE '%' + @Keyword + '%'
        OR CAST(Salary AS NVARCHAR) LIKE '%' + @Keyword + '%'
    )
END


-- Category
CREATE PROCEDURE GetAllCategory
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT * FROM Category
	ORDER BY CategoryID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

CREATE PROCEDURE CheckCategoryExists
    @Name NVARCHAR(255),
    @CategoryID INT
AS
BEGIN
    -- Kiểm tra xem danh mục đã tồn tại chưa (trừ danh mục đang chỉnh sửa)
    IF EXISTS (SELECT 1 FROM Category WHERE Name = @Name AND CategoryID <> @CategoryID)
    BEGIN
        SELECT 1; -- Tồn tại
    END
    ELSE
    BEGIN
        SELECT 0; -- Không tồn tại
    END
END


IF OBJECT_ID('dbo.CheckCategoryExists', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CheckCategoryExists;


CREATE PROCEDURE GetAllCategoryName
AS
BEGIN
	SELECT CategoryID, Name FROM Category
END
EXEC GetAllCategoryName

EXEC GetAllCategory 1, 9

CREATE PROCEDURE GetTotalPageCategory
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Category
END
EXEC GetTotalPageCategory 9;

CREATE PROCEDURE UpdateCategory
	@ID INT,
	@Name NVARCHAR(255),
	@Description NVARCHAR(500)
AS
BEGIN
	UPDATE Category
	SET
		Name = @Name,
		Description = @Description
	WHERE
		CategoryID = @ID
END


CREATE PROCEDURE InsertCategory
	@Name NVARCHAR(255),
	@Description NVARCHAR(500)
AS
BEGIN
	INSERT INTO Category(Name, Description)
	VALUES (@Name, @Description);
END

CREATE PROCEDURE SearchCategoryByName
    @Keyword NVARCHAR(255)
AS
BEGIN
    SELECT CategoryID, Name, Description
    FROM Category
    WHERE Name LIKE '%' + @Keyword + '%'
END







-- Supplier
CREATE PROCEDURE GetAllSupplier
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT * FROM Supplier
	WHERE Status = 1
	ORDER BY SupplierID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

EXEC GetAllSupplier 1, 8

CREATE PROCEDURE GetTotalPageSupplier
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Supplier
END

CREATE PROCEDURE CheckSupplierEmailOrPhoneExists
    @Email NVARCHAR(255),
    @Phone NVARCHAR(20),
    @SupplierID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmailExists BIT = 0;
    DECLARE @PhoneExists BIT = 0;

    -- Kiểm tra email có bị trùng không (loại trừ chính nó khi cập nhật)
    IF EXISTS (SELECT 1 FROM Supplier WHERE Email = @Email AND SupplierID <> @SupplierID)
    BEGIN
        SET @EmailExists = 1;
    END

    -- Kiểm tra số điện thoại có bị trùng không (loại trừ chính nó khi cập nhật)
    IF EXISTS (SELECT 1 FROM Supplier WHERE Phone = @Phone AND SupplierID <> @SupplierID)
    BEGIN
        SET @PhoneExists = 1;
    END

    -- Trả về kết quả
    SELECT @EmailExists AS EmailExists, @PhoneExists AS PhoneExists;
END;

IF OBJECT_ID('dbo.ArchiveSupplier', 'P') IS NOT NULL
    DROP PROCEDURE dbo.ArchiveSupplier;

CREATE PROCEDURE UpdateSupplier
	@ID INT,
	@Name NVARCHAR(100),
	@Phone VARCHAR(15),
	@Email VARCHAR(255),
	@Address NVARCHAR(255),
	@Status INT,
	@UpdateAt DATETIME
AS
BEGIN
	UPDATE Supplier
	SET
		Name = @Name,
		Phone = @Phone,
		Email = @Email,
		Address = @Address,
		Status = @Status,
		UpdatedAt = @UpdateAt
		WHERE
			SupplierID = @ID
END

CREATE PROCEDURE InsertSupplier
	@Name NVARCHAR(255),
	@Phone VARCHAR(15),
	@Email VARCHAR(100),
	@Address NVARCHAR(500)
AS
BEGIN
	INSERT INTO Supplier(Name, Phone, Email, Address)
	VALUES(@Name, @Phone, @Email, @Address);
END

CREATE PROCEDURE GetListSupplier
AS
BEGIN
	SELECT SupplierID, Name 
	FROM Supplier
	WHERE Status = 1
END




CREATE PROCEDURE ArchiveSupplier
    @SupplierID INT
AS
BEGIN
    UPDATE Supplier
    SET Status = 0
    WHERE SupplierID = @SupplierID;
END;

CREATE PROCEDURE SearchSupplier
    @Keyword NVARCHAR(255) = NULL
AS
BEGIN
    SELECT SupplierID, Name, Phone, Email, Address, Status, CreatedAt, UpdatedAt
    FROM Supplier
    WHERE Status = 1
    AND (
        @Keyword IS NULL OR
        Name LIKE '%' + @Keyword + '%' OR
        Email LIKE '%' + @Keyword + '%' OR
        Phone LIKE '%' + @Keyword + '%'
    )
END


-- Product

CREATE PROCEDURE AddProduct
	@Name NVARCHAR(255),
	@CategoryID INT,
	@Price DECIMAL(18, 2),
	@Description NVARCHAR(500),
	@ImageURL NVARCHAR(255)
AS
BEGIN
	INSERT INTO Product(Name, CategoryID, Price, Description, ImageURL) 
	VALUES(@Name, @CategoryID, @Price, @Description, @ImageURL);
END

IF OBJECT_ID('dbo.GetAllProduct', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetAllProduct;

CREATE VIEW vw_GetAllProduct
AS
	SELECT DISTINCT
		p.ProductID AS ProductID,
		p.Name AS Name,
		c.Name AS CategoryName,
		sp.Name AS SupplierName,
		p.Price AS Price,
		p.StockQuantity AS StockQuantity,
		p.Description AS Description,
		p.ImageURL AS ImageURL,
		p.CreatedAt AS CreatedAt,
		p.UpdatedAt AS UpdatedAt
	FROM Product p
	LEFT JOIN StockReceiptDetail sr ON p.ProductID = sr.ProductID
	LEFT JOIN Category c ON p.CategoryID = c.CategoryID
	LEFT JOIN Supplier sp ON sp.SupplierID = sr.SupplierID

DROP VIEW vw_GetAllProduct


CREATE VIEW vw_GetAllProduct AS
SELECT 
    p.ProductID,
    p.Name,
    c.Name AS CategoryName,
    sp.Name AS SupplierName,
    p.Price,
    ISNULL(SUM(srd.RemainingQuantity), 0) AS AvailableStock,
    p.Description,
    p.ImageURL,
    p.CreatedAt,
    p.UpdatedAt
FROM Product p
LEFT JOIN StockReceiptDetail srd ON p.ProductID = srd.ProductID
LEFT JOIN Category c ON p.CategoryID = c.CategoryID
LEFT JOIN Supplier sp ON sp.SupplierID = srd.SupplierID
GROUP BY 
    p.ProductID, 
    p.Name, 
    c.Name, 
    sp.Name, 
    p.Price, 
    p.Description, 
    p.ImageURL, 
    p.CreatedAt, 
    p.UpdatedAt;
GO

CREATE VIEW vw_GetAllProduct AS
SELECT 
    p.ProductID,
    p.Name AS Name,
    c.Name AS CategoryName,
    sp.Name AS SupplierName,
    p.Price,
    -- Tính tổng số lượng nhập từ từng nhà cung cấp
    ISNULL(SUM(srd.Quantity), 0) AS TotalStockBySupplier,
    -- Tính tổng số lượng hiện còn
    ISNULL(SUM(srd.RemainingQuantity), 0) AS AvailableStock,
    p.Description,
    p.ImageURL,
    p.CreatedAt,
    p.UpdatedAt
FROM Product p
LEFT JOIN StockReceiptDetail srd ON p.ProductID = srd.ProductID
LEFT JOIN Supplier sp ON sp.SupplierID = srd.SupplierID
LEFT JOIN Category c ON p.CategoryID = c.CategoryID
GROUP BY 
    p.ProductID, 
    p.Name, 
    c.Name, 
    sp.Name, 
    p.Price, 
    p.Description, 
    p.ImageURL, 
    p.CreatedAt, 
    p.UpdatedAt;
GO


CREATE VIEW vw_GetAllProduct 
AS
SELECT 
    p.ProductID AS ProductID,
    p.Name AS Name,
    c.Name AS CategoryName,
    sp.Name AS SupplierName,
    p.Price AS Price,
    COALESCE(SUM(sr.Quantity), 0) AS StockQuantity, -- Tính tổng số lượng nhập hàng từ StockReceiptDetail
    p.Description AS Description,
    p.ImageURL AS ImageURL,
    p.CreatedAt AS CreatedAt,
    p.UpdatedAt AS UpdatedAt
FROM Product p
LEFT JOIN StockReceiptDetail sr ON p.ProductID = sr.ProductID -- Lấy tổng số lượng nhập hàng
LEFT JOIN Category c ON p.CategoryID = c.CategoryID
LEFT JOIN Supplier sp ON sp.SupplierID = sr.SupplierID  
GROUP BY p.ProductID, p.Name, c.Name, sp.Name, p.Price, p.Description, p.ImageURL, p.CreatedAt, p.UpdatedAt;



SELECT * FROM vw_GetAllProduct

SELECT * FROM Promotion

SELECT * FROM PromotionDetail

	DROP VIEW vw_GetAllProduct

CREATE PROCEDURE GetAllProduct
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT 
		*
	FROM vw_GetAllProduct
	ORDER BY ProductID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

EXEC GetAllProduct 1, 8

CREATE PROCEDURE GetTotalPageProduct
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Product
END

EXEC GetTotalPageProduct 8




CREATE PROCEDURE GetTotalQuantity
	@ProductID INT
AS
BEGIN
	SELECT COALESCE(SUM(sr.Quantity), 0) AS TotalStock
	FROM Product p
	LEFT JOIN StockReceiptDetail sr ON p.ProductID = sr.ProductID
	WHERE p.ProductID = @ProductID -- Thay @ProductID bằng ID của sản phẩm cần lấy
	GROUP BY p.ProductID, p.Name;
END

SELECT p.ProductID, p.Name, COALESCE(SUM(sr.Quantity), 0) AS TotalStock
FROM Product p
LEFT JOIN StockReceiptDetail sr ON p.ProductID = sr.ProductID
WHERE p.ProductID = 1 -- Thay @ProductID bằng ID của sản phẩm cần lấy
GROUP BY p.ProductID, p.Name;


IF OBJECT_ID('dbo.GetListProduct', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetListProduct;

CREATE PROCEDURE GetListProduct
AS
BEGIN
	SELECT 
		ProductID, 
		Product.Name AS ProductName, 
		Category.Name AS CategoryName,
		Supplier.Name AS SupplierName,
		Price,
		ImageURL
	FROM Product
	JOIN Category ON Product.CategoryID = Category.CategoryID
	LEFT JOIN Supplier ON Product.SupplierID = Supplier.SupplierID;
END


CREATE PROCEDURE SearchProduct
    @Keyword NVARCHAR(255) = NULL
AS
BEGIN
    SELECT 
        p.ProductID,
        p.Name AS ProductName,
        c.Name AS CategoryName,
        sp.Name AS SupplierName,
        p.Price,
        COALESCE(SUM(sr.Quantity), 0) AS TotalStock,
        p.Description,
        p.ImageURL,
        p.CreatedAt,
        p.UpdatedAt
    FROM Product p
    LEFT JOIN StockReceiptDetail sr ON p.ProductID = sr.ProductID
    LEFT JOIN Category c ON p.CategoryID = c.CategoryID
    LEFT JOIN Supplier sp ON sp.SupplierID = sr.SupplierID
    WHERE
        (@Keyword IS NULL OR
        p.Name LIKE '%' + @Keyword + '%' OR
        c.Name LIKE '%' + @Keyword + '%' OR
        sp.Name LIKE '%' + @Keyword + '%' OR
        CAST(p.Price AS NVARCHAR) LIKE '%' + @Keyword + '%')
    GROUP BY 
        p.ProductID, p.Name, c.Name, sp.Name, p.Price, p.Description, p.ImageURL, p.CreatedAt, p.UpdatedAt
END



-- Stock
CREATE PROCEDURE GetNextInvoiceID
	@LastID INT
AS
BEGIN
	SELECT @LastID = MAX(ReceiptID) FROM StockReceipt;
	IF @LastID IS NULL
	SET @LastID = 0;
	SELECT @LastID + 1 AS NextInvoiceID;
END
SELECT * FROM sys.tables WHERE name = 'ProductSupplierStock';


CREATE PROCEDURE InsertStockReceiptWithDetails_JSON
	@UserID INT,
	@TotalAmount DECIMAL(18, 2),
	@ProductList NVARCHAR(MAX) -- JSON chứa danh sách sản phẩm
AS
BEGIN
	SET NOCOUNT ON; -- tăng tốc độ thực thi, không hiển thị thông báo nhưng vẫn cập nhật dữ liệu

	DECLARE @ReceiptID INT;
	INSERT INTO StockReceipt(UserID, TotalAmount)
	VALUES (@UserID, @TotalAmount)
	
	-- Lấy ID của hóa đơn vừa tạo
	SET @ReceiptID = SCOPE_IDENTITY(); -- Lấy ID mới nhất của bảng trong cùng scope(phạm vi)
	INSERT INTO StockReceiptDetail(ReceiptID, ProductID, SupplierID, Quantity, Price, RemainingQuantity)
SELECT
    @ReceiptID,
    ProductID,
    SupplierID,
    Quantity,
    Price,
    Quantity -- Gán RemainingQuantity = Quantity
	FROM OPENJSON(@ProductList)
	-- cách tách dữ liệu từ JSON
	WITH (
		ProductID INT '$.ProductID', -- ánh xạ cột dữ liệu trong bảng JSON
		SupplierID INT '$.SupplierID',
		Quantity INT '$.Quantity',
		Price DECIMAL(18, 2) '$.UnitPrice'
	);
END


IF OBJECT_ID('dbo.InsertStockReceiptWithDetails_JSON', 'P') IS NOT NULL
    DROP PROCEDURE dbo.InsertStockReceiptWithDetails_JSON;

CREATE VIEW vw_StockReceiptUser
AS
	SELECT sr.ReceiptID, us.FullName AS EmployeeName , sr.ReceiptDate, sr.TotalAmount
	FROM StockReceipt sr
	INNER JOIN Users us ON sr.UserID = us.UserID;

CREATE PROCEDURE GetAllStock
	@PageNumber INT,
	@PageSize INT
AS
BEGIN
	SELECT * FROM vw_StockReceiptUser
	ORDER BY ReceiptID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
CREATE PROCEDURE GetTotalPageStock
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM vw_StockReceiptUser
END

CREATE VIEW vw_PurchaseProduct
AS
	SELECT 
		sr.ReceiptID,
		p.Name AS ProductName,
		c.Name AS CategoryName,
		sp.Name AS SupplierName,
		srd.Quantity,
		srd.Price
	FROM StockReceiptDetail srd
	LEFT JOIN Product p ON srd.ProductID = p.ProductID
	LEFT JOIN Supplier sp ON sp.SupplierID = srd.SupplierID
	LEFT JOIN Category c ON c.CategoryID = p.CategoryID
	LEFT JOIN StockReceipt sr ON sr.ReceiptID = srd.ReceiptID


SELECT * FROM vw_PurchaseProduct
DROP VIEW IF EXISTS vw_PurchaseProduct;

CREATE PROCEDURE GetListPurchaseDetail
	@ReceiptID INT
AS 
BEGIN
	SELECT * FROM vw_PurchaseProduct
	WHERE ReceiptID = @ReceiptID;
END

EXEC GetListPurchaseDetail 5
-- Trigger


CREATE TRIGGER trg_UpdateStock_OnStockReceipt
ON StockReceiptDetail
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;
	Update p
	SET p.StockQuantity = p.StockQuantity + i.Quantity
	FROM Product p
	INNER JOIN inserted i ON p.ProductID = i.ProductID 
	WHERE p.SupplierID = i.SupplierID
END

SELECT * FROM Product WHERE ProductID = 1;
SELECT * 
FROM Product p 
JOIN StockReceiptDetail s ON p.ProductID = s.ProductID
WHERE p.SupplierID = s.SupplierID;
SELECT * FROM StockReceiptDetail

DROP TRIGGER trg_UpdateStock_OnStockReceipt 


-- Promotion
	
SELECT * FROM Promotion

SELECT * FROM PromotionDetail

CREATE PROCEDURE InsertPromotion
    @Name NVARCHAR(255),
    @Description NVARCHAR(500),
    @DiscountType NVARCHAR(50),
    @StartDate DATETIME,
    @EndDate DATETIME,
    @ProductID INT = NULL,
    @MinQuantity INT = NULL,
    @AppliesToOrder BIT = NULL,
    @GiftProductID INT = NULL,
    @GiftQuantity INT = NULL,
    @MinBillAmount DECIMAL(18,2) = NULL,
    @DiscountValue DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @PromotionID INT;
    
    -- Thêm vào bảng Promotion
    INSERT INTO Promotion (Name, Description, DiscountType, StartDate, EndDate)
    VALUES (@Name, @Description, @DiscountType, @StartDate, @EndDate);
    
    SET @PromotionID = SCOPE_IDENTITY();
    
    -- Thêm vào bảng PromotionDetail
    INSERT INTO PromotionDetail (PromotionID, ProductID, MinQuantity, AppliesToOrder, GiftProductID, GiftQuantity, MinBillAmount, DiscountValue)
    VALUES (@PromotionID, @ProductID, @MinQuantity, @AppliesToOrder, @GiftProductID, @GiftQuantity, @MinBillAmount, @DiscountValue);
END


CREATE PROCEDURE sp_GetAllPromotion
	@PageSize INT,
	@PageNumber INT
AS
BEGIN
	SELECT  
		pr.PromotionID, pr.Name AS PromotionName, pr.DiscountType, pr.StartDate, pr.EndDate
	FROM Promotion pr
	ORDER BY pr.PromotionID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROW ONLY
END

CREATE PROCEDURE GetTotalPagePromotion
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize) FROM Promotion
END


CREATE PROCEDURE GetActivePromotions
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.PromotionID,
        p.Name AS PromotionName,
        p.Description,
        p.DiscountType,
		pd.MinBillAmount,
        pd.ProductID,
        pd.MinQuantity,
        pd.AppliesToOrder,
        pd.GiftProductID,
        pd.GiftQuantity,
        pd.DiscountValue,
        p.StartDate,
        p.EndDate
    FROM Promotion p
    JOIN PromotionDetail pd ON p.PromotionID = pd.PromotionID
    WHERE p.Status = 1  -- Chỉ lấy các khuyến mãi đang áp dụng
    AND GETDATE() BETWEEN p.StartDate AND p.EndDate; -- Chỉ lấy khuyến mãi trong khoảng thời gian hợp lệ
END;

CREATE PROCEDURE usp_InsertOrderFromJson
    @CustomerID INT = NULL,         -- Khách hàng (NULL nếu khách lẻ)
    @UserID INT,                    -- Nhân viên thực hiện đơn hàng
    @TotalAmount DECIMAL(18,2),     -- Tổng tiền trước khuyến mãi
    @FinalTotalAmount DECIMAL(18,2),-- Tổng tiền sau khuyến mãi
    @MethodID INT,                  -- Phương thức thanh toán
    @OrderDetailsJson NVARCHAR(MAX) -- Dữ liệu chi tiết đơn hàng dưới dạng JSON
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Chèn đơn hàng vào bảng Orders
        INSERT INTO Orders (CustomerID, UserID, OrderDate, TotalAmount, FinalTotalAmount, MethodID, PaymentStatus)
        VALUES (@CustomerID, @UserID, GETDATE(), @TotalAmount, @FinalTotalAmount, @MethodID, 1);

        -- Lấy OrderID vừa tạo
        DECLARE @OrderID INT = SCOPE_IDENTITY();

        -- Chèn dữ liệu chi tiết đơn hàng từ JSON vào bảng OrderDetail
        INSERT INTO OrderDetail (OrderID, ProductID, Quantity, Price)
        SELECT @OrderID,
               ProductID,
               Quantity,
               Price
        FROM OPENJSON(@OrderDetailsJson)
        WITH (
            ProductID INT '$.ProductID',
            Quantity INT '$.Quantity',
            Price DECIMAL(18,2) '$.UnitPrice'
        );

        COMMIT TRANSACTION;
        SELECT @OrderID AS OrderID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), 
               @ErrorSeverity = ERROR_SEVERITY(), 
               @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END


IF OBJECT_ID('dbo.usp_InsertOrderFromJson', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_InsertOrderFromJson;

CREATE TRIGGER trg_UpdateStockOnSell
ON OrderDetail
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET p.StockQuantity = p.StockQuantity - i.Quantity
    FROM Product p
    INNER JOIN inserted i ON p.ProductID = i.ProductID;
END
GO



CREATE TRIGGER trg_UpdateStock_OnSale
ON OrderDetail
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Giảm số lượng sản phẩm bán ra
    UPDATE p
    SET p.StockQuantity = p.StockQuantity - i.Quantity
    FROM Product p
    INNER JOIN inserted i ON p.ProductID = i.ProductID;

    -- Kiểm tra xem sản phẩm bán ra có thuộc chương trình khuyến mãi không
    UPDATE p
    SET p.StockQuantity = p.StockQuantity - pd.GiftQuantity
    FROM Product p
    INNER JOIN PromotionDetail pd ON p.ProductID = pd.GiftProductID
    INNER JOIN inserted i ON pd.ProductID = i.ProductID
    WHERE pd.GiftQuantity IS NOT NULL AND pd.GiftQuantity > 0;
END;

CREATE TRIGGER trg_UpdateStockOnSell
ON OrderDetail
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductID INT, @SoldQty INT;
    
    -- Duyệt qua từng dòng đơn hàng được chèn
    DECLARE insCursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT ProductID, Quantity
        FROM inserted;
        
    OPEN insCursor;
    FETCH NEXT FROM insCursor INTO @ProductID, @SoldQty;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @Remaining INT = @SoldQty;
        WHILE @Remaining > 0
        BEGIN
            DECLARE @DetailID INT, @available INT, @SupplierID INT;
            
            -- Chọn một bản ghi nhập hàng (StockReceiptDetail) của sản phẩm có số lượng còn lại > 0
            SELECT TOP 1 
                @DetailID = ReceiptDetailID,
                @available = RemainingQuantity,
                @SupplierID = SupplierID
            FROM StockReceiptDetail
            WHERE ProductID = @ProductID 
              AND RemainingQuantity > 0
            ORDER BY NEWID();  -- chọn ngẫu nhiên
            
            -- Nếu không còn bản ghi nào có hàng thì thoát vòng lặp
            IF @DetailID IS NULL
                BREAK;
            
            -- Tính số lượng cần trừ từ bản ghi này:
            DECLARE @deduct INT = CASE 
                                    WHEN @available >= @Remaining THEN @Remaining 
                                    ELSE @available 
                                  END;
            
            UPDATE StockReceiptDetail
            SET RemainingQuantity = RemainingQuantity - @deduct
            WHERE ReceiptDetailID = @DetailID;
            
            SET @Remaining = @Remaining - @deduct;
        END

        FETCH NEXT FROM insCursor INTO @ProductID, @SoldQty;
    END

    CLOSE insCursor;
    DEALLOCATE insCursor;
END
GO

ALTER TABLE Product ADD TotalStock INT DEFAULT 0;

CREATE TRIGGER trg_UpdateStockBySupplier
ON StockReceiptDetail
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật số lượng nhập theo nhà cung cấp
    MERGE ProductSupplierStock AS target
    USING (
        SELECT ProductID, SupplierID, SUM(Quantity) AS NewStock
        FROM inserted
        GROUP BY ProductID, SupplierID
    ) AS src
    ON target.ProductID = src.ProductID AND target.SupplierID = src.SupplierID
    WHEN MATCHED THEN
        UPDATE SET target.TotalStock = src.NewStock  -- Cập nhật lại đúng số lượng của nhà cung cấp đó
    WHEN NOT MATCHED THEN
        INSERT (ProductID, SupplierID, TotalStock)
        VALUES (src.ProductID, src.SupplierID, src.NewStock);
END;


SELECT * FROM ProductSupplierStock WHERE ProductID = 5;

EXEC sp_helptrigger 'StockReceiptDetail';



DROP TRIGGER trg_UpdateStockBySupplier


-- Kiểm tra số lượng sản phẩm trong bảng StockReceiptDetail
SELECT ProductID, SupplierID, SUM(Quantity) AS TotalQuantity
FROM StockReceiptDetail
GROUP BY ProductID, SupplierID;
SELECT SUM(Quantity) AS TotalStockQuantity
FROM StockReceiptDetail;

SELECT ProductID, SupplierID, SUM(Quantity) AS TotalQuantity
FROM StockReceiptDetail
WHERE ProductID = (SELECT ProductID FROM Product WHERE Name = 'CocaCola')
GROUP BY ProductID, SupplierID;

SELECT ProductID, SupplierName, TotalStockBySupplier, AvailableStock
FROM vw_GetAllProduct
WHERE ProductID = 1;


SELECT * FROM StockReceiptDetail WHERE ReceiptID IN (SELECT ReceiptID FROM StockReceiptDetail);

SELECT * FROM StockReceiptDetail WHERE RemainingQuantity IS NULL;

SELECT ProductID, SUM(Quantity) AS TotalQuantity, SUM(ISNULL(RemainingQuantity, 0)) AS TotalRemaining
FROM StockReceiptDetail
WHERE ProductID = 1
GROUP BY ProductID;


SELECT name FROM sys.triggers WHERE parent_id = OBJECT_ID('StockReceiptDetail');

-- Kiểm tra số lượng tồn kho tổng hợp trong bảng Product
SELECT ProductID, TotalStock FROM Product;

DROP TRIGGER trg_UpdateStockOnSell

CREATE PROCEDURE GetNextSellID
	@LastID INT
AS
BEGIN
	SELECT @LastID = MAX(OrderID) FROM Orders;
	IF @LastID IS NULL
	SET @LastID = 0;
	SELECT @LastID + 1 AS NextSellID;
END


DROP TRIGGER trg_UpdateStock_OnSale




-- Order

CREATE VIEW vw_Order_User
AS
	SELECT 
		o.OrderID,
		u.FullName AS EmployeeName,
		c.FullName AS CustomerName,
		o.OrderDate,
		o.TotalAmount,
		o.FinalTotalAmount
	FROM Orders o
	LEFT JOIN Customer c ON o.CustomerID = c.CustomerID
	INNER JOIN Users u ON o.UserID = u.UserID

SELECT * FROM vw_Order_User


CREATE PROCEDURE GetAllOrder
	@PageSize INT,
	@PageNumber INT
AS
BEGIN
	SELECT 
		*
	FROM vw_Order_User
	ORDER BY OrderID
	OFFSET (@PageNumber - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

CREATE PROCEDURE GetTotalPageOrder
	@PageSize INT
AS
BEGIN
	SELECT CEILING(COUNT(*) * 1.0 / @PageSize)
	FROM Orders
END

-- Order Detail

CREATE PROCEDURE GetOrderDetail
	@OrderID INT
AS
	SELECT
		p.Name AS ProductName,
		od.Quantity,
		p.Price AS UnitPrice
	FROM OrderDetail od
	INNER JOIN Orders o ON od.OrderID = o.OrderID
	INNER JOIN Product p ON od.ProductID = p.ProductID
	WHERE od.OrderID = @OrderID

IF OBJECT_ID('dbo.SearchOrder', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SearchOrder;


EXEC GetOrderDetail 4


CREATE PROCEDURE SearchOrder
    @Keyword NVARCHAR(255) = NULL
AS
BEGIN
    SELECT 
        o.OrderID,
        u.FullName AS EmployeeName,
        c.FullName AS CustomerName,
        o.OrderDate,
        o.TotalAmount,
        o.FinalTotalAmount
    FROM Orders o
    LEFT JOIN Customer c ON o.CustomerID = c.CustomerID
    INNER JOIN Users u ON o.UserID = u.UserID
    WHERE
        (@Keyword IS NULL OR 
            (CAST(o.OrderID AS NVARCHAR(255)) LIKE '%' + @Keyword + '%' OR 
            c.FullName LIKE '%' + @Keyword + '%' OR 
            CAST(o.OrderDate AS NVARCHAR(255)) LIKE '%' + @Keyword + '%'))
    ORDER BY o.OrderID;
END



CREATE PROCEDURE GetRevenueByTimePeriod
    @Type NVARCHAR(10) -- 'Day', 'Week', 'Month'
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'Day'
    BEGIN
        SELECT CAST(OrderDate AS DATE) AS Ngay, SUM(TotalAmount) AS DoanhThu
        FROM Orders
        WHERE OrderDate >= DATEADD(DAY, -6, GETDATE())
        GROUP BY CAST(OrderDate AS DATE)
        ORDER BY Ngay;
    END
    ELSE IF @Type = 'Week'
    BEGIN
        SELECT DATEPART(WEEK, OrderDate) AS Tuan, YEAR(OrderDate) AS Nam, SUM(TotalAmount) AS DoanhThu
        FROM Orders
        WHERE OrderDate >= DATEADD(WEEK, -3, GETDATE())
        GROUP BY DATEPART(WEEK, OrderDate), YEAR(OrderDate)
        ORDER BY Nam, Tuan;
    END
    ELSE IF @Type = 'Month'
    BEGIN
        SELECT MONTH(OrderDate) AS Thang, YEAR(OrderDate) AS Nam, SUM(TotalAmount) AS DoanhThu
        FROM Orders
        WHERE OrderDate >= DATEADD(MONTH, -5, GETDATE())
        GROUP BY MONTH(OrderDate), YEAR(OrderDate)
        ORDER BY Nam, Thang;
    END
END;



CREATE PROCEDURE GetTotalEmployees
AS
BEGIN
    SELECT COUNT(*) AS TotalEmployees FROM Users;
END;

CREATE PROCEDURE GetTotalCustomers
AS
BEGIN
    SELECT COUNT(*) AS TotalCustomers FROM Customer;
END;


CREATE PROCEDURE GetTotalOrders
AS
BEGIN
    SELECT COUNT(*) AS TotalOrders FROM Orders;
END;

CREATE PROCEDURE GetTotalRevenue
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT SUM(FinalTotalAmount) AS TotalRevenue
    FROM Orders;
END;

CREATE PROCEDURE UpdatePasswordByEmail
    @Email NVARCHAR(255),
    @NewPassword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET PasswordHash = @NewPassword
    WHERE Email = @Email;

    -- Kiểm tra xem có cập nhật thành công không
    IF @@ROWCOUNT = 0
        THROW 50001, 'Email không tồn tại trong hệ thống.', 1;
END;



INSERT INTO Users (
    Username,
    PasswordHash,
    FullName,
    Email,
    Phone,
    Gender,
    Address,
    Role,
    Salary,
    BirthDate
)
VALUES (
    'shadow',
    CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2), -- Thay 'admin123' bằng mật khẩu thực tế
    N'Shadow',
    'admin@example.com',
    '0123456789',
    'Nam',
    N'Hà Nội',
    'Admin',
    NULL,       -- Lương có thể NULL
    NULL        -- Ngày sinh có thể NULL
);

SELECT * FROM Product

INSERT INTO Category (Name, Description)  
VALUES  
(N'Rau củ quả', N'Các loại rau, củ, quả tươi sạch hàng ngày'),  
(N'Thịt & Hải sản', N'Thịt tươi sống, hải sản tươi ngon'),  
(N'Sữa & Sản phẩm từ sữa', N'Các loại sữa tươi, sữa chua, bơ, phô mai'),  
(N'Đồ uống', N'Nước giải khát, nước ép trái cây, cà phê, trà'),  
(N'Bánh kẹo', N'Bánh, kẹo, chocolate và các loại snack'),  
(N'Gia vị & Nấu ăn', N'Gia vị, nước sốt, dầu ăn, bột nêm'),  
(N'Mì & Thực phẩm khô', N'Mì gói, nui, bún khô, các loại thực phẩm đóng gói'),  
(N'Đồ hộp & Đồ đông lạnh', N'Thực phẩm đóng hộp, đông lạnh tiện lợi'),  
(N'Hóa mỹ phẩm', N'Sản phẩm chăm sóc cá nhân, dầu gội, sữa tắm'),  
(N'Đồ dùng gia đình', N'Giấy vệ sinh, nước giặt, nước rửa chén'),  
(N'Dụng cụ bếp', N'Nồi, chảo, dao, kéo và các dụng cụ nhà bếp'),  
(N'Chăm sóc mẹ & bé', N'Sản phẩm cho mẹ và bé như sữa bột, tã, khăn ướt'),  
(N'Thực phẩm chức năng', N'Các loại thực phẩm bổ sung dinh dưỡng, vitamin'),  
(N'Chăm sóc thú cưng', N'Thức ăn và phụ kiện cho chó, mèo và thú cưng khác'),  
(N'Văn phòng phẩm', N'Dụng cụ học tập, bút, giấy, sổ tay, văn phòng phẩm');  

INSERT INTO Supplier (Name, Phone, Email, Address)  
VALUES  
(N'Công ty TNHH Thực phẩm Việt', '0901234567', 'contact@vietfood.com', N'123 Đường Lê Lợi, TP. HCM'),  
(N'Công ty CP Sữa Quốc Tế', '0912345678', 'info@milkinternational.com', N'45 Nguyễn Trãi, Hà Nội'),  
(N'Nhà cung cấp Rau sạch', '0923456789', 'rausach@gmail.com', N'67 Trần Hưng Đạo, Đà Nẵng'),  
(N'Công ty TNHH Thủy Hải Sản Biển Đông', '0934567890', 'haisanbd@biendong.com', N'89 Lê Văn Lương, TP. HCM'),  
(N'Công ty TNHH Nước giải khát Fresh', '0945678901', 'freshdrink@beverage.com', N'101 Võ Văn Kiệt, Cần Thơ'),  
(N'Nhà cung cấp Bánh kẹo Sweet', '0956789012', 'sweetcake@candy.com', N'23 Phạm Văn Đồng, Hải Phòng'),  
(N'Công ty Gia vị Việt Nam', '0967890123', 'spicesvn@gmail.com', N'12 Hoàng Diệu, Huế'),  
(N'Công ty CP Đồ đông lạnh Bắc Hà', '0978901234', 'bacahcold@frozen.com', N'56 Nguyễn Văn Cừ, Quảng Ninh'),  
(N'Công ty TNHH Hóa Mỹ Phẩm Lâm Sơn', '0989012345', 'lamsoncosmetic@cosmetics.com', N'78 Hai Bà Trưng, TP. HCM'),  
(N'Nhà phân phối Văn phòng phẩm ABC', '0990123456', 'abcstationery@office.com', N'90 Bạch Đằng, Đà Nẵng');  


INSERT INTO Customer (FullName, Phone, Email, Address)
VALUES
(N'Nguyễn Văn A', '0987654321', 'nguyenvana@example.com', N'Hà Nội'),
(N'Trần Thị B', '0912345678', 'tranthib@example.com', N'Hồ Chí Minh'),
(N'Lê Văn C', '0903456789', 'levanc@example.com', N'Đà Nẵng'),
(N'Phạm Thị D', '0981234567', 'phamthid@example.com', N'Cần Thơ'),
(N'Hoàng Văn E', '0976543210', 'hoangvane@example.com', N'Hải Phòng'),
(N'Đỗ Thị F', '0934567890', 'dothif@example.com', N'Nha Trang'),
(N'Bùi Văn G', '0923456789', 'buivang@example.com', N'Huế'),
(N'Ngô Thị H', '0919876543', 'ngothih@example.com', N'Bắc Ninh'),
(N'Đặng Văn I', '0908765432', 'dangvani@example.com', N'Thanh Hóa'),
(N'Lý Thị J', '0985554443', 'lythij@example.com', N'Bình Dương'),
(N'Trịnh Văn K', '0971237890', 'trinhvank@example.com', N'Hải Dương'),
(N'Vũ Thị L', '0956781234', 'vuthil@example.com', N'Vĩnh Phúc'),
(N'Cao Văn M', '0945678901', 'caovanm@example.com', N'Nam Định'),
(N'Hà Thị N', '0938765432', 'hathin@example.com', N'Phú Thọ'),
(N'Lâm Văn O', '0923456798', 'lamvano@example.com', N'Bắc Giang'),
(N'Phan Thị P', '0912678943', 'phanthip@example.com', N'Quảng Ninh'),
(N'Dương Văn Q', '0909988776', 'duongvanq@example.com', N'Lào Cai'),
(N'Tô Thị R', '0982233445', 'tothir@example.com', N'Thái Nguyên'),
(N'Lương Văn S', '0975566778', 'luongvans@example.com', N'Hà Giang'),
(N'Tạ Thị T', '0966677889', 'tatrit@example.com', N'Lạng Sơn'),
(N'Nguyễn Văn U', '0953344556', 'nguyenvanu@example.com', N'Kon Tum'),
(N'Trần Thị V', '0942233445', 'tranthiv@example.com', N'Bình Phước'),
(N'Lê Văn W', '0931122334', 'levanw@example.com', N'Cà Mau'),
(N'Phạm Thị X', '0929988776', 'phamthix@example.com', N'An Giang'),
(N'Hoàng Văn Y', '0918899775', 'hoangvany@example.com', N'Kiên Giang'),
(N'Đỗ Thị Z', '0907776665', 'dothiz@example.com', N'Sóc Trăng'),
(N'Bùi Văn A1', '0988887776', 'buivana1@example.com', N'Hậu Giang'),
(N'Ngô Thị B2', '0979998887', 'ngothib2@example.com', N'Tây Ninh'),
(N'Đặng Văn C3', '0961112223', 'dangvanc3@example.com', N'Bạc Liêu'),
(N'Lý Thị D4', '0952223334', 'lythid4@example.com', N'Đồng Nai');


--TRUNCATE TABLE Customer
-- Tạo Trigger
/*
CREATE TRIGGER TR_OrderDetail_UpdateStock
ON OrderDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	UPDATE p
	SET p.StockQuantity = p.StockQuantity - i.Quantity
	FROM Product p
	JOIN inserted i ON p.ProductID = i.ProductID;
END
*/
/*
CREATE VIEW ProductDetails AS
SELECT 
    p.ProductID,
    p.ProductName,
    c.CategoryName,
    s.SupplierName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
JOIN Suppliers s ON p.SupplierID = s.SupplierID;

*/

CREATE TABLE ProductSupplierStock (
    ProductID INT NOT NULL,
    SupplierID INT NOT NULL,
    TotalStock INT NOT NULL DEFAULT 0,
    PRIMARY KEY (ProductID, SupplierID)
);




CREATE TRIGGER TR_StockReceiptDetail_UpdateStock
ON OrderDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	SET NOCOUNT ON;  -- Giúp tăng hiệu suất, tránh thông báo số dòng bị ảnh hưởng
	 -- Khi thêm mới đơn nhập hàng (INSERT), cộng vào kho
	UPDATE p
	SET p.StockQuantity = p.StockQuantity + i.Quantity
	FROM Product p
	JOIN inserted i ON p.ProductID = i.ProductID;
	 -- Khi xóa đơn nhập hàng (DELETE), trừ đi số lượng đã nhập
	UPDATE p
	SET p.StockQuantity = p.StockQuantity - d.Quantity
	FROM Product p
	JOIN deleted d ON p.ProductID = d.ProductID;
END
/*
CREATE TRIGGER TR_Orders_UpdateFinalTotal
ON Orders
AFTER INSERT, UPDATE
AS
BEGIN
	IF EXISTS (SELECT 1 FROM inserted WHERE PromotionID IS NOT NULL)
	BEGIN
		UPDATE o 
		SET
		FROM Promotion P
END

*/
-- Tạo Index cho user
CREATE INDEX IX_Users_Username ON Users (Username);
CREATE INDEX IX_Users_Email ON Users (Email);
CREATE INDEX IX_Users_Phone ON Users (Phone);
CREATE INDEX IX_Users_Role ON Users (Role); -- Lọc theo role

--Tạo Index cho Customer
CREATE INDEX IX_Customer_Phone ON Customer (Phone);
CREATE INDEX IX_Customer_Email ON Customer (Email);
-- Tạo index cho product
CREATE INDEX IX_Product_Name ON Product (Name);
--Tạo index danh mục , nhà cung cấp
CREATE INDEX IX_Product_CategoryID ON Product (CategoryID);
CREATE INDEX IX_Product_SupplierID ON Product (SupplierID);
--Tạo index lọc theo giá
CREATE INDEX IX_Product_Price ON Product (Price);
--Tạo index tìm kiếm theo khách hàng, nhân viên
CREATE INDEX IX_Orders_CustomerID ON Orders (CustomerID);
CREATE INDEX IX_Orders_UserID ON Orders (UserID);
--Tạo index cho việc lọc theo ngày
CREATE INDEX IX_Orders_OrderDate ON Orders (OrderDate);
--Tạo index tối ưu truy kiểm tra khuyến mãi còn hiệu lực
CREATE INDEX IX_Promotion_DateRange ON Promotion (StartDate, EndDate);
--Tạo index lọc khuyến mãi theo status
CREATE INDEX IX_Promotion_Status ON Promotion (Status);
--Tối ưu truy vấn join giữa Orders và Promotion (bảng orderPromotion)
CREATE INDEX IX_OrderPromotion_Order_Promotion ON OrderPromotion (OrderID, PromotionID);
--Tìm kiếm phiếu nhập kho theo nhà cung cấp hoặc nhân viên
CREATE INDEX IX_StockReceipt_SupplierID ON StockReceipt (SupplierID);
CREATE INDEX IX_StockReceipt_UserID ON StockReceipt (UserID);

-- Trigger này sẽ kích hoạt khi có thêm/xóa/sửa khuyến mãi trong OrderPromotion, và tự động tính toán lại FinalTotalAmount cho hóa đơn.
CREATE TRIGGER TR_OrderPromotion_UpdateFinalTotal
ON OrderPromotion
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Lấy danh sách OrderID bị ảnh hưởng
    DECLARE @AffectedOrderIDs TABLE (OrderID INT);
    INSERT INTO @AffectedOrderIDs (OrderID)
    SELECT OrderID FROM inserted
    UNION
    SELECT OrderID FROM deleted;

    --Tính tổng DiscountAmount hợp lệ cho các OrderID
    WITH ValidPromotions AS (
        SELECT 
            op.OrderID,
            SUM(op.DiscountAmount) AS TotalDiscount
        FROM OrderPromotion op
        JOIN Promotion p ON op.PromotionID = p.PromotionID
        WHERE 
            p.Status = 1 -- Chỉ khuyến mãi đang hoạt động
            AND GETDATE() BETWEEN p.StartDate AND p.EndDate -- Trong thời gian hiệu lực
            AND op.OrderID IN (SELECT OrderID FROM @AffectedOrderIDs)
        GROUP BY op.OrderID
    )
    --Cập nhật FinalTotalAmount cho Orders
    UPDATE o
    SET o.FinalTotalAmount = o.TotalAmount - ISNULL(vp.TotalDiscount, 0)
    FROM Orders o
    LEFT JOIN ValidPromotions vp ON o.OrderID = vp.OrderID
    WHERE o.OrderID IN (SELECT OrderID FROM @AffectedOrderIDs);
END;


DROP TABLE PromotionGift
DROP TABLE OrderPromotion
DROP TABLE PromotionDetail
DROP TABLE OrderDetail
DROP TABLE Orders

DROP TABLE StockReceipt
DROP TABLE StockReceiptDetail
DROP TABLE Users


--

DROP TABLE Product
DROP TABLE Supplier
DROP TABLE Category
--DROP TABLE Pro