--Cream
function Use()
	Enemy.TakeDamage(3);
	Card.SelectCard(User);
end

Selectable={};
function SelectCards()
	Selectable=User.Hand;
return Selectable;
end

function OnSelect()
x=0;
c=User.Discarded.Count-1;
--if User.Discarded.Count==0 then return Selected end;
for i=0,c,1 do
	if User.Discarded[x].Name==Selected.Name then
		User.Destroy(User.Discarded[x],User.Discarded);
	else x=x+1;
	end
end
return Selected
end

