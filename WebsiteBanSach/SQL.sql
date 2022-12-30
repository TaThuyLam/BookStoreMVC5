create TABLE VOUCHER(
 MaVoucher int Primary key,
 TenVoucher nvarchar(50),
 GiamToiDa int,
 DHToiThieu int,
 NgayHetHan Date,
 SoTienGiam float,
 Mota NText,
)

insert into VOUCHER values(13497631555,N'Giảm 10%',25000,0,'30/12/2022',,N'Áp dụng cho ')