-- exec = execute a stored procedure
-- dbo.sp... = the name of the stored procedure
--  'Corey' or @LastName='Corey'  = parameter passed to the stored procedure
exec dbo.spTestPerson_GetByLastName @LastName='corey'

select *
from dbo.TestPerson