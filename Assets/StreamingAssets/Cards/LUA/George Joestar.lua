--George Joestar
function Use()
	User.GetActions(1);
	Card.SelectCard(User);
end

Selectable={};
function SelectCards()
x=0
for i=0,9,1 do
	if Shop[i].Cost<=5 and Controller.ShopCards[Shop[i].Name]>0 
	then Selectable[x]=Shop[i];x=x+1; end
	end
return Selectable;
end

function OnSelect()
User.Gain(Selected.Name,User.Discarded,true);
return Selected;
end
