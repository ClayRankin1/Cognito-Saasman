#be sure to use NEW Stored Procedures!!!


#update nextactor from names to array of userids
  
  --STEP #1 Update strings to add space at end
update acts set nextActor = nextActor + ' ' where  CHARINDEX('[', RTRIM(nextactor)) = 0
 

  --STEP #2
  --drop table #finalTable
select  distinct actid,username,   a.userID, acts.NextActor 
into #rrr 
from aspnetusers a
left join acts on
(CHARINDEX(UserName + ';', RTRIM(nextactor COLLATE Latin1_General_CS_AS)) > 0

or
CHARINDEX(';' + UserName, RTRIM(nextactor COLLATE Latin1_General_CS_AS)) > 0

or
CHARINDEX( UserName , RTRIM(nextactor COLLATE Latin1_General_CS_AS)) > 0

)
where  NextActor is not null
 
 --STEP #3
SELECT distinct
#rrr.actid AS actid,
(SELECT '[' +  stuff(
    (
    select ',' + cast(userID as varchar) FROM #rrr t1 WHERE t1.actid = #rrr.actid 
    FOR XML PATH('')
    )
, 1, 1, '') + ']')AS NextActor
into #finalTable
FROM
#rrr order by actid
 
 
  
 --STEP #4
 update acts set NextActor = f.NextActor
--select acts.nextactor 
from acts
 join  #finalTable f on f.actid = acts.ActID
 where  CHARINDEX( '[' , RTRIM(acts.NextActor)) = 0
  
 -------------------------------------------------------------------------------------------

 

