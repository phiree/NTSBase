select * from product p inner join productimageurls u
  on p.id=u.product_id
 where suppliername like '%huiqiang%';
delete  from productimageurls 
  where product_id in 
    (select id from product  where suppliername like '%huiqiang%')
;
delete  from product  where suppliername like '%huiqiang%';

