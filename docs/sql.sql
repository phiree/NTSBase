select modelnumber, replace(
					replace(
					replace(
 replace(
					replace(
					replace(
 replace(
					replace(
					replace(
						modelnumber,'\\','$')
								   ,'/','$')
								   ,'<','$') 
									 ,'>','$')
									 ,'?','$')
									 ,'*','$')
									 ,'"','$')
									 ,':','$')
									 ,'|','$')
as cc

 from product;

update product set modelnumber=replace(
					replace(
					replace(
 replace(
					replace(
					replace(
 replace(
					replace(
					replace(
						modelnumber,'\\','$')
								   ,'/','$')
								   ,'<','$') 
									 ,'>','$')
									 ,'?','$')
									 ,'*','$')
									 ,'"','$')
									 ,':','$')
									 ,'|','$')