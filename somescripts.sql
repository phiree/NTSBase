
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

