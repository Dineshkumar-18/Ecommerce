create table customers
(
 customer_id int IDENTITY(101,1) primary key,
 name varchar(255),
 email varchar(255),
 password varchar(255)
);

create table products
(
product_id int IDENTITY(1001,1) primary key,
name varchar(255),
price decimal(10,2),
description varchar(255),
stockQuantity int
);

create table cart
(
cart_id int IDENTITY(2001,1) primary key,
customer_id int references customers(customer_id),
product_id int references products(product_id),
quantity int
);

create table orders
(
order_id int IDENTITY(3001,1) primary key,
customer_id int references customers(customer_id),
order_date DATETIME DEFAULT GETDATE(),
total_price decimal(10,2),
shipping_address varchar(255)
);

create table order_items
(
order_item_id int IDENTITY(4001,1) primary key,
order_id int references orders(order_id),
product_id int references products(product_id),
quantity int
);

select * from products;
select * from customers;
select * from cart;
select * from orders;
select * from order_items;

