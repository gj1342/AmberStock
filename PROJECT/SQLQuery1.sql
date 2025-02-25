create database BeneAmber;
use BeneAmber

create table registerApp(
	userID int primary key identity,
	userName varchar(255) not null,
	pass varchar(255) not null
)

ALTER TABLE registerApp add constraint unique_user Unique(userName) 

/*create table profileSetting(
	userID int,
	firstName varchar(255),
	lastName varchar(255),
	PhoneNumber varchar(20)

	constraint fk_userIDprof foreign key (userID) references registerApp(userID)
)*/

create table Brand(
	BrandID int primary key not null,
	BrandName varchar(255)  not null,
	userID int

	constraint uniqueBrand UNIQUE(BrandName)
	constraint fk_userIDBrand foreign key (userID) references registerApp(userID)
)

create table Category(
	CategoryID int primary key  not null,
	CategoryName varchar(255)  not null,
	userID int

	constraint uniqueCat UNIQUE(CategoryName)
	constraint fk_userIDCat foreign key (userID) references registerApp(userID)
)

create table Product(
	ProductID int primary key  not null,
	ProductName varchar(255)  not null,
	BrandID int  not null,
	p_description varchar(255),
	CategoryID int  not null,
	userID int

	constraint fk_brandID foreign key (BrandID) references Brand(BrandID),
	constraint fk_catID foreign key (CategoryID) references Category(CategoryID),
	constraint fk_userIDProd foreign key (userID) references registerApp(userID),
	constraint uniqueProdName UNIQUE(ProductName)
)

ALTER TABLE Product
DROP COLUMN p_description;

create table prices(
	ProductID int UNIQUE not null,
	sellingPrice decimal(18,2) not null,
	costPrice decimal(18,2) not null,
	ProfitMargin decimal(18,2) not null,
	userID int

	constraint fk_pricesProdID foreign key (ProductID) references Product(ProductID),
	constraint fk_userIDprice foreign key (userID) references registerApp(userID)
)

drop table sales

create table Inventory(
	ProductID int UNIQUE not null,
	Quantity int not null,
	userID int

	constraint fk_prodID foreign key (ProductID) references Product(ProductID),
	constraint fk_userIDinv foreign key (userID) references registerApp(userID),
	constraint check_quantity check (Quantity >= 0)
)

create table condition(
	ConditionID int primary key not null,
	ConditionDescription varchar(255) not null,
	userID int

	constraint condUnique UNIQUE(ConditionDescription)
	constraint fk_userIDcond foreign key (userID) references registerApp(userID)
)

create table quality(
	ProductID int UNIQUE not null,
	ConditionID int not null,
	Qual_Assessment varchar(255) not null,
	userID int

	constraint fk_prodIDqual foreign key (ProductID) references Product(ProductID),
	constraint fk_condID foreign key (ConditionID) references condition(ConditionID),
	constraint fk_userIDqual foreign key (userID) references registerApp(userID)
)

/*create table locations(
	ProductID int not null,
	LocationID int primary key not null,
	LocationDesc varchar(255) not null,
	userID int

	constraint fk_locationProdID foreign key (ProductID) references Product(ProductID),
	constraint fk_userIDloc foreign key (userID) references registerApp(userID)
)*/

create table customer(
	customerID int primary key not null,
	fname varchar(255) not null,
	lname varchar(255) not null,
	town varchar(255) not null,
	city varchar(255) not null,
	userID int

	constraint fk_userIDcust foreign key (userID) references registerApp(userID)
)

drop table customer

create table transac(
	transactionID int primary key not null,
	ProductID int not null,
	TransactionDate datetime not null,
	CustomerID int not null,
	quantityBought int,
	totalPrice decimal(18,2),
	userID int

	constraint fk_transacprodID foreign key (ProductID) references Product(ProductID),
	constraint fk_transacCustID foreign key (CustomerID) references customer(CustomerID),
	constraint fk_userIDtransacforeign foreign key (userID) references registerApp(userID)
)

drop table transac

/* ---                      --- */
/* --- USER     MANAGEMENT -- */
/* --- ---                --*/
/* ---FOR VIEWING      ---*/

create procedure sp_viewUser
as begin
	select * from registerApp
end

create procedure sp_Register
@userName varchar(255),
@pass varchar(255)
as begin
	insert into registerApp (userName, pass) values (@userName, @pass)
end

create procedure sp_logIn
@userName varchar(255),
@pass varchar(255)
as begin
	select userName as 'User Name', pass as 'Password' from registerApp where userName = @userName and pass = @pass
end

create procedure sp_selectspecUser
@userName varchar(255)
as begin
	select userID as 'User ID' from registerApp where userName = @userName
end

create procedure sp_deleteUser 'rica1'
@userName varchar(255)
as begin
	delete from registerApp where userName = @userName
end

create procedure sp_updateUserName
@userID int,
@userName varchar(255)
as begin
	update registerApp set userName = @userName where userID = @userID
end

create procedure sp_updatePassword
@userID int,
@pass varchar(255)
as begin
	update registerApp set pass = @pass where userID = @userID
end

/* ---                     --- */
/* --- MENU               -- */
/* --- ---               --*/
/* ---FORMULA --- */

create procedure sp_sumInv
as begin
	select sum(quantity) from Inventory
end

CREATE PROCEDURE sp_FindMostStockedBrand
AS 
BEGIN
    SELECT TOP 1 
        b.BrandName AS 'Most Stocked Brand', 
        SUM(i.Quantity) AS 'Total Quantity'
    FROM 
        Inventory i
    INNER JOIN 
        Product p ON i.ProductID = p.ProductID
    INNER JOIN 
        Brand b ON p.BrandID = b.BrandID
    GROUP BY 
        b.BrandName
    ORDER BY 
        SUM(i.Quantity) DESC;
END

/* ---                     --- */
/* --- PRODUCT MANAGEMENT -- */
/* --- ---               --*/
/* ---FOR VIEWING --- */

create procedure sp_viewBrand
as begin
	select BrandID as 'Brand ID', BrandName as 'Brand Name' from Brand order by BrandID
end

create procedure sp_viewCat
as begin
	select CategoryID as 'Category ID', CategoryName as 'Category Name' from Category order by CategoryID
end

create procedure sp_viewPrice
as begin
	select ProductID as 'Product ID', sellingPrice as 'Selling Price', costPrice as 'Cost Price', ProfitMargin as 'Profit Margin' from prices
end

create procedure sp_viewPrice
as begin
	select ProductID as 'Product ID', sellingPrice as 'Selling Price', costPrice as 'Cost Price', ProfitMargin as 'Profit Margin' from prices order by ProductID
end

create procedure sp_viewInventory
as begin
	select ProductID as 'Product ID', quantity as 'Quantity' from Inventory where quantity > 0 order by ProductID
end

create procedure sp_viewInventory0
as begin
	select ProductID as 'Product ID', quantity as 'Quantity' from Inventory order by ProductID
end

create procedure sp_viewCondition
as begin
	select ConditionID as 'Condition ID', ConditionDescription as 'Description' from condition order by ConditionID
end

create procedure sp_viewQuality
as begin
	select ProductID as 'Product ID', ConditionID as 'Condition ID', Qual_Assessment as 'Quality Assessment' from quality order by ProductID
end

create procedure sp_viewProd
as begin
	select ProductID as 'Product ID', ProductName as 'Product Name', BrandID as 'Brand ID', CategoryID as 'Category ID' from Product order by ProductID
end

create procedure sp_viewProdwJoin
as begin
	select top 3 Product.ProductID as 'Product ID', ProductName as 'Product Name', Brand.BrandName as 'Brand', Category.CategoryName as 'Category' from product 
	inner join Brand on Product.BrandID = Brand.BrandID inner join Category on Product.CategoryID = Category.CategoryID order by product.ProductID desc;
end

/* ---FOR SEARCHING --- */
create procedure sp_searchBrand 
@BrandID int
as begin
	select BrandID as 'Brand ID', BrandName as 'Brand Name' from Brand where BrandID = @BrandID order by BrandID
end

create procedure sp_searchCat
@CategoryID int
as begin
	select CategoryID as 'Category ID', CategoryName as 'Category Name' from Category where CategoryID = @CategoryID
end

create procedure sp_searchQuality
@ProdID int
as begin
	select ProductID as 'Product ID', ConditionID as 'Condition ID', Qual_Assessment as 'Quality Assessment' from quality where ProductID = @ProdID 
end

create procedure sp_searchConditions
@ConditionID int
as begin
	select ConditionID as 'Condition ID', ConditionDescription as 'Description' from condition where ConditionID = @ConditionID
end

create procedure sp_searchPrice
@ProdID int
as begin
	select ProductID as 'Product ID', sellingPrice as 'Selling Price', costPrice as 'Cost Price', ProfitMargin as 'Profit Margin' from prices where ProductID = @ProdID
end

create procedure sp_searchProd
@ProdID int
as begin
	select ProductID as 'Product ID', ProductName as 'Product Name', BrandID as 'Brand ID', CategoryID as 'Category ID' from Product where ProductID = @ProdID
end

create procedure sp_searchInventory
@ProdID int
as begin
	select ProductID as 'Product ID', quantity as 'Quantity' from Inventory where ProductID = @ProdID
end

create procedure sp_displayInvwStcks
as begin
	select ProductID from Inventory where quantity > 0 order by ProductID
end

create procedure displayCustNamecb
as begin
	select customerID, fname + ' ' + lname as 'Customer Name' from customer 
end

create procedure sp_displayProdID
as begin
	select ProductID from Product order by ProductID
end

create procedure sp_displayBrandName
as begin
	select BrandID, BrandName from Brand order by BrandID
end

create procedure sp_displayCatName
as begin
	select CategoryID, CategoryName from Category order by CategoryID
end

create procedure sp_displayCond
as begin
	select ConditionID, ConditionDescription from condition order by ConditionID
end

/* ---FOR INSERTING --- */
create procedure sp_insertBrand
@BrandID int,
@BrandName varchar(255)
as begin
	insert into Brand(BrandID, BrandName) values (@BrandID, @BrandName)
end

create procedure sp_insertCategory
@CategoryID int,
@CategoryName varchar(255)
as begin
	insert into Category(CategoryID,CategoryName) values (@CategoryID, @CategoryName)
end

create procedure sp_insertCond
@ConditionID int,
@ConditionDescription varchar(255)
as begin
	insert into condition(ConditionID, ConditionDescription) values (@ConditionID, @ConditionDescription)
end

create procedure sp_insertProduct
@ProductID int,
@ProductName varchar(255),
@BrandID int,
@CategoryID int
as begin
	insert into Product (ProductID, ProductName, BrandID, CategoryID) values (@ProductID, @ProductName, @BrandID, @CategoryID)
end

create procedure sp_insertPrice
@ProductID int,
@sellingPrice decimal(18,2),
@costPrice decimal(18,2),
@ProfitMargin decimal(18,2)
as begin
	insert into Prices (ProductID, sellingPrice, costPrice, ProfitMargin) values (@ProductID, @sellingPrice, @costPrice, @ProfitMargin)
end

create procedure sp_insertInventory
@ProductID int,
@quantity int
as begin
	insert into Inventory (ProductID, Quantity) values (@ProductID, @quantity)
end

create procedure sp_insertQuality
@ProductID int,
@ConditionID int,
@Qual_Assessment varchar(255)
as begin
	insert into quality (ProductID, ConditionID, Qual_Assessment) values (@ProductID, @ConditionID, @Qual_Assessment)
end

/* ---FOR EDITING --- */
create procedure sp_editBrand
@BrandID int,
@BrandName varchar(255)
as begin
	update Brand set BrandName = @BrandName where BrandID = @BrandID
end

create procedure sp_editCat
@CategoryID int,
@CategoryName varchar(255)
as begin
	update Category set CategoryName = @CategoryName where CategoryID = @CategoryID
end

create procedure sp_editCondition
@ConditionID int,
@ConditionDescription varchar(255)
as begin
	update condition set ConditionDescription = @ConditionDescription where ConditionID = @ConditionID
end

create procedure sp_editProduct
@ProductID int,
@ProductName varchar(255),
@BrandID int,
@CategoryID int
as begin
	update Product set ProductName = @ProductName, BrandID = @BrandID, CategoryID = @CategoryID where ProductID = @ProductID
end

create procedure sp_editPrice
@ProductID int,
@sellingPrice decimal(18,2),
@costPrice decimal(18,2),
@ProfitMargin decimal(18,2)
as begin
	update Prices set sellingPrice = @sellingPrice, costPrice = @costPrice, ProfitMargin = @ProfitMargin where ProductID = @ProductID
end

create procedure sp_editInventory
@ProductID int,
@quantity int
as begin
	update Inventory set Quantity = @quantity where ProductID = @ProductID
end

create procedure sp_editQuality
@ProductID int,
@CondtionID int,
@Qual_Assessment varchar(255)
as begin
	update quality set ConditionID = @CondtionID, Qual_Assessment = @Qual_Assessment where ProductID = @ProductID
end

/* ---FOR DELETING --- */
create procedure sp_deleteBrand
@BrandID int
as begin
	delete from Brand where BrandID = @BrandID
end

create procedure sp_deleteCondition
@ConditionID int
as begin
	delete from condition where ConditionID = @ConditionID
end

create procedure sp_deleteQuality
@ProductID int
as begin
	delete from quality where ProductID = @ProductID
end

create procedure sp_deleteCat
@CategoryID int
as begin
	delete from Category where CategoryID = @CategoryID
end

create procedure sp_deleteProd
@ProductID int
as begin
	delete from Product where ProductID = @ProductID
end

create procedure sp_deletePrice
@ProductID int
as begin
	delete from prices where ProductID = @ProductID
end

create procedure sp_deleteInventory
@ProductID int
as begin
	delete from Inventory where ProductID = @ProductID
end

/*Formula*/
create procedure sp_totalCostPrice
as begin
	select sum(costPrice) as 'Cost Price' from prices
end

create procedure sp_totalProfit
as begin
	select sum(ProfitMargin) as 'Profit' from prices
end

create procedure sp_totalQuantity
as begin
	select sum(quantity) as 'Quantity' from Inventory
end

/* ---                      --- */
/* --- CUSTOMER MANAGEMENT -- */
/* --- ---                --*/
/* ---FOR VIEWING      ---*/

create procedure sp_viewCustomer
as begin
	select customerID as 'Customer ID', fname + ' ' + lname as 'Customer Name', town + ', ' + city as 'Address' from customer order by customerID
end

/* FOR SEARCHING*/

create procedure sp_searchCust
@CustomerID int
as begin
	select customerID as 'Customer ID', fname + ' ' + lname as 'Customer Name', town + ', ' + city as 'Address' from customer where customerID = @CustomerID
end

/* FOR INSERTING*/
create procedure sp_insertCustomer
@customerID int,
@fname varchar(255),
@lname varchar(255),
@town varchar(255),
@city varchar(255)
as begin
	insert into customer (customerID, fname, lname, town, city) values (@customerID, @fname, @lname, @town, @city)
end

/* FOR EDITING */
create procedure sp_editCustomer
@customerID int,
@fname varchar(255),
@lname varchar(255),
@town varchar(255),
@city varchar(255)
as begin
	update customer set fname = @fname, lname = @lname, town = @town, city = @city where customerID = @customerID
end

/* FOR DELETING */
create procedure sp_deleteCustomer
@customerID int
as begin
	delete from customer where customerID = @customerID
end

/* FOR SEARCHING */
create procedure sp_searchCustomer
@customerID int
as begin
	select customerID as 'Customer ID', fname + ' ' + lname as 'Customer Name', town + ', ' + city as 'Address' from customer where customerID = @customerID
end

/*Formula*/
create procedure countCustomer
as begin
	select count(customerID) as 'Total Number of Customers' from customer
end

/* ---                         --- */
/* --- TRANSACTION MANAGEMENT -- */
/* --- ---                    --*/
/* ---FOR VIEWING          --- */

create procedure sp_viewPricecbBox
@ProductID int
as begin
	select prices.sellingPrice as 'Selling Price' from Product inner join prices on product.ProductID = prices.ProductID where product.ProductID = @ProductID
end

create procedure sp_viewTransac
as begin
	select transactionID as 'Transaction ID', ProductID as 'Product ID', TransactionDate as 'Transaction Date', CustomerID as 'CustomerID', quantityBought as 'Quantity Bought', totalPrice as 'Total Price' from transac order by transactionID
end

create procedure sp_viewProdNameinTrans
@ProductID int
as begin
	select productName as 'Product Name' from product where productID = @productID
end

create procedure sp_viewCustomerinTrans
@CustomerID int 
as begin
	select fname + ' ' + lname as 'Customer Name' from customer where customerID = @CustomerID
end

create procedure sp_viewQuantInv
@ProductID int
as begin
	select quantity from Inventory where ProductID = @ProductID
end

/* ---FOR SEARCHING       --- */
create procedure sp_searchTransac
@transacID int
as begin
	select transactionID as 'Transaction ID', ProductID as 'Product ID', TransactionDate as 'Transaction Date', CustomerID as 'CustomerID', quantityBought as 'Quantity Bought', totalPrice as 'Total Price' from transac where transactionID = @transacID
end

/* ---FOR INSERTING       --- */
create procedure sp_insertTransac
@transactionID int,
@ProductID int,
@TransactionDate datetime,
@CustomerID int,
@quantityBought int,
@totalPrice decimal(18,2)
as begin
	insert into transac (transactionID, ProductID, TransactionDate, CustomerID, quantityBought, totalPrice) values (@transactionID, @ProductID, @TransactionDate, @CustomerID, @quantityBought, @totalPrice)
	update Inventory set Quantity = Quantity - @quantityBought where ProductID = @ProductID
end

/* ---FOR EDITING       --- */

create procedure sp_updateminTransac
@transactionID int,
@ProductID int,
@TransactionDate datetime,
@CustomerID int,
@quantityBought int,
@totalPrice decimal(18,2)
as begin
	update transac set ProductID = @ProductID, TransactionDate = @TransactionDate, CustomerID = @CustomerID, quantityBought = @quantityBought, totalPrice = @totalPrice where transactionID = @transactionID
	update Inventory set Quantity = Quantity - @quantityBought where ProductID = @ProductID
end

create procedure sp_updateaddTransac
@transactionID int,
@ProductID int,
@TransactionDate datetime,
@CustomerID int,
@quantityBought int,
@totalPrice decimal(18,2)
as begin
	update transac set ProductID = @ProductID, TransactionDate = @TransactionDate, CustomerID = @CustomerID, quantityBought = @quantityBought, totalPrice = @totalPrice where transactionID = @transactionID
	update Inventory set Quantity = Quantity + @quantityBought where ProductID = @ProductID
end

/* ---FOR DELETING       --- */
create procedure sp_deleteTransacOnly
@TransactionID int
as begin
	delete from transac where transactionID = @TransactionID
end

CREATE PROCEDURE sp_deleteTransac
    @transactionID INT
AS
BEGIN
    DECLARE @ProductID INT;
    DECLARE @QuantityBought INT;

    SELECT @ProductID = ProductID, @QuantityBought = quantityBought
    FROM transac
    WHERE transactionID = @transactionID;

    DELETE FROM transac
    WHERE transactionID = @transactionID;

    UPDATE Inventory
    SET Quantity = Quantity + @QuantityBought
    WHERE ProductID = @ProductID;
END;

/* ---FOR VIEW NAMES       --- */

create procedure sp_viewQuality2
as begin
	select Product.ProductName as 'Product Name', condition.ConditionDescription as 'Condition',Qual_Assessment as 'Quality Assessment' from quality inner join Product on quality.ProductID = Product.ProductID inner join condition on quality.ConditionID = condition.ConditionID
end

create procedure sp_viewProdwJoin1
as begin
	select Product.ProductID as 'Product ID', ProductName as 'Product Name', Brand.BrandName as 'Brand', Category.CategoryName as 'Category' from product 
	inner join Brand on Product.BrandID = Brand.BrandID inner join Category on Product.CategoryID = Category.CategoryID order by product.ProductID;
end

create procedure sp_viewPrice1
as begin
	select Product.ProductName as 'Product Name', sellingPrice as 'Selling Price', costPrice as 'Cost Price', ProfitMargin as 'Profit Margin' from prices inner join Product on prices.ProductID = Product.ProductID order by prices.ProductID
end

create procedure sp_viewInventory1
as begin
	select Product.ProductName as 'Product Name', quantity as 'Quantity' from Inventory inner join Product on Inventory.ProductID = Product.ProductID where quantity > 0 order by inventory.ProductID
end

create procedure sp_viewInventory2
as begin
	select Product.ProductName as 'Product Name', quantity as 'Quantity' from Inventory inner join Product on Inventory.ProductID = Product.ProductID order by inventory.ProductID
end

create procedure sp_viewCustNameTransac
as begin
	select transactionID as 'Transaction ID', Product.ProductName as 'Product Name', TransactionDate as 'Transaction Date', customer.fname + ' ' + customer.lname as 'Customer Name', quantityBought as 'Quantity Bought', totalPrice as 'Total Price' from transac inner join product on transac.productID = Product.ProductID inner join customer on transac.customerID = customer.CustomerID order by transactionID
end

CREATE PROCEDURE sp_GetCustomerTotalSpend
AS
BEGIN
    SELECT
        c.customerID,
        c.fname,
        c.lname,
        SUM(t.totalPrice) AS totalSpend
    FROM
        customer c
    LEFT JOIN
        transac t ON c.customerID = t.CustomerID
    GROUP BY
        c.customerID, c.fname, c.lname, c.town, c.city;
END;

CREATE PROCEDURE sp_GetCustomerTotalSpendSearch
@customerID int
AS
BEGIN
    SELECT
        c.customerID,
        c.fname,
        c.lname,
        SUM(t.totalPrice) AS totalSpend
    FROM
        customer c
    LEFT JOIN
        transac t ON c.customerID = t.CustomerID
	
	where c.customerID = @customerID

    GROUP BY
        c.customerID, c.fname, c.lname, c.town, c.city;
END;