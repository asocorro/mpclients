alter table client add Territory  AS (substring([client_id],(0),(3)))

 alter table MembershipUsers add territory varchar(150)

alter table Orders add OnBehalfOf varchar(50)