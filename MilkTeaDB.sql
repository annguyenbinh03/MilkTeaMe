﻿GO
USE master;
GO
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MilkTeaMeDB')
BEGIN
    DROP DATABASE MilkTeaMeDB;
END
GO
CREATE DATABASE MilkTeaMeDB;
GO
USE MilkTeaMeDB;
GO

-- Bảng danh mục sản phẩm
CREATE TABLE Category (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(10) UNIQUE NOT NULL CHECK (Name IN ('milktea', 'combo', 'topping'))
);
GO

CREATE TABLE Size (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(10) UNIQUE NOT NULL CHECK (Name IN ('S', 'M', 'L'))
);

-- Bảng sản phẩm
CREATE TABLE Product (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NULL,
    CategoryId INT NOT NULL,
    ImageUrl NVARCHAR(500),
    SoldCount INT DEFAULT 0,
    Status VARCHAR(10) CHECK (Status IN ('active', 'inactive', 'deleted')) DEFAULT 'active',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE NO ACTION
);
GO

CREATE TABLE ProductSize (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    SizeId INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE CASCADE,
    FOREIGN KEY (SizeId) REFERENCES Size(Id) ON DELETE CASCADE,
    UNIQUE (ProductId, SizeId)
);


-- Bảng trung gian ProductCombo
CREATE TABLE ProductCombo (
    ComboId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
	ProductSizeId INT NULL,
    PRIMARY KEY (ComboId, ProductId),
    FOREIGN KEY (ComboId) REFERENCES Product(Id) ON DELETE NO ACTION,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE NO ACTION,
    FOREIGN KEY (ProductSizeId) REFERENCES ProductSize(Id) ON DELETE NO ACTION,
);
GO

-- Bảng nhân viên
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NULL,
    Role VARCHAR(10) CHECK (Role IN ('manager', 'staff', 'customer')),
    Phone NVARCHAR(15) UNIQUE NOT NULL,
    Email NVARCHAR(255) UNIQUE,
    Status VARCHAR(10) CHECK (Status IN ('active', 'inactive', 'resigned')) DEFAULT 'active',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Bảng đơn hàng
CREATE TABLE [Order] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Status VARCHAR(10) CHECK (Status IN ('pending', 'completed', 'cancelled')) DEFAULT 'pending',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Bảng chi tiết đơn hàng
CREATE TABLE OrderDetail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    Price DECIMAL(10,2) NOT NULL,
	SizeId INT NULL,
	ParentId INT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id) ON DELETE NO ACTION,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE NO ACTION,
	FOREIGN KEY (SizeId) REFERENCES Size(Id) ON DELETE NO ACTION,
	FOREIGN KEY (ParentId) REFERENCES [OrderDetail](Id) ON DELETE NO ACTION,
);
GO

-- Bảng phương thức thanh toán
CREATE TABLE PaymentMethod (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(10) UNIQUE NOT NULL CHECK (Name IN ('vnpay', 'cash'))
);
GO

-- Bảng thanh toán
CREATE TABLE Payment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PaymentMethodId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    TransactionCode NVARCHAR(50) NULL,
    Status VARCHAR(10) CHECK (Status IN ('pending', 'completed', 'failed')) DEFAULT 'pending',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrderId) REFERENCES [Order](Id) ON DELETE NO ACTION,
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethod(Id) ON DELETE NO ACTION
);
GO

-- Thêm dữ liệu mẫu vào bảng Category
INSERT INTO Category (Name) VALUES ('milktea'), ('combo'), ('topping');
GO

-- Thêm dữ liệu mẫu vào bảng PaymentMethod
INSERT INTO PaymentMethod (Name) VALUES ('vnpay'), ('cash');
GO

INSERT INTO Size (Name) VALUES ('S'), ('M'), ('L');

-- Thêm dữ liệu mẫu vào bảng Product
INSERT INTO Product (Name, Description, Price, CategoryId, ImageUrl, Status, SoldCount)
VALUES 
(N'Trà sữa truyền thống', N'Thức uống quốc dân, với vị trà đậm đà, vị sữa ngọt ngào, là lựa chọn hoàn hảo cho mọi lứa tuổi để giải khát và thư giãn.', NULL, 1, 'https://toigingiuvedep.vn/wp-content/uploads/2021/06/hinh-anh-tra-sua-dep-ngon.jpeg', 'active', 1),
(N'Trà sữa matcha', N'Sự kết hợp tinh tế giữa trà xanh matcha Nhật Bản thanh mát và sữa béo ngậy, tạo nên thức uống thơm ngon, độc đáo.', NULL, 1, 'https://cf.shopee.vn/file/115b108e80bd3d48179938d1248ac30b', 'active', 2),
(N'Trà sữa khoai môn', N'Vị khoai môn ngọt ngào, béo ngậy, thơm lừng, hòa quyện cùng trà sữa thanh mát, tạo nên một thức uống giải khát tuyệt vời, chinh phục mọi trái tim.', NULL, 1, 'https://th.bing.com/th/id/OIP.WqrCdDU4dwF5q-Dz6D5M2QHaHa?rs=1&pid=ImgDetMain', 'active', 2),
(N'Trà sữa socola', N'Thức uống đầy quyến rũ với hương thơm socola nồng nàn và vị ngọt đắng cân bằng. Vị trà sữa dịu nhẹ hòa quyện cùng vị socola đậm đà, tạo nên một trải nghiệm vị giác độc đáo và khó quên.', NULL, 1, 'https://th.bing.com/th/id/OIP.l3isdgVDMi6k8rKCdXd02QHaHa?rs=1&pid=ImgDetMain', 'active', 1),
(N'Trà sữa dâu', N'Thức uống ngọt ngào và tươi mát với hương dâu tây tự nhiên. Vị chua ngọt của dâu tây hòa quyện cùng trà sữa thơm ngon, tạo nên một trải nghiệm vị giác đầy thú vị.', NULL, 1, 'https://png.pngtree.com/png-clipart/20210912/original/pngtree-strawberry-pearl-milk-tea-png-image_6742466.jpg', 'active', 2),
(N'Trà sữa Thái xanh', N' Thức uống độc đáo với hương vị trà Thái đặc trưng, thơm nồng và đậm đà. Vị trà xanh Thái Lan hòa quyện cùng sữa béo ngậy, tạo nên một trải nghiệm vị giác mới lạ và khó quên.', NULL, 1, 'https://phunuketnoi.com/wp-content/uploads/2021/11/tra-thai-xanh-45645544.jpg', 'active', 3),
(N'Hồng trà tắc mật ong', N'Hồng trà kết hợp cùng vị chua nhẹ của tắc và độ ngọt thanh của mật ong, tạo nên thức uống giải khát tươi mát.', NULL, 1, 'https://blog.tiemphonui.com/wp-content/uploads/2022/05/tra-tac-mat-ong-giai-nhiet.jpg', 'active', 1),
(N'Trà mãng cầu', N'Trà mãng cầu kết hợp giữa vị chua ngọt tự nhiên của mãng cầu với vị thanh mát của trà, mang lại cảm giác sảng khoái.', NULL, 1, 'https://th.bing.com/th/id/OIP.OeyB3Hr4As_Qk_PteLFt9gHaHa?rs=1&pid=ImgDetMain', 'active', 1),

(N'Trân châu đen', N' Với cấu trúc dai ngon đặc trưng cùng độ mịn của bột nếp, trân châu đen đã trở thành một lựa chọn tuyệt vời khi kết hợp với trà sữa.', 3000, 3, 'https://th.bing.com/th/id/OIP.8pDSylnxgjbjxjWHO-wlsAHaHa?rs=1&pid=ImgDetMain', 'active', 1),
(N'Trân châu trắng', N'Những viên trân châu trắng trong suốt, giòn dai, vô cùng hoàn hảo để thưởng thức cùng trà sữa.', 3000, 3, 'https://th.bing.com/th/id/R.9b8db31195f809e2ed31cf886327ca98?rik=x%2bTZx7I2QTx9HA&riu=http%3a%2f%2fcdn.shopify.com%2fs%2ffiles%2f1%2f0537%2f9997%2farticles%2fcach-lam-tran-chau-3q-tai-nha-don-gian-nhat_1_1024x1024.jpg%3fv%3d1675220873&ehk=NBQyrFv8W0T0iMxmqcVIFimNxFfYSQLKUfMyVci9VRQ%3d&risl=&pid=ImgRaw&r=0', 'active', 1),
(N'Trân châu hoàng kim', N'Trân châu hoàng kim rực rỡ với sắc vàng nâu lấp lánh, kết hợp hài hòa với nước đường ngọt thanh, tạo nên một sự kết hợp đậm chất riêng.', 6000, 3, 'https://vn-test-11.slatic.net/p/c723e2cc42d44bd07a77122bdbf59b59.jpg', 'active', 0),
(N'Thạch dừa', N'Thạch dừa có lớp vỏ giòn, dai với đa dạng hương vị và màu sắc hấp dẫn.', 4000, 3, 'https://th.bing.com/th/id/OIP.vLJbaj6AMrSiJrd5CNJmRwHaHV?rs=1&pid=ImgDetMain', 'active', 0),
(N'Bánh flan', N'Bánh flan sẽ có vị riêng nhưng thường là béo mềm, kết cấu có tính đàn hồi tốt và thơm mùi matcha, trứng gà hoặc caramel.', 5000, 3, 'https://th.bing.com/th/id/OIP.qCtmYbQFsm3Plju9WqXLpgHaHa?w=600&h=600&rs=1&pid=ImgDetMain', 'active', 0),
(N'Milk foam', N'Milk foam là lớp kem sữa béo ngậy có màu trắng muốt được thêm bên trên bề mặt của trà sữa để tăng thêm độ ngon cho thức uống.', 3000, 3, 'https://th.bing.com/th/id/OIP.WeLQhOjqGOTtPcwwBNAISgHaHa?rs=1&pid=ImgDetMain', 'active', 0),

(N'Combo Classic', N'1 Trà sữa truyền thống(M) + 1 Trà sữa matcha(M)', 65000, 2, 'https://icon-library.com/images/combo-icon/combo-icon-8.jpg', 'active', 0),
(N'Combo Socola Matcha', N'1 Trà sữa socola(S) + 1 Trà sữa matcha(S) + 2 Trân châu đen', 80000, 2, 'https://static.vecteezy.com/system/resources/thumbnails/014/435/706/small_2x/combo-offer-banner-icon-flat-design-flat-illustration-on-white-background-vector.jpg', 'active', 0),
(N'Combo Thái Xanh & Mãng Cầu', N'1 Trà sữa Thái xanh(M) + 1 Trà mãng cầu(M) + 2 Trân châu trắng', 85000, 2, 'https://icon-library.com/images/combo-icon/combo-icon-0.jpg', 'active', 0),
(N'Combo Trà Sữa 3 Vị', N'1 Trà sữa khoai môn(M) + 1 Trà sữa dâu(M) + 1 Trà sữa Thái xanh(M) + 3 Thạch dừa', 120000, 2, 'https://th.bing.com/th/id/OIP.l7Pa-DvOFOHs_JJ4HN_fKAHaHa?rs=1&pid=ImgDetMain', 'active', 0);
GO

INSERT INTO ProductSize (ProductId, SizeId, Price) VALUES
(1, 1, 30000), (1, 2, 35000), (1, 3, 40000),
(2, 1, 35000), (2, 2, 40000), (2, 3, 45000),
(3, 1, 33000), (3, 2, 38000), (3, 3, 43000),
(4, 1, 37000), (4, 2, 42000), (4, 3, 47000),
(5, 1, 34000), (5, 2, 39000), (5, 3, 44000),
(6, 1, 33000), (6, 2, 38000), (6, 3, 43000),
(7, 1, 31000), (7, 2, 36000), (7, 3, 41000),
(8, 1, 30000), (8, 2, 35000), (8, 3, 40000);
GO
-- Thêm dữ liệu vào bảng trung gian ProductCombo
INSERT INTO ProductCombo (ComboId, ProductId, Quantity, ProductSizeId) VALUES 
(15, 1, 1, 2), (15, 2, 1, 5), 
(16, 4, 1, 10), (16, 2, 1, 4), (16, 9, 2, null), 
(17, 6, 1, 17), (17, 8, 1, 20), (17, 10, 2, null),
(18, 3, 1, 8), (18, 5, 1, 14), (18, 6, 1, 17), (18, 12, 3, null);
GO

-- Thêm dữ liệu mẫu vào bảng User
INSERT INTO [User] (Username, Password, Role, Phone, Email, Status)
VALUES 
('manager', '1','manager','0901234567', 'admin1@example.com', 'active'),
('staff', '1','staff','09012345678', 'staff@example.com', 'active'),
('customer', '1','customer','09012345679', 'customer@example.com', 'active');
GO

INSERT INTO [Order] (TotalPrice, Status, CreatedAt, UpdatedAt) VALUES
(63000, 'completed', GETDATE(), GETDATE()),  
(75000, 'completed', DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE())),  
(82000, 'completed', DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -2, GETDATE())),  
(54000, 'completed', DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -3, GETDATE())),  
(98000, 'completed', DATEADD(DAY, -4, GETDATE()), DATEADD(DAY, -4, GETDATE())),  
(72000, 'completed', DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, -5, GETDATE())),  
(89000, 'completed', DATEADD(DAY, -6, GETDATE()), DATEADD(DAY, -6, GETDATE()));  
GO  

INSERT INTO OrderDetail (OrderId, ProductId, Quantity, Price, SizeId, ParentId) VALUES
(1, 1, 1, 30000, 1, NULL),  
(1, 3, 1, 33000, 2, NULL),

(2, 2, 2, 35000, 1, NULL),  
(2, 6, 1, 38000, 2, NULL),

(3, 5, 1, 34000, 1, NULL),  
(3, 8, 2, 35000, 2, NULL),

(4, 4, 1, 37000, 2, NULL),  
(4, 7, 1, 31000, 3, NULL),

(5, 6, 1, 33000, 1, NULL),  
(5, 9, 3, 4000, NULL, NULL),

(6, 3, 2, 33000, 1, NULL),  
(6, 10, 2, 3000, NULL, NULL),

(7, 2, 1, 35000, 2, NULL),  
(7, 5, 1, 34000, 3, NULL);
GO  
