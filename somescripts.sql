
/*没有图片的产品*/


select  p.supplierName,s.name,p.modelnumber,p.name 
from ntsbase.product p
inner join ntsbase.supplier s
on p.suppliername=s.name or p.suppliername=s.englishname
 left outer join
ntsbase.productimageurls i
on p.id=i.product_id
where i.product_id is null
order by suppliername,modelnumber

select p.suppliername ,s.name,s.englishname
from product p 
right outer join 
supplier s
on p.suppliername=s.name or p.suppliername=s.englishname
where p.suppliername is null

/*重复的产品信息*/

create table tempcc
(
 cc int,
 modelnumber varchar(50),
suppliercode varchar(50)
)DEFAULT COLLATE utf8_unicode_ci
;
insert into tempcc select count(*) cc,modelnumber,suppliercode

from ntsbase_test.product  
group by modelnumber,suppliercode
order by cc desc;
select * from tempcc where cc>1;
select a.* from ntsbase_test.product a,tempcc b
 where a.modelnumber = b.modelnumber 
and a.suppliercode = b.suppliercode
;
drop table tempcc


