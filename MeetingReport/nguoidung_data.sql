use QLKhoaHoc
go

update NguoiDung set Passwords = 0xE10ADC3949BA59ABBE56E057F20F883E
where Usernames in ('admin', 'dev', 'user2')
go

update NguoiDung set Passwords = 0xE1B162476227C62743C1229E6872E13A, RandomKey= 'khl'
where Usernames in ('user1')

select Usernames, Passwords,RandomKey from NguoiDung 