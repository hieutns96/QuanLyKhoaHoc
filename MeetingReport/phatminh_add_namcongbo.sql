use QLKhoaHoc
go

alter table PhatMinhGiaiPhap
add NamCongBo date
go

update PhatMinhGiaiPhap set NamCongBo = CAST(0xA43C0B00 AS Date)
where MaPM in (1, 2)
go

update PhatMinhGiaiPhap set NamCongBo = CAST(0xD73D0B00 AS Date)
where MaPM in (3, 4)
go

update PhatMinhGiaiPhap set NamCongBo = CAST(0x7C400B00 AS Date)
where MaPM in (5, 6)
go

